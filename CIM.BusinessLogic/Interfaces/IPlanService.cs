using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CIM.Model;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IPlanService
    {
        int Plus(int A, int B);
        Task<PagingModel<ProductionPlanModel>> Paging(int page, int howmany);
        List<ProductionPlanModel> Compare(List<ProductionPlanModel> import, List<ProductionPlanModel> dbPlan);
        List<ProductionPlanModel> ReadImport(string path);
        List<ProductionPlanModel> Get();
        bool Insert(List<ProductionPlanModel> import); 
    }
}
