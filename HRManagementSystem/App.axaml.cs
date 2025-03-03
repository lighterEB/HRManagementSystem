using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HRManagementSystem.Data;
using HRManagementSystem.Models.Identity;
using HRManagementSystem.Services.Implementations;
using HRManagementSystem.Services.Interfaces;
using HRManagementSystem.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace HRManagementSystem;

public class App : Application
{
    private ServiceProvider _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        
        // 添加全局异常处理
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var exception = args.ExceptionObject as Exception;
            Console.WriteLine($"未处理的全局异常: {exception}");
        };

        // ReactiveUI 全局异常处理
        RxApp.DefaultExceptionHandler = Observer.Create<Exception>(ex =>
        {
            Console.WriteLine($"ReactiveUI未处理异常: {ex}");
        });

        // 处理未观察的任务异常
        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            Console.WriteLine($"未观察的任务异常: {args.Exception}");
            args.SetObserved(); // 标记为已观察，防止程序崩溃
        };

        // 数据库上下文
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=HRManagement.db"));

        // Identity配置
        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<User>>();

        // 服务注册
        services.AddScoped<IUserService, UserService>();
        services.AddDataProtection();
        // 后续其他服务在此添加...

        // 构建服务提供者
        _serviceProvider = services.BuildServiceProvider();

        // 初始化数据库
        InitializeDatabase();

        // 主窗口配置
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new LoginWindow();

        base.OnFrameworkInitializationCompleted();
    }

    private void InitializeDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // 自动迁移
        if (dbContext.Database.GetPendingMigrations().Any()) dbContext.Database.Migrate();

        // 初始化系统管理员
        SeedAdminUser(scope.ServiceProvider).Wait();
    }


    private async Task SeedAdminUser(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        // 创建管理员角色
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new ApplicationRole
            {
                Name = "Admin",
                Description = "系统管理员",
                IsSystemRole = true
            });

        // 创建默认管理员用户
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = "admin",
                Email = "admin@example.com",
                RealName = "系统管理员",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@1234");
            if (result.Succeeded) await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // 获取服务的辅助方法
    public static T GetService<T>()
    {
        var app = Current as App;
        return app._serviceProvider.GetService<T>();
    }
}