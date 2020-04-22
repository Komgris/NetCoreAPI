using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.EntityFrameworkCore;
namespace CIM.DAL.Implements
{
    public class LossLevel3Repository : Repository<LossLevel3>, ILossLevel3Repository
    {
        private readonly string _connectionString;
        public LossLevel3Repository(cim_dbContext context) : base(context)
        {
            _connectionString = _entities.Database.GetDbConnection().ConnectionString;
        }

        public async Task<IList<LossLevelComponentMappingModel>> ListComponentMappingAsync()
        {
            IList<LossLevelComponentMappingModel> data = null;
            //If you get a Object reference not set to instance of object error then make sure your sp result column is not null
            var proc = _entities.LoadStoredProc("sp_ListComponentMappingAsync");
            await proc.ExecAsync(x => Task.Run(() => data = x.ToList<LossLevelComponentMappingModel>()));
            return data;

        }

        public async Task<IList<LossLevelMachineMappingModel>> ListMachineMappingAsync()
        {

            IList<LossLevelMachineMappingModel> data = null;
            //If you get a Object reference not set to instance of object error then make sure your sp result column is not null
            var proc = _entities.LoadStoredProc("sp_ListMachineMappingAsync");
            await proc.ExecAsync(x => Task.Run(() => data = x.ToList<LossLevelMachineMappingModel>()));
            return data;
        }

        //public async Task<PagingModel<LossLevel3ViewModel>> ListAsPaging(int page, int howmany, string keyword, bool isActive)
        //{
        //    List<LossLevel3ViewModel> data = null;
        //    var proc = _entities.LoadStoredProc("[sp_ListLossLevel3]");
        //    proc.AddParam("total_count", out IOutParam<int> totalCount);
        //    proc.AddParam("@keyword", keyword);
        //    proc.AddParam("@is_active", isActive);
        //    proc.AddParam("@howmany", howmany);
        //    proc.AddParam("@page", page);
        //    await proc.ExecAsync(x => Task.Run(() => data = x.ToList<LossLevel3ViewModel>()));
        //    return ToPagingModel(data, totalCount.Value, page, howmany);
        //}

        public async Task<PagingModel<LossLevel3ViewModel>> ListAsPaging(int page, int howmany, string keyword, bool isActive)
        {
            List<LossLevel3ViewModel> output = new List<LossLevel3ViewModel>();

            int total = 0;


            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    string sql = @"EXEC	[dbo].[sp_ListLossLevel3] @is_active = 1";
            //    var output = connection.QueryAsync<LossLevel3ViewModel>(sql).Result.ToList();
            //    Console.WriteLine(output.Count());
            //}

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    DynamicParameters parameter = new DynamicParameters();
            //    parameter.Add("@keyword", keyword, DbType.String, ParameterDirection.Input);
            //    parameter.Add("@is_active", isActive, DbType.Boolean, ParameterDirection.Input);
            //    parameter.Add("@page", page, DbType.Int16, ParameterDirection.Input);
            //    parameter.Add("@howmany", howmany, DbType.Int16, ParameterDirection.Input);
            //    string sql = @"sp_ListLossLevel3_cte";
            //    var result = connection.QueryAsync(sql, parameter, null, null, CommandType.StoredProcedure).Result.ToList();

            //    Console.WriteLine(output.Count());


            //    foreach (var item in result)
            //    {
            //        output.Add(new LossLevel3ViewModel
            //        {
            //            Id = Convert.ToInt16(item.Id),
            //            Name = Convert.ToString(item.Name),
            //            Description = Convert.ToString(item.Description),
            //            IsActive = Convert.ToBoolean(item.IsActive),
            //            LossLevel2Id = Convert.ToInt16(item.LossLevel2Id),
            //            LossLevel1Id = Convert.ToInt16(item.LossLevel1Id),
            //            LossLevel1Name = Convert.ToString(item.LossLevel1Name),
            //            LossLevel2Name = Convert.ToString(item.LossLevel2Name),

            //        });
            //    }
            //}

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@keyword", keyword, DbType.String, ParameterDirection.Input);
            parameter.Add("@is_active", isActive, DbType.Boolean, ParameterDirection.Input);
            parameter.Add("@page", page, DbType.Int16, ParameterDirection.Input);
            parameter.Add("@howmany", howmany, DbType.Int16, ParameterDirection.Input);
            string sql = @"sp_ListLossLevel3";
            List<SpListLossLevel3> result = await execStoreProcedure2<SpListLossLevel3>(sql, parameter);

            foreach (var item in result)
            {
                if (total == 0)
                {
                    total = Convert.ToInt16(item.TotalCount);
                }
                
                output.Add(new LossLevel3ViewModel
                {
                    Id = Convert.ToInt16(item.Id),
                    Name = Convert.ToString(item.Name),
                    Description = Convert.ToString(item.Description),
                    IsActive = Convert.ToBoolean(item.IsActive),
                    LossLevel2Id = Convert.ToInt16(item.LossLevel2_Id),
                    LossLevel1Id = Convert.ToInt16(item.LossLevel1_Id),
                    LossLevel1Name = Convert.ToString(item.LossLevel1Name),
                    LossLevel2Name = Convert.ToString(item.LossLevel2Name),
                });
            }

            
            return new PagingModel<LossLevel3ViewModel>
            {
                HowMany = total,
                Data = output
            };
        }
    }
}
