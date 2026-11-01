using API_LCKR.Models;

namespace API_LCKR.Repository
{
    public class UserRepository
    {
        private readonly LckrDatabaseContext _dbContext;

        public UserRepository(LckrDatabaseContext context)
        {
            _dbContext = context;
        }

        public User? FindUserEmail(string email)
        {
            var database = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            return database;
        }
    }
}
