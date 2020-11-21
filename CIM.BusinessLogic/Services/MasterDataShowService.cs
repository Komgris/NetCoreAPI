using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class MasterDataShowService : BaseService, IMasterDataShowService
    {
        private readonly IWasteRepository _wasteRepository;
        private readonly IMaterialMasterShowRepository _materialMasterShowRepository;
        private readonly IMachineMasterShowRepository _materialMachineShowRepository;
        private readonly IProductMasterShowRepository _productMasterShowRepository;
        private readonly ILossMasterShowRepository _lossMasterShowRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public MasterDataShowService(
            IUnitOfWorkCIM unitOfWork, 
            IWasteRepository wasteRepository,
            IMaterialMasterShowRepository materialMasterShowRepository,
            IMachineMasterShowRepository materialMachineShowRepository,
            IProductMasterShowRepository productMasterShowRepository,
            ILossMasterShowRepository lossMasterShowRepository
            )
        {
            _wasteRepository = wasteRepository;
            _materialMasterShowRepository = materialMasterShowRepository;
            _materialMachineShowRepository = materialMachineShowRepository;
            _productMasterShowRepository = productMasterShowRepository;
            _lossMasterShowRepository = lossMasterShowRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LossMasterShowModel>> GetListLoss()
        {
            return await _lossMasterShowRepository.List("[dbo].[sp_ListLossMaster]", null);
        }

        public async Task<List<MachineMasterShowModel>> GetListMachine()
        {
            return await _materialMachineShowRepository.List("[dbo].[sp_ListMachineMaster]", null);
        }

        public async Task<List<MaterialMasterShowModel>> GetListMaterial()
        {
            return await _materialMasterShowRepository.List("[dbo].[sp_ListMaterialMaster]", null);
        }

        public async Task<List<ProductMasterShowModel>> GetListProduct()
        {
            return await _productMasterShowRepository.List("[dbo].[sp_ListProductMaster]", null);
        }

        public async Task<List<WasteModel>> GetListWaste()
        {
            return await _wasteRepository.List("[dbo].[sp_ListWasteMaster]", null);
        }
    }
}
