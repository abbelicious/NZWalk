using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public interface IRegionRepository
    {
        Task<Region> CreateAsync(Region region);
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region?> UpdateAsync(Guid guid, Region region);
        Task<Region?> Delete(Guid id);
    }
}
