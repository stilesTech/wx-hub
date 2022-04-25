using System;
namespace Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class AuthUser
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string UserName { get; set; }
    }
}
