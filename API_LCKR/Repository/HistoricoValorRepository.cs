using API_LCKR.Models;

namespace API_LCKR.Repository
{
    public class HistoricoValorRepository
    {
        private readonly LckrDatabaseContext _dbContext;

        public HistoricoValorRepository(LckrDatabaseContext context)
        {
            _dbContext = context;
        }

        public List<CoachSalaryHistory> ListarHistorico(string cpf)
        {
            return _dbContext.CoachSalaryHistories
                .Where(h => h.CoachCpf == cpf)
                .OrderByDescending(h => h.DataInicio)
                .ToList();
        }

        public CoachSalaryHistory? BuscarUltimo(string cpf)
        {
            return _dbContext.CoachSalaryHistories
                .Where(h => h.CoachCpf == cpf)
                .OrderByDescending(h => h.DataInicio)
                .FirstOrDefault();
        }

        public CoachSalaryHistory? BuscarVigente(string cpf, DateOnly data)
        {
            return _dbContext.CoachSalaryHistories
                .Where(h => h.CoachCpf == cpf && h.DataInicio <= data && (h.DataFim == null || h.DataFim >= data))
                .OrderByDescending(h => h.DataInicio)
                .FirstOrDefault();
        }

        public void RegistrarReajuste(Coach coach, CoachSalaryHistory? anterior, CoachSalaryHistory novo)
        {
            if (anterior != null)
            {
                anterior.DataFim = novo.DataInicio.AddDays(-1);
                anterior.UpdatedAt = DateTime.UtcNow;
            }

            coach.SalarioAtual = novo.Salario;
            coach.UpdatedAt = DateTime.UtcNow;

            _dbContext.CoachSalaryHistories.Add(novo);
            _dbContext.SaveChanges();
        }
    }
}
