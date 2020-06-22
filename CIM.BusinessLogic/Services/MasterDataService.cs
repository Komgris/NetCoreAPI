using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class MasterDataService : IMasterDataService
    {
        private ILossLevel2Repository _lossLevel2Repository;
        private ILossLevel3Repository _lossLevel3Repository;
        private IResponseCacheService _responseCacheService;
        private IRouteRepository _routeRepository;
        private IRouteMachineRepository _routeMachineRepository;
        private IRouteProductGroupRepository _routeProductGroupRepository;
        private IComponentRepository _machineComponentRepository;
        private IMachineRepository _machineRepository;
        private IProductionStatusRepository _productionStatusRepository;
        private IProductRepository _productsRepository;
        private IProductionPlanRepository _productionPlanRepository;
        private IUnitsRepository _unitsRepository;
        private IWasteLevel1Repository _wasteLevel1Repository;
        private IWasteLevel2Repository _wasteLevel2Repository;
        private IMaterialRepository _materialRepository;
        private IMachineTypeRepository _machineTypeRepository;
        private IComponentTypeRepository _componentTypeRepository;
        private IProductTypeRepository _productTypeRepository;
        private IProductGroupRepository _productGroupRepository;
        private IProductFamilyRepository _productFamilyRepository;
        private IMaterialTypeRepository _materialTypeRepository;
        private ITeamTypeRepository _teamTypeRepository;

        public MasterDataService(
            ILossLevel2Repository lossLevel2Repository,
            ILossLevel3Repository lossLevel3Repository,
            IResponseCacheService responseCacheService,
            IRouteRepository routeRepository,
            IRouteMachineRepository routeMachineRepository,
            IRouteProductGroupRepository routeProductGroupRepository,
            IMachineRepository machineRepository,
            IComponentRepository machineComponentRepository,
            IProductionStatusRepository productionStatusRepository,
            IProductRepository productRepository,
            IProductionPlanRepository productionPlanRepository,
            IUnitsRepository unitsRepository,
            IWasteLevel1Repository wasteLevel1Repository,
            IWasteLevel2Repository wasteLevel2Repository,
            IMaterialRepository materialRepository,
            IMachineTypeRepository machineTypeRepository,
            IComponentTypeRepository componentTypeRepository,
            IProductTypeRepository productTypeRepository,
            IProductGroupRepository productGroupRepository,
            IProductFamilyRepository productFamilyRepository,
            IMaterialTypeRepository materialTypeRepository,
            ITeamTypeRepository teamTypeRepository
            )
        {
            _lossLevel2Repository = lossLevel2Repository;
            _lossLevel3Repository = lossLevel3Repository;
            _responseCacheService = responseCacheService;
            _routeRepository = routeRepository;
            _routeMachineRepository = routeMachineRepository;
            _routeProductGroupRepository = routeProductGroupRepository;
            _machineComponentRepository = machineComponentRepository;
            _machineRepository = machineRepository;
            _productionStatusRepository = productionStatusRepository;
            _productsRepository = productRepository;
            _productionPlanRepository = productionPlanRepository;
            _unitsRepository = unitsRepository;
            _wasteLevel1Repository = wasteLevel1Repository;
            _wasteLevel2Repository = wasteLevel2Repository;
            _materialRepository = materialRepository;
            _machineTypeRepository = machineTypeRepository;
            _componentTypeRepository = componentTypeRepository;
            _productFamilyRepository = productFamilyRepository;
            _productGroupRepository = productGroupRepository;
            _productTypeRepository = productTypeRepository;
            _materialTypeRepository = materialTypeRepository;
            _teamTypeRepository = teamTypeRepository;
        }
        public MasterDataModel Data { get; set; }

        private IList<LossLevelComponentMappingModel> _lossLevel3ComponentMapping;
        private IList<LossLevelMachineMappingModel> _lossLevel3MachineMapping;
        private IList<LossLevel3DictionaryModel> _lossLevel3s;
        private IList<WasteDictionaryModel> _wastesLevel1;
        private IList<WasteDictionaryModel> _wastesLevel2;
        private IList<MaterialDictionaryModel> _productBOM;

        private IDictionary<int, LossLevel3DictionaryModel> GetLossLevel3()
        {
            var output = new Dictionary<int, LossLevel3DictionaryModel>();
            foreach (var item in _lossLevel3s)
            {
                output[item.Id] = new LossLevel3DictionaryModel
                {
                    Id = item.Id,
                    Name = $"{item.Name} - {item.Description}",
                    Components = _lossLevel3ComponentMapping.Where(x => x.LossLevelId == item.Id).Select(x => x.ComponentId).ToArray()
                };
            }
            return output;
        }


        private async Task<IDictionary<int, MachineComponentModel>> GetComponents()
        {
            var output = new Dictionary<int, MachineComponentModel>();
            var activeMachines = await _machineComponentRepository.AllAsync();
            foreach (var item in activeMachines)
            {
                output[item.Id] = new MachineComponentModel
                {

                    Id = item.Id,
                    Name = item.Name,
                    MachineId = item.MachineId,
                    TypeId = item.TypeId,
                    LossList = _lossLevel3ComponentMapping.Where(x => x.ComponentId == item.Id).Select(x => x.LossLevelId).ToArray(),

                };
            }
            return output;
        }

        private async Task<IDictionary<int, MachineModel>> GetMachines(IDictionary<int, MachineComponentModel> components, IDictionary<int, int[]> routeMachines)
        {
            var output = new Dictionary<int, MachineModel>();
            var activeMachines = await _machineRepository.WhereAsync(x => x.IsActive && !x.IsDelete);

            foreach (var item in activeMachines)
            {
                var machineComponents = components.Where(x => x.Value.MachineId == item.Id);
                output[item.Id] = new MachineModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ComponentList = machineComponents.Select(x => x.Value).ToList(),
                    LossList = _lossLevel3MachineMapping.Where(x => x.MachineId == item.Id).Select(x => x.LossLevelId).ToArray(),
                    RouteList = routeMachines.Where(x => x.Value.Contains(item.Id)).Select(x => x.Key).ToList()
                };
            }
            return output;
        }

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
                        MachineList = routeMachines[item.Id].Distinct().ToDictionary(x => x, x => machines[x])
                    };
                }
            }
            return output;
        }

        private async Task<IDictionary<string, ProductionPlanDictionaryModel>> GetProductionPlan(IDictionary<int, ProductDictionaryModel> products)
        {
            var output = new Dictionary<string, ProductionPlanDictionaryModel>();
            var dbModel = await _productionPlanRepository.WhereAsync(x=>x.IsActive == true && x.Product.IsActive == true);
            foreach (var item in dbModel)
            {
                output[item.PlanId] = new ProductionPlanDictionaryModel
                {
                    PlanId = item.PlanId,
                    Product = products[item.ProductId],
                    RouteId = item.RouteId,
                };
            }
            return output;
        }


        public async Task<MasterDataModel> GetData()
        {

            if (Data == null)
            {
                Data = await _responseCacheService.GetAsTypeAsync<MasterDataModel>($"{Constans.RedisKey.MASTER_DATA}");

                if (Data == null)
                {
                    Data = await Refresh();
                }

            }
            return Data;
        }

        public async Task<MasterDataModel> Refresh()
        {

            _lossLevel3ComponentMapping = await _lossLevel3Repository.ListComponentMappingAsync();
            _lossLevel3MachineMapping = await _lossLevel3Repository.ListMachineMappingAsync();
            _lossLevel3s = (await _lossLevel3Repository.AllAsync()).Select(x => MapperHelper.AsModel(x, new LossLevel3DictionaryModel())).ToList();
            _wastesLevel1 = await _wasteLevel1Repository.ListAsDictionary();
            _wastesLevel2 = await _wasteLevel2Repository.ListAsDictionary();
            _productBOM = await _materialRepository.ListProductBOM();

            var masterData = new MasterDataModel();
            masterData.LossLevel3s = GetLossLevel3();
            masterData.RouteMachines = await GetRouteMachine();
            masterData.Components = await GetComponents();
            masterData.Machines = await GetMachines(masterData.Components, masterData.RouteMachines);
            masterData.Routes = await GetRoutes(masterData.RouteMachines, masterData.Machines);
            masterData.Products = await _productsRepository.ListAsDictionary(_productBOM);
            masterData.ProductionPlan = await GetProductionPlan(masterData.Products);
            masterData.ProductGroupRoutes = await GetProductGroupRoutes();
            masterData.WastesByProductType = GetWastesByProductType(_wastesLevel1, _wastesLevel2);
            masterData.ProcessDriven = await GetProcessDriven();

            masterData.Dictionary.Products = GetProductDictionary(masterData.Products);
            masterData.Dictionary.ProductsByCode = masterData.Dictionary.Products.ToDictionary(x => x.Value, x => x.Key);
            masterData.Dictionary.ProductionStatus = await GetProductionStatusDictionary();
            masterData.Dictionary.Units = await GetUnitsDictionary();
            masterData.Dictionary.CompareResult = GetProductionPlanCompareResult();
            masterData.Dictionary.WastesLevel2 = _wastesLevel2.ToDictionary(x => x.Id, x => x.Description);
            masterData.Dictionary.MachineType = await GetMachineTypeDictionary();
            masterData.Dictionary.ComponentType = await GetComponentTypeDictionary();
            masterData.Dictionary.ProductFamily = await GetProductFamilyDictionary();
            masterData.Dictionary.ProductGroup = await GetProductGroupDictionary();
            masterData.Dictionary.ProductType = await GetProductTypeDictionary();
            masterData.Dictionary.Machine = await GetMachineDictionary();
            masterData.Dictionary.MaterialType = await GetMaterialTypeDictionary();
            masterData.Dictionary.TeamType = await GetTeamTypeDictionary();

            await _responseCacheService.SetAsync($"{Constans.RedisKey.MASTER_DATA}", masterData);
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
                output[routeId] = db.Where(x => x.RouteId == routeId).Select(x => x.MachineId).ToArray();
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

        private async Task<IDictionary<int, string>> GetUnitsDictionary()
        {
            var db = (await _unitsRepository.AllAsync()).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Uom);
            }
            return output;
        }

        private async Task<IDictionary<int, IDictionary<int, string>>> GetProductGroupRoutes()
        {
            var db = (await _routeProductGroupRepository.AllAsync());
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
            var db = (await _machineTypeRepository.AllAsync()).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, ProcessDrivenModel>> GetProcessDriven()
        {
            var output = new Dictionary<int, ProcessDrivenModel>();
            var lossLevel2Db = (await _lossLevel2Repository.WhereAsync(x => x.LossLevel1Id == 2 && x.IsActive && !x.IsDelete));

            foreach (var item in lossLevel2Db)
            {
                var lossLevel3 = (await _lossLevel3Repository.WhereAsync(x => x.LossLevel2Id == item.Id && x.IsActive && !x.IsDelete))
                    .ToDictionary(x => x.Id, y => y.Description);

                output.Add(item.Id, new ProcessDrivenModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    LossLevel3 = lossLevel3
                });
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetComponentTypeDictionary()
        {
            var db = (await _componentTypeRepository.AllAsync()).OrderBy(x => x.Id);
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsKey(item.Id))
                    output.Add(item.Id, item.Name);
            }
            return output;
        }

        private async Task<IDictionary<int, string>> GetProductTypeDictionary()
        {
            var db = (await _productTypeRepository.AllAsync()).OrderBy(x => x.Id);
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
            var db = (await _productFamilyRepository.AllAsync()).OrderBy(x => x.Id);
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
            var db = (await _productGroupRepository.AllAsync()).OrderBy(x => x.Id);
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
            var db = (await _machineRepository.AllAsync()).OrderBy(x => x.Id);
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

    }
}
