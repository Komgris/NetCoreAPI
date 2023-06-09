﻿using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IBomRepository :IRepository<MaterialGroup, MaterialGroupModel>
    {
        Task<List<MaterialGroupMaterialModel>> ListMaterialByBom(int bomId);
        Task<PagingModel<MaterialGroupModel>> ListBom(int page, int howMany, string keyword, bool isActive);
    }
}
