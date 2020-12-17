using System;
using System.Collections.Generic;
using System.Text;

namespace MMM.Domain.Models
{
    public class ReportProductionPlanGuillotineModel
    {
        public string Shop_No { get; set; }
        public SubClassProductDescription ProductDescription { get; set; }
        public List<RawMaterial> RawMaterials { get; set; }
        public List<PreconditionsAndSetup> MachinePreConditionAndSetup { get; set; }
        public List<CuttingProgram> CuttingPrograms { get; set; }
        public List<ColorSortingRecord> ColorSorting { get; set; }
        public SubClassCuttingProcess CuttingProcess { get; set; }
        public int PadNumber { get; set; }
        public List<InProcessInspection> InProcessInspections { get; set; }
        public SubClassOutgoingInspection OutgoingInspection { get; set; }
        public SubClassPackWrap PackWrap { get; set; }
        public SubClassWasteAnalysis WasteAnalysis { get; set; }
        public SubClassAverageCal AverageCal { get; set; }
    }
    
}
