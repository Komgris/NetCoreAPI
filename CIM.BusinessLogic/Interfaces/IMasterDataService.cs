using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMasterDataService
    {
        Task GetData();

        IDictionary<int, MachineComponentModel> Components { get; set; }
        IDictionary<int, MachineModel> Machines { get; set; }
        IDictionary<int, RouteModel> Routes { get; set; }
    }
}
