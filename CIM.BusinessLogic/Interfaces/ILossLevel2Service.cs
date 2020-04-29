using System;
using System.Collections.Generic;
using System.Text;
using CIM.Model;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ILossLevel2Service : IBaseService
    {
        Task<PagingModel<LossLevel2ListModel>> List(string keyword, int page, int howmany, bool isActive);
    }
}
