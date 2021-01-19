using System.Collections.Generic;

namespace QuizApp.Models
{
    public class Question
    {
        public readonly int Id;
        public readonly int QuizId;
        public readonly string Content;
        public readonly List<string> Options;
        public readonly int CorrectOptionIndex;

        public Question(int id, int quizId, string content, List<string> options, int correctOptionIndex)
        {
            Id = id;
            QuizId = quizId;
            Content = content;
            Options = options;
            CorrectOptionIndex = correctOptionIndex;
        }

        public override string ToString()
        {
            string options = string.Empty;

            for (int i = 0; i < Options.Count; i++)
            {
                options += $"#{i+1}. {Options[i]}\n";
            }

            return $"Question #{Id}\n" +
                   $"{Content}\n" +
                   $"{options}";
        }
    }
}