using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {
        private readonly CampaignDbContext _context;
        public GenericRepository(CampaignDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public TEntity Get(Guid id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity Create(TEntity entity, bool saveChanges = true)
        {
            entity.RealCreationTime = DateTime.Now;
            _context.Set<TEntity>().Add(entity);
            if (saveChanges)
                _context.SaveChanges();
            return entity;
        }

        public void Update(TEntity entity, bool saveChanges = true)
        {
            _context.Set<TEntity>().Update(entity);
            if (saveChanges)
                _context.SaveChanges();
        }

        public void Delete(TEntity entity, bool saveChanges = true)
        {
            _context.Set<TEntity>().Remove(entity); 
            if (saveChanges)
                _context.SaveChanges();
        }
    }
}
