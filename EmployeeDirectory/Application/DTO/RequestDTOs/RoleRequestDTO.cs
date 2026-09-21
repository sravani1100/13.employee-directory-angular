namespace EmployeeDirectory.Application.DTO.RequestDTOs;

public class RoleRequestDTO
{
    public string RoleName { get; set; } = string.Empty;

    public string LocationName { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;

    public string? Description { get; set; }
}

