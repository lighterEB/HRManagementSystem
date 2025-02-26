using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HRManagementSystem.Data;
using HRManagementSystem.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        // 配置服务容器
        var services = new ServiceCollection();

        // 注册数据库上下文
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=HRManagement.db"));

        // 添加Identity服务 (如果需要的话)
        // services.AddIdentityCore<User>()
        //     .AddRoles<ApplicationRole>()
        //     .AddEntityFrameworkStores<AppDbContext>();

        // 其他服务注册
        // 例如: services.AddTransient<IEmployeeService, EmployeeService>();

        // 构建服务提供者
        _serviceProvider = services.BuildServiceProvider();

        // 初始化应用程序的生命周期
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 确保数据库创建
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated(); // 或使用 Migrate()
            }

            // 设置主窗口
            desktop.MainWindow = new LoginWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    // 获取服务的辅助方法
    public static T GetService<T>()
    {
        var app = Current as App;
        return app._serviceProvider.GetService<T>();
    }
}