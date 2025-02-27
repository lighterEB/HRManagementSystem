using System.Threading.Tasks;
using HRManagementSystem.Models.Identity;

namespace HRManagementSystem.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(string userId);
    Task<User?> GetUserByEmployeeIdAsync(int employeeId);
    Task UpdateLastLoginAsync(string userId);
    Task<bool> ResetPasswordAsync(string userId, string newPassword);
    Task<bool> ChangeUserStatusAsync(string userId, bool isActive);
}