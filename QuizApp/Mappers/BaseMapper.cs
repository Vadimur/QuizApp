using System.Collections.Generic;
using System.Linq;

namespace QuizApp.Mappers
{
    public abstract class BaseMapper<TEntity, T>
    {
        public abstract T MapFromDto(TEntity item);
        
        public abstract TEntity MapToDto(T item);
        
        public virtual IEnumerable<T> MapManyFromDto(IEnumerable<TEntity> items)
        {
            if (items == null | !items.Any())
                return new List<T>();
            
            return items.Select(MapFromDto).ToList();
        }
        
        public virtual IEnumerable<TEntity> MapManyToDto(IEnumerable<T> items)
        {
            if (items == null | !items.Any())
                return new List<TEntity>();
            
            return items.Select(MapToDto).ToList();
        }
        
    }
}