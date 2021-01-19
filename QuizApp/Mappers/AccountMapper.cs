using QuizApp.DataAccess.Entities;
using QuizApp.Models;

namespace QuizApp.Mappers
{
    public class AccountMapper : BaseMapper<AccountEntity, Account>
    {
        public override Account MapFromDto(AccountEntity item)
        {
            if (item == null)
                return null;
            Account account = new Account(item.Id, item.Username, item.PasswordHash, (Role)item.Role);
            return account;
        }

        public override AccountEntity MapToDto(Account item)
        {
            if (item == null)
                return null;
            
            AccountEntity accountEntity = new AccountEntity()
            {
                Id = item.Id,
                Username = item.Username,
                PasswordHash = item.PasswordHash,
                Role = (RoleEntity) item.Role
            };

            return accountEntity;
        }
    }
}