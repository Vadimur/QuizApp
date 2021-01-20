using System.Linq;
using QuizApp.DataAccess.Entities;
using QuizApp.DataAccess.Exceptions;
using QuizApp.DataAccess.Repositories;
using QuizApp.DataAccess.Repositories.Interfaces;
using QuizApp.Exceptions;
using QuizApp.Helpers;
using QuizApp.Mappers;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class AuthorizationService
    {
        private readonly IAccountRepository _accountsRepository;
        private readonly AccountMapper _mapper;

        public AuthorizationService()
        {
            _accountsRepository = AccountRepository.GetInstance();
            _mapper = new AccountMapper();
        }
        
        public Account Authorize(string username, string password)
        {
            if (username.Equals("admin") && password.Equals("admin"))
            {
                return new Account(-1, username, password.GetDeterministicHashCode(), Role.Admin);
            }

            try
            {
               
                Account account = _mapper.MapFromDto(_accountsRepository.GetAll().FirstOrDefault(a =>
                    a.Username.Equals(username)));
                
                if (account == null || !account.IsPasswordCorrect(password))
                    return null;
                
                return account;
               
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }

        public bool Register(string username, string password, Role role = Role.Participant)
        {
            if (username.Equals("admin"))
            {
                return false;
            }
            
            try
            {
                return _accountsRepository.Add(username, password.GetDeterministicHashCode(), (RoleEntity) role);
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
            
        }
        
    }
}