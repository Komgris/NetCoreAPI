using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MachineAlertModel
    {

        public List<ActiveMachineModel> Machines { get; set; } = new List<ActiveMachineModel>();

        public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();

    }
}
