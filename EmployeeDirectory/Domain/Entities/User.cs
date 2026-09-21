using EmployeeDirectory.Domain.Entities;

namespace Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string EmployeeNumber {  get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public int EmployeeId {  get; set; }

    public int RoleId { get; set; }
    public Employee Employee { get; set; } = null!;

    public Role Role { get; set; } = null!;
}

