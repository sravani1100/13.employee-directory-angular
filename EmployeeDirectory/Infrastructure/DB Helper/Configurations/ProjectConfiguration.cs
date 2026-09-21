using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDirectory.Common.DB_Helper.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> entity)
    {
        entity.ToTable("Projects");
        entity.HasKey(p => p.ProjectId);

        entity.Property(p => p.ProjectName)
              .IsRequired()
              .HasMaxLength(50);

        entity.HasData(
        new Project
        {
            ProjectId = 1,
            ProjectName = "Employee Directory"
        },
        new Project
        {
            ProjectId = 2,
            ProjectName = "E-Commerce Platform"
        }
        );
    }
}

