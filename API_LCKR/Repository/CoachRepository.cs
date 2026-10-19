using API_LCKR.DTOs;
using API_LCKR.Models;

namespace API_LCKR.Repository
{
    public class CoachRepository
    {
        private readonly LckrDatabaseContext _dbContext;

        public CoachRepository(LckrDatabaseContext context)
        {
            _dbContext = context;
        }

        public Coach? FindCoachDto(CoachDto coach)
        {
            var database = _dbContext.Coaches.Find(coach.Cpf);
            return database;
        }
        public Coach? FindCoachCpf(string cpf)
        {
            var database = _dbContext.Coaches.Find(cpf);
            return database;
        }
        public Coach? FindCoachEmail(string email)
        {
            var database = _dbContext.Coaches.FirstOrDefault(d => d.Email == email);
            return database;
        }

        public bool Pendencias(string cpf)
        {
            var pendenciasLessons = _dbContext.Lessons.Any(l => l.CoachCpf == cpf && l.Status == StatusLesson.Agendada);
            var pendenciasPayments = _dbContext.Payments.Any(
                p => p.CpfCoach == cpf && (p.Status == StatusPagamento.Pendente || p.Status == StatusPagamento.Atrasado));

            return pendenciasLessons || pendenciasPayments;
        }

        public List<Coach?> ListCoaches()
        {
            var listCoach = _dbContext.Coaches.ToList();

            return listCoach;
        }

        public void addCoach (Coach coach)
        {
            _dbContext.Coaches.Add(coach);
            _dbContext.SaveChanges();
        }

        public void UpdateCoach(Coach coach, CoachDto coachDto)
        {
            coach.Nome = coachDto.Nome;
            coach.Email = coachDto.Email;
            coach.Senha = coachDto.Senha;
            coach.Telefone = coachDto.Telefone;
            coach.Especialidade = coachDto.Especialidade;
            coach.Status = coachDto.Status;
            coach.SalarioAtual = coachDto.SalarioAtual;
            coach.UpdatedAt = DateTime.UtcNow;

            _dbContext.Coaches.Update(coach);
            _dbContext.SaveChanges();
        }

        public void InativarCoach(Coach coach)
        {
            coach.Status = StatusCoach.Inativo;
            coach.UpdatedAt = DateTime.UtcNow;

            _dbContext.Coaches.Update(coach);
            _dbContext.SaveChanges();
        }
    }
}
