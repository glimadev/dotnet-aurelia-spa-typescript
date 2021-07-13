using FluentAssertions;
using AureliaWithDotNet.Domain.Models;
using AureliaWithDotNet.Web.ViewModels;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AureliaWithDotNet.IntegrationTest
{
    public class AssetControllerPostTest : BaseIntegrationTest
    {
        private HttpResponseMessage responseFail;
        private HttpResponseMessage responseOK;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Asset asset = new();
            asset.PurchaseDate = DateTime.Now;
            asset.AssetName = "Teste";

            responseFail = await client.PostAsJsonAsync($"/api/asset", asset);

            asset.EMailAdressOfDepartment = "teste@gmail.com";
            asset.CountryOfDepartment = "Brasil";
            asset.Department = 1;
            asset.Broken = false;

            responseOK = await client.PostAsJsonAsync($"/api/asset", asset);
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
        public void StatusCodeIsCreated() => responseOK.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
    }
}
