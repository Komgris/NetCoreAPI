using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
    }
}
