using System.Collections.Generic;

namespace QuizApp.Entities
{
    public class Quiz
    {
        public readonly int Id;
        public readonly int OwnerId;
        public readonly string Name;
        public readonly List<Question> Questions;
        public readonly string Category;

        public Quiz(int id, int ownerId, string name, string category)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            Questions = new List<Question>();
            Category = category;
        }

        public bool AddQuestion(string content, List<string> answers, int rightAnswerId)
        {
            if (string.IsNullOrEmpty(content) || answers == null || answers.Count < 2 || rightAnswerId < 0 || rightAnswerId > answers.Count - 1)
            {
                return false;
            }
            Questions.Add(new Question(Questions.Count + 1, Id, content, answers, rightAnswerId));
            return true;
        }
    }
}