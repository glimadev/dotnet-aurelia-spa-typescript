using AureliaWithDotNet.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AureliaWithDotNet.Domain.Interfaces
{
    public interface ICountryRepository
    {
        bool IsValid(string countryName);
        Task<IEnumerable<Country>> GetAll();
    }
}
