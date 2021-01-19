using QuizApp.DataAccess.Entities;
using QuizApp.Models;

namespace QuizApp.Mappers
{
    public class QuestionMapper : BaseMapper<QuestionEntity, Question>
    {
        public override Question MapFromDto(QuestionEntity item)
        {
            if (item == null)
                return null;
            
            Question question = new Question(item.Id, item.QuizId, item.Content, item.Options, item.CorrectOptionIndex);
            return question;
        }

        public override QuestionEntity MapToDto(Question item)
        {
            if (item == null)
                return null;
            
            QuestionEntity questionEntity = new QuestionEntity
            {
                Id = item.Id,
                QuizId = item.QuizId,
                Content = item.Content,
                Options = item.Options,
                CorrectOptionIndex = item.CorrectOptionIndex
            };
            
            return questionEntity;
        }
    }
}