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

        public async Task<PagingModel<Domain.Models.LossLevel3ListModel>> List(int page, int howmany, string keyword)
        {

            int total = 0;
            string sql = @"sp_ListLossLevel3";
            Dictionary<string, object> dictParameter = new Dictionary<string, object>
            {
                { "@keyword", keyword },
                { "@page", page},
                { "@howmany", howmany}
            };

            List<Domain.Models.LossLevel3ListModel> result = await ExecStoreProcedure<Domain.Models.LossLevel3ListModel>(sql, dictParameter);
            return new PagingModel<Domain.Models.LossLevel3ListModel>
            {
                HowMany = total,
                Data = result
            };
        }
    }
}
