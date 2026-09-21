using EmployeeDirectory.Domain.Enums;

namespace EmployeeDirectory.Application.DTO.ResponseDTOs;

public class EmployeeResponseDTO
{
    public int EmployeeId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string? RoleName { get; set; }

    public int? DepartmentId { get; set; }
    public string? Department { get; set; }

    public int? LocationId { get; set; }
    public string Location { get; set; } = string.Empty;

    public DateOnly JoiningDate { get; set; }

    public Status Status { get; set; }

    public string? Manager { get; set; }

    public List<string> Projects { get; set; } = new();
}

