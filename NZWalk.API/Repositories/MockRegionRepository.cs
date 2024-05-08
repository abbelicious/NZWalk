using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public class MockRegionRepository : IRegionRepository
    {
        public Task<Region> CreateAsync(Region region)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Region>> GetAllAsync()
        {

            return new List<Region>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Code = "SAM",
                    Name = "Sameer's Region Name"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Code = "JAN",
                    Name = "Jan's Region Name"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Code = "EJB",
                    Name = "Ejb's Region Name"
                }
            };
        }

        public Task<Region?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> UpdateAsync(Guid guid, Region region)
        {
            throw new NotImplementedException();
        }
    }
}
