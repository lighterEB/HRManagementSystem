using System;
using System.ComponentModel.DataAnnotations;
using HRManagementSystem.Models.Identity;

namespace HRManagementSystem.Models;

public class AuditLog
{
    public int Id { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }

    [Required] [StringLength(50)] public string Action { get; set; } = string.Empty;

    [Required] [StringLength(100)] public string EntityName { get; set; } = string.Empty;

    [StringLength(50)] public string? EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    [StringLength(50)] public string? ClientIP { get; set; }

    [Required] public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}