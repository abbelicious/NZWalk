using Microsoft.EntityFrameworkCore;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _context;
        public SQLWalkRepository(NZWalksDbContext context)
        {
            _context = context;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();  
            return walk;
        }

        public async Task<Walk?> Delete(Guid id)
        {
            var existingWalk = await _context.Walks.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (existingWalk == null)
            {
                return null;
            }
            _context.Walks.Remove(existingWalk);
            await _context.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null,
                                                  string? filterQuery = null,
                                                  string? sortyBy = null,
                                                  bool isAscending = true ,
                                                  int pageNumber = 1, 
                                                  int pageSize = 1000)
        {
            var walks = _context.Walks.Include("Difficulty")
                                       .Include("Region").AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)) // StringComparison.OrdinalIgnoreCase ignores if it is lower or upper case.
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            // Sorting
            if(!string.IsNullOrWhiteSpace(sortyBy))
            {
                if(sortyBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name): walks.OrderByDescending(x => x.Name);
                }
                else if(sortyBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _context.Walks.Include("Difficulty")
                                       .Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid guid, Walk walk)
        {
                var walkToUpdate = await _context.Walks.FirstOrDefaultAsync(x => x.Id == guid);
                if (walkToUpdate == null)
                {
                    return null;
                }
                walkToUpdate.Name = walk.Name;
                walkToUpdate.Description = walk.Description;
                walkToUpdate.LengthInKm= walk.LengthInKm;
                walkToUpdate.WalkImageUrl= walk.WalkImageUrl;
                walkToUpdate.DifficultyId= walk.DifficultyId;
                walkToUpdate.RegionId= walk.RegionId;
                await _context.SaveChangesAsync();
                return walkToUpdate;
        }

    }
}
