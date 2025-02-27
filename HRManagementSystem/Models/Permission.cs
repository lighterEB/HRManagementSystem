using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models;

public class Permission
{
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;

    [Required] [StringLength(100)] public string NormalizedName { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string Code { get; set; } = string.Empty;

    [StringLength(255)] public string? Description { get; set; }

    [Required] [StringLength(50)] public string Category { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}