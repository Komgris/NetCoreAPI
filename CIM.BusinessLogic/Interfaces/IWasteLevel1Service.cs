using System;
using System.Collections.Generic;
using System.Text;

using CIM.Model;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IWasteLevel1Service : IBaseService
    {
        Task<PagingModel<WasteLevel1Model>> List(string keyword, int page, int howMany, bool isActive, int? processTypeId);
        Task<WasteLevel1Model> Create(WasteLevel1Model model);
        Task<WasteLevel1Model> Update(WasteLevel1Model model);
        Task<WasteLevel1Model> Get(int id);
    }
}
