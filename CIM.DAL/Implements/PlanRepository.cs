using System;
using System.Collections.Generic;
using System.Text;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;

namespace CIM.DAL.Implements
{
    public class PlanRepository : Repository<ProductionPlan>, IPlanRepository
    {
        public PlanRepository(cim_dbContext context) : base(context)
        {

        }
    }
}
