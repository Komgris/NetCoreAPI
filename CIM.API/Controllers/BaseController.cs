using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CIM.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public JsonSerializerSettings JsonsSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
}
}