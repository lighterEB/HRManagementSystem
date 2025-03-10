using System.Collections.Generic;
using System.Threading.Tasks;
using HRManagementSystem.Models;
using HRManagementSystem.Models.Identity;

namespace HRManagementSystem.Services.Interfaces;

public interface IPermissionManagementService
{
    // 角色管理
    Task<ApplicationRole?> CreateRoleAsync(string roleName, string description, bool isSystemRole = false);
    Task<bool> DeleteRoleAsync(string roleId);
    Task<ApplicationRole?> UpdateRoleAsync(string roleId, string roleName, string description);
    Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
    Task<ApplicationRole?> GetRoleByIdAsync(string roleId);
    Task<bool> RoleExistsAsync(string roleName);

    // 权限管理
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(string roleId);
    Task<bool> AssignPermissionsToRoleAsync(string roleId, IEnumerable<string> permissionIds);
    Task<bool> RemovePermissionsFromRoleAsync(string roleId, IEnumerable<string> permissionIds);

    // 菜单管理
    Task<IEnumerable<Menu>> GetAllMenusAsync();
    Task<IEnumerable<Menu>> GetRoleMenusAsync(string roleId);
    Task<bool> AssignMenusToRoleAsync(string roleId, IEnumerable<string> menuIds);
    Task<bool> RemoveMenusFromRoleAsync(string roleId, IEnumerable<string> menuIds);

    // 用户角色管理
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<bool> AssignRolesToUserAsync(string userId, IEnumerable<string> roleIds);
    Task<bool> RemoveRolesFromUserAsync(string userId, IEnumerable<string> roleIds);
}