using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ComponentType
    {
        public ComponentType()
        {
            Component = new HashSet<Component>();
            ComponentTypeLossLevel3 = new HashSet<ComponentTypeLossLevel3>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Component> Component { get; set; }
        public virtual ICollection<ComponentTypeLossLevel3> ComponentTypeLossLevel3 { get; set; }
    }
}
