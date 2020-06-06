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
    public class BomRepository : Repository<MaterialGroup>, IBomRepository
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

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMaterialGroupMaterial", parameterList);

                return (dt.ToModel<BomMaterialModel>());
            });
        }

        public async Task<PagingModel<BomModel>> ListBom(int page, int howmany, string keyword)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howmany},
                                            { "@page", page}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMaterialGroup", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<BomModel>(), totalCount, page, howmany);
            });
        }
    }
}
