using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class AccidentParticipants
    {
        public int Id { get; set; }
        public string EmNo { get; set; }
        public int AccidentId { get; set; }
        public string Note { get; set; }

        public virtual Accidents Accident { get; set; }
    }
}
