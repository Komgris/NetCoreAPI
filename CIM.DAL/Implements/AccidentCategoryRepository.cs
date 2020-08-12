using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class AccidentCategoryRepository : Repository<AccidentCategory, AccidentCategoryModel>, IAccidentCategoryRepository
    {
        public AccidentCategoryRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
}