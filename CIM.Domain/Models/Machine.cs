using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Machine
    {
        public Machine()
        {
            MachineComponent = new HashSet<MachineComponent>();
            RouteMachine = new HashSet<RouteMachine>();
            Sensor = new HashSet<Sensor>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public int MachineTypeId { get; set; }
        public string Plcaddress { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual MachineType MachineType { get; set; }
        public virtual MachineStatus Status { get; set; }
        public virtual ICollection<MachineComponent> MachineComponent { get; set; }
        public virtual ICollection<RouteMachine> RouteMachine { get; set; }
        public virtual ICollection<Sensor> Sensor { get; set; }
    }
}
