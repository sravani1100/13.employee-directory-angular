namespace EmployeeDirectory.Domain.Entities;

public class Location
{
    public int LocationId { get; set; }

    public string LocationName { get; set; } = string.Empty;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public ICollection<LocationDepartment> LocationDepartments { get; set; } = new List<LocationDepartment>();

    public override string ToString()
    {
        return LocationName;
    }
}

