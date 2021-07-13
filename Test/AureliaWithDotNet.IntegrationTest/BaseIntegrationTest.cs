using AureliaWithDotNet.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace AureliaWithDotNet.IntegrationTest
{
    public abstract class BaseIntegrationTest
    {
        private IServiceScope scope;
        protected IServiceProvider serviceProvider;
        protected HttpClient client;

        public static WebApplicationFactory<Startup> WebAppFactory { get; set; }

        [OneTimeSetUp]
        public void BaseIntegrationTestOneTimeSetUp()
        {
            client = WebAppFactory.CreateDefaultClient();
            scope = WebAppFactory.Services.CreateScope();
            serviceProvider = scope.ServiceProvider;
        }

        [OneTimeTearDown]
        public void BaseIntegrationTestOneTimeTearDown() => scope.Dispose();
    }
}
