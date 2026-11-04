using API_LCKR.DTOs;
using API_LCKR.Helpers;
using API_LCKR.Models;
using API_LCKR.Repository;

namespace API_LCKR.Services
{
    public class FolhaService
    {
        private readonly FolhaRepository _repository;
        private readonly PeriodoRepository _periodoRepository;
        private readonly ExportacaoService _exportacaoService;

        public FolhaService(FolhaRepository repository, PeriodoRepository periodoRepository, ExportacaoService exportacaoService)
        {
            _repository = repository;
            _periodoRepository = periodoRepository;
            _exportacaoService = exportacaoService;
        }

        public FolhaDto CalcularFolha(string mes)
        {
            var (inicio, fim) = MesReferencia.Intervalo(mes);
            var aulas = _repository.ListarAulas(inicio, fim)
                .Where(a => a.Status == StatusLesson.Realizada)
                .ToList();
            var historicos = BuscarHistoricos(aulas);
            var periodo = _periodoRepository.BuscarPorMes(mes);

            var folha = new FolhaDto
            {
                MesReferencia = mes,
                DataInicio = inicio,
                DataFim = fim,
                StatusPeriodo = periodo?.Status ?? StatusPeriodo.Aberto
            };

            var minutosTotais = 0;

            foreach (var grupo in aulas.GroupBy(a => a.CoachCpf).OrderBy(g => g.First().CoachCpfNavigation.Nome))
            {
                var coach = grupo.First().CoachCpfNavigation;
                var item = new FolhaCoachDto
                {
                    CpfCoach = coach.Cpf,
                    NomeCoach = coach.Nome
                };
                var minutosCoach = 0;

                foreach (var aula in grupo)
                {
                    var vigente = BuscarVigente(historicos, aula.CoachCpf, aula.DataAula);

                    if (vigente == null)
                    {
                        folha.Pendencias.Add(new PendenciaFolhaDto
                        {
                            CpfCoach = coach.Cpf,
                            NomeCoach = coach.Nome,
                            IdAula = aula.Id,
                            DataAula = aula.DataAula,
                            HoraInicio = aula.HoraInicio,
                            Mensagem = $"O coach {coach.Nome} não possui valor da hora vigente para a aula de {aula.DataAula:dd/MM/yyyy} às {aula.HoraInicio:HH\\:mm}."
                        });

                        continue;
                    }

                    var minutos = Minutos(aula);
                    minutosCoach += minutos;

                    item.Aulas.Add(new FolhaAulaDto
                    {
                        IdAula = aula.Id,
                        DataAula = aula.DataAula,
                        HoraInicio = aula.HoraInicio,
                        HoraFim = aula.HoraFim,
                        Modalidade = aula.Modalidade,
                        Horas = ParaHoras(minutos),
                        ValorHora = vigente.Salario,
                        Valor = CalcularValor(vigente.Salario, minutos),
                        CpfCoachPrevisto = aula.CoachCpfPrevisto
                    });
                }

                if (item.Aulas.Count == 0)
                {
                    continue;
                }

                item.QuantidadeAulas = item.Aulas.Count;
                item.Horas = ParaHoras(minutosCoach);
                item.Valor = item.Aulas.Sum(a => a.Valor);
                minutosTotais += minutosCoach;

                folha.Coaches.Add(item);
            }

            folha.TotalHoras = ParaHoras(minutosTotais);
            folha.ValorTotal = folha.Coaches.Sum(c => c.Valor);

            return folha;
        }

        public ResumoMesDto ResumoMes(string mes)
        {
            var (inicio, fim) = MesReferencia.Intervalo(mes);
            var aulas = _repository.ListarAulas(inicio, fim);
            var historicos = BuscarHistoricos(aulas);

            var minutosRealizados = 0;
            var minutosAgendados = 0;
            var valorRealizado = 0m;
            var valorAgendado = 0m;
            var aulasSemValor = 0;

            foreach (var aula in aulas)
            {
                var vigente = BuscarVigente(historicos, aula.CoachCpf, aula.DataAula);
                var minutos = Minutos(aula);

                if (aula.Status == StatusLesson.Realizada)
                {
                    minutosRealizados += minutos;
                }
                else
                {
                    minutosAgendados += minutos;
                }

                if (vigente == null)
                {
                    aulasSemValor++;
                    continue;
                }

                var valor = CalcularValor(vigente.Salario, minutos);

                if (aula.Status == StatusLesson.Realizada)
                {
                    valorRealizado += valor;
                }
                else
                {
                    valorAgendado += valor;
                }
            }

            return new ResumoMesDto
            {
                MesReferencia = mes,
                TotalHoras = ParaHoras(minutosRealizados),
                HorasAgendadas = ParaHoras(minutosAgendados),
                CoachesAtivos = _repository.ContarCoachesAtivos(),
                ValorRealizado = valorRealizado,
                FolhaPrevista = valorRealizado + valorAgendado,
                AulasSemValor = aulasSemValor
            };
        }

        public byte[] ExportarPdf(string mes)
        {
            var folha = CalcularFolha(mes);

            return _exportacaoService.GerarPdf(folha, _repository.BuscarOrganizacao().Nome);
        }

        public byte[] ExportarExcel(string mes)
        {
            var folha = CalcularFolha(mes);

            return _exportacaoService.GerarExcel(folha, _repository.BuscarOrganizacao().Nome);
        }

        private List<CoachSalaryHistory> BuscarHistoricos(List<Lesson> aulas)
        {
            var cpfs = aulas.Select(a => a.CoachCpf).Distinct().ToList();

            return _repository.ListarHistoricos(cpfs);
        }

        private static CoachSalaryHistory? BuscarVigente(List<CoachSalaryHistory> historicos, string cpf, DateOnly data)
        {
            return historicos
                .Where(h => h.CoachCpf == cpf && h.DataInicio <= data && (h.DataFim == null || h.DataFim >= data))
                .OrderByDescending(h => h.DataInicio)
                .FirstOrDefault();
        }

        private static int Minutos(Lesson aula)
        {
            return (int)(aula.HoraFim - aula.HoraInicio).TotalMinutes;
        }

        private static decimal ParaHoras(int minutos)
        {
            return Math.Round(minutos / 60m, 2, MidpointRounding.AwayFromZero);
        }

        private static decimal CalcularValor(decimal valorHora, int minutos)
        {
            return Math.Round(valorHora * minutos / 60m, 2, MidpointRounding.AwayFromZero);
        }
    }
}
