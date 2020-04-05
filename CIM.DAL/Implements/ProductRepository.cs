using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(cim_dbContext context) : base(context)
        {

        }

        public async Task<PagingModel<ProductModel>>Paging(int page,int howmany)
        {
            var query = _entities.Product;
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;
            int row = query.Count();
            var paging = query.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec);
            var data = await paging
                .Select(x => new ProductModel {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    BriteItemPerUpcitem = x.BriteItemPerUpcitem,
                    ProductFamily_Id = x.ProductFamilyId,
                    ProductGroup_Id = x.ProductGroupId,
                    ProductType_Id = x.ProductTypeId,
                    PackingMedium = x.PackingMedium,
                    Igweight = x.Igweight,
                    Pmweight = x.Pmweight,
                    WeightPerUom = x.WeightPerUom,
                }).ToListAsync();
            return new PagingModel<ProductModel> {
                HowMany = row,
                Data = data
            };
        }

        public async Task<List<ProductModel>> Get()
        {
            var query = _entities.Product;
            var data = await query
                .Select(x => new ProductModel {
                    Code = x.Code,
                    Description = x.Description,
                    BriteItemPerUpcitem = x.BriteItemPerUpcitem,
                    ProductFamily_Id = x.ProductFamilyId,
                    ProductGroup_Id = x.ProductGroupId,
                    ProductType_Id = x.ProductTypeId,
                    PackingMedium = x.PackingMedium,
                    Igweight = x.Igweight,
                    Pmweight = x.Pmweight,
                    WeightPerUom = x.WeightPerUom,

                }).ToListAsync();
            return data;
        }
    }
}
