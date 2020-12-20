using Microsoft.Extensions.DependencyInjection;
using Taxes.Services.Services;

namespace Taxes.Services
{
    public class ServiceRegistry
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<MunicipalitiesService>();
            services.AddScoped<TaxSchedulerService>();
        }
    }
}
