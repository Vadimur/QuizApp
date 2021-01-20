using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess.Entities;
using QuizApp.DataAccess.Exceptions;
using QuizApp.DataAccess.Repositories.Interfaces;

namespace QuizApp.DataAccess.Repositories
{
    
    public class AccountRepository : BaseRepository<AccountEntity>, IAccountRepository
    {
        private const string StoragePath = "Accounts.json";
        
        private static AccountRepository _instance;

        private AccountRepository(string path) : base(path)
        {
            
        }
        public static AccountRepository GetInstance()
        {
            return _instance ??= new AccountRepository(StoragePath);
        }
        
        public AccountEntity Find(int id)
        {
            FetchItems();
            return Items.FirstOrDefault(a => a.Id == id);
        }

        public bool Delete(int id)
        {
            FetchItems();
            var account = Items.FirstOrDefault(a => a.Id == id);
            if (account == null)
                return false;
            Items.Remove(account);
            SaveChanges();
            return true;
        }

        public bool Add(string username, int passwordHash, RoleEntity role)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;
            
            FetchItems();
            int accountId = 0;

            if (Items.Count != 0)
            {
                accountId = Items.Max(a => a.Id) + 1;
            }

            AccountEntity existingAccount = Items.FirstOrDefault(a => a.Username.Equals(username));
            if (existingAccount != null)
            {
                return false;
            }
            AccountEntity newAccount = new AccountEntity(accountId, username, passwordHash, role);

            Items.Add(newAccount);
            SaveChanges();
            return true;
            
        }
    }
}