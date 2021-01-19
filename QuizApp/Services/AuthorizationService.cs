using System.Linq;
using QuizApp.DataAccess.Entities;
using QuizApp.DataAccess.Exceptions;
using QuizApp.DataAccess.Repositories;
using QuizApp.DataAccess.Repositories.Interfaces;
using QuizApp.Exceptions;
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
            
        }
        
    }
}