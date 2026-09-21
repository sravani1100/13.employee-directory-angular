using AutoMapper;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Mappings;

public class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        CreateMap<LocationRequestDTO, Location>();

        CreateMap<LocationRequestDTO, Location>()
            .ForMember(dest => dest.LocationId, opt => opt.Ignore());

        CreateMap<Location, LocationResponseDTO>();
    }
}

