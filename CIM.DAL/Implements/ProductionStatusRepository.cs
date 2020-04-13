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
    public class ProductionStatusRepository : Repository<ProductionStatus>, IProductionStatusRepository
    {
        public ProductionStatusRepository(cim_dbContext context) : base(context)
        {
        }        

    }
}
