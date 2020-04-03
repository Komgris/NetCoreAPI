using System;

namespace CIM.Model
{
    public class AlertModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ItemType { get; set; }
        public int ItemId { get; set; }
        public int ComponentStatusId { get; set; }

        public int StatusId { get; set; }

    }
}