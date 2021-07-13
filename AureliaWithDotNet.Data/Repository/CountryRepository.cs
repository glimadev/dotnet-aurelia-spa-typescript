using AureliaWithDotNet.Domain.Interfaces;
using AureliaWithDotNet.Domain.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AureliaWithDotNet.Data.Repository
{
    public class CountryRepository : ICountryRepository
    {
        //If the country is found, the country is valid.
        public bool IsValid(string countryName) => 
            new HttpClient().Send(new HttpRequestMessage(HttpMethod.Get, $"https://restcountries.eu/rest/v2/name/{countryName}"))
            .StatusCode == HttpStatusCode.OK;

        public async Task<IEnumerable<Country>> GetAll()
        {
           var result = new HttpClient().Send(new HttpRequestMessage(HttpMethod.Get, $"https://restcountries.eu/rest/v2/all"));

           return await result.Content.ReadFromJsonAsync<IEnumerable<Country>>();
        }
    }
}
