using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IResponseCacheService
    {
        Task SetAsync(string key, object model);

        Task<string> GetAsync(string key);
        Task<T> GetAsTypeAsync<T>(string key);
        Task RemoveAsync(string key);
    }
}
