using System.Collections.Generic;

namespace QuizApp.Entities
{
    public class Question
    {
        public readonly int Id;
        public readonly int QuizId;
        public readonly string Content;
        public readonly List<string> Answers;
        public readonly int CorrectAnswerId;

        public Question(int id, int quizId, string content, List<string> answers, int correctAnswerId)
        {
            Id = id;
            QuizId = quizId;
            Content = content;
            Answers = answers;
            CorrectAnswerId = correctAnswerId;
        }
    }
}