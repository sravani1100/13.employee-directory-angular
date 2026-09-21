using EmployeeDirectory.Domain.Enums;

namespace Application.DTO.ResponseDTOs;

public class EmployeeDetailsResponseDTO
{
    public int EmployeeId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? MobileNumber { get; set; }

    public DateOnly JoiningDate { get; set; }

    public Status Status { get; set; }

    public int? ManagerId { get; set; }
    public string? Manager { get; set; }

    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;

    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }

    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;

    public int? ProjectId { get; set; }
    public string? ProjectName { get; set; }
}
