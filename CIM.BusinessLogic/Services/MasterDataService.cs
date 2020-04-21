﻿using CIM.BusinessLogic.Interfaces;
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
        private ILossLevel3Repository _lossLevel3Repository;
        private IResponseCacheService _responseCacheService;
        private IRouteRepository _routeRepository;
        private IRouteMachineRepository _routeMachineRepository;
        private IRouteProductGroupRepository _routeProductGroupRepository;
        private IMachineComponentRepository _machineComponentRepository;
        private IMachineRepository _machineRepository;
        private IProductionStatusRepository _productionStatusRepository;
        private IProductRepository _productsRepository;

        public MasterDataService(
            ILossLevel3Repository lossLevel3Repository,
            IResponseCacheService responseCacheService,
            IRouteRepository routeRepository,
            IRouteMachineRepository routeMachineRepository,
            IRouteProductGroupRepository routeProductGroupRepository,
            IMachineRepository machineRepository,
            IMachineComponentRepository machineComponentRepository,
            IProductionStatusRepository productionStatusRepository,
            IProductRepository productRepository
            )
        {
            _lossLevel3Repository = lossLevel3Repository;
            _responseCacheService = responseCacheService;
            _routeRepository = routeRepository;
            _routeMachineRepository = routeMachineRepository;
            _routeProductGroupRepository = routeProductGroupRepository;
            _machineComponentRepository = machineComponentRepository;
            _machineRepository = machineRepository;
            _productionStatusRepository = productionStatusRepository;
            _productsRepository = productRepository;
        }
        public MasterDataModel Data { get; set; }

        private IList<LossLevelComponentMappingModel> _lossLevel3ComponentMapping;
        private IList<LossLevelMachineMappingModel> _lossLevel3MachineMapping;
        private IList<LossLevel3Model> _lossLevel3s;

        private IDictionary<int, LossLevel3Model> GetLossLevel3()
        {
            var output = new Dictionary<int, LossLevel3Model>();
            foreach (var item in _lossLevel3s)
            {
                output[item.Id] = new LossLevel3Model
                {
                    Id = item.Id,
                    Name = item.Name,
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

        private async Task<IDictionary<int, MachineModel>> GetMachines(IDictionary<int, MachineComponentModel> components)
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
                output[item.Id] = new RouteModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    MachineList = routeMachines[item.Id].ToDictionary(x => x, x => machines[x])
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
            _lossLevel3s = (await _lossLevel3Repository.AllAsync()).Select(x => MapperHelper.AsModel(x, new LossLevel3Model())).ToList();

            var masterData = new MasterDataModel();
            masterData.LossLevel3s = GetLossLevel3();
            masterData.RouteMachines = await GetRouteMachine();
            masterData.Components = await GetComponents();
            masterData.Machines = await GetMachines(masterData.Components);
            masterData.Routes = await GetRoutes(masterData.RouteMachines, masterData.Machines);
            masterData.ProductGroupRoutes = await GetProductGroupRoutes();

            masterData.Dictionary.Products = await GetProductDictionary();
            masterData.Dictionary.Lines.Add("Line001", "Line001");
            masterData.Dictionary.ComponentAlerts.Add(1, new { Name = "Error", Description = "Some description" });
            masterData.Dictionary.ProductionStatus = await GetProductionStatusDictionary();
            await _responseCacheService.SetAsync($"{Constans.RedisKey.MASTER_DATA}", masterData);
            return masterData;

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

        private async Task<IDictionary<int, string>> GetProductDictionary()
        {
            var db = (await _productsRepository.WhereAsync(x => x.IsActive == true));
            var output = new Dictionary<int, string>();
            foreach (var item in db)
            {
                if (!output.ContainsValue(item.Code))
                    output.Add(item.Id, item.Code);
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

    }
}