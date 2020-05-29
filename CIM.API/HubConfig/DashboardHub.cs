using CIM.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.API.HubConfig
{
    public class DashboardHub : Hub
    {
        public async Task BroadcastDashboardHub(List<MachineCacheModel> data) => await Clients.All.SendAsync("broadcastdashboarddata", data);

    }
}
