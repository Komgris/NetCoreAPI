using CIM.DAL.Interfaces;
using CIM.DAL.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class BomRepository : Repository<BomTemp>, IBomRepository
    {
        private IDirectSqlRepository _directSqlRepository;

        public BomRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<List<BomMaterialModel>> ListMaterialByBom(int bomId)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@bom_id", bomId},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListBOMMaterial", parameterList);

                return (dt.ToModel<BomMaterialModel>());
            });
        }
    }
}
