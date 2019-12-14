using System;
using System.Collections.Generic;
using System.Text;
using CIM.Model;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IPlanService
    {
        int Plus(int A, int B);
        List<ProductionPlanModel> List();
        List<ProductionPlanModel> Compare(List<ProductionPlanModel> import, List<ProductionPlanModel> dbPlan);
    }
}
