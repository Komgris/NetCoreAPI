using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RecordProductionPlanInformationDetailModel
    {
        public int Id { get; set; }
        public int RecordInformationId { get; set; }
        public int MaterialId { get; set; }
        public string LotNo { get; set; }
        public int ColorId { get; set; }
        public string Remark { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
