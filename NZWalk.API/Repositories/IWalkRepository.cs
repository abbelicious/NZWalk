using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk Walk);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, 
                                     string? filterQuery = null,
                                     string? sortyBy = null,
                                     bool isAscending = true,
                                     int pageNumber = 1,
                                     int pageSize = 1000);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid guid, Walk Walk);
        Task<Walk?> Delete(Guid id);
    }
}
