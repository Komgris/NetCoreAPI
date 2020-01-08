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
                    Id = x.Id,
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
                    
                    PlanId = x.PlantId,
                    Id = x.Id
                    
                }).ToList();
            return data;
        }
        public void InsertProduction(List<ProductionPlanModel> import)
        {
            foreach (var plan in import)
            {                
                    var insert = new ProductionPlan();
                    insert.PlantId = plan.PlanId;
                    _entities.ProductionPlan.Add(insert);
                    _entities.SaveChanges();
            }
           
        }
        public void DeleteProduction(string id)
        {
        
                var delete = _entities.ProductionPlan.Where(x => x.PlantId == id).FirstOrDefault();
                if (delete != null)
                {
                    _entities.ProductionPlan.Remove(delete);
                }
                _entities.SaveChanges();
            
        }
        public void UpdateProduction(List<ProductionPlanModel> list)
        {
            foreach (var plan in list)
            {
                var update = _entities.ProductionPlan.Where(x => x.PlantId == plan.PlanId).FirstOrDefault();
                if(update != null)
                {
                    update.PlanStart = DateTime.Now;
                }
                _entities.SaveChanges();
            }
        }
    }

}
