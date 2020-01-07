using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string PlanId { get; set; }
        public string PlantId { get; set; }
    }
}
