using System.Collections.Generic;

namespace QuizApp.DataAccess.Entities
{
    public class QuizEntity
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public List<QuestionEntity> Questions { get; set; }
        public string Category { get; set; } 
        
    }
}