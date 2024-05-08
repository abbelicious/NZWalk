using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using NZWalk.API.Models.DTOs;
using NZWalk.API.Models.Domain;
using AutoMapper;
using NZWalk.API.Repositories;
using NZWalk.API.CustomActionFilters;
using System.Net;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }
        
        // Create Walk
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
            await _walkRepository.CreateAsync(walkDomainModel);
            
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
        
        // Get: Filter query
        //  /api/walks=filterOn=Name&filterQuery=Track&sortBy=Name&isAscending&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, 
                                                [FromQuery] string? filterQuery,
                                                [FromQuery] string? sortBy,
                                                [FromQuery] bool? isAscending,
                                                [FromQuery] int pageNumber = 1,
                                                [FromQuery] int pageSize = 1000)          
        {
                var walksDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //create an exception 
            //throw new Exception("This is a new exception");

                return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{guid:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid guid)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(guid);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkDto)
        {
                // map
                var walkDomainModel = _mapper.Map<Walk>(updateWalkDto);
                walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
        [HttpDelete]
        [Route("{guid:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid guid)
        {
            // Delete
            var walkDomainModel = await _walkRepository.Delete(guid);
            //await _context.SaveChangesAsync();
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            // return deleted object
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }

    }
}
