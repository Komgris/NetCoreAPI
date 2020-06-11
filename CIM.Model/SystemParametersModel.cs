using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model {
    public class SystemParametersModel {
        public bool HasTagChanged { get; set; } = false;
        public List<int> ListMachineIdsResetCounter { get; set; }
    }
}
