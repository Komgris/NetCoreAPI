using System;
using System.Collections.Generic;
using CIM.DAL.Interfaces;
using CIM.Model;
using System.Linq;
using CIM.BusinessLogic.Interfaces;
using CIM.Domain.Models;

namespace CIM.BusinessLogic.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;
        public PlanService(
            IUnitOfWorkCIM unitOfWork,
            IPlanRepository planRepository
            )
        {
            _planRepository = planRepository;
        }
        public int Plus(int A, int B)
        {
            return A + B;
        }

        public List<ProductionPlanModel> List()
        {

            var result = _planRepository.All().Select(x => new ProductionPlanModel {
                Id = x.Id,
                PlanId = x.PlantId,
            }).ToList();

            return result;
        }

        public List<ProductionPlanModel> Compare(List<ProductionPlanModel> import, List<ProductionPlanModel> dbPlan)
        {
            foreach (var plan in import)
            {
                plan.IsDuplicate = dbPlan.Any(x => x.PlanId == plan.PlanId);
            }
            return import;
        }
    }
}
