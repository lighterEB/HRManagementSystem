using System;
using System.ComponentModel.DataAnnotations;
using HRManagementSystem.Models.Identity;

namespace HRManagementSystem.Models;

public class Announcement
{
    public int Id { get; set; }

    [Required] [StringLength(200)] public string Title { get; set; } = string.Empty;

    [Required] [StringLength(4000)] public string Content { get; set; } = string.Empty;

    [Required] public DateTime Date { get; set; } = DateTime.UtcNow.Date;

    public DateTime? ExpiryDate { get; set; }

    public bool IsImportant { get; set; }

    [Required] public string CreatedById { get; set; } = string.Empty;

    public User CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}