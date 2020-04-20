using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
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
        protected cim_dbContext _entities;
        protected readonly DbSet<T> _dbset;

        public Repository(cim_dbContext context)
        {

            _entities = context;
            _dbset = context.Set<T>();
        }

        public virtual IQueryable<T> All()
        {
            var result = _dbset;
            return result;
        }

        public async Task<IList<T>> AllAsync()
        {
            var result = await _dbset.ToListAsync();
            return result;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            T query = await _dbset.FirstOrDefaultAsync(predicate);
            return query;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbset.Where(predicate);
            return query;
        }
        public async Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            IList<T> query = await _dbset.Where(predicate).ToListAsync();
            return query;
        }


        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void Edit(T entity)
        {
            _dbset.Attach(entity);
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

        public async Task<PagingModel<T>> ToPagingModelAsync<T>(IQueryable<T> sqlQuery, int page, int howmany)
        where T : new()
        {
            var output = new PagingModel<T>();
            output.Total = await sqlQuery.CountAsync();
            output.HowMany = howmany;
            output.Page = page;
            output.NextPage = page + 1;
            output.PreviousPage = page - 1;
            output.PreviousPage = output.PreviousPage < 0 ? 0 : output.PreviousPage;
            var lastPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((output.Total / howmany))));
            output.NextPage = output.NextPage > lastPage ? lastPage : output.NextPage;
            var skip = (page - 1) * howmany;
            output.Data = await sqlQuery.Skip(skip).Take(howmany).ToListAsync();
            return output;
        }

        public PagingModel<T> ToPagingModel<T>(List<T> data, int total, int page, int howmany)
        where T : new()
        {
            var output = new PagingModel<T>();
            output.Total = total;
            output.HowMany = howmany;
            output.Page = page;
            output.NextPage = page + 1;
            output.PreviousPage = page - 1;
            output.PreviousPage = output.PreviousPage < 0 ? 0 : output.PreviousPage;
            var lastPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((output.Total / howmany))));
            output.NextPage = output.NextPage > lastPage ? lastPage : output.NextPage;
            var skip = (page - 1) * howmany;
            output.Data = data;
            return output;
        }
    }
}
