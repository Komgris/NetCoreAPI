using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    public class BaseController : ControllerBase {

        internal IHubContext<GlobalHub> _hub;
        internal IResponseCacheService _responseCacheService;
        internal IReportService _service;

        #region general

        public JsonSerializerSettings JsonsSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

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

        internal async Task BoardcastClientData(string channel, object data)
        {
            await _hub.Clients.All.SendAsync(channel, JsonConvert.SerializeObject(data, JsonsSetting));
        }

        #endregion

        #region Management

        internal async Task SetBoardcastDataCached(string channelKey, BoardcastModel model)
        {
            var cache = await GetCached<BoardcastModel>(channelKey);
            if (cache == null)
            {
                cache = model;
            }
            else
            {
                foreach (BoardcastDataModel dashboard in model.Data)
                {
                    cache.SetData(dashboard);
                }
            }
            await _responseCacheService.SetAsync(channelKey, cache);
        }

        internal async Task HandleBoardcastingManagementData(string channelKey, BoardcastModel boardcastData)
        {
            if (boardcastData != null)
            {
                await SetBoardcastDataCached(channelKey, boardcastData);
                await BoardcastClientData(channelKey, boardcastData);
            }
        }

        #endregion

        #region Operation
        internal async Task HandleBoardcastingActiveProcess(BoardcastType updateType, string productionPlan, int routeId, ActiveProductionPlanModel activeModel)
        {
            var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan}";
            var boardcastData = await _service.GenerateBoardcastData(updateType, productionPlan, routeId);
            if (boardcastData.Data.Count > 0)
            {
                await SetBoardcastActiveDataCached(rediskey, routeId, activeModel, boardcastData);

                activeModel.ActiveProcesses[routeId].BoardcastData = boardcastData;
                await BoardcastClientData(channelKey, activeModel);
            }
        }

        internal async Task SetBoardcastActiveDataCached(string channelKey, int routeId, ActiveProductionPlanModel activeModel, BoardcastModel model)
        {
            var cache = activeModel.ActiveProcesses[routeId].BoardcastData;
            if(cache is null)
            {
                activeModel.ActiveProcesses[routeId].BoardcastData = model;
            }
            else
            {
                foreach (BoardcastDataModel dashboard in model.Data)
                {
                    cache.SetData(dashboard);
                }
                activeModel.ActiveProcesses[routeId].BoardcastData = cache;
            }

            await _responseCacheService.SetAsync(channelKey, activeModel);
        }

        #endregion

    }
}