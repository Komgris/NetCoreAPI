using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {

        }

        public async Task<PagingModel<ProductModel>> Paging(string keyword, int page, int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            var dbModel = _entities.Product.Where(x => x.IsActive == true &&
           (string.IsNullOrEmpty(keyword) ? true : (x.Code.Contains(keyword) || x.Description.Contains(keyword))))
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

            return await ToPagingModelAsync(dbModel, page, howmany);

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

        public async Task<IDictionary<int, ProductDictionaryModel>> ListAsDictionary(IList<MaterialDictionaryModel> productBOM)
        {

            var output = await _dbset.Where(x => x.IsActive == true && x.IsDelete == false)
            .Select(x => new ProductDictionaryModel
            {
                Id = x.Id,
                Code = x.Code,
                GroupId = x.ProductGroupId,
                TypeId = x.ProductTypeId,
            }).ToListAsync();
            foreach (var item in output)
            {
                item.Materials = productBOM.Where(x => x.ProductId == item.Id).ToDictionary(x => x.Id, x => x);
            }
            
            return output.ToDictionary(x => x.Id, x => x);
        }

    }
}
