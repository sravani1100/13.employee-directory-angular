using Application.DTO.RequestDTOs;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginRequestDTO request);
}

