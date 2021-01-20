using System;
using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess.Exceptions;
using QuizApp.DataAccess.Repositories;
using QuizApp.DataAccess.Repositories.Interfaces;
using QuizApp.Exceptions;
using QuizApp.Mappers;
using QuizApp.Models;

namespace QuizApp.Services
{
    public class QuizManagingService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly QuizMapper _mapper;

        public QuizManagingService()
        {
            _quizRepository = QuizRepository.GetInstance();
            _answerRepository = AnswerRepository.GetInstance();
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

        public IEnumerable<Quiz> GetAllQuizzes()
        {
            try
            {
                var result = _mapper.MapManyFromDto(_quizRepository.GetAll());
                if (result == null || !result.Any())
                {
                    return null;
                }

                return result.OrderBy(q => q.Id);
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
                var result = _mapper.MapManyFromDto(_quizRepository.FindByOwner(ownerId));
                if (result == null || !result.Any())
                {
                    return null;
                }
                return result.OrderBy(q => q.Id);
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
                bool isSuccessAnswerDeletion = _answerRepository.DeleteAnswersOnQuiz(id);
                bool isSuccessQuizDeletion = _quizRepository.Delete(id);
                return isSuccessAnswerDeletion && isSuccessQuizDeletion;
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }

        public Quiz FindQuiz(int id)
        {
            try
            {
                return _mapper.MapFromDto(_quizRepository.Find(id));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }
    }
}