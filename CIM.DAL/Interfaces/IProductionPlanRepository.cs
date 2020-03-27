using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.Domain.Models;
using CIM.Model;

namespace CIM.DAL.Interfaces
{
    public interface IProductionPlanRepository : IRepository<ProductionPlan>
    {
        Task<PagingModel<ProductionPlanModel>> Paging(int page, int howmany);
        List<ProductionPlanModel> Get();
        void InsertProduction(List<ProductionPlanModel> import);
        void DeleteProduction(string id);
        void UpdateProduction(List<ProductionPlanModel> list);
    }

}