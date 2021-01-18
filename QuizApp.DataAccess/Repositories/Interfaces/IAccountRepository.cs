using QuizApp.DataAccess.Entities;

namespace QuizApp.DataAccess.Repositories.Interfaces
{
    public interface IAccountRepository : IBaseRepository<AccountEntity>
    {
        bool Add(string username, string password, RoleEntity role);
    }
}