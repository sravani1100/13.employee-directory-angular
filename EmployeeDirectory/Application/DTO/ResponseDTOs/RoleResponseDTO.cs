namespace EmployeeDirectory.Application.DTO.ResponseDTOs;

public class RoleResponseDTO
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }
}

