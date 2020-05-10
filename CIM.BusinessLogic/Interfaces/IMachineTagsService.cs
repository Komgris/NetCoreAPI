using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMachineTagsService : IBaseService
    {
        Task<string> Get();
    }
}
