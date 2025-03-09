using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRManagementSystem.Data;
using HRManagementSystem.Models;
using HRManagementSystem.Models.Identity;
using HRManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRManagementSystem.Services.Implementations
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(
            AppDbContext dbContext,
            UserManager<User> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<DatabaseInitializer> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 确保数据库已创建并更新到最新版本
        /// </summary>
        public async Task EnsureDatabaseCreatedAsync()
        {
            try
            {
                // 应用所有待处理的迁移
                await _dbContext.Database.MigrateAsync();
                _logger.LogInformation("数据库迁移完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "数据库迁移失败");
                throw; // 重新抛出异常，因为这是一个关键操作
            }
        }

        /// <summary>
        /// 初始化系统所需的基础数据
        /// </summary>
        public async Task InitializeSystemDataAsync()
        {
            try
            {
                // 使用事务确保数据一致性
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                
                // 1. 初始化角色
                await InitializeRolesAsync();
                
                // 2. 初始化管理员用户
                await InitializeAdminUserAsync();
                
                // 3. 初始化菜单
                await InitializeMenusAsync();
                
                // 4. 初始化角色-菜单关联
                await InitializeRoleMenusAsync();
                
                // 提交事务
                await transaction.CommitAsync();
                
                _logger.LogInformation("系统数据初始化成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化系统数据时发生错误");
                // 在这里可以决定是否重新抛出异常
                // 对于初始系统数据，可能希望即使失败也不要阻止应用启动
            }
        }

        // 初始化角色
        private async Task InitializeRolesAsync()
        {
            // 定义基础角色
            var roles = new[]
            {
                new ApplicationRole { Name = "Admin", Description = "系统管理员", IsSystemRole = true },
                new ApplicationRole { Name = "HR", Description = "人力资源专员", IsSystemRole = true },
                new ApplicationRole { Name = "Manager", Description = "部门经理", IsSystemRole = false },
                new ApplicationRole { Name = "Employee", Description = "普通员工", IsSystemRole = true }
            };
            
            foreach (var role in roles)
            {
                // 检查角色是否已存在
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                    _logger.LogInformation("创建角色: {RoleName}", role.Name);
                }
            }
        }

        // 初始化管理员用户
        private async Task InitializeAdminUserAsync()
        {
            var adminUser = await _userManager.FindByNameAsync("admin");
            
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "13800000000",
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    RealName = "系统管理员", // 必填字段
                    // CreatedAt有默认值，可以不用显式设置
                    // EmployeeId保持为null，表示这是系统用户，不关联具体员工
                };
                
                // 创建管理员用户，密码为 "Admin@123"
                var result = await _userManager.CreateAsync(adminUser, "Admin@123");
                
                if (result.Succeeded)
                {
                    // 分配Admin角色
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    _logger.LogInformation("创建管理员用户成功");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("创建管理员用户失败: {Errors}", errors);
                }
            }
        }

        // 初始化菜单
        private async Task InitializeMenusAsync()
        {
            // 检查是否已有菜单
            if (await _dbContext.Menus.AnyAsync())
            {
                return; // 已存在菜单，跳过初始化
            }
            
            // 创建父菜单
            var dashboardMenu = new Menu
            {
                Name = "仪表盘",
                Icon = "Dashboard",
                Url = "DashboardView",
                OrderIndex = 1,
                IsVisible = true,
                IsActive = true
            };
            
            var employeeMenu = new Menu
            {
                Name = "员工管理",
                Icon = "People",
                OrderIndex = 2,
                IsVisible = true,
                IsActive = true
            };
            
            var departmentMenu = new Menu
            {
                Name = "部门管理",
                Icon = "Business",
                OrderIndex = 3,
                IsVisible = true,
                IsActive = true
            };
            
            var attendanceMenu = new Menu
            {
                Name = "考勤管理",
                Icon = "Schedule",
                OrderIndex = 4,
                IsVisible = true,
                IsActive = true
            };
            
            var systemMenu = new Menu
            {
                Name = "系统管理",
                Icon = "Settings",
                OrderIndex = 5,
                IsVisible = true,
                IsActive = true
            };
            
            // 添加父菜单
            _dbContext.Menus.Add(dashboardMenu);
            _dbContext.Menus.Add(employeeMenu);
            _dbContext.Menus.Add(departmentMenu);
            _dbContext.Menus.Add(attendanceMenu);
            _dbContext.Menus.Add(systemMenu);
            
            // 保存以获取ID
            await _dbContext.SaveChangesAsync();
            
            // 创建子菜单
            var childMenus = new List<Menu>
            {
                // 员工管理子菜单
                new Menu
                {
                    Name = "员工列表",
                    Icon = "FormatListBulleted",
                    Url = "EmployeeListView",
                    ParentId = employeeMenu.Id,
                    OrderIndex = 1,
                    IsVisible = true,
                    IsActive = true
                },
                new Menu
                {
                    Name = "员工档案",
                    Icon = "Assignment",
                    Url = "EmployeeProfileView",
                    ParentId = employeeMenu.Id,
                    OrderIndex = 2,
                    IsVisible = true,
                    IsActive = true
                },
                
                // 部门管理子菜单
                new Menu
                {
                    Name = "部门列表",
                    Icon = "FormatListBulleted",
                    Url = "DepartmentListView",
                    ParentId = departmentMenu.Id,
                    OrderIndex = 1,
                    IsVisible = true,
                    IsActive = true
                },
                new Menu
                {
                    Name = "组织架构",
                    Icon = "AccountTree",
                    Url = "OrganizationStructureView",
                    ParentId = departmentMenu.Id,
                    OrderIndex = 2,
                    IsVisible = true,
                    IsActive = true
                },
                
                // 考勤管理子菜单
                new Menu
                {
                    Name = "考勤记录",
                    Icon = "EventNote",
                    Url = "AttendanceRecordView",
                    ParentId = attendanceMenu.Id,
                    OrderIndex = 1,
                    IsVisible = true,
                    IsActive = true
                },
                new Menu
                {
                    Name = "请假申请",
                    Icon = "EventBusy",
                    Url = "LeaveRequestView",
                    ParentId = attendanceMenu.Id,
                    OrderIndex = 2,
                    IsVisible = true,
                    IsActive = true
                },
                
                // 系统管理子菜单
                new Menu
                {
                    Name = "用户管理",
                    Icon = "ManageAccounts",
                    Url = "UserManagementView",
                    ParentId = systemMenu.Id,
                    OrderIndex = 1,
                    IsVisible = true,
                    IsActive = true
                },
                new Menu
                {
                    Name = "角色管理",
                    Icon = "AdminPanelSettings",
                    Url = "RoleManagementView",
                    ParentId = systemMenu.Id,
                    OrderIndex = 2,
                    IsVisible = true,
                    IsActive = true
                },
                new Menu
                {
                    Name = "菜单管理",
                    Icon = "Menu",
                    Url = "MenuManagementView",
                    ParentId = systemMenu.Id,
                    OrderIndex = 3,
                    IsVisible = true,
                    IsActive = true
                }
            };
            
            // 添加子菜单
            _dbContext.Menus.AddRange(childMenus);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("初始化菜单成功");
        }

        // 初始化角色-菜单关联
        private async Task InitializeRoleMenusAsync()
        {
            // 获取所有角色和菜单
            var roles = await _roleManager.Roles.ToListAsync();
            var menus = await _dbContext.Menus.ToListAsync();
            
            if (!roles.Any() || !menus.Any())
            {
                return; // 没有角色或菜单，跳过初始化
            }
            
            // 检查是否已有角色-菜单关联
            if (await _dbContext.RoleMenus.AnyAsync())
            {
                return; // 已存在关联，跳过初始化
            }
            
            var roleMenus = new List<RoleMenu>();
            
            // 1. Admin角色可以访问所有菜单
            var adminRole = roles.FirstOrDefault(r => r.Name == "Admin");
            if (adminRole != null)
            {
                foreach (var menu in menus)
                {
                    roleMenus.Add(new RoleMenu
                    {
                        RoleId = adminRole.Id,
                        MenuId = menu.Id
                    });
                }
            }
            
            // 2. HR角色可以访问部分菜单
            var hrRole = roles.FirstOrDefault(r => r.Name == "HR");
            if (hrRole != null)
            {
                // 获取特定菜单
                var hrAccessibleMenus = menus.Where(m => 
                    m.Name == "仪表盘" || 
                    m.Name == "员工管理" || 
                    m.Name == "部门管理" || 
                    m.Name == "考勤管理" ||
                    m.ParentId != null && (
                        menus.Any(pm => pm.Id == m.ParentId && (
                            pm.Name == "员工管理" || 
                            pm.Name == "部门管理" || 
                            pm.Name == "考勤管理"
                        ))
                    )
                ).ToList();
                
                foreach (var menu in hrAccessibleMenus)
                {
                    roleMenus.Add(new RoleMenu
                    {
                        RoleId = hrRole.Id,
                        MenuId = menu.Id
                    });
                }
            }
            
            // 3. Manager角色可以访问部分菜单
            var managerRole = roles.FirstOrDefault(r => r.Name == "Manager");
            if (managerRole != null)
            {
                // 获取特定菜单
                var managerAccessibleMenus = menus.Where(m => 
                    m.Name == "仪表盘" || 
                    m.Name == "部门管理" || 
                    m.Name == "考勤管理" ||
                    m.ParentId != null && (
                        menus.Any(pm => pm.Id == m.ParentId && (
                            pm.Name == "部门管理" || 
                            pm.Name == "考勤管理"
                        ))
                    )
                ).ToList();
                
                foreach (var menu in managerAccessibleMenus)
                {
                    roleMenus.Add(new RoleMenu
                    {
                        RoleId = managerRole.Id,
                        MenuId = menu.Id
                    });
                }
            }
            
            // 4. Employee角色只能访问基本功能
            var employeeRole = roles.FirstOrDefault(r => r.Name == "Employee");
            if (employeeRole != null)
            {
                // 获取特定菜单
                var employeeAccessibleMenus = menus.Where(m => 
                    m.Name == "仪表盘" || 
                    m.Name == "考勤管理" ||
                    m.ParentId != null && (
                        menus.Any(pm => pm.Id == m.ParentId && 
                            pm.Name == "考勤管理"
                        )
                    )
                ).ToList();
                
                foreach (var menu in employeeAccessibleMenus)
                {
                    roleMenus.Add(new RoleMenu
                    {
                        RoleId = employeeRole.Id,
                        MenuId = menu.Id
                    });
                }
            }
            
            // 添加角色-菜单关联
            _dbContext.RoleMenus.AddRange(roleMenus);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("初始化角色-菜单关联成功");
        }
    }
}      