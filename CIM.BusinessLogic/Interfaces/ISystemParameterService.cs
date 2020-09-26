using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ISystemParameterService : IBaseService
    {
        Task Create(SystemParameterModel data);
        Task Update(SystemParameterModel data);
        Task Delete(int id);
        Task<SystemParameterModel> Get(int id);
        Task<PagingModel<SystemParameterModel>> List(string keyword, int page, int howMany, bool isActive);         
    }
}
