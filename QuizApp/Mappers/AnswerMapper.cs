using System;
using QuizApp.DataAccess.Entities;
using QuizApp.Models;

namespace QuizApp.Mappers
{
    public class AnswerMapper  : BaseMapper<AnswerEntity, Answer>
    {
        public override Answer MapFromDto(AnswerEntity item)
        {
            if (item == null)
                return null;
            
            Answer answer = new Answer(item.ParticipantId, item.QuizId, item.QuestionId, item.AnswerId, item.SecondsElapsed);
            return answer;
        }

        public override AnswerEntity MapToDto(Answer item)
        {
            if (item == null)
                return null;

            AnswerEntity answerEntity = new AnswerEntity
            {
                AnswerId = item.AnswerId,
                ParticipantId = item.ParticipantId,
                QuestionId = item.QuestionId,
                QuizId = item.QuizId,
                SecondsElapsed = item.ElapsedSeconds
            };
            return answerEntity;
        }
    }
}