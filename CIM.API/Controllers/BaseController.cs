using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    [MiddlewareFilter(typeof(CustomAuthenticationMiddlewarePipeline))]
    public class BaseController : ControllerBase {

        internal IMasterDataService _masterDataService;
        internal IHubContext<GlobalHub> _hub;

        #region general

        public JsonSerializerSettings JsonsSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public JsonSerializerSettings JsonsFormatting = new JsonSerializerSettings
        {
            DateFormatString = "dd-MMM-yyyy HH:mm:ss"
        };

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        internal async Task<string> RefreshMasterData(MasterDataType masterdataType)
        {
            await _masterDataService.Refresh(masterdataType);
            var master = await _masterDataService.GetData();
            await _hub.Clients.All.SendAsync(Constans.SIGNAL_R_CHANNEL.CHANNEL_MASTER_DATA, JsonConvert.SerializeObject(master, JsonsSetting));
            return "OK";
        }

        #endregion
    }
}