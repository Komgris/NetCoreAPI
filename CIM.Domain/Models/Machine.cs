using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Machine
    {
        public Machine()
        {
            Component = new HashSet<Component>();
            MachineTeam = new HashSet<MachineTeam>();
            RecordMachineStatus = new HashSet<RecordMachineStatus>();
            RecordMaintenancePlan = new HashSet<RecordMaintenancePlan>();
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
        public virtual ICollection<Component> Component { get; set; }
        public virtual ICollection<MachineTeam> MachineTeam { get; set; }
        public virtual ICollection<RecordMachineStatus> RecordMachineStatus { get; set; }
        public virtual ICollection<RecordMaintenancePlan> RecordMaintenancePlan { get; set; }
        public virtual ICollection<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
        public virtual ICollection<RouteMachine> RouteMachine { get; set; }
        public virtual ICollection<Sensor> Sensor { get; set; }
    }
}
