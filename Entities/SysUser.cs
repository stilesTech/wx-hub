using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("sys_user")]
    public class SysUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true,ColumnName ="id")]
        [DisplayName("标识")]
        public int Id { get; set; }

        [DisplayName("账号")]
        [SugarColumn(ColumnName = "account")]
        public string Account { get; set; }

        [DisplayName("用户名")]
        [SugarColumn(ColumnName = "user_name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空!")]
        [RegularExpression(@"^[\u4E00-\u9FA5\uf900-\ufa2d\w\.\s]{6,18}$", ErrorMessage = "*6-18位拼音或数字")]
        [DataType(DataType.Password)]
        [MaxLength(50)]
        [MinLength(6)]
        [DisplayName("密码")]
        [SugarColumn(ColumnName = "password")]
        public string Password { get; set; }

        [DisplayName("角色")]
        [SugarColumn(ColumnName = "role_id")]
        public int RoleId { get; set; }

        [DisplayName("是否锁定")]
        [SugarColumn(ColumnName = "is_locked")]
        public bool IsLocked { get; set; }

        [DisplayName("状态")]
        [SugarColumn(ColumnName = "status")]
        public byte Status { get; set; }

        [DisplayName("创建时间")]
        [SugarColumn(ColumnName = "created_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime CreatedTime { get; set; }

        [DisplayName("更新时间")]
        [SugarColumn(ColumnName = "updated_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime UpdatedTime { get; set; }


    }
}
