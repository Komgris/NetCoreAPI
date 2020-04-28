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
    public class LossLevel1Repository : Repository<LossLevel1>, ILossLevel1Repository
    {
        private IDirectSqlRepository _directSqlRepository;

        public LossLevel1Repository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<LossLevel1Model>> List(int page, int howmany, string keyword, bool isActive)
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
                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListLossLevel1", parameterList);
                int totalCount;
                if (dt.Rows.Count == 0)
                {
                    totalCount = 0;
                }
                else
                {
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                }
                return ToPagingModel(dt.ToModel<LossLevel1Model>(), totalCount, page, howmany);
            });
        }
    }
}
