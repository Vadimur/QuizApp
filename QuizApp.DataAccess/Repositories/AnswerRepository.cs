using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess.Entities;
using QuizApp.DataAccess.Repositories.Interfaces;

namespace QuizApp.DataAccess.Repositories
{
    public class AnswerRepository  : BaseRepository<AnswerEntity>, IAnswerRepository
    {
        private const string StoragePath = "AnswersStorage.json";
        private static AnswerRepository _instance;

        private AnswerRepository(string storagePath) : base(storagePath)
        {
        }

        public static AnswerRepository GetInstance()
        {
            return _instance ??= new AnswerRepository(StoragePath);
        }
        
        public AnswerEntity Find(int quizId, int questionId, int userId)
        {
            FetchItems();

            return Items.FirstOrDefault(a =>
                a.QuizId == quizId && a.QuestionId == questionId && a.ParticipantId == userId);
        }

        public IEnumerable<int> GetAnsweredQuestionIds(int quizId, int participantId)
        {
            FetchItems();

            IEnumerable<int> answeredQuestionsId =
                Items
                    .Where(a => a.QuizId == quizId && a.ParticipantId == participantId)
                    .Select(a => a.QuestionId);
            
            return answeredQuestionsId;
        }
        
        public IEnumerable<AnswerEntity> GetAnswersForQuizQuestions(int quizId)
        {
            FetchItems();
            return Items.Where(a => a.QuizId == quizId).ToList();
        }

        public IEnumerable<AnswerEntity> GetQuizParticipantAnswers(int quizId, int participantId)
        {
            FetchItems();
            return Items.Where(a => a.QuizId == quizId && a.ParticipantId == participantId).ToList();
        }
        
        public bool DeleteAnswersOnQuestion(int quizId, int questionId)
        {
            FetchItems();
            List<AnswerEntity> answers = Items.Where(a => a.QuizId == quizId && a.QuestionId == questionId).ToList();
            if (!answers.Any())
                return true;

            bool isSuccess = true;
            foreach (var answerEntity in answers)
            {
                isSuccess = isSuccess && Items.Remove(answerEntity);
            }
            
            SaveChanges();
            return isSuccess;
        }

        public bool DeleteAnswersOnQuiz(int quizId)
        {
            FetchItems();
            List<AnswerEntity> answers = Items.Where(a => a.QuizId == quizId).ToList();
            if (!answers.Any())
                return true;

            bool isSuccess = true;
            foreach (var answerEntity in answers)
            {
                isSuccess = isSuccess && Items.Remove(answerEntity);
            }
            
            SaveChanges();
            return isSuccess;
        }

    }
}