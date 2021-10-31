using System;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        T Get(Guid id);
        T Create(T entity, bool saveChanges = true);
        void Update(T entity, bool saveChanges = true);
        void Delete(T entity, bool saveChanges = true);
    }
}
