using EmployeeDirectory.Domain.Enums;

namespace EmployeeDirectory.Application.DTO.RequestDTOs;

public class EmployeeRequestDTO
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public string Email { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public DateTime JoiningDate { get; set; }

    public Status Status { get; set; }

    public int? ManagerId { get; set; }

    public string LocationName { get; set; } = string.Empty;

    public string ProjectName { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

