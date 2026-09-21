using AutoMapper;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Mappings;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Department, DepartmentResponseDTO>();
        CreateMap<DepartmentRequestDTO, Department>();
    }
}

