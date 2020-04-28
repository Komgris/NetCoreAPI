using System;
using System.Collections.Generic;
using System.Text;
using CIM.Model;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ILossLevel1Service : IBaseService
    {
        Task<PagingModel<LossLevel1Model>> List(string keyword, int page, int howmany, bool isActive);
    }
}
