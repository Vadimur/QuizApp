using System.Collections.Generic;
using QuizApp.DataAccess.Entities;

namespace QuizApp.DataAccess.Repositories.Interfaces
{
    public interface IQuizRepository : IBaseRepository<QuizEntity>
    {
        bool Add(int ownerId, string name, string category);
        IEnumerable<QuizEntity> FindByOwner(int ownerId);
        bool Update(QuizEntity quizEntity);
        QuizEntity Find(int id);
        bool Delete(int id);
    }
}