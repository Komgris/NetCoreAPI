using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIM.API.Installer
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}