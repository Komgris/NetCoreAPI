using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Services
{
    public class MasterDataService : BaseService, IMasterDataService
    {
        private IDirectSqlRepository _directSqlRepository;
        private ILossLevel2Repository _lossLevel2Repository;
        private ILossLevel3Repository _lossLevel3Repository;
        private IResponseCacheService _responseCacheService;
        private IRouteRepository _routeRepository;
        private IRouteMachineRepository _routeMachineRepository;
        private IRouteProductGroupRepository _routeProductGroupRepository;
        private IMachineRepository _machineRepository;
        private IProductionStatusRepository _productionStatusRepository;
        private IProductRepository _productsRepository;
        private IProductionPlanRepository _productionPlanRepository;
        private IUnitsRepository _unitsRepository;
        private IWasteLevel1Repository _wasteLevel1Repository;
        private IWasteLevel2Repository _wasteLevel2Repository;
        private IMaterialRepository _materialRepository;
        private IMachineTypeRepository _machineTypeRepository;
        private IProductTypeRepository _productTypeRepository;
        private IProductGroupRepository _productGroupRepository;
        private IProductFamilyRepository _productFamilyRepository;
        private IMaterialTypeRepository _materialTypeRepository;
        private ITeamTypeRepository _teamTypeRepository;
        private ITeamRepository _teamRepository;
        private IUserPositionRepository _userPositionRepository;
        private IEducationRepository _educationRepository;
        private IUserGroupRepository _userGroupRepository;
        private IProcessTypeRepository _processTypeRepository;
        private IAppRepository _appRepository;
        private IAppFeatureRepository _appFeatureRepository;
        private IColorRepository _colorRepository;

        private IWasteNonePrimeRepository _wastenoneprimeRepository;
        private IWasteRepository _wasteRepository;

        private IConfiguration _configuration;
        public MasterDataService(
            ILossLevel2Repository lossLevel2Repository,
            ILossLevel3Repository lossLevel3Repository,
            IResponseCacheService responseCacheService,
            IRouteRepository routeRepository,
            IRouteMachineRepository routeMachineRepository,
            IRouteProductGroupRepository routeProductGroupRepository,
            IMachineRepository machineRepository,
            IProductionStatusRepository productionStatusRepository,
            IProductRepository productRepository,
            IProductionPlanRepository productionPlanRepository,
            IUnitsRepository unitsRepository,
            IWasteLevel1Repository wasteLevel1Repository,
            IWasteLevel2Repository wasteLevel2Repository,
            IMaterialRepository materialRepository,
            IMachineTypeRepository machineTypeRepository,
            IProductTypeRepository productTypeRepository,
            IProductGroupRepository productGroupRepository,
            IProductFamilyRepository productFamilyRepository,
            IMaterialTypeRepository materialTypeRepository,
            ITeamTypeRepository teamTypeRepository,
            ITeamRepository teamRepository,
            IUserPositionRepository userPositionRepository,
            IEducationRepository educationRepository,
            IProcessTypeRepository processTypeRepository,
            IUserGroupRepository userGroupRepository,
            IAppRepository appRepository,
            IAppFeatureRepository appFeatureRepository,
            IDirectSqlRepository directSqlRepository,
            IWasteNonePrimeRepository wastenoneprimeRepository,
            IColorRepository colorRepository,
            IWasteRepository wasteRepository,
        IConfiguration configuration
            )
        {
            _directSqlRepository = directSqlRepository;
            _lossLevel2Repository = lossLevel2Repository;
            _lossLevel3Repository = lossLevel3Repository;
            _responseCacheService = responseCacheService;
            _routeRepository = routeRepository;
            _routeMachineRepository = routeMachineRepository;
            _routeProductGroupRepository = routeProductGroupRepository;
            _machineRepository = machineRepository;
            _productionStatusRepository = productionStatusRepository;
            _productsRepository = productRepository;
            _productionPlanRepository = productionPlanRepository;
            _unitsRepository = unitsRepository;
            _wasteLevel1Repository = wasteLevel1Repository;
            _wasteLevel2Repository = wasteLevel2Repository;
            _materialRepository = materialRepository;
            _machineTypeRepository = machineTypeRepository;
            _productFamilyRepository = productFamilyRepository;
            _productGroupRepository = productGroupRepository;
            _productTypeRepository = productTypeRepository;
            _materialTypeRepository = materialTypeRepository;
            _teamTypeRepository = teamTypeRepository;
            _teamRepository = teamRepository;
            _userPositionRepository = userPositionRepository;
            _educationRepository = educationRepository;
            _processTypeRepository = processTypeRepository;
            _userGroupRepository = userGroupRepository;
            _appRepository = appRepository;
            _appFeatureRepository = appFeatureRepository;
            _wastenoneprimeRepository = wastenoneprimeRepository;
            _wasteRepository = wasteRepository;
            _configuration = configuration;
            _colorRepository = colorRepository;
        }
        public MasterDataModel Data { get; set; }

        //private IList<LossLevelComponentMappingModel> _lossLevel3ComponentMapping;
        //private IList<LossLevelMachineMappingModel> _lossLevel3MachineMapping;
        private IList<LossLevel3DictionaryModel> _lossLevel3s;
        //private IList<WasteDictionaryModel> _wastesLevel1;
        //private IList<WasteDictionaryModel> _wastesLevel2;
        private IList<MaterialDictionaryModel> _productBOM;
        private IList<WasteModel> _waste;

        private IDictionary<int, LossLevel3DictionaryModel> GetLossLevel3()
        {
            var output = new Dictionary<int, LossLevel3DictionaryModel>();
            foreach (var item in _lossLevel3s)
            {
                output[item.Id] = new LossLevel3DictionaryModel
                {
                    Id = item.Id,
                    Name = $"{item.Name} - {item.Description}",
                    //ProcessTypeId = item.ProcessTypeId,
                    LossLevel2Id = item.LossLevel2Id,
                    //Components = _lossLevel3ComponentMapping.Where(x => x.LossLevelId == item.Id).Select(x => x.ComponentId).ToArray()
                };
            }
            return output;
        }


        //private async Task<IDictionary<int, MachineComponentModel>> GetComponents()
        //{
        //    var output = new Dictionary<int, MachineComponentModel>();
        //    var activeMachines = await _machineComponentRepository.AllAsync();
        //    foreach (var item in activeMachines)
        //    {
        //        output[item.Id] = new MachineComponentModel
        //        {

        //            Id = item.Id,
        //            Name = item.Name,
        //            MachineId = item.MachineId,
        //            TypeId = item.TypeId,
        //            LossList = _lossLevel3ComponentMapping.Where(x => x.ComponentId == item.Id).Select(x => x.LossLevelId).ToArray(),

        //        };
        //    }
        //    return output;
        //}

        private async Task<IDictionary<int, MachineModel>> GetMachines(IDictionary<int, MachineComponentModel> components, IDictionary<int, int[]> routeMachines)
        {
            var output = new Dictionary<int, MachineModel>();
            var activeMachines = await _machineRepository.WhereAsync(x => x.IsActive && !x.IsDelete);
            var machineTypes = (await _machineTypeRepository.WhereAsync(x => x.IsActive && !x.IsDelete)).ToDictionary(x => x.Id, x => x);

            foreach (var item in activeMachines)
            {
                //var machineComponents = components.Where(x => x.Value.MachineId == item.Id);
                var image = machineTypes.ContainsKey(item.MachineTypeId) ? machineTypes[item.MachineTypeId].Image : "";
                output[item.Id] = new MachineModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = image,
                    MachineTypeId = item.MachineTypeId,
                    //ComponentList = machineComponents.Select(x => x.Value).ToList(),
                    //LossList = _lossLevel3MachineMapping.Where(x => x.MachineId == item.Id).Select(x => x.LossLevelId).ToArray(),
                    //RouteList = routeMachines.Where(x => x.Value.Contains(item.Id)).Select(x => x.Key).ToList()
                };
            }
            return output;
        }

        //private async Task<IDictionary<int, MachineModel>> GetMachines(IDictionary<int, MachineComponentModel> components, IDictionary<int, int[]> routeMachines)
        //{
        //    var output = new Dictionary<int, MachineModel>();
        //    var activeMachines = await _machineRepository.WhereAsync(x => x.IsActive && !x.IsDelete);
        //    var machineTypes = (await _machineTypeRepository.WhereAsync(x => x.IsActive && !x.IsDelete)).ToDictionary(x => x.Id, x => x);

        //    foreach (var item in activeMachines)
        //    {
        //        var machineComponents = components.Where(x => x.Value.MachineId == item.Id);
        //        var image = machineTypes.ContainsKey(item.MachineTypeId) ? machineTypes[item.MachineTypeId].Image : "";
        //        output[item.Id] = new MachineModel
        //        {
        //            Id = item.Id,
        //            Name = item.Name,
        //            Image = image,
        //            MachineTypeId = item.MachineTypeId,
        //            ComponentList = machineComponents.Select(x => x.Value).ToList(),
        //            LossList = _lossLevel3MachineMapping.Where(x => x.MachineId == item.Id).Select(x => x.LossLevelId).ToArray(),
        //            RouteList = routeMachines.Where(x => x.Value.Contains(item.Id)).Select(x => x.Key).ToList()
        //        };
        //    }
        //    return output;
        //}

        private async Task<IDictionary<int, RouteModel>> GetRoutes(IDictionary<int, int[]> routeMachines, IDictionary<int, MachineModel> machines)
        {
            var output = new Dictionary<int, RouteModel>();
            var dbModel = await _routeRepository.AllAsync();
            foreach (var item in dbModel)
            {
                if (routeMachines.ContainsKey(item.Id))
                {
                    output[item.Id] = new RouteModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ProcessTypeId = item.ProcessTypeId,
                        MachineList = routeMachines[item.Id].Distinct().ToDictionary(x => x, x => machines[x])
                    };
                }
            }
            return output;
        }

        //private async Task<IDictionary<string, ProductionPlanDictionaryModel>> GetMachine()
        //{

        //}

        private async Task<IDictionary<string, ProductionPlanDictionaryModel>> GetProductionPlan(IDictionary<int, ProductDictionaryModel> products)
        {
            var output = new Dictionary<string, ProductionPlanDictionaryModel>();
            //var dbModel = await _productionPlanRepository.WhereAsync(x => x.IsActive == true && x.Product.IsActive == true);
            //foreach (var item in dbModel)
            //{
            //    output[item.PlanId] = new ProductionPlanDictionaryModel
            //    {
            //        PlanId = item.PlanId,
            //        Product = products[item.ProductId],
            //        RouteId = item.RouteId,
            //    };
            //}
            return output;
        }


        public async Task<MasterDataModel> GetData()
        {

            if (Data == null)
            {
                Data = await _responseCacheService.GetAsTypeAsync<MasterDataModel>($"{Constans.RedisKey.MASTER_DATA}");

                if (Data == null)
                {
                    Data = await Refresh(MasterDataType.All);
                }
                //Data = PostProcess(Data);

            }
            return Data;
        }

        public async Task<MasterDataModel> GetDataOperation()
        {

            if (Data == null)
            {
                Data = await _responseCacheService.GetAsTypeAsync<MasterDataModel>($"{Constans.RedisKey.MASTER_DATA_Oper}");

                if (Data == null)
                {
                    Data = await Refresh(MasterDataType.All);
                }
                //Data = PostProcess(Data);

            }
            return Data;
        }

        private MasterDataModel PostProcess(MasterDataModel data)
        {
            //Map loss to process driven and mp
            var processDrivenLoss = new List<int>();
            foreach (var processType in data.ProcessDrivenByProcessType)
            {
                foreach (var processDriven in processType.Value)
                {
                    processDrivenLoss.AddRange(processDriven.Value.LossLevel3.Keys);
                }
            }

            var mpList = new List<int>();
            foreach (var processType in data.ManufacturingPerformanceByProcessType)
            {
                foreach (var mp in processType.Value)
                {
                    mpList.AddRange(mp.Value.LossLevel3.Keys);
                }
            }

            foreach (var loss in data.LossLevel3s)
            {
                loss.Value.IsProcessDriven = processDrivenLoss.Any(x => x == loss.Key);
                loss.Value.IsMP = mpList.Any(x => x == loss.Key); ;
            }
            return data;
        }

        public async Task<MasterDataModel> Refresh(MasterDataType masterdataType)
        {
            var masterData = await _responseCacheService.GetAsTypeAsync<MasterDataModel>($"{Constans.RedisKey.MASTER_DATA}");
            var masterDataOper = await _responseCacheService.GetAsTypeAsync<MasterDataModel>($"{Constans.RedisKey.MASTER_DATA_Oper}");
            if (masterData == null) masterData = new MasterDataModel();
            if (masterDataOper == null) masterDataOper = new MasterDataModel();

            switch (masterdataType)
            {
                case MasterDataType.All:
                    //_lossLevel3ComponentMapping = await _lossLevel3Repository.ListComponentMappingAsync();
                    //_lossLevel3MachineMapping = await _lossLevel3Repository.ListMachineMappingAsync();
                    _lossLevel3s = (await _lossLevel3Repository.WhereAsync(x => x.IsActive && x.LossLevel2Id == 32)).Select(x => MapperHelper.AsModel(x, new LossLevel3DictionaryModel())).ToList();
                    //_wastesLevel1 = await _wasteLevel1Repository.ListAsDictionary();
                    //_wastesLevel2 = await _wasteLevel2Repository.ListAsDictionary();
                    _productBOM = await _materialRepository.ListProductBOM();
                    _waste = (await _wasteRepository.WhereAsync(x => x.IsActive && !x.IsDelete)).Select(x => MapperHelper.AsModel(x, new WasteModel())).ToList();

                    masterData.LossLevel3s = masterDataOper.LossLevel3s = GetLossLevel3();
                    //masterData.RouteMachines = await GetRouteMachine();
                    //masterData.Components = masterDataOper.Components = await GetComponents();
                    masterData.Machines = masterDataOper.Machines = await GetMachines(masterData.Components, masterData.RouteMachines);
                    //masterData.Routes = masterDataOper.Routes = await GetRoutes(masterData.RouteMachines, masterData.Machines);
                    masterData.Products = masterDataOper.Products = await _productsRepository.ListAsDictionary(_productBOM);
                    //masterData.ProductionPlan = await GetProductionPlan(masterData.Products);
                    //masterData.ProductGroupRoutes = await GetProductGroupRoutes();
                    //masterData.WastesByProductType = masterDataOper.WastesByProductType = GetWastesByProductType(_wastesLevel1, _wastesLevel2);
                    //masterData.WastesByProcessType = masterDataOper.WastesByProcessType = GetWastesByProcessType(_wastesLevel1, _wastesLevel2);
                    //masterData.ManufacturingPerformance = masterDataOper.ManufacturingPerformance = await GetManufacturingPerformanceNoMachine();
                    //masterData.AppFeature = await GetAppFeature();
                    masterData.RedirectUrl = masterDataOper.RedirectUrl = _configuration.GetValue<string>("RedirectUrl");
                    masterData.EnabledVerifyToken = _configuration.GetValue<bool>("EnabledVerifyToken");

                    masterData.Dictionary.Products = GetProductDictionary(masterData.Products);
                    //masterData.Dictionary.ProductProcesstype = await GetProductByProcessTypeDictionary();
                    masterData.Dictionary.ProductsByCode = masterData.Dictionary.Products.ToDictionary(x => x.Value, x => x.Key);
                    //masterData.Dictionary.RouteByName = masterData.Routes.ToDictionary(x => x.Value.Name, x => x.Key);
                    masterData.Dictionary.ProductionStatus = await GetProductionStatusDictionary();
                    //masterData.Dictionary.Units = await GetUnitsDictionary();
                    //masterData.Dictionary.CompareResult = GetProductionPlanCompareResult();
                    //masterData.Dictionary.WastesLevel1 = _wastesLevel1.ToDictionary(x => x.Id, x => x.Description);
                    //masterData.Dictionary.WastesLevel2 = masterDataOper.Dictionary.WastesLevel2 = _wastesLevel2.ToDictionary(x => x.Id, x => x.Description);
                    masterData.Dictionary.MachineType = await GetMachineTypeDictionary();
                    //masterData.Dictionary.ProductFamily = await GetProductFamilyDictionary();
                    //masterData.Dictionary.ProductGroup = await GetProductGroupDictionary();
                    //masterData.Dictionary.ProductType = await GetProductTypeDictionary();
                    masterData.Dictionary.Machine = masterDataOper.Dictionary.Machine = await GetMachineDictionary();
                    //masterData.Dictionary.MaterialType = await GetMaterialTypeDictionary();
                    //masterData.Dictionary.TeamType = await GetTeamTypeDictionary();
                    //masterData.Dictionary.Team = await GetTeamDictionary();
                    //masterData.Dictionary.UserPosition = await GetUserPositionDictionary();
                    //masterData.Dictionary.Education = await GetEducationDictionary();
                    //masterData.Dictionary.ProcessType = masterDataOper.Dictionary.ProcessType = await GetProcessTypeDictionary();
                    //masterData.Dictionary.UserGroup = await GetUserGroupDictionary();
                    //masterData.Dictionary.Language = await GetLanguageDictionary();
                    //masterData.Dictionary.App = await GetAppDictionary();
                    //masterData.Dictionary.LanguageVersion = masterDataOper.Dictionary.LanguageVersion = await GetLanguageUrlDictionary();
                    masterData.Dictionary.Color = masterDataOper.Dictionary.Color = masterData.Dictionary.Color = await GetColorDictionary(); 
                    masterData.Dictionary.WasteByMachine = masterDataOper.Dictionary.WasteByMachine = await GetWasteByMachineAsync(_waste);
                    break;

                case MasterDataType.LossLevel3s:
                    _lossLevel3s = (await _lossLevel3Repository.AllAsync()).Select(x => MapperHelper.AsModel(x, new LossLevel3DictionaryModel())).ToList();
                    //_lossLevel3ComponentMapping = await _lossLevel3Repository.ListComponentMappingAsync();
                    //_lossLevel3MachineMapping = await _lossLevel3Repository.ListMachineMappingAsync();
                    masterData.LossLevel3s = masterDataOper.LossLevel3s = GetLossLevel3();
                    //masterData.ManufacturingPerformance = masterDataOper.ManufacturingPerformance = await GetManufacturingPerformanceNoMachine();
                    break;
                case MasterDataType.RouteMachines:
                    //masterData.RouteMachines = await GetRouteMachine();
                    break;
                case MasterDataType.Components:
                    //_lossLevel3ComponentMapping = await _lossLevel3Repository.ListComponentMappingAsync();
                    //masterData.Components =
                    //masterDataOper.Components = await GetComponents();
                    break;
                case MasterDataType.Machines:
                    //_lossLevel3ComponentMapping = await _lossLevel3Repository.ListComponentMappingAsync();
                    //_lossLevel3MachineMapping = await _lossLevel3Repository.ListMachineMappingAsync();
                    //masterData.Components =
                    //masterDataOper.Components = await GetComponents();
                    //masterData.RouteMachines = await GetRouteMachine();
                    masterData.Machines = masterDataOper.Machines = await GetMachines(masterData.Components, masterData.RouteMachines);
                    //masterData.Routes = masterDataOper.Routes = await GetRoutes(masterData.RouteMachines, masterData.Machines);
                    //masterData.Dictionary.RouteByName = masterData.Routes.ToDictionary(x => x.Value.Name, x => x.Key);
                    masterData.Dictionary.Machine = await GetMachineDictionary();
                    break;
                case MasterDataType.Products:
                    //_productBOM = await _materialRepository.ListProductBOM();
                    //masterData.Products = await _productsRepository.ListAsDictionary(_productBOM);
                    //masterData.Dictionary.Products = GetProductDictionary(masterData.Products);
                    //masterData.Dictionary.ProductProcesstype = await GetProductByProcessTypeDictionary();
                    masterData.Dictionary.ProductsByCode = masterData.Dictionary.Products.ToDictionary(x => x.Value, x => x.Key);
                    break;
                case MasterDataType.ProductionPlan:
                    //_productBOM = await _materialRepository.ListProductBOM();
                    masterData.Products = masterDataOper.Products = await _productsRepository.ListAsDictionary(_productBOM);
                    //masterData.ProductionPlan = await GetProductionPlan(masterData.Products);
                    break;
                case MasterDataType.ProductGroupRoutes:
                    //masterData.ProductGroupRoutes = await GetProductGroupRoutes();
                    break;
                case MasterDataType.WastesByProductType:
                    //_wastesLevel1 = await _wasteLevel1Repository.ListAsDictionary();
                    //_wastesLevel2 = await _wasteLevel2Repository.ListAsDictionary();
                    //masterData.WastesByProductType = masterDataOper.WastesByProcessType = GetWastesByProductType(_wastesLevel1, _wastesLevel2);
                    break;
                case MasterDataType.ProcessDriven:
                    //masterData.ProcessDriven =
                    //masterDataOper.ProcessDriven = await GetProcessDriven();
                    //masterData.ManufacturingPerformance = masterDataOper.ManufacturingPerformance = await GetManufacturingPerformanceNoMachine();
                    break;
                case MasterDataType.ProductionStatus:
                    masterData.Dictionary.ProductionStatus = await GetProductionStatusDictionary();
                    break;
                case MasterDataType.Units:
                    //masterData.Dictionary.Units = await GetUnitsDictionary();
                    break;
                case MasterDataType.WastesLevel1:
                    //_wastesLevel1 = await _wasteLevel1Repository.ListAsDictionary();
                    //_wastesLevel2 = await _wasteLevel2Repository.ListAsDictionary();
                    //masterData.Dictionary.WastesLevel1 = _wastesLevel1.ToDictionary(x => x.Id, x => x.Description);
                    //masterData.WastesByProcessType = masterDataOper.WastesByProcessType = GetWastesByProcessType(_wastesLevel1, _wastesLevel2);
                    break;
                case MasterDataType.WastesLevel2:
                    //_wastesLevel2 = await _wasteLevel2Repository.ListAsDictionary();
                    //masterData.Dictionary.WastesLevel2 = masterDataOper.Dictionary.WastesLevel2 = _wastesLevel2.ToDictionary(x => x.Id, x => x.Description);
                    //masterData.WastesByProcessType = masterDataOper.WastesByProcessType = GetWastesByProcessType(_wastesLevel1, _wastesLevel2);
                    break;
                case MasterDataType.MachineType:
                    masterData.Dictionary.MachineType = await GetMachineTypeDictionary();
                    break;
                case MasterDataType.ProductFamily:
                    //masterData.Dictionary.ProductFamily = await GetProductFamilyDictionary();
                    break;
                case MasterDataType.ProductGroup:
                    //masterData.Dictionary.ProductGroup = await GetProductGroupDictionary();
                    break;
                case MasterDataType.ProductType:
                    //masterData.Dictionary.ProductType = await GetProductTypeDictionary();
                    break;
                case MasterDataType.MaterialType:
                    //masterData.Dictionary.MaterialType = await GetMaterialTypeDictionary();
                    break;
                case MasterDataType.TeamType:
                    //masterData.Dictionary.TeamType = await GetTeamTypeDictionary();
                    break;
                case MasterDataType.Team:
                    //masterData.Dictionary.Team = await GetTeamDictionary();
                    break;
                case MasterDataType.UserPosition:
                    //masterData.Dictionary.UserPosition = await GetUserPositionDictionary();
                    break;
                case MasterDataType.Education:
                    //masterData.Dictionary.Education = await GetEducationDictionary();
                    break;
                case MasterDataType.ProcessType:
                    //masterData.Dictionary.ProcessType = masterDataOper.Dictionary.ProcessType = await GetProcessTypeDictionary();
                    break;
                case MasterDataType.UserGroup:
                    //masterData.Dictionary.UserGroup = await GetUserGroupDictionary();
                    break;
            }
            await _responseCacheService.SetAsync($"{Constans.RedisKey.MASTER_DATA}", masterData);
            await _responseCacheService.SetAsync($"{Constans.RedisKey.MASTER_DATA_Oper}", masterDataOper);

            return masterData;

        }

        private IDictionary<int, Dictionary<int, WasteDictionaryModel>> GetWastesByProductType(IList<WasteDictionaryModel> wastesLevel1, IList<WasteDictionaryModel> wastesLevel2)
        {
            var output = new Dictionary<int, Dictionary<int, WasteDictionaryModel>>();

            foreach (var item in wastesLevel1)
            {
                item.Sub = wastesLevel2.Where(x => x.ParentId == item.Id).ToDictionary(x => x.Id, x => x);
            }

            var productTypeIds = wastesLevel1.Where(x => x.ProductTypeId != null).Select(x => (int)x.ProductTypeId).Distinct().ToList();
            foreach (var productTypeId in productTypeIds)
            {
                var wasteByProductType = wastesLevel1.Where(x => x.ProductTypeId == productTypeId).ToDictionary(x => x.Id, x => x);
                output.Add(productTypeId, wasteByProductType);
            }
            return output;
        }

        private IDictionary<int, Dictionary<int, WasteDictionaryModel>> GetWastesByProcessType(IList<WasteDictionaryModel> wastesLevel1, IList<WasteDictionaryModel> wastesLevel2)
        {
            var output = new Dictionary<int, Dictionary<int, WasteDictionaryModel>>();

            foreach (var item in wastesLevel1)
            {
                item.Sub = wastesLevel2.Where(x => x.ParentId == item.Id).ToDictionary(x => x.Id, x => x);
            }

            var processTypeIds = wastesLevel1.Where(x => x.ProcessTypeId != null).Select(x => (int)x.ProcessTypeId).Distinct().ToList();
            foreach (var processTypeId in processTypeIds)
            {
                var wasteByprocessType = wastesLevel1.Where(x => x.ProcessTypeId == processTypeId).ToDictionary(x => x.Id, x => x);
                output.Add(processTypeId, wasteByprocessType);
            }
            return output;
        }
        public async Task Clear()
        {
            await _responseCacheService.SetAsync($"{Constans.RedisKey.MASTER_DATA}", null);
        }

        private async Task<IDictionary<int, int[]>> GetRouteMachine()
        {
            var db = (await _routeMachineRepository.AllAsync());
            var output = new Dictionary<int, int[]>();
            var routeList = db.Select(x => x.RouteId).Distinct().ToList();
            foreach (var routeId in routeList)
            {
                output[routeId] = db.Where(x => x.RouteId == routeId && x.IsActive == true).OrderBy(x => x.Sequence).Select(x => x.MachineId).ToArray();
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetProductionStatusDictionary()
        {
            var db = (await _productionStatusRepository.WhereAsync(x => x.IsActive == true));
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                output.Add(item.Id, item.Name);
            }
            return output;
        }

        private IDictionary<int, string> GetProductDictionary(IDictionary<int, ProductDictionaryModel> products)
        {
            var output = new Dictionary<int, string>();
            foreach (var item in products)
            {
                if (!output.ContainsValue(item.Value.Code))
                    output.Add(item.Key, item.Value.Code);
            }
            return output;
        }

        private async Task<IDictionary<int, ProductDictionaryModel>> GetProductByProcessTypeDictionary()
        {
            var output = new Dictionary<int, ProductDictionaryModel>();
            var productGroup = (await _productGroupRepository.WhereAsync(x => x.IsActive == true)).OrderBy(x => x.Id);
            var product = await _productsRepository.Where(x => x.IsActive == true && x.IsDelete == false)
            .Select(x => new ProductDictionaryModel
            {
                Id = x.Id,
                Code = x.Code,
            }).ToListAsync();

            foreach (var item in product)
            {
                item.ProcessTypeId = productGroup.FirstOrDefault(x => x.Id == item.GroupId).ProcessTypeId;
                output.Add(item.Id, item);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetUnitsDictionary()
        {
            var db = (await _unitsRepository.AllAsync()).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, IDictionary<int, string>>> GetProductGroupRoutes()
        {
            var db = (await _routeProductGroupRepository.WhereAsync(x => x.IsActive == true));
            var output = new Dictionary<int, IDictionary<int, string>>();

            foreach (var item in db)
            {
                if (!output.ContainsKey(item.ProductGroupId))
                {
                    var dbRoute = db.Where(x => x.ProductGroupId == item.ProductGroupId).ToDictionary(x => x.RouteId, y => y.Route.Name);
                    output.Add(item.ProductGroupId, dbRoute);
                }
            }
            return output;
        }

        private IDictionary<int, string> GetProductionPlanCompareResult()
        {
            var planCompare = new Dictionary<int, string>();
            planCompare.Add(Constans.CompareMapping.InvalidDateTime, "Imported finished date time must be further then now + 6h");
            planCompare.Add(Constans.CompareMapping.InvalidTarget, "Imported target is lower then current target");
            planCompare.Add(Constans.CompareMapping.PlanFinished, "Plan Finished");
            planCompare.Add(Constans.CompareMapping.NEW, "NEW");
            planCompare.Add(Constans.CompareMapping.NoProduct, "No Product ID");
            planCompare.Add(Constans.CompareMapping.Inprocess, "Inprocess");
            return planCompare;
        }

        private async Task<IDictionary<int, string>> GetMachineTypeDictionary()
        {
            var db = (await _machineTypeRepository.WhereAsync(x => x.IsActive == true)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        //private async Task<IDictionary<int, ProcessDrivenModel>> GetProcessDriven()
        //{
        //    var output = new Dictionary<int, ProcessDrivenModel>();
        //    var lossLevel2Db = (await _lossLevel2Repository.WhereAsync(x => x.LossLevel1Id == 2 && x.IsActive && !x.IsDelete));

        //    foreach (var item in lossLevel2Db)
        //    {
        //        var lossLevel3 = (await _lossLevel3Repository.WhereAsync(x => x.LossLevel2Id == item.Id && x.IsActive && !x.IsDelete && x.ProcessTypeId == 1))
        //            .ToDictionary(x => x.Id, y => y.Description);

        //        output.Add(item.Id, new ProcessDrivenModel()
        //        {
        //            Id = item.Id,
        //            Name = item.Name,
        //            Description = item.Description,
        //            LossLevel3 = lossLevel3
        //        });
        //    }
        //    return output;
        //}

        //private async Task<IDictionary<int, Dictionary<int, ProcessDrivenModel>>> GetProcessDrivenByProcessType()
        //{
        //    var output = new Dictionary<int, Dictionary<int, ProcessDrivenModel>>();
        //    var lossLevel2Db = (await _lossLevel2Repository.WhereAsync(x => x.LossLevel1Id == 2 && x.IsActive && !x.IsDelete));
        //    var lossLevel3 = (await _lossLevel3Repository.WhereAsync(x => lossLevel2Db.Select(o => o.Id).Contains(x.LossLevel2Id) && x.IsActive && !x.IsDelete))
        //         .Select(i => new { Id = i.Id, Description = i.Description, Lv2id = i.LossLevel2Id, processId = i.ProcessTypeId });

        //    var processTypeList = lossLevel3.Select(o => o.processId).Distinct();

        //    foreach (var item in lossLevel2Db)
        //    {
        //        foreach (int proc in processTypeList)
        //        {
        //            if (!output.ContainsKey(proc)) output.Add(proc, new Dictionary<int, ProcessDrivenModel>());

        //            output[proc].Add(
        //                       item.Id, new ProcessDrivenModel()
        //                       {
        //                           Id = item.Id,
        //                           Name = item.Name,
        //                           Description = item.Description,
        //                           LossLevel3 = lossLevel3.Where(i => i.Lv2id == item.Id && i.processId == proc).ToDictionary(t => t.Id, t => t.Description)
        //                       });
        //        }
        //    }
        //    return output;
        //}

        private async Task<IDictionary<int, ManufacturingPerformanceNoMachineModel>> GetManufacturingPerformanceNoMachine()
        {
            var output = new Dictionary<int, ManufacturingPerformanceNoMachineModel>();
            var losslv2MC = new List<int>() { 13, 14, 15, 16, 20, 18, 19 };
            var lossLevel2Db = (await _lossLevel2Repository.WhereAsync(x => losslv2MC.Contains(x.Id) && x.LossLevel1Id == 3 && x.IsActive && !x.IsDelete));

            foreach (var item in lossLevel2Db)
            {
                var lossLevel3 = (await _lossLevel3Repository.WhereAsync(x => x.LossLevel2Id == item.Id && x.IsActive && !x.IsDelete))
                    .ToDictionary(x => x.Id, y => y.Description);

                output.Add(item.Id, new ManufacturingPerformanceNoMachineModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    LossLevel3 = lossLevel3
                });
            }
            return output;
        }

        //private async Task<IDictionary<int, Dictionary<int, ManufacturingPerformanceNoMachineModel>>> GetManufacturingPerformanceNoMachineByProcessType()
        //{
        //    var output = new Dictionary<int, Dictionary<int, ManufacturingPerformanceNoMachineModel>>();
        //    var losslv2MC = new List<int>() { 13, 14, 15, 16, 20, 18, 19 };
        //    var lossLevel2Db = (await _lossLevel2Repository.WhereAsync(x => losslv2MC.Contains(x.Id) && x.LossLevel1Id == 3 && x.IsActive && !x.IsDelete));
        //    var lossLevel3 = (await _lossLevel3Repository.WhereAsync(x => lossLevel2Db.Select(o => o.Id).Contains(x.LossLevel2Id) && x.IsActive && !x.IsDelete))
        //                     .Select(i => new { Id = i.Id, Description = i.Description, Lv2id = i.LossLevel2Id });

        //    var processTypeList = lossLevel3.Select(o => o.processId).Distinct();
        //    foreach (var item in lossLevel2Db)
        //    {
        //        foreach (int proc in processTypeList)
        //        {
        //            if (!output.ContainsKey(proc)) output.Add(proc, new Dictionary<int, ManufacturingPerformanceNoMachineModel>());

        //            output[proc].Add(
        //                       item.Id, new ManufacturingPerformanceNoMachineModel()
        //                       {
        //                           Id = item.Id,
        //                           Name = item.Name,
        //                           Description = item.Description,
        //                           LossLevel3 = lossLevel3.Where(i => i.Lv2id == item.Id).ToDictionary(t => t.Id, t => t.Description)
        //                       });

        //        }
        //    }
        //    return output;
        //}


        private async Task<IDictionary<int, AppFeatureModel>> GetAppFeature()
        {
            var output = new Dictionary<int, AppFeatureModel>();
            var dbModel = await _appFeatureRepository.AllAsync();
            foreach (var item in dbModel)
            {
                output[item.FeatureId] = new AppFeatureModel
                {
                    FeatureId = item.FeatureId,
                    Name = item.Name,
                    AppId = item.AppId
                };
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetProductTypeDictionary()
        {
            var db = (await _productTypeRepository.WhereAsync(x => x.IsActive == true)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Description);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetProductFamilyDictionary()
        {
            var db = (await _productFamilyRepository.WhereAsync(x => x.IsActive == true)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Description);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetProductGroupDictionary()
        {
            var db = (await _productGroupRepository.WhereAsync(x => x.IsActive == true)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetMachineDictionary()
        {
            var db = (await _machineRepository.WhereAsync(x => x.IsActive == true)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetMaterialTypeDictionary()
        {
            var db = (await _materialTypeRepository.AllAsync()).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetTeamTypeDictionary()
        {
            var db = (await _teamTypeRepository.WhereAsync(x => x.IsActive && !x.IsDelete)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetTeamDictionary()
        {
            var db = (await _teamRepository.WhereAsync(x => x.IsActive.Value && !x.IsDelete)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetUserPositionDictionary()
        {
            var db = (await _userPositionRepository.WhereAsync(x => x.IsActive.Value)).OrderBy(x => x.Name);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetEducationDictionary()
        {
            var db = (await _educationRepository.WhereAsync(x => x.IsActive.Value)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Educational);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetProcessTypeDictionary()
        {
            var db = (await _processTypeRepository.AllAsync()).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetUserGroupDictionary()
        {
            var db = (await _userGroupRepository.WhereAsync(x => x.IsActive && !x.IsDelete)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<string, string>> GetLanguageDictionary()
        {
            var output = new Dictionary<string, string>();
            output.Add("en", "EN");
            output.Add("th", "TH");
            return output;
        }

        private async Task<IDictionary<int, string>> GetAppDictionary()
        {
            var db = (await _appRepository.WhereAsync(x => x.IsActive && !x.IsDelete)).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, AppModel>> GetLanguageUrlDictionary()
        {
            var db = await _appRepository.WhereAsync(x => x.IsActive && !x.IsDelete);
            var output = new Dictionary<int, AppModel>();

            foreach (var item in db)
            {
                output[item.Id] = new AppModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Url = item.Url,
                    ThUrl = item.ThUrl
                };
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetColorDictionary()
        {
            var db = (await _colorRepository.AllAsync()).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name.Replace("\r\n", string.Empty).Trim());
            }
            return output;
        }

        private async Task<IDictionary<int, List<WasteModel>>> GetWasteByMachineAsync(IList<WasteModel> waste)
        {
            var output = new Dictionary<int, List<WasteModel>>();
            var machineDb = await _machineRepository.WhereAsync(x => x.IsActive);

            foreach (var item in machineDb)
            {
                output[item.Id] = waste.Where(x => x.MachineId == item.Id).Select(x => MapperHelper.AsModel(x, new WasteModel())).ToList();
            }
            return output;
        }
    }
}
