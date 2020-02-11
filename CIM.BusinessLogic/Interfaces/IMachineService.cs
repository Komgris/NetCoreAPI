using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMachineService
    {
        List<MachineCacheModel> ListCached();
    }
}
