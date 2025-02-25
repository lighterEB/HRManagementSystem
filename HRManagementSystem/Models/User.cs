using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Models;

public class User : IdentityUser
{
    [Required] [StringLength(50)] public string FirstName { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string LastName { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}

public enum UserRole
{
    Administrator,
    HRManager,
    HRStaff,
    Employee
}