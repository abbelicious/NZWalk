using Microsoft.EntityFrameworkCore;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _context;
        public SQLRegionRepository(NZWalksDbContext context)
        {
            _context = context;
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
            return region;
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();
        }


        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Region?> UpdateAsync(Guid guid, Region region)
        {
            var regionToUpdate = await _context.Regions.FirstOrDefaultAsync(x => x.Id == guid);
            if (regionToUpdate == null)
            {
                return null;
            }
            regionToUpdate.Code = region.Code;
            regionToUpdate.Name = region.Name;
            regionToUpdate.RegionImageUrl = region.RegionImageUrl;
            await _context.SaveChangesAsync();
            return regionToUpdate;

        }
        public async Task<Region?> Delete(Guid id)
        {
            var existingRegion = await _context.Regions.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (existingRegion == null)
            {
                return null;
            }
            _context.Regions.Remove(existingRegion);
            await _context.SaveChangesAsync();
            return existingRegion;
        }
    }
}
