using API_LCKR.Models;
using Microsoft.EntityFrameworkCore;

namespace API_LCKR.Repository
{
    public class AulaRepository
    {
        private readonly LckrDatabaseContext _dbContext;

        public AulaRepository(LckrDatabaseContext context)
        {
            _dbContext = context;
        }

        public Lesson? BuscarAula(Guid id)
        {
            return _dbContext.Lessons
                .Include(l => l.CoachCpfNavigation)
                .Include(l => l.CoachCpfPrevistoNavigation)
                .FirstOrDefault(l => l.Id == id);
        }

        public List<Lesson> ListarAulas(DateOnly? inicio, DateOnly? fim, string? cpfCoach)
        {
            var consulta = _dbContext.Lessons
                .Include(l => l.CoachCpfNavigation)
                .Include(l => l.CoachCpfPrevistoNavigation)
                .AsQueryable();

            if (inicio != null)
            {
                consulta = consulta.Where(l => l.DataAula >= inicio);
            }

            if (fim != null)
            {
                consulta = consulta.Where(l => l.DataAula <= fim);
            }

            if (!string.IsNullOrWhiteSpace(cpfCoach))
            {
                consulta = consulta.Where(l => l.CoachCpf == cpfCoach);
            }

            return consulta
                .OrderBy(l => l.DataAula)
                .ThenBy(l => l.HoraInicio)
                .ToList();
        }

        public bool ExisteSobreposicao(string cpfCoach, DateOnly data, TimeOnly inicio, TimeOnly fim, Guid? ignorarId)
        {
            return _dbContext.Lessons.Any(l =>
                l.CoachCpf == cpfCoach
                && l.DataAula == data
                && l.Id != ignorarId
                && inicio < l.HoraFim
                && fim > l.HoraInicio);
        }

        public void AdicionarAula(Lesson aula)
        {
            _dbContext.Lessons.Add(aula);
            _dbContext.SaveChanges();
        }

        public void AtualizarAula(Lesson aula)
        {
            aula.UpdatedAt = DateTime.UtcNow;

            _dbContext.Lessons.Update(aula);
            _dbContext.SaveChanges();
        }

        public void ExcluirAula(Lesson aula)
        {
            _dbContext.Lessons.Remove(aula);
            _dbContext.SaveChanges();
        }
    }
}
