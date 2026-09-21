namespace Application.DTO.ResponseDTOs;

public class RoleCardResponseDTO
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }

    public int? LocationId { get; set; }
    public string? LocationName { get; set; } = string.Empty;

    public int TotalEmployees { get; set; }
}

