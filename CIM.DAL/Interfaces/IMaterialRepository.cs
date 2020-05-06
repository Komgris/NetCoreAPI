﻿using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        Task<IList<MaterialDictionaryModel>> ListProductBOM();
    }
}
