using AutoMapper;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Mappings;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<RoleRequestDTO, Role>()
             .ForMember(dest => dest.DepartmentId,
                 opt => opt.Ignore())
             .ForMember(dest => dest.Department,
                 opt => opt.Ignore());

        CreateMap<Role, RoleResponseDTO>()
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department!.DepartmentName));
    }
}


