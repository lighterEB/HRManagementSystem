using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagementSystem.Models
{
    public class Position
    {
        public int Id { get; init; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}