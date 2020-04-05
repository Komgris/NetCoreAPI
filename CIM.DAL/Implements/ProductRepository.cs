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

        public async Task<PagingModel<ProductModel>>Paging(string keyword, int page,int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            var dbModel = await _entities.Product.Where(x => x.IsActive == true &
            (x.Code.Contains(keyword) || x.Description.Contains(keyword)))
                                .Select(
                    x => new ProductModel
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Description = x.Description,
                        BriteItemPerUpcitem = x.BriteItemPerUpcitem,
                        ProductFamily_Id = x.ProductFamily_Id,
                        ProductGroup_Id = x.ProductGroup_Id,
                        ProductType_Id = x.ProductType_Id,
                        PackingMedium = x.PackingMedium,
                        NetWeight = x.NetWeight,
                        Igweight = x.Igweight,
                        Pmweight = x.Pmweight,
                        WeightPerUom = x.WeightPerUom,
                        IsActive = x.IsActive,
                        IsDelete = x.IsDelete,
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        UpdatedAt = x.UpdatedAt,
                        UpdatedBy = x.UpdatedBy,
                    }).ToListAsync();

            int total = dbModel.Count();
            dbModel = dbModel.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec).ToList();

            var output = new List<ProductModel>(dbModel);

            return new PagingModel<ProductModel>
            {
                HowMany = total,
                Data = output
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
                    ProductFamily_Id = x.ProductFamily_Id,
                    ProductGroup_Id = x.ProductGroup_Id,
                    ProductType_Id = x.ProductType_Id,
                    PackingMedium = x.PackingMedium,
                    Igweight = x.Igweight,
                    Pmweight = x.Pmweight,
                    WeightPerUom = x.WeightPerUom,

                }).ToListAsync();
            return data;
        }
    }
}
