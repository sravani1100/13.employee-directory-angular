using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDirectory.Common.DB_Helper.Configurations;

public class EmployeeProjectConfiguration : IEntityTypeConfiguration<EmployeeProject>
{
    public void Configure(EntityTypeBuilder<EmployeeProject> entity)
    {
        entity.ToTable("Employees_Projects");
        entity.HasKey(ep => ep.EmployeeProjectId);

        entity.HasIndex(ep => new { ep.EmployeeId, ep.ProjectId })
              .IsUnique();

        entity.HasOne(ep => ep.Employee)
              .WithMany(e => e.EmployeeProjects)
              .HasForeignKey(ep => ep.EmployeeId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(ep => ep.Project)
              .WithMany(p => p.EmployeeProjects)
              .HasForeignKey(ep => ep.ProjectId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}

