using System;
using System.Collections.Generic;
using System.Text;

using CIM.Model;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IWasteLevel2Service : IBaseService
    {
        Task<PagingModel<WasteLevel2Model>> List(string keyword, int page, int howMany, bool isActive);
        Task<WasteLevel2Model> Create(WasteLevel2Model model);
        Task<WasteLevel2Model> Update(WasteLevel2Model model);
        Task<WasteLevel2Model> Get(int id);
    }
}
