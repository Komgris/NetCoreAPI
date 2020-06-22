using CIM.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.API.HubConfig
{
    public class GlobalHub : Hub
    {
        public async Task BroadcastData(object data) => await Clients.All.SendAsync("broadcastdata", data);
    }
}
