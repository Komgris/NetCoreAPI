using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class LossLevel3Repository : Repository<LossLevel3>, ILossLevel3Repository
    {
        public LossLevel3Repository(cim_dbContext context) : base(context)
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

        public async Task<PagingModel<LossLevel3ViewModel>> ListAsPaging(int page, int howmany, string keyword, bool isActive)
        {
            List<LossLevel3ViewModel> data = null;
            var proc = _entities.LoadStoredProc("[sp_ListLossLevel3]");
            proc.AddParam("total_count", out IOutParam<int> totalCount);
            proc.AddParam("@keyword", keyword);
            proc.AddParam("@is_active", isActive);
            proc.AddParam("@howmany", howmany);
            proc.AddParam("@page", page);
            await proc.ExecAsync(x => Task.Run(() => data = x.ToList<LossLevel3ViewModel>()));
            return ToPagingModel(data, totalCount.Value, page, howmany);
        }

    }
}
