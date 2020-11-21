﻿using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class ProductMasterShowRepository : Repository<Product, ProductMasterShowModel>, IProductMasterShowRepository
    {
        public ProductMasterShowRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
}
