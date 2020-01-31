using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.API.Cache
{
    public interface IResponseCacheService
    {
        Task SetAsync(string key, object model);

        Task<string> GetAsync(string key);
    }
}
