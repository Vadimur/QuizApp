using System;

namespace QuizApp.Models
{
    public class Answer
    {
        public readonly int ParticipantId;
        public readonly int QuizId;
        public readonly int QuestionId;
        public readonly int AnswerId;
        public readonly int ElapsedSeconds;

        public Answer(int participantId, int quizId, int questionId, int answerId, int elapsedSecs)
        {
            ParticipantId = participantId;
            QuizId = quizId;
            QuestionId = questionId;
            AnswerId = answerId;
            ElapsedSeconds = elapsedSecs;
        }
    }
}