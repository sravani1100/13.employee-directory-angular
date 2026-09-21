using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDirectory.Common.DB_Helper.Configurations;

public class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
{
    public void Configure(EntityTypeBuilder<EmployeeRole> entity)
    {
        entity.ToTable("Employees_Roles");
        entity.HasKey(er => er.EmployeeRoleId);

        entity.HasIndex(er => new { er.EmployeeId, er.RoleId })
              .IsUnique();

        entity.HasOne(er => er.Employee)
              .WithMany(e => e.EmployeeRoles)
              .HasForeignKey(er => er.EmployeeId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(er => er.Role)
              .WithMany(r => r.EmployeeRoles)
              .HasForeignKey(er => er.RoleId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}

