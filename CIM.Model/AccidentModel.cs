using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class AccidentModel
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public DateTime HappenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Note { get; set; }

        public List<AccidentParticipantsModel> Participants { get; set; } = new List<AccidentParticipantsModel>();
    }
}
