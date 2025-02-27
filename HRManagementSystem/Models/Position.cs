using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagementSystem.Models;

public class Position
{
    public int Id { get; set; }

    [Required] [StringLength(100)] public string Title { get; set; } = string.Empty;

    [StringLength(20)] public string? Code { get; set; }

    [StringLength(500)] public string? Description { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public bool IsManagerPosition { get; set; }

    public int? Rank { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal? BaseSalary { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}