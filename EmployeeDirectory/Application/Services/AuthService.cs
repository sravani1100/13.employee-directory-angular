using Application.DTO.RequestDTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<string?> LoginAsync(LoginRequestDTO request)
    {
        _logger.LogInformation("Login attempt for user {UserName}.", request.UserName);

        User? user = await _userRepository.GetByUserNameAsync(request.UserName);
        if (user == null)
        {
            _logger.LogWarning("Login failed. User {UserName} not found.", request.UserName);
            return null;
        }

        bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isValid)
        {
            _logger.LogWarning("Login failed. Invalid password for user {UserName}.", request.UserName);
            return null;
        }

        _logger.LogInformation("User {UserName} authenticated successfully.", user.UserName);

        return _jwtService.GenerateToken(user.UserName, user.Role.RoleName);
    }
}
