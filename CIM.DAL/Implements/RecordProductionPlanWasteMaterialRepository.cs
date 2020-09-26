using CIM.DAL.Interfaces;
using CIM.DAL.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanWasteMaterialRepository : Repository<RecordProductionPlanWasteMaterials, object>, IRecordProductionPlanWasteMaterialRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public RecordProductionPlanWasteMaterialRepository(cim_3m_1Context context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<List<RecordProductionPlanWasteMaterialModel>> ListByLoss(int recordManufacturingLossId)
        {
            return null;
                //await _entities.RecordProductionPlanWasteMaterials.Where(x => x.Waste.RecordManufacturingLossId == recordManufacturingLossId && x.Waste.IsDelete == false)
                //.Select(x => new RecordProductionPlanWasteMaterialModel
                //{
                //    Amount = x.Amount,
                //    Id = x.Id,
                //    MaterialId = x.MaterialId,
                //    WasteId = x.WasteId,
                //}).ToListAsync();

        }

        public async Task<PagingModel<RecordProductionPlanWasteNonePrimeModel>> NonePrimeOutputList(string keyword, int page, int howmany)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howmany},
                                            { "@page", page}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListNonPrimeOutput", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<RecordProductionPlanWasteNonePrimeModel>(), totalCount, page, howmany);
            });
        }

        public async Task<PagingModel<RecordProductionPlanWasteNonePrimeModel>> NonePrimeOutputListByDate(DateTime date, int page, int howmany)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@date", date},
                                            {"@howmany", howmany},
                                            { "@page", page}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListNonPrimeOutputByDate", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<RecordProductionPlanWasteNonePrimeModel>(), totalCount, page, howmany);
            });
        }

        public async Task<List<RecordProductionPlanWasteNonePrimeModel>> NonePrimeOutputListByMonth(int month, int year)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@month", month},
                                            {"@year", year}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListNonPrimeOutputByMonth", parameterList);

                return (dt.ToModel<RecordProductionPlanWasteNonePrimeModel>());
            });
        }
    }
}
