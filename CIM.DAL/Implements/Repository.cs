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
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using CIM.DAL.Utility;

namespace CIM.DAL.Implements
{
    public abstract class Repository<T, TModel> : IRepository<T, TModel>
         where T : class
         where TModel : class, new()
    {
        protected cim_dbContext _entities;
        protected readonly DbSet<T> _dbset;
        private readonly IConfiguration _configuration;

        public Repository(
            cim_dbContext context,
            IConfiguration configuration
        )
        {
            _entities = context;
            _dbset = context.Set<T>();
            _configuration = configuration;
        }

        public async Task<List<T>> Sql<T>(string sql, Dictionary<string, object> parameterDic)
        {
            var parameters = new DynamicParameters();
            foreach (var item in parameterDic)
            {
                parameters.Add(item.Key, item.Value);
            }

            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (var connection = new SqlConnection(connectionString))
            {
                var output = await connection.QueryAsync<T>(sql, parameters);
                return output.ToList();
            }
        }

        public async Task<List<T>> ExecStoreProcedure<T>(string storeProcedureName, Dictionary<string, object> parameterDic)
        {
            var parameters = new DynamicParameters();
            foreach (var item in parameterDic)
            {
                parameters.Add(item.Key, item.Value);
            }

            var connectionString = _configuration.GetConnectionString("CIMDatabase");

            using (var connection = new SqlConnection(connectionString))
            {
                var output = await connection.QueryAsync<T>(storeProcedureName, parameters, null, null, CommandType.StoredProcedure);
                return output.ToList();
            }
        }

        public async Task<PagingModel<TModel>> ListAsPaging(string storeProcedureName, Dictionary<string, object> parameterList, int page, int howMany)
        {
            return await Task.Run(() =>
            {
                var dt = ExecuteSPWithQuery(storeProcedureName, parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<TModel>(), totalCount, page, howMany);
            });
        }
        public async Task<List<TModel>> List(string storeProcedureName, Dictionary<string, object> parameterList)
        {
            return await Task.Run(() =>
            {
                var dt = ExecuteSPWithQuery(storeProcedureName, parameterList);

                return dt.ToModel<TModel>();
            });
        }

        public DataTable ExecuteSPWithQuery(string sql, Dictionary<string, object> parameters)
        {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                        foreach (var p in parameters)
                            if (p.Value != null) command.Parameters.AddWithValue(p.Key, p.Value);

                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());

                    return dt;
                }
            }
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
            var partial = ((decimal)output.Total / howmany);
            var ceiling = Math.Ceiling(Convert.ToDecimal(partial));
            output.LastPage = Convert.ToInt32(ceiling);
            output.Data = data;
            output.ShowNext = output.NextPage <= output.LastPage;
            output.ShowPrevious = output.PreviousPage > 0;
            return output;
        }

    }

}
