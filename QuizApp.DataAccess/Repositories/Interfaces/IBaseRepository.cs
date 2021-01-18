using System.Collections.Generic;

namespace QuizApp.DataAccess.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T: class
    {
        void Add(T item);
        IEnumerable<T> GetAll();
        T Find(int id);
        void Delete(int id);
        void Save();

    }
}