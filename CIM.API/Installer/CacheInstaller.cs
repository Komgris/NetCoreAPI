using CIM.API.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.API.Installer
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if (!redisCacheSettings.Enabled)
            {
                return;
            }

            services.AddStackExchangeRedisCache(options =>
            {
                //var conf = new StackExchange.Redis.ConfigurationOptions();
                //conf.EndPoints.Add("localhost:6379");
                //conf.SyncTimeout = 4000;
                //conf.AbortOnConnectFail = false;
                //options.ConfigurationOptions = conf;
                options.Configuration = redisCacheSettings.ConnectionString;
            }
            
            
            ) ;
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }

    }
}
