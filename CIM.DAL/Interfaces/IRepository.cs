using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();
        Task<IList<T>> AllAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);
    }
}
