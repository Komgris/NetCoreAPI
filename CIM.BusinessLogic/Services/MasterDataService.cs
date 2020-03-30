using CIM.BusinessLogic.Interfaces;
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
        private IRouteRepository _routeRepository;
        private IMachineComponentRepository _machineComponentRepository;
        private IMachineRepository _machineRepository;

        public MasterDataService(
            IRouteRepository routeRepository,
            IMachineRepository machineRepository,
            IMachineComponentRepository machineComponentRepository
            )
        {
            _routeRepository = routeRepository;
            _machineComponentRepository = machineComponentRepository;
            _machineRepository = machineRepository;

        }

        public IDictionary<int, ComponentModel> Components { get; set; }
        public IDictionary<int, MachineModel> Machines { get; set; }
        public IDictionary<int, RouteModel> Routes { get; set; }

        private async Task<IDictionary<int, ComponentModel>> GetComponents()
        {
            var output = new Dictionary<int, ComponentModel>();
            var activeMachines = await _machineComponentRepository.AllAsync();
            foreach (var item in activeMachines)
            {
                output[item.Id] = new ComponentModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    MachineId = item.MachineId,
                };
            }
            return output;
        }

        private async Task<IDictionary<int, MachineModel>> GetMachines()
        {
            var output = new Dictionary<int, MachineModel>();
            var activeMachines = await _machineRepository.WhereAsync(x => x.IsActive && x.IsDelete == false);
            foreach (var item in activeMachines)
            {
                output[item.Id] = new MachineModel
                {
                    Id = item.Id,
                    Name = item.Name,
                };
            }
            return output;
        }

        private async Task<IDictionary<int, RouteModel>> GetRoutes()
        {
            var output = new Dictionary<int, RouteModel>();
            var dbModel = await _routeRepository.AllAsync();
            foreach (var item in dbModel)
            {
                output[item.Id] = new RouteModel
                {
                    Id = item.Id,
                    Name = item.Name,
                };
            }
            return output;
        }


        public async Task GetData()
        {
            Components = await GetComponents();
            Machines = await GetMachines();
            Routes = await GetRoutes();
        }

    }
}
