using System.Collections.Generic;
using System.Linq;
using QuizApp.DataAccess.Entities;
using QuizApp.Models;

namespace QuizApp.Mappers
{
    public class QuizMapper : BaseMapper<QuizEntity, Quiz>
    {
        private readonly QuestionMapper _questionMapper;

        public QuizMapper()
        {
            _questionMapper = new QuestionMapper();
        }
        public override Quiz MapFromDto(QuizEntity item)
        {
            if (item == null)
                return null;
            
            List<Question> questions = new List<Question>();
            if (item.Questions != null)
            {
                questions.AddRange(_questionMapper.MapManyFromDto(item.Questions));
            }

            Quiz newQuiz = new Quiz(item.Id, item.OwnerId, item.Name, item.Category, questions);
            return newQuiz;
        }

        public override QuizEntity MapToDto(Quiz item)
        {
            if (item == null)
                return null;

            List<QuestionEntity> questionEntities = new List<QuestionEntity>();
            if (item.Questions != null)
            {
                questionEntities.AddRange(_questionMapper.MapManyToDto(item.Questions));
            }

            QuizEntity newQuizEntity = new QuizEntity
            {
                Id = item.Id,
                Category = item.Category,
                Name = item.Name,
                OwnerId = item.OwnerId,
                Questions = questionEntities,
            };
            
            return newQuizEntity;
        }
    }
}