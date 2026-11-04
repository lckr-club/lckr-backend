using API_LCKR.Models;

namespace API_LCKR.Repository
{
    public class PeriodoRepository
    {
        private readonly LckrDatabaseContext _dbContext;

        public PeriodoRepository(LckrDatabaseContext context)
        {
            _dbContext = context;
        }

        public PayrollPeriod? BuscarPorMes(string mes)
        {
            return _dbContext.PayrollPeriods.FirstOrDefault(p => p.MesReferencia == mes);
        }

        public List<PayrollPeriod> ListarPeriodos()
        {
            return _dbContext.PayrollPeriods
                .OrderByDescending(p => p.MesReferencia)
                .ToList();
        }

        public bool EstaFechado(DateOnly data)
        {
            return _dbContext.PayrollPeriods.Any(p =>
                p.Status == StatusPeriodo.Fechado
                && p.DataInicio <= data
                && p.DataFim >= data);
        }

        public PayrollPeriod? BuscarFechadoAPartirDe(DateOnly data)
        {
            return _dbContext.PayrollPeriods
                .Where(p => p.Status == StatusPeriodo.Fechado && p.DataFim >= data)
                .OrderBy(p => p.DataInicio)
                .FirstOrDefault();
        }

        public void FecharPeriodo(PayrollPeriod periodo, bool novo, List<Payment> cancelar, List<Payment> gerar)
        {
            using var transacao = _dbContext.Database.BeginTransaction();

            foreach (var pagamento in cancelar)
            {
                pagamento.Status = StatusPagamento.Cancelado;
            }

            _dbContext.SaveChanges();

            if (novo)
            {
                _dbContext.PayrollPeriods.Add(periodo);
            }

            _dbContext.Payments.AddRange(gerar);
            _dbContext.SaveChanges();

            transacao.Commit();
        }

        public void AtualizarPeriodo(PayrollPeriod periodo)
        {
            periodo.UpdatedAt = DateTime.UtcNow;

            _dbContext.PayrollPeriods.Update(periodo);
            _dbContext.SaveChanges();
        }
    }
}
