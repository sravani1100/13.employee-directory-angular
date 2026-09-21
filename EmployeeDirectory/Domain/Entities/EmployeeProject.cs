namespace EmployeeDirectory.Domain.Entities;
    public class EmployeeProject
    {
        public int EmployeeProjectId { get; set; }
        
        public int EmployeeId {  get; set; }
        public Employee? Employee { get; set; } 

        public int ProjectId { get; set; }
        public Project? Project { get; set; } 
    }

