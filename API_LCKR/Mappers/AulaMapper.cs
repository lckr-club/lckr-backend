using API_LCKR.DTOs;
using API_LCKR.Models;

namespace API_LCKR.Mappers
{
    public class AulaMapper
    {
        public Lesson ParaEntidade(AulaDto aula)
        {
            var entidade = new Lesson
            {
                Status = StatusLesson.Agendada
            };

            Atualizar(entidade, aula);

            return entidade;
        }

        public void Atualizar(Lesson entidade, AulaDto aula)
        {
            entidade.DataAula = aula.DataAula!.Value;
            entidade.HoraInicio = aula.HoraInicio!.Value;
            entidade.HoraFim = aula.HoraFim!.Value;
            entidade.Modalidade = aula.Modalidade;
            entidade.Descricao = aula.Descricao;
            entidade.Capacidade = aula.Capacidade;
            entidade.CoachCpf = aula.CpfCoach;
        }

        public AulaRespostaDto ParaDto(Lesson aula)
        {
            return new AulaRespostaDto
            {
                Id = aula.Id,
                DataAula = aula.DataAula,
                HoraInicio = aula.HoraInicio,
                HoraFim = aula.HoraFim,
                Modalidade = aula.Modalidade,
                Descricao = aula.Descricao,
                Capacidade = aula.Capacidade,
                Status = aula.Status,
                CpfCoach = aula.CoachCpf,
                NomeCoach = aula.CoachCpfNavigation.Nome,
                CpfCoachPrevisto = aula.CoachCpfPrevisto,
                NomeCoachPrevisto = aula.CoachCpfPrevistoNavigation?.Nome
            };
        }
    }
}
