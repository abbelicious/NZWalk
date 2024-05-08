using AutoMapper;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTOs;

namespace NZWalk.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //Mapping Regions with DTO
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Region, AddRegionRequestDto>().ReverseMap();
            CreateMap<Region, UpdateRegionRequestDto>().ReverseMap();
            
            //Mapping Walks with DTO
            //CreateMap<WalkDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Walk, AddWalkRequestDto>().ReverseMap();
            CreateMap<Walk, UpdateWalkRequestDto>().ReverseMap();
            
            // Difficulty
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
