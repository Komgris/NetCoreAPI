using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ActiveMachine3MModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Image { get; set; }
        public string ProductionPlanId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public bool IsAutoRecord { get; set; }
        public bool LossRecording { get; set; }
        public string UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public int? CounterLastHr { get; set; } = 0;
        public int? CounterOut { get; set; } = 0;
        public decimal  CounterDefect { get; set; } = 0;
        public int Hour { get; set; } = DateTime.Now.Hour;
        public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();
        public void ResetNewPlan(string planId)
        {
            ProductionPlanId = planId;
            CounterLastHr = 0;
            CounterOut = 0;
            Hour = DateTime.Now.Hour;
        } 
    }
}