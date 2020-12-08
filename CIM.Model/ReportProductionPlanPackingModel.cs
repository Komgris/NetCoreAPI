using System;
using System.Collections.Generic;
using System.Text;

namespace MMM.Domain.Models
{
    public class ReportProductionPlanPackingModel
    {
        public string Header { get; set; }
        public string Shop_No { get; set; }
        public SubClassProductDescription ProductDescription { get; set; }
        public List<RawMaterial> RawMaterials { get; set; }
        public List<PreconditionsAndSetup> MachinePreConditionAndSetup { get; set; }
        public List<FiveTesting> FiveTestingRecord { get; set; }
        public List<ColorSortingRecord> ColorSorting { get; set; }
        public string TheEmployee { get; set; }
        public SubClassPackingProcess PackingProcess { get; set; }
        public SubClassWasteAnalysis WasteAnalysis { get; set; }
        public double AverageCal { get; set; }
    }



}
