using HRManagementSystem.Models;
using HRManagementSystem.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Data;

// 继承IdentityDbContext并指定用户类型和角色类型
public class AppDbContext : IdentityDbContext<User, ApplicationRole, string>
{
    // 构造函数，接收DbContextOptions
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // 基于Identity框架的表
    // User和Role表由基类自动创建

    // 权限相关表
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    public DbSet<RoleMenu> RoleMenus { get; set; } = null!;

    // 人力资源核心表
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Position> Positions { get; set; } = null!;

    // 考勤和薪资
    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<LeaveRequest> LeaveRequests { get; set; } = null!;
    public DbSet<Salary> Salaries { get; set; } = null!;

    // 工作流相关
    public DbSet<Announcement> Announcements { get; set; } = null!;
    public DbSet<TodoItem> TodoItems { get; set; } = null!;

    // 审计日志
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    // 配置数据库模型
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // 首先调用基类的OnModelCreating以设置Identity相关表
        base.OnModelCreating(builder);

        // 自定义Identity表名
        builder.Entity<User>().ToTable("Users");
        builder.Entity<ApplicationRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        // 配置权限表
        builder.Entity<Permission>(b =>
        {
            b.HasKey(p => p.Id);
            b.HasIndex(p => p.NormalizedName).IsUnique();
            b.HasIndex(p => p.Code).IsUnique();
            b.ToTable("Permissions");
        });

        // 配置角色-权限关系（多对多）
        builder.Entity<RolePermission>(b =>
        {
            b.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            b.HasOne<ApplicationRole>()
                .WithMany()
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            b.ToTable("RolePermissions");
        });

        // 配置菜单表
        builder.Entity<Menu>(b =>
        {
            b.HasKey(m => m.Id);

            // 自引用关系 - 父子菜单
            b.HasOne(m => m.ParentMenu)
                .WithMany(m => m.ChildMenus)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("Menus");
        });

        // 配置角色-菜单关系（多对多）
        builder.Entity<RoleMenu>(b =>
        {
            b.HasKey(rm => new { rm.RoleId, rm.MenuId });

            b.HasOne<ApplicationRole>()
                .WithMany()
                .HasForeignKey(rm => rm.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(rm => rm.Menu)
                .WithMany()
                .HasForeignKey(rm => rm.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            b.ToTable("RoleMenus");
        });

        // 配置员工表
        builder.Entity<Employee>(b =>
        {
            b.HasKey(e => e.Id);

            // 员工号唯一约束
            b.HasIndex(e => e.EmployeeNumber).IsUnique();

            // 邮箱唯一约束
            b.HasIndex(e => e.Email).IsUnique();

            // 员工-用户关系（一对一）
            b.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // 员工-部门关系（多对一）
            b.HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // 员工-职位关系（多对一）
            b.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.SetNull);

            // 员工-经理关系（自引用，多对一）
            b.HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            b.ToTable("Employees");
        });

        // 配置部门表
        builder.Entity<Department>(b =>
        {
            b.HasKey(d => d.Id);

            // 部门名称唯一约束
            b.HasIndex(d => d.Name).IsUnique();

            // 部门-部门经理关系（一对一）
            b.HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            // 部门-父部门关系（自引用，多对一）
            b.HasOne(d => d.ParentDepartment)
                .WithMany(d => d.ChildDepartments)
                .HasForeignKey(d => d.ParentDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("Departments");
        });

        // 配置职位表
        builder.Entity<Position>(b =>
        {
            b.HasKey(p => p.Id);

            // 职位-部门关系（多对一）
            b.HasOne(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            b.ToTable("Positions");
        });

        // 配置考勤记录表
        builder.Entity<Attendance>(b =>
        {
            b.HasKey(a => a.Id);

            // 考勤-员工关系（多对一）
            b.HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // 考勤-创建者关系（多对一）
            b.HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("Attendances");
        });

        // 配置请假记录表
        builder.Entity<LeaveRequest>(b =>
        {
            b.HasKey(l => l.Id);

            // 请假-员工关系（多对一）
            b.HasOne(l => l.Employee)
                .WithMany()
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // 请假-审批人关系（多对一）
            b.HasOne(l => l.ApprovedBy)
                .WithMany()
                .HasForeignKey(l => l.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);

            // 请假-创建者关系（多对一）
            b.HasOne(l => l.CreatedBy)
                .WithMany()
                .HasForeignKey(l => l.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("LeaveRequests");
        });

        // 配置薪资表
        builder.Entity<Salary>(b =>
        {
            b.HasKey(s => s.Id);

            // 薪资-员工关系（多对一）
            b.HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // 薪资-创建者关系（多对一）
            b.HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("Salaries");
        });

        // 配置公告表
        builder.Entity<Announcement>(b =>
        {
            b.HasKey(a => a.Id);

            // 公告-创建者关系（多对一）
            b.HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("Announcements");
        });

        // 配置待办事项表
        builder.Entity<TodoItem>(b =>
        {
            b.HasKey(t => t.Id);

            // 待办-分配给谁关系（多对一）
            b.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            // 待办-创建者关系（多对一）
            b.HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("TodoItems");
        });

        // 配置审计日志表
        builder.Entity<AuditLog>(b =>
        {
            b.HasKey(a => a.Id);

            // 审计日志-用户关系（多对一）
            b.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("AuditLogs");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=HRManagement.db");
    }
}