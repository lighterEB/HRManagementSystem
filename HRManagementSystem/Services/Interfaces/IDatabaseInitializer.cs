using System.Threading.Tasks;

namespace HRManagementSystem.Services.Interfaces;

public interface IDatabaseInitializer
{
    /// <summary>
    ///     确保数据库结构已创建并迁移到最新版本
    /// </summary>
    Task EnsureDatabaseCreatedAsync();

    /// <summary>
    ///     初始化系统所需的基础数据
    /// </summary>
    Task InitializeSystemDataAsync();
}