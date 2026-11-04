using API_LCKR.Models;
using Microsoft.EntityFrameworkCore;

namespace API_LCKR.Repository
{
    public class FolhaRepository
    {
        private readonly LckrDatabaseContext _dbContext;

        public FolhaRepository(LckrDatabaseContext context)
        {
            _dbContext = context;
        }

        public List<Lesson> ListarAulas(DateOnly inicio, DateOnly fim)
        {
            return _dbContext.Lessons
                .Include(l => l.CoachCpfNavigation)
                .Where(l => l.DataAula >= inicio && l.DataAula <= fim)
                .OrderBy(l => l.DataAula)
                .ThenBy(l => l.HoraInicio)
                .ToList();
        }

        public List<CoachSalaryHistory> ListarHistoricos(List<string> cpfs)
        {
            return _dbContext.CoachSalaryHistories
                .Where(h => cpfs.Contains(h.CoachCpf))
                .ToList();
        }

        public int ContarCoachesAtivos()
        {
            return _dbContext.Coaches.Count(c => c.Status == StatusCoach.Ativo);
        }

        public int ContarAulasAgendadas(DateOnly inicio, DateOnly fim)
        {
            return _dbContext.Lessons.Count(l => l.DataAula >= inicio && l.DataAula <= fim && l.Status == StatusLesson.Agendada);
        }

        public Organization BuscarOrganizacao()
        {
            return _dbContext.Organizations.OrderBy(o => o.Cnpj).First();
        }
    }
}
