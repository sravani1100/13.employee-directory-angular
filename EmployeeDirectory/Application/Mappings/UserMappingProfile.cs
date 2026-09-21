using Application.DTO.RequestDTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;
    public class UserMappingProfile : Profile
    {
    public UserMappingProfile()
    {
        CreateMap<UserRequestDTO, User>();
    }
}

