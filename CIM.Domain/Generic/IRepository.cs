using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIM.Domain.Interface.Generic
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        Task<List<T>> WhereAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    }
}
