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
    public class AssetControllerPutTest : BaseIntegrationTest
    {
        private HttpResponseMessage responseFail;
        private HttpResponseMessage responseOK;

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

                asset.CountryOfDepartment = null;

                responseFail = await client.PutAsJsonAsync($"/api/asset/{asset.Id}", asset);

                asset.AssetName = "Test2";
                asset.EMailAdressOfDepartment = "teste@gmail.com";
                asset.CountryOfDepartment = "Brasil";
                asset.Department = 1;
                asset.Broken = false;

                responseOK = await client.PutAsJsonAsync($"/api/asset/{asset.Id}", asset);
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
        public async Task FailWasReturned()
        {
            var failWasReturned = await responseFail.Content.ReadFromJsonAsync<ResultServiceVM>();
            failWasReturned.Success.Should().Be(false);
            failWasReturned.Messages.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Test]
        public void StatusCodeIsFail() => responseFail.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);

        [Test]
        public void StatusCodeIsOK() => responseOK.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
    }
}
