using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models;

public class Department
{
    public int Id { get; init; }

    [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;

    [StringLength(500)] public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}