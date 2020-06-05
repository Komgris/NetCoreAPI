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

        internal async Task BoardcastClientData<T>(string channel, object data)
        {
            await _hub.Clients.All.SendAsync(channel, ObjectForBoardcast<T>(data));
        }

        #endregion

        #region Management

        internal async Task SetBoardcastDataCached(string channelKey, BoardcastModel model)
        {
            var cache = await GetCached<BoardcastModel>(channelKey);
            if (cache == null)
            {
                await _responseCacheService.SetAsync(channelKey, model);
            }
            else
            {
                foreach (BoardcastDataModel dashboard in model.Data)
                {
                    cache.SetData(dashboard);
                }
                await _responseCacheService.SetAsync(channelKey, cache);
            }
        }

        internal async Task HandleBoardcastingManagementData(string channelKey, BoardcastModel boardcastData)
        {
            if (boardcastData != null)
            {
                await SetBoardcastDataCached(channelKey, boardcastData);
                await BoardcastClientData<BoardcastModel>(channelKey, boardcastData);
            }
        }

        #endregion

        #region Operation

        internal async Task HandleBoardcastingActiveProcess(BoardcastType updateType, string productionPlan, int routeId, ActiveProductionPlanModel activeModel)
        {
            var channelKey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var boardcastData = await _service.GenerateBoardcastData(updateType, productionPlan, routeId);
            if (boardcastData.Data.Count > 0)
            {
                activeModel.ActiveProcesses[routeId].BoardcastData = boardcastData;
                await BoardcastClientData<ActiveProductionPlanModel>(channelKey, activeModel);
                await SetBoardcastActiveDataCached(channelKey, routeId, activeModel, boardcastData);
            }
        }
        internal async Task SetBoardcastActiveDataCached(string channelKey, int routeId, ActiveProductionPlanModel activeModel, BoardcastModel model)
        {
            foreach (BoardcastDataModel dashboard in model.Data)
            {
                activeModel.ActiveProcesses[routeId].BoardcastData.SetData(dashboard);
            }
            await _responseCacheService.SetAsync(channelKey, activeModel);
        }

        #endregion

    }
}