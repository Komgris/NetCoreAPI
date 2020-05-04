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
using System.Data;

namespace CIM.DAL.Implements
{
    public class ComponentTypeLossLevel3Repository : Repository<ComponentTypeLossLevel3>, IComponentTypeLossLevel3Repository
    {
        private IDirectSqlRepository _directSqlRepository;

        public ComponentTypeLossLevel3Repository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<ComponentTypeLossLevel3ListModel>> List(int? componentTypeId, int? lossLevel3Id, int page, int howmany)
        {
            //throw new NotImplementedException();
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            { "@ComponentTypeId", componentTypeId },
                                            { "@LossLevel3Id", lossLevel3Id},
                                            { "@page", page},
                                            { "@howmany", howmany}
                                        };


                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListComponentTypeLossLevel3", parameterList);
                int totalCount;
                if (dt.Rows.Count == 0)
                {
                    totalCount = 0;
                }
                else
                {
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                }
                return ToPagingModel(dt.ToModel<ComponentTypeLossLevel3ListModel>(), totalCount, page, howmany);
            });
        }
    }
}
