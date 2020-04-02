using System;

namespace CIM.Model
{
    public class AlertModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Type { get; set; }
        public int ComponentStatusId { get; set; }

        public int StatusId { get; set; }

    }
}