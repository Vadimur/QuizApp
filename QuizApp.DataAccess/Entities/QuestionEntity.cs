using System.Collections.Generic;

namespace QuizApp.DataAccess.Entities
{
    public class QuestionEntity
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Content { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswerId { get; set; }
        
    }
}