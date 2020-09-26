namespace CIM.Model
{
    public class RecordProductionPlanWasteMaterialModel
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public decimal Amount { get; set; }
        public int WasteId { get; set; }
    }
}