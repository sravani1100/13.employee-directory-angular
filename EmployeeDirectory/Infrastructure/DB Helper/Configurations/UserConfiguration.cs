using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DB_Helper.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");

        entity.HasKey(u => u.UserId);

        entity.Property(u => u.UserName)
              .IsRequired()
              .HasMaxLength(254);

        entity.Property(u => u.EmployeeNumber)
              .IsRequired()
              .HasMaxLength(20);

        entity.Property(u => u.PasswordHash)
           .HasMaxLength(500)
           .IsRequired();

        entity.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(u => u.Employee)
            .WithOne(e => e.User)
            .HasForeignKey<User>(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(u => u.EmployeeNumber)
           .IsUnique();

        entity.HasIndex(u => u.UserName)
            .IsUnique();
    }
}

