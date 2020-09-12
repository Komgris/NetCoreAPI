using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    public class BaseNoSecureController : ControllerBase {

        internal IMasterDataService _masterDataService;

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
            return "OK";
        }

        #endregion
    }
}