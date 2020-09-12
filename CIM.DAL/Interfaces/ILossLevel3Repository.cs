using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface ILossLevel3Repository : IRepository<LossLevel3, LossLevel3Model>
    {
        Task<IList<LossLevelComponentMappingModel>> ListComponentMappingAsync();
        Task<IList<LossLevelMachineMappingModel>> ListMachineMappingAsync();
        Task<PagingModel<LossLevel3ListModel>> List(int page, int howmany, string keyword, bool isActive, int? lossLevel2Id);
    }
}