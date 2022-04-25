using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Querys
{
    public class LoginModel
    {
        [Required]
        [DisplayName("账号")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }
        [DisplayName("记住我?")]
        public bool RememberMe { get; set; }
    }

    
    
}
