using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess;
using QuizApp.DataAccess.Entities;
using QuizApp.DataAccess.Exceptions;
using QuizApp.DataAccess.Repositories;
using QuizApp.DataAccess.Repositories.Interfaces;
using QuizApp.Entities;
using QuizApp.Exceptions;
using QuizApp.Mappers;

namespace QuizApp.Services
{
    public class AuthorizationService
    {
        private readonly IAccountRepository _accountsRepository;
        private readonly AccountMapper _mapper;

        public AuthorizationService()
        {
            _accountsRepository = new AccountRepository();
            _mapper = new AccountMapper();
        }
        
        public Account Authorize(string username, string password)
        {
            if (username.Equals("admin") && password.Equals("admin"))
            {
                return new Account(0, username, password.GetHashCode(), Role.Admin);
            }

            try
            {
                AccountEntity account = _accountsRepository.GetAll().FirstOrDefault(a =>
                    a.Username.Equals(username) && a.IsPasswordCorrect(password));
                return _mapper.MapFromDto(account);
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }

        public bool Register(string username, string password, Role role = Role.Participant)
        {
            try
            {
                return _accountsRepository.Add(username, password, (RoleEntity) role);
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
            
            /*List<Account> accounts;
            try
            {
                accounts = _mapper.MapManyFromDto(_accountsRepository.GetAll()).ToList();
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
            
            int accountId = 1;

            if (accounts.Count != 0)
            {
                accountId = accounts.Max(a => a.Id) + 1;
            }

            Account existingAccount = accounts.FirstOrDefault(a => a.Username.Equals(username));
            if (existingAccount != null)
            {
                return false;
            }
            Account newAccount = new Account(accountId, username, password.GetHashCode(), role);
            try
            {
                _accountsRepository.Add(_mapper.MapToDto(newAccount));
                _accountsRepository.Save();
                return true;
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }*/
            
        }
        
    }
}