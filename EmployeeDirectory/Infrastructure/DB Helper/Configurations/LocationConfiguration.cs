using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDirectory.Common.DB_Helper.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> entity)
    {
        entity.ToTable("Locations");

        entity.HasKey(l => l.LocationId);

        entity.Property(l => l.LocationName)
              .IsRequired()
              .HasMaxLength(50);

        entity.HasIndex(l => l.LocationName)
              .IsUnique();
    }
}

