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
    public class MachineTypeLossLevel3Repository : Repository<MachineTypeLossLevel3>, IMachineTypeLossLevel3Repository
    {
        private IDirectSqlRepository _directSqlRepository;

        public MachineTypeLossLevel3Repository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<MachineTypeLossLevel3ListModel>> List(int? machineTypeId, int? lossLevel3Id, int page, int howmany)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            { "@MachineTypeId", machineTypeId },
                                            { "@LossLevel3Id", lossLevel3Id},
                                            { "@page", page},
                                            { "@howmany", howmany}
                                        };


                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMachineTypeLossLevel3", parameterList);
                int totalCount;
                if (dt.Rows.Count == 0)
                {
                    totalCount = 0;
                }
                else
                {
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                }
                return ToPagingModel(dt.ToModel<MachineTypeLossLevel3ListModel>(), totalCount, page, howmany);
            });
        }
    }
}
