using AureliaWithDotNet.Data;
using AureliaWithDotNet.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AureliaWithDotNet.IntegrationTest
{
    [SetUpFixture]
    public class Setup
    {
        private WebApplicationFactory<Startup> webAppFactory;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            StartApiServer();

            BaseIntegrationTest.WebAppFactory = webAppFactory;

            await webAppFactory.MigrateDbAndSeedAsync();
        }

        private void StartApiServer() => webAppFactory = new WebApplicationFactory<Startup>().EnsureServerStarted();

        [OneTimeTearDown]
        public void OneTimeTearDown() => webAppFactory?.Dispose();
    }

    public static class WebApplicationFactoryExtensions
    {
        public static async Task MigrateDbAndSeedAsync<TStartup>(this WebApplicationFactory<TStartup> webApplicationFactory) where TStartup : class
        {
            var services = webApplicationFactory.Services;

            using (var scope = services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AureliaWithDotNetDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<WebApplicationFactory<Startup>>>();
                await db.Database.EnsureCreatedAsync();
            }
        }

        public static WebApplicationFactory<TStartup> EnsureServerStarted<TStartup>(this WebApplicationFactory<TStartup> webApplicationFactory) where TStartup : class
        {
            webApplicationFactory.CreateDefaultClient();
            return webApplicationFactory;
        }
    }
}
