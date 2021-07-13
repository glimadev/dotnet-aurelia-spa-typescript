using AureliaWithDotNet.Domain.Interfaces;
using AureliaWithDotNet.Domain.Models;
using System.Threading.Tasks;

namespace AureliaWithDotNet.Data.Repository
{
    public class AssetRepository : Repository<Asset>, IAssetRepository
    {
        public AssetRepository(AureliaWithDotNetDbContext context)
              : base(context)
        {

        }

        public async Task<Asset> GetById(int id) => await GetById(x => x.Id == id);
    }
}
