using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess;
using QuizApp.DataAccess.Exceptions;
using QuizApp.Entities;
using QuizApp.Exceptions;

namespace QuizApp.Services
{
    public class AuthorizationService
    {
        private readonly Serializer _serializer;
        private List<AccountModel> _accounts = new List<AccountModel>();
        private bool _areAccountsLoaded = false;
        private const string AccountsFilePath = "accounts.json";

        public AuthorizationService()
        {
            _serializer = new Serializer();
        }
        
        public Account Authorize(string username, string password)
        {
            FetchAccounts();

            AccountModel account = _accounts.FirstOrDefault(a =>
                a.Username.Equals(username) && a.IsPasswordCorrect(password));
            
            return account == null ? null : new Account(account.Id, account.Username, account.Role);
        }

        public bool Register(string username, string password, Role role = Role.Participant)
        {
            FetchAccounts();

            int accountId = 0;

            if (_accounts.Count != 0)
            {
                accountId = _accounts.Max(a => a.Id) + 1;
            }

            var existingAccount = _accounts.FirstOrDefault(a => a.Username.Equals(username));
            if (existingAccount != null)
            {
                return false;
            }
            _accounts.Add(new AccountModel(accountId, username, password, Role.Participant));
            
            _serializer.Serialize(AccountsFilePath, _accounts);
            return true;
        }

        private void FetchAccounts()
        {
            if (_areAccountsLoaded) return;
            
            try
            {
                _accounts = _serializer.Deserialize<List<AccountModel>>(AccountsFilePath);
                if (_accounts == null)
                {
                    throw new ServiceException("An error occurred. Please try again later");
                }
                _areAccountsLoaded = true;
            }
            catch (SerializerException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }

    }
}