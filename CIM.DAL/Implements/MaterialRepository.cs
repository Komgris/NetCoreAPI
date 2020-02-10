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
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(cim_dbContext context) : base(context)
        {
        }

        public List<MaterialModel> List()
        {
            var query = _entities.Material;
            var data = query.Where(x => x.IsActive && x.IsDelete == false)
                .Select(x => new MaterialModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    ProductCategory = x.ProductCategory,
                    ICSGroup = x.ICSGroup,
                    MaterialGroup = x.MaterialGroup,
                    UOM = x.UOM,
                    BHTPerUnit = x.BHTPerUnit
                }).ToList();
            return data;
        }

        public void Insert(MaterialModel model)
        {
            var insert = new Material();
            insert.Code = model.Code;
            insert.Description = model.Description;
            insert.ProductCategory = model.ProductCategory;
            insert.ICSGroup = model.ICSGroup;
            insert.MaterialGroup = model.MaterialGroup;
            insert.UOM = model.UOM;
            insert.BHTPerUnit = model.BHTPerUnit;
            insert.IsActive = model.IsActive;
            insert.IsDelete = false;
            insert.CreatedBy = model.CreatedBy;
            insert.UpdatedBy = model.UpdatedBy;
            _entities.Material.Add(insert);
            _entities.SaveChanges();
        }

        public async Task<PagingModel<MaterialModel>> Paging(int page, int howmany)
        {
            var query = _entities.Material;
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;
            int row = query.Count();

            var paging = query.Where(x => x.IsActive && x.IsDelete == false).OrderBy(s => s.Id).Skip(skipRec).Take(takeRec);

            var data = await paging.Select(x => new MaterialModel
            {
                Id = x.Id,
                Code = x.Code,
                Description = x.Description,
                ProductCategory = x.ProductCategory,
                ICSGroup = x.ICSGroup,
                MaterialGroup = x.MaterialGroup,
                UOM = x.UOM,
                BHTPerUnit = x.BHTPerUnit,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy
            }).ToListAsync();
            return new PagingModel<MaterialModel>
            {
                HowMany = row,
                Data = data
            };
        }

        public void Update(MaterialModel model)
        {
            var update = _entities.Material.Where(x => x.Id == model.Id).FirstOrDefault();
            if (update != null)
            {
                update.Code = model.Code;
                update.Description = model.Description;
                update.ProductCategory = model.ProductCategory;
                update.ICSGroup = model.ICSGroup;
                update.MaterialGroup = model.MaterialGroup;
                update.UOM = model.UOM;
                update.BHTPerUnit = model.BHTPerUnit;
                update.IsActive = model.IsActive;
                update.IsDelete = false;
                update.UpdatedBy = model.UpdatedBy;
                _entities.SaveChanges();
            }
        }
    }
}
