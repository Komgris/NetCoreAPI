using System;
using System.Collections.Generic;
using System.Text;

using CIM.Model;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IComponentTypeLossLevel3Service : IBaseService
    {
        Task<PagingModel<ComponentTypeLossLevel3ListModel>> List(int? componentTypeId, int? lossLevel3Id, int page, int howmany);
        Task Update(List<int> lossLevel3Ids, int componentTypeId);
    }
}
