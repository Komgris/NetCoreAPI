﻿using CIM.DAL.Interfaces;
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

        public async Task<PagingModel<MaterialModel>> List(int page, int howmany)
        {
            var query = _entities.Material.Where(x => x.IsActive && x.IsDelete == false);
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;
            int row = query.Count();

            var paging = query.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec);

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

        public async Task<MaterialModel> Get(int id)
        {
            var dbModel = await _entities.Material.Where(x => x.Id == id)
                .Select(x => new MaterialModel()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    ProductCategory = x.ProductCategory,
                    ICSGroup = x.ICSGroup,
                    MaterialGroup = x.MaterialGroup,
                    UOM = x.UOM,
                    BHTPerUnit = x.BHTPerUnit,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy
                }).SingleOrDefaultAsync();
            return dbModel;
        }
    }
}
