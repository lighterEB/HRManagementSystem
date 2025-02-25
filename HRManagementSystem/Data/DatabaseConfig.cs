using System;
using System.IO;

namespace HRManagementSystem.Data
{
    public static class DatabaseConfig
    {
        public static string DbPath
        {
            get
            {
                // 在应用程序目录下创建数据库文件
                var folder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "HRManagementSystem");
                
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                
                return Path.Combine(folder, "hrms.db");
            }
        }
    }
}