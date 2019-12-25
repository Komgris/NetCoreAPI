using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.Domain.Models;
using CIM.Model;

namespace CIM.DAL.Interfaces
{
    public interface IPlanRepository : IRepository<ProductionPlan> {
        Task<PagingModel<ProductionPlanModel>> Paging(int page, int howmany);
        List<ProductionPlanModel> Get();
    }
    
       
    
}
