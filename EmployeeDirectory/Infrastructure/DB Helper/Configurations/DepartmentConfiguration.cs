using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDirectory.Common.DB_Helper.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> entity)
    {
        entity.ToTable("Departments");

        entity.HasKey(d => d.DepartmentId);

        entity.Property(d => d.DepartmentName)
              .IsRequired()
              .HasMaxLength(50);

        entity.HasIndex(d => d.DepartmentName)
              .IsUnique();
    }
}

