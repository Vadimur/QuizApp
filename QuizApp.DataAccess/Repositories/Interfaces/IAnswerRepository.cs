using System.Collections.Generic;
using QuizApp.DataAccess.Entities;

namespace QuizApp.DataAccess.Repositories.Interfaces
{
    public interface IAnswerRepository : IBaseRepository<AnswerEntity>
    {
        IEnumerable<AnswerEntity> GetAnswersForQuizQuestions(int quizId);
        AnswerEntity Find(int quizId, int questionId, int userId);
        IEnumerable<int> GetAnsweredQuestionIds(int quizId, int participantId);
        IEnumerable<AnswerEntity> GetQuizParticipantAnswers(int quizId, int participantId);
        bool DeleteAnswersOnQuestion(int quizId, int questionId);
        bool DeleteAnswersOnQuiz(int quizId);

    }
}