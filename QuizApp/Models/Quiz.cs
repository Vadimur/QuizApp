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

        public Quiz(int id, int ownerId, string name, string category, List<Question> questions = null)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            Questions = questions ?? new List<Question>();
            Category = category;
        }

        public override string ToString()
        {
            return $"Quiz #{Id}. {Name} | {Category} | Questions: {Questions.Count}";
        }

    }
}