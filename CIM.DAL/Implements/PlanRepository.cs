using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CIM.DAL.Implements
{
    public class PlanRepository : Repository<ProductionPlan>, IPlanRepository
    {
        public PlanRepository(cim_dbContext context) : base(context)
        {
        }
        public async Task<PagingModel<ProductionPlanModel>> Paging( int page, int howmany)
        {
            var query = _entities.ProductionPlan;
            int skipRec = (page-1)*howmany;
            int takeRec = howmany;
            int row = query.Count();

            var paging = query.OrderBy(s=>s.PlantId).Skip(skipRec).Take(takeRec);

            var data = await paging
                .Select(x => new ProductionPlanModel
                {
                    PlanId = x.PlantId
                }).ToListAsync();
            return new PagingModel<ProductionPlanModel>
            {
                HowMany = row,
                Data = data
            };
        }
        public List<ProductionPlanModel> Get()
        {
            var query = _entities.ProductionPlan;
            var data =  query
                .Select(x => new ProductionPlanModel
                {
                    
                    PlanId = x.PlantId
                }).ToList();
            return data;
        }
    }

}
