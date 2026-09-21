using Application.DTO.RequestDTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO request)
    {
        _logger.LogInformation(
            "Login request received for username {UserName}.",
            request?.UserName);

        var token = await _authService.LoginAsync(request);

        if (token == null)
        {
            _logger.LogWarning(
                "Login failed for username {UserName}.",
                request?.UserName);

            return Unauthorized("Invalid username or password.");
        }

        Response.Cookies.Append(
            "access_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

        _logger.LogInformation(
            "Login successful for username {UserName}.",
            request.UserName);

        return Ok(new
        {
            Message = "Login successful"
        });
    }

    [Authorize]
    [HttpGet("currentUser")]
    public async Task<IActionResult> GetCurrentUser()
    {
        return Ok(new
        {
            username = User.Identity?.Name,
            role = User.FindFirst(
                    ClaimTypes.Role)?.Value
        });
    }
}
