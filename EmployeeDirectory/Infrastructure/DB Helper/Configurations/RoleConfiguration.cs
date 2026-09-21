using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDirectory.Common.DB_Helper.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.ToTable("Roles");
        entity.HasKey(r => r.RoleId);

        entity.Property(r => r.RoleName)
              .IsRequired()
              .HasMaxLength(50);

        entity.Property(r => r.Description)
              .HasMaxLength(200);

        entity.HasIndex(r => r.RoleName)
              .IsUnique();

        entity.HasOne(r => r.Department)
              .WithMany(d => d.Roles)
              .HasForeignKey(r => r.DepartmentId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}

