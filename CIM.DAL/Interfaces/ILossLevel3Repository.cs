using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface ILossLevel3Repository : IRepository<LossLevel3>
    {
        Task<IList<LossLevelComponentMappingModel>> ListComponentMappingAsync();
        Task<IList<LossLevelMachineMappingModel>> ListMachineMappingAsync();
        Task<PagingModel<LossLevel3ViewModel>> ListAsPaging(int page, int howmany, string keyword, bool isActive);
        //Task<IList<LossLevel3ViewModel>> ListAsPaging2();
    }
}
