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
        public bool IsReady { get; set; }
        public bool IsAutoLossRecord { get; set; }
        public string UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public RecordProductionPlanOutputModel RecordProductionPlanOutput { get; set; }
    }
}
