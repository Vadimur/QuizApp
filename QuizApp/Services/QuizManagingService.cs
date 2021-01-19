using System;
using System.Collections.Generic;
using QuizApp.DataAccess.Exceptions;
using QuizApp.DataAccess.Repositories;
using QuizApp.DataAccess.Repositories.Interfaces;
using QuizApp.Entities;
using QuizApp.Exceptions;
using QuizApp.Mappers;

namespace QuizApp.Services
{
    public class QuizCreatingService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly QuizMapper _mapper;

        public QuizCreatingService()
        {
            _quizRepository = new QuizRepository();
            _mapper = new QuizMapper();
        }
        public bool CreateQuiz(int ownerId, string name, string category)
        {
            try
            {
                return _quizRepository.Add(ownerId, name, category);
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }
        
        public IEnumerable<Quiz> GetQuizzesCreatedByCurrentUser(int ownerId)
        {
            try
            {
                return _mapper.MapManyFromDto(_quizRepository.FindByOwner(ownerId));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }
        public bool DeleteQuiz(int id)
        {
            try
            {
                return _quizRepository.Delete(id);
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }
    }
}