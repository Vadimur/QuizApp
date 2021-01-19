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
    public class QuizEditingService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly QuizMapper _quizMapper;
        private readonly QuestionMapper _questionMapper;

        public QuizEditingService()
        {
            _quizRepository = QuizRepository.GetInstance();
            _quizMapper = new QuizMapper();
            _questionMapper = new QuestionMapper();
        }
        public bool AddQuestion(Quiz quiz, string question, IEnumerable<string> options, int correctOptionId)
        {
            if (quiz == null || quiz.Questions == null)
            {
                return false;
            }
            int questionId = 0;
            if (quiz.Questions.Count != 0)
            {
                questionId = quiz.Questions.Max(q => q.Id) + 1;
            }
            
            int correctOptionIndex = correctOptionId - 1;
            Question newQuestion = new Question(questionId, quiz.Id, question, options.ToList(), correctOptionIndex);
            quiz.Questions.Add(newQuestion);
            
            try
            {
                return _quizRepository.Update(_quizMapper.MapToDto(quiz));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
            
        }

        public bool DeleteQuestion(Quiz quiz, int questionId)
        {
            if (quiz == null || quiz.Questions == null || quiz.Questions.Count == 0)
                return false;

            Question questionForDeleting = quiz.Questions.FirstOrDefault(q => q.Id == questionId);
            if (questionForDeleting == null)
                return false;
            
            bool result = quiz.Questions.Remove(questionForDeleting);
            if (result == false)
                return false;
            
            try
            {
                return _quizRepository.Update(_quizMapper.MapToDto(quiz));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }
    }
}