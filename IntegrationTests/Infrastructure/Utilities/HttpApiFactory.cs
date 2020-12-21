using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TaxScheduler;

namespace IntegrationTests.Infrastructure.Utilities
{
    public class HttpApiFactory: WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.UseSetting("ConnectionStrings:TaxesDb", DatabaseUtility.ConnectionString);
        }
    }
}
