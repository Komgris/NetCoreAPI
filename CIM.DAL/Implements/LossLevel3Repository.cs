using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace CIM.DAL.Implements
{
    public class LossLevel3Repository : Repository<LossLevel3>, ILossLevel3Repository
    {
        public LossLevel3Repository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {
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

        public async Task<PagingModel<SpListLossLevel3>> List(int page, int howmany, string keyword, bool isActive)
        {

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

            //List<LossLevel3ViewModel> output = new List<LossLevel3ViewModel>();
            //foreach (var item in result)
            //{
            //    if (total == 0)
            //    {
            //        total = Convert.ToInt16(item.TotalCount);
            //    }
            //    //output.Add(new LossLevel3ViewModel
            //    //{
            //    //    Id = Convert.ToInt16(item.Id),
            //    //    Name = Convert.ToString(item.Name),
            //    //    Description = Convert.ToString(item.Description),
            //    //    IsActive = Convert.ToBoolean(item.IsActive),
            //    //    LossLevel2Id = Convert.ToInt16(item.LossLevel2_Id),
            //    //    LossLevel1Id = Convert.ToInt16(item.LossLevel1_Id),
            //    //    LossLevel1Name = Convert.ToString(item.LossLevel1Name),
            //    //    LossLevel2Name = Convert.ToString(item.LossLevel2Name),
            //    //});
            //}

            if (result.Count == 0)
            {
                return new PagingModel<SpListLossLevel3>
                {
                    HowMany = total,
                    Data = null
                };
            }

            total = Convert.ToInt16(result[0].TotalCount);
            return new PagingModel<SpListLossLevel3>
            {
                HowMany = total,
                Data = result
            };
        }

        public async Task<LossLevel3> Get(int id)
        {
            string sql = @" SELECT TOP 1 *
                            FROM          dbo.LossLevel3
                            WHERE		  dbo.LossLevel3.Id = @id;";

            Dictionary<string, object> dictParameter = new Dictionary<string, object>
            {
                { "@id", id }
            };
            List<LossLevel3> result = await Sql<LossLevel3>(sql, dictParameter);

            if (result.Count == 0)
            {
                return null;
            }
            return result[0];
        }
    }
}
