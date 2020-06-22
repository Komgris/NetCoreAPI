using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ViewMasterLoss
    {
        public int LossLevel3Id { get; set; }
        public string LossLevel3Name { get; set; }
        public string LossLevel3Desc { get; set; }
        public int LossLevel2Id { get; set; }
        public string LossLevel2Name { get; set; }
        public string LossLevel2Desc { get; set; }
        public int LossLevel1Id { get; set; }
        public string LossLevel1Name { get; set; }
        public string LossLevel1Desc { get; set; }
    }
}
