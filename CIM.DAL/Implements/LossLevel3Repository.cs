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

        public async Task<PagingModel<LossLevel3ViewModel>> ListAsPaging(int page, int howmany, string keyword, bool isActive)
        {
            List<LossLevel3ViewModel> output = new List<LossLevel3ViewModel>();
            int total = 0;
            string sql = @"sp_ListLossLevel3";
            Dictionary<string, object> dictParameter = new Dictionary<string, object>
            {
                { "@keyword", keyword },
                { "@is_active", isActive},
                { "@page", page},
                { "@howmany", howmany}
            };

            List<SpListLossLevel3> result = await ExecStoreProcedure<SpListLossLevel3>(sql, dictParameter);
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

        public async Task<LossLevel3EditableModel> Get(int Id)
        {
            LossLevel3EditableModel output = new LossLevel3EditableModel();

            string sql = @" SELECT TOP 1 *
                            FROM          dbo.LossLevel3
                            WHERE		  dbo.LossLevel3.Id = @Id;";

            Dictionary<string, object> dictParameter = new Dictionary<string, object>
            {
                { "@Id", Id }
            };
            List<LossLevel3> result = await Sql<LossLevel3>(sql, dictParameter);

            //foreach (var item in result)
            //{
            //    output.Id = Convert.ToInt16(item.Id);
            //    output.Name = Convert.ToString(item.Name);
            //    output.Description = Convert.ToString(item.Description);
            //    output.IsActive = Convert.ToBoolean(item.IsActive);
            //    output.LossLevel2Id = Convert.ToInt16(item.LossLevel2Id);
            //}
            if (result.Count == 0)
            {
                //throw new System.Exception("Id not found");
                return null;
            }

            output.Id = Convert.ToInt16(result[0].Id);
            output.Name = Convert.ToString(result[0].Name);
            output.Description = Convert.ToString(result[0].Description);
            output.IsActive = Convert.ToBoolean(result[0].IsActive);
            output.LossLevel2Id = Convert.ToInt16(result[0].LossLevel2Id);

            return output;
        }
    }
}
