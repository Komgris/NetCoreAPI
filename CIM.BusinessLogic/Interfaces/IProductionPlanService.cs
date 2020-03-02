using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CIM.Model;
using OfficeOpenXml;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IProductionPlanService
    {
        int Plus(int A, int B);
        Task<PagingModel<ProductionPlanModel>> Paging(int page, int howmany);
        List<ProductionPlanModel> Compare(List<ProductionPlanModel> import, List<ProductionPlanModel> dbPlan);
        List<ProductionPlanModel> ReadImport(string path);
        List<ProductionPlanModel> Get();
        void Insert(List<ProductionPlanModel> import);
        void Delete(string id);
        void Update(List<ProductionPlanModel> list);
        Task Load(ProductionPlanModel model);
    }
}
