using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RecordMaintenancePlanService : BaseService, IRecordMaintenancePlanService
    {
        private readonly IResponseCacheService _responseCacheService;
        private readonly IRecordMaintenancePlanRepository _recordMaintenancePlanRepositiry;

        public RecordMaintenancePlanService(
            IResponseCacheService responseCacheService,
            IRecordMaintenancePlanRepository recordMaintenancePlanRepository
            )
        {
            _responseCacheService = responseCacheService;
            _recordMaintenancePlanRepositiry = recordMaintenancePlanRepository;
        }

        public async Task<List<RecordMaintenancePlanModel>> ListByMonth(int month, int year)
        {
            var output = await _recordMaintenancePlanRepositiry.List("sp_ListRecordMaintenancePlanByMonth", new Dictionary<string, object>()
                {
                    {"@month", month},
                    {"@year", year}
                });
            return output;
        }
    }
}
