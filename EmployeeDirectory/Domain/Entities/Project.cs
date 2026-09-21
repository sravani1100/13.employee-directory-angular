namespace EmployeeDirectory.Domain.Entities;

public class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

    public override string ToString()
    {
        return ProjectName;
    }
}

