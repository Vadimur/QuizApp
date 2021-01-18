using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess.Entities;
using QuizApp.DataAccess.Exceptions;
using QuizApp.DataAccess.Repositories.Interfaces;

namespace QuizApp.DataAccess.Repositories
{
    
    public class AccountRepository : BaseRepository<AccountEntity>, IAccountRepository
    {
        
        public AccountRepository() : base("accounts.json")
        {
            
        }
        
        public override AccountEntity Find(int id)
        {
            FetchItems();
            return Items.FirstOrDefault(a => a.Id == id);
        }

        public override void Delete(int id)
        {
            FetchItems();
            var account = Items.FirstOrDefault(a => a.Id == id);
            if (account == null)
                return;
            Items.Remove(account);
        }

        public bool Add(string username, string password, RoleEntity role)
        {
            FetchItems();
            int accountId = 1;

            if (Items.Count != 0)
            {
                accountId = Items.Max(a => a.Id) + 1;
            }

            AccountEntity existingAccount = Items.FirstOrDefault(a => a.Username.Equals(username));
            if (existingAccount != null)
            {
                return false;
            }
            AccountEntity newAccount = new AccountEntity(accountId, username, password, role);

            Items.Add(newAccount);
            Save();
            return true;
            
        }
    }
}