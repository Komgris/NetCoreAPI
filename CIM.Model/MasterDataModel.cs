﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MasterDataModel
    {

        public IDictionary<int, LossLevel3Model> LossLevel3s { get; set; }
        public IDictionary<int, MachineComponentModel> Components { get; set; }
        public IDictionary<int, MachineModel> Machines { get; set; }
        public IDictionary<int, int[]> RouteMachines { get; set; }
        public IDictionary<int, RouteModel> Routes { get; set; }
        public IDictionary<int, IDictionary<int,string>> ProductGroupRoutes { get; set; }

        public DictionaryModel Dictionary { get; set; } = new DictionaryModel();
    }

}