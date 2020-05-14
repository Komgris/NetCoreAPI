using CIM.DAL.Implements;
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
    public class ProductMaterialRepository : Repository<ProductMaterial>, IProductMaterialRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public ProductMaterialRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<List<ProductMaterialModel>> ListMaterialByProduct(int productId)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@product_id", productId},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListProductMaterial", parameterList);

                return (dt.ToModel<ProductMaterialModel>());
            });
        }
    }
}
