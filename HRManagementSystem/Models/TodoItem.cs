using System;
using System.ComponentModel.DataAnnotations;
using HRManagementSystem.Models.Identity;

namespace HRManagementSystem.Models;

public class TodoItem
{
    public int Id { get; set; }

    [Required] [StringLength(500)] public string Task { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public int? Priority { get; set; }

    public int? AssignedToId { get; set; }
    public Employee? AssignedTo { get; set; }

    [Required] public string CreatedById { get; set; } = string.Empty;

    public User CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}