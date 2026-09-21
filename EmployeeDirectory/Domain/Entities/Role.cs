namespace EmployeeDirectory.Domain.Entities;

public class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; } 

    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();


    public override string ToString()
    {
        return RoleName;
    }
}
