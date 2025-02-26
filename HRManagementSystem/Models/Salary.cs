using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagementSystem.Models
{
    public class Salary
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        public int Month { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Allowance { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Bonus { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Overtime { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Insurance { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Tax { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherDeductions { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalAmount { get; set; }
        
        [Required]
        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Pending";
        
        public DateTime? PaymentDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public string? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
    }
}