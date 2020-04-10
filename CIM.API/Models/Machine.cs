using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class Machine
    {
        public Machine()
        {
            MachineComponent = new HashSet<MachineComponent>();
            RecordMachineStatus = new HashSet<RecordMachineStatus>();
            RecordManufacturingLoss = new HashSet<RecordManufacturingLoss>();
            RouteMachine = new HashSet<RouteMachine>();
            Sensor = new HashSet<Sensor>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public int MachineTypeId { get; set; }
        public string StatusTag { get; set; }
        public string CounterInTag { get; set; }
        public string CounterOutTag { get; set; }
        public string CounterResetTag { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual MachineType MachineType { get; set; }
        public virtual MachineStatus Status { get; set; }
        public virtual ICollection<MachineComponent> MachineComponent { get; set; }
        public virtual ICollection<RecordMachineStatus> RecordMachineStatus { get; set; }
        public virtual ICollection<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
        public virtual ICollection<RouteMachine> RouteMachine { get; set; }
        public virtual ICollection<Sensor> Sensor { get; set; }
    }
}
