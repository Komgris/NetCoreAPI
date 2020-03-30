using CIM.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.API.HubConfig
{
    public class MachineHub : Hub
    {
        public async Task BroadcastMachineData(List<MachineCacheModel> data) => await Clients.All.SendAsync("broadcastmachinedata", data);

    }
}
