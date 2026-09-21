using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDirectory.Common.DB_Helper.Configurations;

public class LocationDepartmentConfiguration : IEntityTypeConfiguration<LocationDepartment>
{
    public void Configure(EntityTypeBuilder<LocationDepartment> entity)
    {
        entity.ToTable("Locations_Departments");
        entity.HasKey(ld => ld.LocationDepartmentId);

        entity.HasIndex(ld => new { ld.DepartmentId, ld.LocationId })
              .IsUnique();

        entity.HasOne(ld => ld.Department)
              .WithMany(d => d.LocationDepartments)
              .HasForeignKey(ld => ld.DepartmentId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(ld => ld.Location)
              .WithMany(l => l.LocationDepartments)
              .HasForeignKey(ld => ld.LocationId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}

