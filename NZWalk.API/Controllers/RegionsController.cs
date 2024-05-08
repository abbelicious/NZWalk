using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.CustomActionFilters;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTOs;
using NZWalk.API.Repositories;
using System.Diagnostics;
using System.Text.Json;
namespace NZWalk.API.Controllers
{
    //https://localhost:123/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(NZWalksDbContext dbContext, 
                                IRegionRepository regionRepository,
                                IMapper mapper,
                                ILogger<RegionsController> logger)
        {
            _context = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }
       
        // GET: https//localhost:123/api/regions
        [HttpGet]
       //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                throw new Exception("This is a custom exception");
                // Get Data from database - Domain models
                var regionsDomain = await _regionRepository.GetAllAsync();
                // Map Domain Models to DTOs
                var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
                // Return DTOs
                _logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");
                return Ok(regionsDto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }       

        // GET: https//localhost:123/api/regions/{guid}
        [HttpGet]
        [Route("{guid:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute]Guid guid)
        {
            var regions = await _regionRepository.GetByIdAsync(guid);
            if(regions == null)
            {
                return NotFound();
            }
            var regionsDto = _mapper.Map<List<RegionDto>>(regions);

            return Ok(regionsDto);
        }

        // POST To Create New region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
            // Use Domain Model to create Region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            // Map domain model back to dto
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(Create), new {id = regionDto.Id}, regionDto);
        }

        // Update
        // GET: https//localhost:123/api/regions/{guid}
        [HttpPut]
        [Route("{guid:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Update([FromRoute] Guid guid, [FromBody] UpdateRegionRequestDto updateRegionRequestDto )
        {
                //Map DTO to domain model
                var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

                regionDomainModel = await _regionRepository.UpdateAsync(guid, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                // Convert DomainModel to DTO
                var regionDto = _mapper.Map<RegionDto>(updateRegionRequestDto);
                return Ok(regionDto);
        }

        // Delete: https//localhost:123/api/regions/{guid}
        [HttpDelete]
        [Route("{guid:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid guid)
        {
            // Delete
            var regionDomainModel = await _regionRepository.Delete(guid);
            //await _context.SaveChangesAsync();
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            // return deleted object
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
