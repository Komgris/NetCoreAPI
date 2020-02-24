using System;
using System.Collections.Generic;
using CIM.DAL.Interfaces;
using CIM.Model;
using System.Linq;
using CIM.BusinessLogic.Interfaces;
using CIM.Domain.Models;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Data;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class PlanService : IPlanService
    {
        private readonly IProductionPlanRepository _planRepository;
        public PlanService(
            IUnitOfWorkCIM unitOfWork,
            IProductionPlanRepository planRepository
            )
        {
            _planRepository = planRepository;
        }
        public int Plus(int A, int B)
        {
            return A + B;
        }
        public List<ProductionPlanModel> Get()
        {
            var result = _planRepository.Get();
            return result;
        }
        public Task<PagingModel<ProductionPlanModel>> Paging(int page,int howmany)
        {
            var result =  _planRepository.Paging(page, howmany);
            //var result = _planRepository.All().Select(x => new ProductionPlanModel {
            //    Id = x.Id,
            //    PlanId = x.PlantId,
            //}).ToList();
            return result;
        }
        public void Insert(List<ProductionPlanModel> import)
        {
            List<ProductionPlanModel> fromDb = _planRepository.Get();
            List<ProductionPlanModel> newPlan = new List<ProductionPlanModel>();
            List<ProductionPlanModel> existsPlan = new List<ProductionPlanModel>();
            foreach (var plan in import)
            {
                if (fromDb.Any(x => x.PlantId == plan.PlantId))
                {                    
                    existsPlan.Add(plan);
                }
                else
                {
                    newPlan.Add(plan);
                }
            }
            _planRepository.InsertProduction(newPlan);
            _planRepository.UpdateProduction(existsPlan);           
        }
        public void Delete(string id)
        {
            _planRepository.DeleteProduction(id);
        }
        public void Update(List<ProductionPlanModel> list)
        {
            _planRepository.UpdateProduction(list);
        }
        public List<ProductionPlanModel> Compare(List<ProductionPlanModel> import, List<ProductionPlanModel> dbPlan)
        {
            foreach (var plan in import)
            {
                if (dbPlan.Any(x => x.PlantId == plan.PlantId))
                {
                    plan.Status = "Inprocess";
                }
                else
                {
                    plan.Status = "New";
                }

                //plan.Status = dbPlan.(x => x.PlanId == plan.PlanId);
                //plan.Status  =
            }
            return import;
        }
        public List<ProductionPlanModel> ReadImport(string path)
        {
            FileInfo excel = new FileInfo(path);
            using (var package = new ExcelPackage(excel))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.First();
                List<ProductionPlanModel> intList = ConvertImportToList(worksheet);
                return intList;
            }
        }
        public List<ProductionPlanModel> ConvertImportToList(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            int totalCols = oSheet.Dimension.End.Column;
            List<ProductionPlanModel> listImport = new List<ProductionPlanModel>();
            for (int i = 2; i <= totalRows; i++)
            {
                ProductionPlanModel data = new ProductionPlanModel();
                data.PlantId = (oSheet.Cells[i, 1].Value ?? string.Empty).ToString();
                data.ProductId = oSheet.Cells[i, 2].GetValue<int>();
                data.Target = Convert.ToInt32(oSheet.Cells[i, 3].Value ?? string.Empty);
                data.Unit = Convert.ToInt32(oSheet.Cells[i, 4].Value ?? string.Empty);
                listImport.Add(data);
            }            
            return listImport;
        }
    }
}
