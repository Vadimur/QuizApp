using QuizApp.DataAccess.Entities;

namespace QuizApp.DataAccess.Repositories.Interfaces
{
    public interface IAccountRepository : IBaseRepository<AccountEntity>
    {
        bool Add(string username, int passwordHash, RoleEntity role);
        AccountEntity Find(int id);
        bool Delete(int id);
    }
}