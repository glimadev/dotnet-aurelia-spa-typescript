using FluentAssertions;
using AureliaWithDotNet.Domain.Interfaces;
using AureliaWithDotNet.Domain.Models;
using AureliaWithDotNet.Web.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AureliaWithDotNet.IntegrationTest
{
    public class AssetControllerGetTest : BaseIntegrationTest
    {
        private HttpResponseMessage response;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var assetRepository = serviceProvider.GetService<IAssetRepository>();

            Asset asset = new()
            {
                AssetName = "Test",
                Department = 1,
                CountryOfDepartment = "Brazil",
                EMailAdressOfDepartment = "teste@gmail.com",
                PurchaseDate = DateTime.Now,
                Broken = true
            };

            try
            {
                assetRepository.Add(asset);

                response = await client.GetAsync($"/api/asset/{asset.Id}");
            }
            catch 
            {
                throw;
            }
            finally
            {
                assetRepository.Delete(asset);
            }
        }

        [Test]
        public async Task DataWasReturned()
        {
            var result = await response.Content.ReadFromJsonAsync<ResultServiceDataVM<Asset>>();
            result.Success.Should().Be(true);
            result.Data.Should().NotBeNull();
        }

        [Test]
        public void StatusCodeIsOk() => response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
    }
}
