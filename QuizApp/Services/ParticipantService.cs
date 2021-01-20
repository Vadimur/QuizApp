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
    public class ParticipantService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly AnswerMapper _answerMapper;

        public ParticipantService()
        {
            _answerRepository = AnswerRepository.GetInstance();
            _answerMapper = new AnswerMapper();
        }

        public IEnumerable<Question> GetQuestionsForParticipant(Quiz quiz, int participantId)
        {
            if (quiz == null || quiz.Questions == null)
            {
                return null;
            }

            IEnumerable<int> answeredQuestionsId;
            try
            {
                answeredQuestionsId = _answerRepository.GetAnsweredQuestionIds(quiz.Id, participantId);
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            } 
            var questions = quiz.Questions.Where(q => answeredQuestionsId.Contains(q.Id) == false).ToList();

            return questions;
        }

        public void SaveAnswer(int participantId, Question question, int participantAnswerId, TimeSpan timeElapsed)
        {
            Answer newAnswer = new Answer(participantId, question.QuizId, question.Id, participantAnswerId, (int)timeElapsed.TotalSeconds);
            try
            {
                _answerRepository.Add(_answerMapper.MapToDto(newAnswer));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
        }
        
    }
}