using CIM.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public abstract class Repository<T> : IRepository<T>
         where T : class
    {
        protected DbContext _entities;
        protected readonly DbSet<T> _dbset;

        public Repository(DbContext context)
        {

            _entities = context;
            _dbset = context.Set<T>();
        }

        public virtual IQueryable<T> All()
        {
            var result = _dbset;
            return result;
        }

        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            List<T> query = await _dbset.Where(predicate).ToListAsync();
            return query;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbset.Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Any(predicate);
        }
    }
}
