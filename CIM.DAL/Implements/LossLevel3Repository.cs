using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using CIM.DAL.Utility;

namespace CIM.DAL.Implements
{
    public class LossLevel3Repository : Repository<LossLevel3, LossLevel3Model>, ILossLevel3Repository
    {
        private IDirectSqlRepository _directSqlRepository;

        public LossLevel3Repository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration ) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
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

        public async Task<PagingModel<LossLevel3ListModel>> List(int page, int howmany, string keyword, bool isActive)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            { "@keyword", keyword },
                                            { "@is_active", isActive},
                                            { "@page", page},
                                            { "@howmany", howmany}
                                        };
                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListLossLevel3", parameterList);
                int totalCount;
                if (dt.Rows.Count == 0)
                {
                    totalCount = 0;
                }
                else
                {
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                }
                return ToPagingModel(dt.ToModel<LossLevel3ListModel>(), totalCount, page, howmany);
            });
        }
    }
}
