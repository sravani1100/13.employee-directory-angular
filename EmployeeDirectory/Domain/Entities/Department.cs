namespace EmployeeDirectory.Domain.Entities;

public class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public ICollection<Role> Roles { get; set; } = new List<Role>();

    public ICollection<LocationDepartment> LocationDepartments { get; set; } = new List<LocationDepartment>();

    public override string ToString()
    {
        return DepartmentName;
    }
}

