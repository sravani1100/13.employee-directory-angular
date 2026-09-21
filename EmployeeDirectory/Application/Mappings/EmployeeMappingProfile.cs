using Application.DTO.ResponseDTOs;
using AutoMapper;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Mappings;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<EmployeeRequestDTO, Employee>();

        CreateMap<Employee, EmployeeResponseDTO>()

            .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(
                    src => $"{src.FirstName} {src.LastName}"))

            .ForMember(
                dest => dest.RoleName,
                opt => opt.MapFrom(
                    src => src.EmployeeRoles
                        .Select(x => x.Role!.RoleName)
                        .FirstOrDefault()))

            .ForMember(
                 dest => dest.DepartmentId,
                 opt => opt.MapFrom(
                    src => src.EmployeeRoles
                        .Select(x => x.Role!.DepartmentId)
                        .FirstOrDefault()))

            .ForMember(
                dest => dest.Department,
                opt => opt.MapFrom(
                    src => src.EmployeeRoles
                        .Select(x => x.Role!.Department!.DepartmentName)
                        .FirstOrDefault()))

            .ForMember(
                dest => dest.Location,
                opt => opt.MapFrom(
                    src => src.Location!.LocationName))

            .ForMember(
                dest => dest.JoiningDate,
                opt => opt.MapFrom(
                    src => DateOnly.FromDateTime(src.JoiningDate)))

            .ForMember(
                dest => dest.Manager,
                opt => opt.MapFrom(
                    src => src.Manager == null
                        ? string.Empty
                        : $"{src.Manager.FirstName} {src.Manager.LastName}"))

            .ForMember(
                dest => dest.Projects,
                opt => opt.MapFrom(
                    src => src.EmployeeProjects
                        .Select(x => x.Project!.ProjectName)
                        .ToList()));



        CreateMap<Employee, EmployeeDetailsResponseDTO>()

    .ForMember(
    dest => dest.DateOfBirth,
    opt => opt.MapFrom(
        src => src.DateOfBirth.HasValue
            ? (DateOnly?)DateOnly.FromDateTime(src.DateOfBirth.Value)
            : null))

    .ForMember(
        dest => dest.JoiningDate,
        opt => opt.MapFrom(
            src => DateOnly.FromDateTime(src.JoiningDate)))

    .ForMember(
        dest => dest.Manager,
        opt => opt.MapFrom(
            src => src.Manager == null
                ? null
                : $"{src.Manager.FirstName} {src.Manager.LastName}"))

    .ForMember(
        dest => dest.LocationName,
        opt => opt.MapFrom(
            src => src.Location == null
                ? string.Empty
                : src.Location.LocationName))

    .ForMember(
        dest => dest.RoleId,
        opt => opt.MapFrom(
            src => src.EmployeeRoles
                .Select(x => x.RoleId)
                .FirstOrDefault()))

    .ForMember(
    dest => dest.DepartmentId,
    opt => opt.MapFrom(src =>
        src.EmployeeRoles
            .Select(x => x.Role!.DepartmentId)
            .FirstOrDefault()))

.ForMember(
    dest => dest.DepartmentName,
    opt => opt.MapFrom(src =>
        src.EmployeeRoles
            .Select(x => x.Role!.Department!.DepartmentName)
            .FirstOrDefault() ?? string.Empty))


    .ForMember(
        dest => dest.ProjectId,
        opt => opt.MapFrom(
            src => src.EmployeeProjects
                .Select(x => (int?)x.ProjectId)
                .FirstOrDefault()))

    .ForMember(
        dest => dest.ProjectName,
        opt => opt.MapFrom(
            src => src.EmployeeProjects
                .Select(x => x.Project!.ProjectName)
                .FirstOrDefault()));
    }
}

