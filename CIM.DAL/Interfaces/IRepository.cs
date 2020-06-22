using CIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRepository<T, TModel> 
        where T : class
        where TModel : new()
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

        Task<PagingModel<TModel>> ListAsPaging(string storeProcedureName, Dictionary<string, object> parameterList, int page, int howMany);

    }
}
