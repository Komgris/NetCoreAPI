using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMasterDataShowService : IBaseService
    {
        Task<List<WasteModel>> GetListWaste();

        Task<List<MaterialMasterShowModel>> GetListMaterial();

        Task<List<MachineMasterShowModel>> GetListMachine();

        Task<List<LossMasterShowModel>> GetListLoss();

        Task<List<ProductMasterShowModel>> GetListProduct();

        Task<List<ProductionMasterShowModel>> GetListProduction();
    }
}
