using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Models.Identity
{
    // 集成Identity框架并扩展业务属性
    public class User : IdentityUser
    {
        // 基础信息
        [PersonalData]
        [Required(ErrorMessage = "真实姓名不能为空")]
        [StringLength(100, ErrorMessage = "真实姓名不能超过100个字符")]
        public string RealName { get; set; } = string.Empty;

        // 关联员工ID
        [ForeignKey(nameof(Employee))] public int? EmployeeId { get; set; }

        // 账户状态
        public bool IsActive { get; set; } = true;

        // 时间戳
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginTime { get; set; }

        // 导航属性
        public virtual Employee? Employee { get; set; }
    }
}
