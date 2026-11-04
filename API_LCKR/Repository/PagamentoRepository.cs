using API_LCKR.Models;
using Microsoft.EntityFrameworkCore;

namespace API_LCKR.Repository
{
    public class PagamentoRepository
    {
        private readonly LckrDatabaseContext _dbContext;

        public PagamentoRepository(LckrDatabaseContext context)
        {
            _dbContext = context;
        }

        public void AtualizarAtrasados(DateOnly hoje)
        {
            _dbContext.Payments
                .Where(p => p.Status == StatusPagamento.Pendente && p.DataPagamento < hoje)
                .ExecuteUpdate(s => s.SetProperty(p => p.Status, StatusPagamento.Atrasado));
        }

        public List<Payment> ListarPagamentos(string? mes, string? cpfCoach, StatusPagamento? status)
        {
            var consulta = _dbContext.Payments
                .Include(p => p.CpfCoachNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(mes))
            {
                consulta = consulta.Where(p => p.MesReferencia == mes);
            }

            if (!string.IsNullOrWhiteSpace(cpfCoach))
            {
                consulta = consulta.Where(p => p.CpfCoach == cpfCoach);
            }

            if (status != null)
            {
                consulta = consulta.Where(p => p.Status == status);
            }

            return consulta
                .OrderByDescending(p => p.MesReferencia)
                .ThenBy(p => p.CpfCoachNavigation.Nome)
                .ToList();
        }

        public Payment? BuscarPagamento(Guid id)
        {
            return _dbContext.Payments
                .Include(p => p.CpfCoachNavigation)
                .FirstOrDefault(p => p.Id == id);
        }

        public bool ExistePagoNoMes(string mes)
        {
            return _dbContext.Payments.Any(p => p.MesReferencia == mes && p.Status == StatusPagamento.Pago);
        }

        public void AtualizarPagamento(Payment pagamento)
        {
            _dbContext.Payments.Update(pagamento);
            _dbContext.SaveChanges();
        }
    }
}
