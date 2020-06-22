using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Accidents
    {
        public Accidents()
        {
            AccidentParticipants = new HashSet<AccidentParticipants>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime HappenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Note { get; set; }

        public virtual ICollection<AccidentParticipants> AccidentParticipants { get; set; }
    }
}
