using System;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReactiveUI;

namespace HRManagementSystem;

public class App : Application
{
    private ServiceProvider? _serviceProvider;
    
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

        // 添加日志记录服务
        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Information));

        // 数据库上下文
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={DatabaseConfig.DbPath}"));

        // Identity 配置
        services.AddIdentity<User, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // 服务注册
        services.AddScoped<IUserService, UserService>();
        services.AddDataProtection();
        services.AddScoped<CustomSignInManager>(provider =>
            new CustomSignInManager(
                provider.GetRequiredService<UserManager<User>>(),
                provider.GetRequiredService<IHttpContextAccessor>(),
                provider.GetRequiredService<IUserClaimsPrincipalFactory<User>>(),
                provider.GetRequiredService<IOptions<IdentityOptions>>(),
                provider.GetRequiredService<ILogger<SignInManager<User>>>(),
                provider.GetRequiredService<IAuthenticationSchemeProvider>()
            ));
        // 后续其他服务在此添加...
        
        // 注册数据库初始化服务
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        
        // 构建服务提供者
        _serviceProvider = services.BuildServiceProvider();
        if (_serviceProvider == null) throw new InvalidOperationException("服务提供者构建失败");

        Console.WriteLine("服务提供者已成功构建");

        // 主窗口配置
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 使用数据库初始化服务
            using (var scope = _serviceProvider.CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<App>();
                var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
                
                try
                {
                    // 确保数据库已创建并应用迁移
                    logger.LogInformation("开始确保数据库已创建...");
                    databaseInitializer.EnsureDatabaseCreatedAsync().Wait();
                    logger.LogInformation("数据库创建和迁移完成");
                    
                    // 初始化系统数据
                    logger.LogInformation("开始初始化系统数据...");
                    databaseInitializer.InitializeSystemDataAsync().Wait();
                    logger.LogInformation("系统数据初始化完成");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "数据库初始化过程中发生错误");
                }
            }
            
            // 检查服务是否正确注册
            Console.WriteLine("Checking service registration...");
            Console.WriteLine("IHttpContextAccessor registered: " +
                              (_serviceProvider.GetService<IHttpContextAccessor>() != null));
            Console.WriteLine("UserClaimsPrincipalFactory registered: " +
                              (_serviceProvider.GetService<IUserClaimsPrincipalFactory<User>>() != null));
            Console.WriteLine("AuthenticationSchemeProvider registered: " +
                              (_serviceProvider.GetService<IAuthenticationSchemeProvider>() != null));
            Console.WriteLine("SignInManager<User> registered: " +
                              (_serviceProvider.GetService<SignInManager<User>>() != null));
            Console.WriteLine("UserManager<User> registered: " +
                              (_serviceProvider.GetService<UserManager<User>>() != null));

            var signInManager = GetService<CustomSignInManager>();
            var userManager = GetService<UserManager<User>>();

            if (signInManager == null || userManager == null)
                throw new InvalidOperationException("SignInManager 或 UserManager 未正确初始化");

            Console.WriteLine("正在创建主窗口...");
            desktop.MainWindow = new LoginWindow(signInManager, userManager);
            Console.WriteLine("主窗口已创建");
        }

        base.OnFrameworkInitializationCompleted();
    }

    // 获取服务的辅助方法
    private static T GetService<T>()
    {
        var app = Current as App;
        if (app == null) throw new InvalidOperationException("应用程序实例为空");
        if (app._serviceProvider == null) throw new InvalidOperationException("服务提供者为空");
        return app._serviceProvider.GetService<T>() ?? throw new InvalidOperationException("服务获取失败");
    }
}