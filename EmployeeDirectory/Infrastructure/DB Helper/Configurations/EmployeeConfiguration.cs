using Domain.Entities;
using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDirectory.Common.DB_Helper.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> entity)
    {
        entity.ToTable("Employees");
        entity.HasKey(e => e.EmployeeId);

        entity.Property(e => e.EmployeeNumber)
              .IsRequired()
              .HasMaxLength(20);

        entity.Property(e => e.FirstName)
              .IsRequired()
              .HasMaxLength(50);

        entity.Property(e => e.LastName)
              .IsRequired()
              .HasMaxLength(50);

        entity.Property(e => e.Email)
              .IsRequired()
              .HasMaxLength(254);

        entity.Property(e => e.DateOfBirth);

        entity.Property(e => e.MobileNumber)
              .HasMaxLength(15);

        entity.Property(e => e.JoiningDate)
              .IsRequired();

        entity.Property(e => e.Status)
              .HasConversion<string>()
              .HasMaxLength(15)
              .IsRequired();

        entity.HasIndex(e => e.EmployeeNumber)
              .IsUnique();

        entity.HasIndex(e => e.Email)
              .IsUnique();

        entity.HasIndex(e => e.MobileNumber)
              .IsUnique();

        entity.HasOne(e => e.Location)
              .WithMany(l => l.Employees)
              .HasForeignKey(e => e.LocationId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.Manager)
              .WithMany(m => m.Employees)
              .HasForeignKey(e => e.ManagerId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.User)
              .WithOne(u => u.Employee)
              .HasForeignKey<User>(u => u.EmployeeId)
              .OnDelete(DeleteBehavior.Restrict); 

    }
}

