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
                .Select(x=>new ProductModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    BriteItemUpcItem = x.BriteItemUpcItem,
                    ProductFamilyId = x.ProductFamilyId,
                    ProductGroupId = x.ProductGroupId,
                    ProductTypeId = x.ProductTypeId,
                    PackingMedium = x.PackingMedium,
                    IgWeight = x.IgWeight,
                    PmWeight = x.PmWeight,
                    WeightUom = x.WeightUom,
                }).ToListAsync();
            return new PagingModel<ProductModel>
            {
                HowMany = row,
                Data = data
            };
        }
        public List<ProductModel> Get()
        {
            var query = _entities.Product;
            var data = query
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    BriteItemUpcItem = x.BriteItemUpcItem,
                    ProductFamilyId = x.ProductFamilyId,
                    ProductGroupId = x.ProductGroupId,
                    ProductTypeId = x.ProductTypeId,
                    PackingMedium = x.PackingMedium,
                    IgWeight = x.IgWeight,
                    PmWeight = x.PmWeight,
                    WeightUom = x.WeightUom,

                }).ToList();
            return data;
        }
        public void InsertProduct(List<ProductModel> import)
        {
            foreach (var plan in import)
            {
                var insert = new Product();
                insert.Id = plan.Id;
                insert.Code = plan.Code;
                insert.Description = plan.Description;
                insert.BriteItemUpcItem = plan.BriteItemUpcItem;
                insert.ProductFamilyId = plan.ProductFamilyId;
                insert.ProductGroupId = plan.ProductGroupId;
                insert.ProductTypeId = plan.ProductTypeId;
                insert.PackingMedium = plan.PackingMedium;
                insert.IgWeight = plan.IgWeight;
                insert.PmWeight = plan.PmWeight;
                insert.PmWeight = plan.PmWeight;

                _entities.Product.Add(insert);
                _entities.SaveChanges();
            }
        }
        public void DeleteProduct(string id)
        {

            var delete = _entities.Product.Where(x => x.Id == int.Parse(id)).FirstOrDefault();
            if (delete != null)
            {
                _entities.Product.Remove(delete);
            }
            _entities.SaveChanges();

        }
    }
}
