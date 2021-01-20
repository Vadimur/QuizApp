using System.Collections.Generic;
using QuizApp.DataAccess.Exceptions;
using QuizApp.DataAccess.Repositories.Interfaces;

namespace QuizApp.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T: class 
    {
        private readonly Serializer _serializer;
        protected List<T> Items = new List<T>();
        private bool _areItemsLoaded;
        private readonly string _storagePath;
        
        protected BaseRepository(string storagePath)
        {
            _storagePath = storagePath;
            _serializer = new Serializer();
        }

        public virtual void Add(T item)
        {
            FetchItems();
            Items.Add(item);
            SaveChanges();
        }

        public virtual IEnumerable<T> GetAll()
        {
            FetchItems();
            return Items;
        }

        protected void SaveChanges()
        {
            FetchItems(); 
            _serializer.Serialize(_storagePath, Items);
        }

        protected void FetchItems()
        {
            if (_areItemsLoaded) return;
            
            try
            {
                Items = _serializer.Deserialize<List<T>>(_storagePath);
                if (Items == null)
                {
                    throw new DataAccessException("An error occurred. Please try again later");
                }
                _areItemsLoaded = true;
            }
            catch (SerializerException exception)
            {
                throw new DataAccessException("An error occurred. Please try again later", exception);
            }
        }
    }
}