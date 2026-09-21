using Domain.Entities;
using EmployeeDirectory.Domain.Enums;

namespace EmployeeDirectory.Domain.Entities;

public class Employee
{  
    public int EmployeeId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? MobileNumber { get; set; }

    public DateTime JoiningDate { get; set; }

    public Status Status { get; set; } = Status.Active;

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public int LocationId { get; set; }
    public Location? Location { get; set; }

    public User User { get; set; } = null!;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
}
