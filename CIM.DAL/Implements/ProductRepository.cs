﻿using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using CIM.DAL.Utility;

namespace CIM.DAL.Implements
{
    public class ProductRepository : Repository<Product, ProductModel>, IProductRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public ProductRepository(cim_3m_1Context context, IDirectSqlRepository directSqlRepository, IConfiguration configuration ) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<ProductModel>> Paging(string keyword, int page, int howMany, bool isActive, int? processTypeId)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howMany},
                                            {"@page", page},
                                            {"@is_active", isActive},
                                            {"@processtypeid", processTypeId}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListProduct", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<ProductModel>(), totalCount, page, howMany);
            });
        }

        public async Task<IDictionary<int, ProductDictionaryModel>> ListAsDictionary(IList<MaterialDictionaryModel> productBOM)
        {

            var output = await _dbset.Where(x => x.IsActive == true && x.IsDelete == false)
            .Select(x => new ProductDictionaryModel
            {
                Id = x.Id,
                Code = x.Code,
            }).ToListAsync();
            foreach (var item in output)
            {
                item.Materials = productBOM.Where(x => x.ProductId == item.Id).ToDictionary(x => x.Id, x => x);
            }
            
            return output.ToDictionary(x => x.Id, x => x);
        }

    }
}
