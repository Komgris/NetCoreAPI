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

        public async Task<PagingModel<ProductModel>> Paging(string keyword, int page, int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            var dbModel = _entities.Product.Where(x => x.IsActive == true &
           string.IsNullOrEmpty(keyword) ? true : (x.Code.Contains(keyword) || x.Description.Contains(keyword)))
                                .Select(
                    x => new ProductModel
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Description = x.Description,
                        BriteItemPerUpcitem = x.BriteItemPerUpcitem,
                        ProductFamily_Id = x.ProductFamilyId,
                        ProductGroup_Id = x.ProductGroupId,
                        ProductType_Id = x.ProductTypeId,
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
                    });

            return await ToPagingModel(dbModel, page, howmany);

        }

        public async Task<List<ProductModel>> Get()
        {
            var query = _entities.Product;
            var data = await query
                .Select(x => new ProductModel
                {
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
