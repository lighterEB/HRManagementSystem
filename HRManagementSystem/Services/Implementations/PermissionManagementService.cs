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

namespace HRManagementSystem.Services.Implementations;

public class PermissionManagementService : IPermissionManagementService
{
    private readonly AppDbContext _dbContext;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public PermissionManagementService(
        UserManager<User> userManager,
        RoleManager<ApplicationRole> roleManager,
        AppDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    // 角色管理实现
    public async Task<ApplicationRole?> CreateRoleAsync(string roleName, string description, bool isSystemRole = false)
    {
        try
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return null;

            var role = new ApplicationRole
            {
                Name = roleName,
                Description = description,
                IsSystemRole = isSystemRole,
                NormalizedName = roleName.ToUpper()
            };

            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded ? role : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null || role.IsSystemRole)
                return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<ApplicationRole?> UpdateRoleAsync(string roleId, string roleName, string description)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return null;

            role.Name = roleName;
            role.Description = description;
            role.NormalizedName = roleName.ToUpper();

            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded ? role : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId)
    {
        return await _roleManager.FindByIdAsync(roleId);
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    // 权限管理实现
    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return await _dbContext.Permissions
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Code)
            .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(string roleId)
    {
        return await _dbContext.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task<bool> AssignPermissionsToRoleAsync(string roleId, IEnumerable<string> permissionIds)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return false;

            var existingPermissions = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

            _dbContext.RolePermissions.RemoveRange(existingPermissions);

            var newPermissions = permissionIds.Select(pid => new RolePermission
            {
                RoleId = roleId,
                PermissionId = pid
            });

            await _dbContext.RolePermissions.AddRangeAsync(newPermissions);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RemovePermissionsFromRoleAsync(string roleId, IEnumerable<string> permissionIds)
    {
        try
        {
            var permissionsToRemove = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
                .ToListAsync();

            _dbContext.RolePermissions.RemoveRange(permissionsToRemove);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // 菜单管理实现
    public async Task<IEnumerable<Menu>> GetAllMenusAsync()
    {
        return await _dbContext.Menus
            .Include(m => m.ChildMenus)
            .Where(m => m.ParentId == null)
            .OrderBy(m => m.OrderIndex)
            .ToListAsync();
    }

    public async Task<IEnumerable<Menu>> GetRoleMenusAsync(string roleId)
    {
        return await _dbContext.RoleMenus
            .Where(rm => rm.RoleId == roleId)
            .Select(rm => rm.Menu)
            .ToListAsync();
    }

    public async Task<bool> AssignMenusToRoleAsync(string roleId, IEnumerable<string> menuIds)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return false;

            var existingMenus = await _dbContext.RoleMenus
                .Where(rm => rm.RoleId == roleId)
                .ToListAsync();

            _dbContext.RoleMenus.RemoveRange(existingMenus);

            var newMenus = menuIds.Select(mid => new RoleMenu
            {
                RoleId = roleId,
                MenuId = mid
            });

            await _dbContext.RoleMenus.AddRangeAsync(newMenus);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RemoveMenusFromRoleAsync(string roleId, IEnumerable<string> menuIds)
    {
        try
        {
            var menusToRemove = await _dbContext.RoleMenus
                .Where(rm => rm.RoleId == roleId && menuIds.Contains(rm.MenuId))
                .ToListAsync();

            _dbContext.RoleMenus.RemoveRange(menusToRemove);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // 用户角色管理实现
    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Array.Empty<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> AssignRolesToUserAsync(string userId, IEnumerable<string> roleIds)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var roles = await _roleManager.Roles
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            var result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RemoveRolesFromUserAsync(string userId, IEnumerable<string> roleIds)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var roles = await _roleManager.Roles
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            return result.Succeeded;
        }
        catch (Exception)
        {
            return false;
        }
    }
}