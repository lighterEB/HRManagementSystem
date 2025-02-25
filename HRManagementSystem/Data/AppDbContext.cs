using System.IO;
using HRManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext()
    {
        var folder = Path.GetDirectoryName(DatabaseConfig.DbPath);
        if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            Directory.CreateDirectory(folder);
    }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Position> Positions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DatabaseConfig.DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置 Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.EmployeeNumber).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            // 与 User 的关联
            entity.HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // 与 Department 的关联
            entity.HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 与 Position 的关联
            entity.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 配置 Department
        modelBuilder.Entity<Department>(entity => { entity.HasIndex(d => d.Name).IsUnique(); });

        // 配置 Position
        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasOne(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}