using System;

namespace CIM.Model
{
    public class AlertModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int ItemType { get; set; }
        public int ItemId { get; set; }
        public int ItemStatusId { get; set; }

        public int StatusId { get; set; }
        public int LossLevel3Id { get; set; }
        public int RouteId { get; set; }
    }
}