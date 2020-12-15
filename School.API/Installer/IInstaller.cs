using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace School.API.Installer
{
    public interface IInstaller
    {
        void InstallServicesAssembly(IServiceCollection services, IConfiguration configuration);
    }
}
