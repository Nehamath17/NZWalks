using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domains;

namespace NZWalksAPI.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficuty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await nZWalksDbContext.WalkDifficuty.FindAsync(id);
            if (existingWalkDifficulty != null)
            {
                nZWalksDbContext.WalkDifficuty.Remove(existingWalkDifficulty);
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalkDifficulty;
            }
            return null;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficuty.ToListAsync();  
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficuty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await nZWalksDbContext.WalkDifficuty.FindAsync(id);

            if (existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;

        }
    }
}
