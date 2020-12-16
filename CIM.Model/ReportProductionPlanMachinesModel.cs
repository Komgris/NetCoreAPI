using System;
using System.Collections.Generic;
using System.Text;

namespace MMM.Domain.Models
{
    public class SubClassProductDescription
    {
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Target { get; set; }
    }

    public class RawMaterial
    {
        public string StockNo { get; set; }
        public string QCNo { get; set; }
        public string Color { get; set; }

    }

    public class RawComponent
    {
        public string Component { get; set; }
        public string StockNo { get; set; }
        public string LotNo { get; set; }

    }


    public class PreconditionsAndSetup
    {
        public string Description { get; set; }
        public bool IsCheck { get; set; }
        public bool No { get; set; }
        public string Remark { get; set; }
    }

    public class CuttingProgram
    {
        public string Description { get; set; }
        public bool IsCheck { get; set; }
        public double TrimWaste { get; set; }
    }

    public class ColorSortingRecord
    {
        public string Name { get; set; }
    }

    public class SubClassCuttingProcess
    {
        public string TheEmployee { get; set; }
        public double BacksheetTime { get; set; }
        public List<CuttingProcessRecord> CuttingProcessRecord { get; set; }
        public SumOfCuttingProcessRecord Sum { get; set; }
    }
    public class CuttingProcessRecord
    {
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double SetUpHr { get; set; }
        public double MachineHr { get; set; }
        public double ReSetupHr { get; set; }
        public double ProdDownHr { get; set; }
        public double MachDownHr { get; set; }
        public double SumHr { get; set; }
        public double CalPad { get; set; }
        public double GoodPad { get; set; }
        public double BadPad { get; set; }

    }

    public class SumOfCuttingProcessRecord
    {
        public double SetUpHr { get; set; }
        public double MachineHr { get; set; }
        public double ReSetupHr { get; set; }
        public double ProdDownHr { get; set; }
        public double MachDownHr { get; set; }
        public double SumHr { get; set; }
        public double CalPad { get; set; }
        public double GoodPad { get; set; }
        public double BadPad { get; set; }

    }

    public class InProcessInspection
    {
        public string Description { get; set; }
        public bool IsCheck { get; set; }
        public bool No { get; set; }
    }

    public class SubClassOutgoingInspection
    {
        public List<OutgoingInspection> OutgoingInspectionRecord { get; set; }
        public SumOfOutgoingInspection Sum { get; set; }
    }
    public class OutgoingInspection
    {
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double SumTime { get; set; }
        public double GoodPad { get; set; }
        public double BadPad { get; set; }
        public string PackingEmployee { get; set; }
        public string Remark { get; set; }

    }

    public class SumOfOutgoingInspection
    {
        public double SumTime { get; set; }
        public double GoodPad { get; set; }
        public double BadPad { get; set; }

    }

    public class SubClassPackWrap
    {
        public bool PackOutsource { get; set; }
        public bool Wrap { get; set; }
    }

    public class SubClassAverageCal
    {
        public double Cal1 { get; set; }
        public double Cal2 { get; set; }
        public double Cal3 { get; set; }
    }

    public class FiveTesting
    {
        public string Description { get; set; }
        public bool Example_1 { get; set; }
        public bool Example_2 { get; set; }
        public bool Example_3 { get; set; }
        public bool Example_4 { get; set; }
        public bool Example_5 { get; set; }
        public string Remark { get; set; }
    }

    public class SubClassPackingProcess
    {
        public List<PackingProcessRecord> PackingProcessRecord { get; set; }
        public SumOfPackingProcessRecord Sum { get; set; }
    }
    public class PackingProcessRecord
    {
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double SetUpHr { get; set; }
        public double MachineHr { get; set; }
        public double ReSetupHr { get; set; }
        public double ProdDownHr { get; set; }
        public double MachDownHr { get; set; }
        public double SumHr { get; set; }
        public double GoodPad { get; set; }
        public double BadPad { get; set; }
        public string Remark { get; set; }

    }
    public class SumOfPackingProcessRecord
    {
        public double SetUpHr { get; set; }
        public double MachineHr { get; set; }
        public double ReSetupHr { get; set; }
        public double ProdDownHr { get; set; }
        public double MachDownHr { get; set; }
        public double SumHr { get; set; }
        //public double CalPad { get; set; }
        public double GoodPad { get; set; }
        public double BadPad { get; set; }

    }
    public class SubClassWasteAnalysis
    {
        public List<WasteAnalysis> WasteAnalysisRecord { get; set; }
        public int SumAmount { get; set; }
    }
    public class WasteAnalysis
    {
        public string Description { get; set; }
        public int Amount { get; set; }
        public String Remark { get; set; }
    }
}
