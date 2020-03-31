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
        private IRouteMachineRepository _routeMachineRepository;
        private IMachineComponentRepository _machineComponentRepository;
        private IMachineRepository _machineRepository;

        public MasterDataService(
            IRouteRepository routeRepository,
            IRouteMachineRepository routeMachineRepository,
            IMachineRepository machineRepository,
            IMachineComponentRepository machineComponentRepository
            )
        {
            _routeRepository = routeRepository;
            _routeMachineRepository = routeMachineRepository;
            _machineComponentRepository = machineComponentRepository;
            _machineRepository = machineRepository;
        }

        public IDictionary<int, MachineComponentModel> Components { get; set; }
        public IDictionary<int, MachineModel> Machines { get; set; }
        public IDictionary<int, int[]> RouteMachines { get; set; }
        public IDictionary<int, RouteModel> Routes { get; set; }

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
                };
            }
            return output;
        }

        private async Task<IDictionary<int, MachineModel>> GetMachines(IDictionary<int, MachineComponentModel> components)
        {
            var output = new Dictionary<int, MachineModel>();
            var activeMachines = await _machineRepository.WhereAsync( x=>x.IsActive && !x.IsDelete);
                
            foreach (var item in activeMachines)
            {
                output[item.Id] = new MachineModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ComponentList = components.Where(x=>x.Value.MachineId == item.Id).ToDictionary( x=>x.Key, x=>x.Value)
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
                    MachineList = routeMachines[item.Id].ToDictionary( x=>x, x=> machines[x])
                };
            }
            return output;
        }


        public async Task GetData()
        {
            RouteMachines = await GetRouteMachine();
            Components = await GetComponents();
            Machines = await GetMachines(Components);
            Routes = await GetRoutes(RouteMachines, Machines);
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
    }
}
