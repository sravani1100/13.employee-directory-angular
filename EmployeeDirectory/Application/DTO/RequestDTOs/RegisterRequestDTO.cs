using System.ComponentModel.DataAnnotations;

namespace Application.DTO.RequestDTOs;

public class RegisterRequestDTO
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}

