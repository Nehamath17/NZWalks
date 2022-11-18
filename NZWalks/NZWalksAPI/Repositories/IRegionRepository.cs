using NZWalksAPI.Models.Domains;

namespace NZWalksAPI.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
