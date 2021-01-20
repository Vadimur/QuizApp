using System;
using System.Collections.Generic;
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
    public class StatisticsService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly AnswerMapper _answerMapper;
        private readonly QuizMapper _quizMapper;
        
        public StatisticsService()
        {
            _answerRepository = AnswerRepository.GetInstance();
            _quizRepository = QuizRepository.GetInstance();
            _answerMapper = new AnswerMapper();
            _quizMapper = new QuizMapper();
            
        }

        public IEnumerable<Statistics<Question>> GetQuestionStatistics(int quizId)
        {
            Quiz selectedQuiz;
            try
            {
                selectedQuiz = _quizMapper.MapFromDto(_quizRepository.Find(quizId));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            } 
            
            if (selectedQuiz == null)
                return null;
            IEnumerable<Answer> answers;
            try
            {
                answers = _answerMapper.MapManyFromDto(_answerRepository.GetAnswersForQuizQuestions(selectedQuiz.Id));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            } 
            if (answers == null || !answers.Any())
                return null;
            
            List<Statistics<Question>> questionStats = new List<Statistics<Question>>();
            
            
            foreach (var question in selectedQuiz.Questions)
            {
                int totalAnswers = answers.Count(a => a.QuestionId == question.Id);
                int correctAnswers = answers.Count(a =>
                    a.QuestionId == question.Id && a.AnswerId == question.CorrectOptionIndex);
                int averageSecs = 0;
                if (totalAnswers != 0)
                {
                    averageSecs = (int) answers.Where(a => a.QuestionId == question.Id).Average(a => a.ElapsedSeconds);
                }
                questionStats.Add(new Statistics<Question>(question, totalAnswers, correctAnswers, averageSecs));
            }

            return questionStats;
        }
        
        public Statistics<Quiz> GetQuizStatistics(int quizId)
        {
            Quiz selectedQuiz;
            try
            {
                selectedQuiz = _quizMapper.MapFromDto(_quizRepository.Find(quizId));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            } 
            
            if (selectedQuiz == null)
                return null;
            
            IEnumerable<Answer> answers;
            try
            {
                answers = _answerMapper.MapManyFromDto(_answerRepository.GetAnswersForQuizQuestions(selectedQuiz.Id));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            }
            
            if (answers == null || !answers.Any())
                return null;
            
            int totalAnswers = answers.Count();
            int correctAnswers = 0;
            int totalSecs = 0;
            foreach (var question in selectedQuiz.Questions)
            {
                correctAnswers += answers.Count(a =>
                    a.QuestionId == question.Id && a.AnswerId == question.CorrectOptionIndex);
                if (totalAnswers != 0)
                    totalSecs += answers.Where(a => a.QuestionId == question.Id).Sum(a => a.ElapsedSeconds);
            }
            
            int avgSecsOnQuestion;
            if (totalSecs == 0)
            {
                avgSecsOnQuestion = 0;
            }
            else
            {
                avgSecsOnQuestion = totalSecs / totalAnswers;
            }

            Statistics<Quiz> quizStats = new Statistics<Quiz>(selectedQuiz, totalAnswers, correctAnswers, avgSecsOnQuestion);

            return quizStats;
        }
        
        public Statistics<Quiz> GetQuizStatisticsForParticipant(int quizId, int participantId)
        {
            Quiz selectedQuiz;
            try
            {
                selectedQuiz = _quizMapper.MapFromDto(_quizRepository.Find(quizId));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            } 
            
            if (selectedQuiz == null)
                return null;
            
            IEnumerable<Answer> answers;
            try
            {
                answers = _answerMapper.MapManyFromDto(_answerRepository.GetQuizParticipantAnswers(quizId, participantId));
            }
            catch (DataAccessException exception)
            {
                throw new ServiceException("An error occurred. Please try again later", exception);
            } 
            if (answers == null || !answers.Any())
                return null;
            
            int totalAnswers = answers.Count();
            int correctAnswers = 0;
            int totalSecs = 0;
            foreach (var question in selectedQuiz.Questions)
            {
                correctAnswers += answers.Count(a =>
                    a.QuestionId == question.Id && a.AnswerId == question.CorrectOptionIndex && a.ParticipantId == participantId);
                IEnumerable<Answer> questionAnswers = answers.Where(a => a.QuestionId == question.Id);
                if (questionAnswers.Any())
                {
                    totalSecs += questionAnswers.Sum(a => a.ElapsedSeconds);
                }
            }
            
            int avgSecsOnQuestion = totalSecs / totalAnswers;
            Statistics<Quiz> quizStats = new Statistics<Quiz>(selectedQuiz, totalAnswers, correctAnswers, avgSecsOnQuestion);
            return quizStats;
        }
    }
}