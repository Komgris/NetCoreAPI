using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class UnitsRepository : Repository<Units>, IUnitsRepository
    {
        public UnitsRepository(cim_dbContext context) : base(context)
        {
        }
    }
}