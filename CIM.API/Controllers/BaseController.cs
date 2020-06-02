using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace CIM.API.Controllers
{
    public class BaseController : ControllerBase {

        internal IHubContext<GlobalHub> _hub;
        internal IResponseCacheService _responseCacheService;

        public JsonSerializerSettings JsonsSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        internal async Task BoardcastingDashboard<T>(string channel,object data)
        {
            await _hub.Clients.All.SendAsync(channel, ObjectForBoardcast<T>(data));
        }

        internal T ObjectForBoardcast<T>(object obj)
        {
            var dataString = JsonConvert.SerializeObject(obj, JsonsSetting);
            return JsonConvert.DeserializeObject<T>(dataString);
        }

        internal string CacheForBoardcast<T>(string cache)
        {
            var model = JsonConvert.DeserializeObject<T>(cache);
            return JsonConvert.SerializeObject(model, JsonsSetting);
        }

        internal async Task<string> GetCached(string channelKey)
        {
            return await _responseCacheService.GetAsync(channelKey);
        }

        internal async Task<T> GetCached<T>(string channelKey)
        {
            return await _responseCacheService.GetAsTypeAsync<T>(channelKey);
        }
    }
}