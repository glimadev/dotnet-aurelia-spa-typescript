using AureliaWithDotNet.Domain.Models;
using System.Threading.Tasks;

namespace AureliaWithDotNet.Domain.Interfaces
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<Asset> GetById(int id);
    }
}
