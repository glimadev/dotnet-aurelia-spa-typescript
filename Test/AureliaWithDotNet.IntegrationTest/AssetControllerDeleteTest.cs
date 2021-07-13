using FluentAssertions;
using AureliaWithDotNet.Domain.Interfaces;
using AureliaWithDotNet.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AureliaWithDotNet.IntegrationTest
{
    public class AssetControllerDeleteTest : BaseIntegrationTest
    {
        private HttpResponseMessage response;
        private HttpResponseMessage response2;
        private IAssetRepository assetRepository;
        private Asset asset;

        [OneTimeSetUp]
        public async Task Setup()
        {
            assetRepository = serviceProvider.GetService<IAssetRepository>();

            asset = new()
            {
                AssetName = "Test",
                Department = 1,
                CountryOfDepartment = "Brazil",
                EMailAdressOfDepartment = "teste@gmail.com",
                PurchaseDate = DateTime.Now,
                Broken = true
            };

            assetRepository.Add(asset);

            response = await client.DeleteAsync($"/api/asset/{asset.Id}");

            response2 = await client.DeleteAsync($"/api/asset/{asset.Id}");
        }

        [Test]
        public async Task DataWasDeleted()
        {
            var result = await assetRepository.GetById(asset.Id);
            result.Should().BeNull();
        }

        [Test]
        public void StatusCodeIsOk() => response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);

        [Test]
        public void StatusNotFound() => response2.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
    }
}
