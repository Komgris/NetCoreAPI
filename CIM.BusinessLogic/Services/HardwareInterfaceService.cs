using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;

namespace CIM.BusinessLogic.Services
{
    public class HardwareInterfaceService : BaseService, IHardwareInterfaceService {

        string networkKey = "network";
        IResponseCacheService _responseCacheService;
        public HardwareInterfaceService(
            IResponseCacheService responseCacheService)
        {
            _responseCacheService = responseCacheService;
        }
        public async Task<object> GetNetworkStatus()
        {
            var network = await _responseCacheService.GetAsTypeAsync<Dictionary<string, NetworkStatusModel>>(networkKey);
            return network;
        }

        public async Task UpdateNetworkStatus(List<NetworkStatusModel> listData)
        {
            var network = await _responseCacheService.GetAsTypeAsync<Dictionary<string, NetworkStatusModel>>(networkKey)
                ?? new Dictionary<string, NetworkStatusModel>();

            foreach (var device in listData)
            {
                network[device.Ip] = device;
            }

            await _responseCacheService.SetAsync(networkKey, network);
        }
    }

}
