using API_LCKR.DTOs;
using API_LCKR.Helpers;
using API_LCKR.Models;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace API_LCKR.Services
{
    public class ExportacaoService
    {
        private static readonly CultureInfo Cultura = new("pt-BR");

        public byte[] GerarPdf(FolhaDto folha, string nomeOrganizacao)
        {
            var exibicao = MesReferencia.Exibicao(folha.MesReferencia);

            return Document.Create(documento =>
            {
                documento.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4);
                    pagina.Margin(30);
                    pagina.DefaultTextStyle(texto => texto.FontSize(10));

                    pagina.Header().Column(coluna =>
                    {
                        coluna.Item().Text(nomeOrganizacao).FontSize(16).Bold();
                        coluna.Item().Text($"Folha de pagamento {exibicao}").FontSize(13);
                        coluna.Item().Text($"Período: {folha.DataInicio:dd/MM/yyyy} a {folha.DataFim:dd/MM/yyyy}   Situação: {Situacao(folha.StatusPeriodo)}");
                    });

                    pagina.Content().PaddingVertical(12).Column(coluna =>
                    {
                        coluna.Spacing(14);

                        coluna.Item().Table(tabela =>
                        {
                            tabela.ColumnsDefinition(colunas =>
                            {
                                colunas.RelativeColumn(4);
                                colunas.RelativeColumn(2);
                                colunas.RelativeColumn(1);
                                colunas.RelativeColumn(1.5f);
                                colunas.RelativeColumn(2);
                            });

                            tabela.Header(cabecalho =>
                            {
                                Cabecalho(cabecalho.Cell(), "Coach");
                                Cabecalho(cabecalho.Cell(), "CPF");
                                Cabecalho(cabecalho.Cell(), "Aulas", true);
                                Cabecalho(cabecalho.Cell(), "Horas", true);
                                Cabecalho(cabecalho.Cell(), "Valor", true);
                            });

                            foreach (var coach in folha.Coaches)
                            {
                                Celula(tabela.Cell(), coach.NomeCoach);
                                Celula(tabela.Cell(), FormatarCpf(coach.CpfCoach));
                                Celula(tabela.Cell(), coach.QuantidadeAulas.ToString(), true);
                                Celula(tabela.Cell(), coach.Horas.ToString("N2", Cultura), true);
                                Celula(tabela.Cell(), coach.Valor.ToString("C", Cultura), true);
                            }

                            tabela.Cell().ColumnSpan(3).PaddingVertical(4).Text("Total").Bold();
                            tabela.Cell().PaddingVertical(4).AlignRight().Text(folha.TotalHoras.ToString("N2", Cultura)).Bold();
                            tabela.Cell().PaddingVertical(4).AlignRight().Text(folha.ValorTotal.ToString("C", Cultura)).Bold();
                        });

                        foreach (var coach in folha.Coaches)
                        {
                            coluna.Item().Column(detalhe =>
                            {
                                detalhe.Item().PaddingBottom(4).Text($"{coach.NomeCoach} ({FormatarCpf(coach.CpfCoach)})").Bold();

                                detalhe.Item().Table(tabela =>
                                {
                                    tabela.ColumnsDefinition(colunas =>
                                    {
                                        colunas.RelativeColumn(1.6f);
                                        colunas.RelativeColumn(1.6f);
                                        colunas.RelativeColumn(2.4f);
                                        colunas.RelativeColumn(1);
                                        colunas.RelativeColumn(1.6f);
                                        colunas.RelativeColumn(1.6f);
                                    });

                                    tabela.Header(cabecalho =>
                                    {
                                        Cabecalho(cabecalho.Cell(), "Data");
                                        Cabecalho(cabecalho.Cell(), "Horário");
                                        Cabecalho(cabecalho.Cell(), "Modalidade");
                                        Cabecalho(cabecalho.Cell(), "Horas", true);
                                        Cabecalho(cabecalho.Cell(), "Valor hora", true);
                                        Cabecalho(cabecalho.Cell(), "Valor", true);
                                    });

                                    foreach (var aula in coach.Aulas)
                                    {
                                        Celula(tabela.Cell(), aula.DataAula.ToString("dd/MM/yyyy"));
                                        Celula(tabela.Cell(), $"{aula.HoraInicio:HH\\:mm} às {aula.HoraFim:HH\\:mm}");
                                        Celula(tabela.Cell(), aula.Modalidade);
                                        Celula(tabela.Cell(), aula.Horas.ToString("N2", Cultura), true);
                                        Celula(tabela.Cell(), aula.ValorHora.ToString("C", Cultura), true);
                                        Celula(tabela.Cell(), aula.Valor.ToString("C", Cultura), true);
                                    }
                                });
                            });
                        }

                        if (folha.Pendencias.Count > 0)
                        {
                            coluna.Item().Column(pendencias =>
                            {
                                pendencias.Item().PaddingBottom(4).Text("Pendências").Bold().FontColor(Colors.Red.Darken2);

                                foreach (var pendencia in folha.Pendencias)
                                {
                                    pendencias.Item().Text(pendencia.Mensagem);
                                }
                            });
                        }
                    });

                    pagina.Footer().AlignCenter().Text(texto =>
                    {
                        texto.Span($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}   Página ");
                        texto.CurrentPageNumber();
                        texto.Span(" de ");
                        texto.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        public byte[] GerarExcel(FolhaDto folha, string nomeOrganizacao)
        {
            var exibicao = MesReferencia.Exibicao(folha.MesReferencia);

            using var planilha = new XLWorkbook();

            var resumo = planilha.Worksheets.Add("Resumo");

            resumo.Cell(1, 1).Value = $"{nomeOrganizacao} - Folha de pagamento {exibicao}";
            resumo.Cell(1, 1).Style.Font.Bold = true;
            resumo.Cell(1, 1).Style.Font.FontSize = 14;
            resumo.Cell(2, 1).Value = $"Período: {folha.DataInicio:dd/MM/yyyy} a {folha.DataFim:dd/MM/yyyy}   Situação: {Situacao(folha.StatusPeriodo)}";

            EscreverCabecalho(resumo, 4, "Coach", "CPF", "Aulas", "Horas", "Valor (R$)");

            var linha = 5;

            foreach (var coach in folha.Coaches)
            {
                resumo.Cell(linha, 1).Value = coach.NomeCoach;
                resumo.Cell(linha, 2).Value = FormatarCpf(coach.CpfCoach);
                resumo.Cell(linha, 3).Value = coach.QuantidadeAulas;
                resumo.Cell(linha, 4).Value = coach.Horas;
                resumo.Cell(linha, 5).Value = coach.Valor;
                linha++;
            }

            resumo.Cell(linha, 1).Value = "Total";
            resumo.Cell(linha, 4).Value = folha.TotalHoras;
            resumo.Cell(linha, 5).Value = folha.ValorTotal;
            resumo.Row(linha).Style.Font.Bold = true;
            resumo.Range(5, 4, linha, 5).Style.NumberFormat.Format = "#,##0.00";
            resumo.Columns(1, 5).AdjustToContents(4, linha);

            var aulas = planilha.Worksheets.Add("Aulas");

            EscreverCabecalho(aulas, 1, "Coach", "CPF", "Data", "Início", "Fim", "Modalidade", "Horas", "Valor hora (R$)", "Valor (R$)", "Coach previsto");

            linha = 2;

            foreach (var coach in folha.Coaches)
            {
                foreach (var aula in coach.Aulas)
                {
                    aulas.Cell(linha, 1).Value = coach.NomeCoach;
                    aulas.Cell(linha, 2).Value = FormatarCpf(coach.CpfCoach);
                    aulas.Cell(linha, 3).Value = aula.DataAula.ToDateTime(TimeOnly.MinValue);
                    aulas.Cell(linha, 4).Value = aula.HoraInicio.ToString("HH:mm");
                    aulas.Cell(linha, 5).Value = aula.HoraFim.ToString("HH:mm");
                    aulas.Cell(linha, 6).Value = aula.Modalidade;
                    aulas.Cell(linha, 7).Value = aula.Horas;
                    aulas.Cell(linha, 8).Value = aula.ValorHora;
                    aulas.Cell(linha, 9).Value = aula.Valor;
                    aulas.Cell(linha, 10).Value = aula.CpfCoachPrevisto == null ? string.Empty : FormatarCpf(aula.CpfCoachPrevisto);
                    linha++;
                }
            }

            aulas.Column(3).Style.DateFormat.Format = "dd/MM/yyyy";
            aulas.Columns(7, 9).Style.NumberFormat.Format = "#,##0.00";
            aulas.Columns(1, 10).AdjustToContents();

            if (folha.Pendencias.Count > 0)
            {
                var pendencias = planilha.Worksheets.Add("Pendências");

                EscreverCabecalho(pendencias, 1, "Coach", "CPF", "Data", "Início", "Mensagem");

                linha = 2;

                foreach (var pendencia in folha.Pendencias)
                {
                    pendencias.Cell(linha, 1).Value = pendencia.NomeCoach;
                    pendencias.Cell(linha, 2).Value = FormatarCpf(pendencia.CpfCoach);
                    pendencias.Cell(linha, 3).Value = pendencia.DataAula.ToDateTime(TimeOnly.MinValue);
                    pendencias.Cell(linha, 4).Value = pendencia.HoraInicio.ToString("HH:mm");
                    pendencias.Cell(linha, 5).Value = pendencia.Mensagem;
                    linha++;
                }

                pendencias.Column(3).Style.DateFormat.Format = "dd/MM/yyyy";
                pendencias.Columns(1, 5).AdjustToContents();
            }

            using var memoria = new MemoryStream();

            planilha.SaveAs(memoria);

            return memoria.ToArray();
        }

        private static void Cabecalho(IContainer celula, string texto, bool direita = false)
        {
            var container = celula.BorderBottom(1).BorderColor(Colors.Grey.Darken1).PaddingVertical(4);

            (direita ? container.AlignRight() : container).Text(texto).Bold();
        }

        private static void Celula(IContainer celula, string texto, bool direita = false)
        {
            var container = celula.BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3);

            (direita ? container.AlignRight() : container).Text(texto);
        }

        private static void EscreverCabecalho(IXLWorksheet aba, int linha, params string[] titulos)
        {
            for (var i = 0; i < titulos.Length; i++)
            {
                aba.Cell(linha, i + 1).Value = titulos[i];
            }

            var faixa = aba.Range(linha, 1, linha, titulos.Length);

            faixa.Style.Font.Bold = true;
            faixa.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        private static string Situacao(StatusPeriodo status)
        {
            return status == StatusPeriodo.Fechado ? "Fechado" : "Aberto";
        }

        private static string FormatarCpf(string cpf)
        {
            return cpf.Length == 11
                ? $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}"
                : cpf;
        }
    }
}
