using NZWalksAPI.Data;
using NZWalksAPI.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }
        
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }
    }
}
