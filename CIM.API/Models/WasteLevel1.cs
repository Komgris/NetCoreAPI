using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class WasteLevel1
    {
        public WasteLevel1()
        {
            WasteLevel2 = new HashSet<WasteLevel2>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<WasteLevel2> WasteLevel2 { get; set; }
    }
}
