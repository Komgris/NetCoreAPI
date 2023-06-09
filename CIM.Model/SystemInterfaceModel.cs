﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model {
    public class SystemInterfaceModel {
        public bool HasTagChanged { get; set; } = false;
        public Dictionary<int, bool> ListMachineIdsResetCounter { get; set; } = new Dictionary<int, bool>();
        public ProductionInfoModel ProductionInfo { get; set; }

    }
}
