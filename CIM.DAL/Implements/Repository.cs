using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

namespace CIM.DAL.Implements
{
    public abstract class Repository<T> : IRepository<T>
         where T : class
    {
        protected cim_dbContext _entities;
        protected readonly DbSet<T> _dbset;
        private readonly string _connectionString;

        public Repository(cim_dbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
            _connectionString = _entities.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<T>> Sql<T>(string sql, Dictionary<string, object> parameterDic)
        {
            var parameters = new List<DynamicParameters>();
            foreach (var item in parameterDic)
            {
                var p = new DynamicParameters();
                p.Add(item.Key, item.Value);
                parameters.Add(p);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var output = await connection.QueryAsync<T>(sql, parameters);
                return output.ToList();
            }
        }

        public async Task<List<T>> execStoreProcedure<T>(string storeProcedureName, Dictionary<string, object> parameterDic)
        {
            var parameters = new List<DynamicParameters>();
            foreach (var item in parameterDic)
            {
                var p = new DynamicParameters();
                p.Add(item.Key, item.Value);
                parameters.Add(p);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var output = await connection.QueryAsync<T>(storeProcedureName, parameters, null, null, CommandType.StoredProcedure);
                return output.ToList();
            }
        }

        //to do it not complete need to p'ton fix for output data table
        //public async Task<IEnumerable<DataTable>> execStoreProcedureDataTable(string storeProcedureName, Dictionary<string, object> parameterDic)
        //{
        //    var parameters = new List<DynamicParameters>();
        //    foreach (var item in parameterDic)
        //    {
        //        var p = new DynamicParameters();
        //        p.Add(item.Key, item.Value);
        //        parameters.Add(p);
        //    }
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        var output = await connection.QueryAsync<DataTable>(storeProcedureName, parameters, null, null, CommandType.StoredProcedure);
        //        return output;
        //    }
        //}

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

    public static class Extension
    {
        public static List<T> ToModel<T>(this DataTable dt)
        {
            List<string> columns = (from DataColumn dc in dt.Columns select dc.ColumnName).ToList();

            var fields = typeof(T).GetFields();
            var properties = typeof(T).GetProperties();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields.Where(fieldInfo => columns.Contains(fieldInfo.Name)))
                {
                    fieldInfo.SetValue(ob, !dr.IsNull(fieldInfo.Name) ? dr[fieldInfo.Name] : fieldInfo.FieldType.IsValueType ? Activator.CreateInstance(fieldInfo.FieldType) : null);
                }

                foreach (var propertyInfo in properties.Where(propertyInfo => columns.Contains(propertyInfo.Name)))
                {
                    propertyInfo.SetValue(ob, !dr.IsNull(propertyInfo.Name) ? dr[propertyInfo.Name] : propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null);
                }

                lst.Add(ob);
            }

            return lst;
        }
    }
}
