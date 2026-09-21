using Application.DTO.RequestDTOs;
using AutoMapper;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Mappings;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectRequestDTO, Project>();

        CreateMap<Project, ProjectResponseDTO>();

        CreateMap<ProjectRequestDTO, Project>()
            .ForMember(
                dest => dest.ProjectId,
                opt => opt.Ignore());
    }
}

