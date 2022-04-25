using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Entities
{
    [SugarTable("user")]
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
        [DisplayName("标识")]
        public string Id { get; set; }

        [SugarColumn(ColumnName = "open_id")]
        public string OpenId { get; set; }

        [SugarColumn(ColumnName = "nick_name")]
        public string NickName { get; set; }

        [SugarColumn(ColumnName = "avatar_url")]
        public string AvatarUrl { get; set; }

        public Boolean Gender { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Language { get; set; }

        [DisplayName("创建时间")]
        [SugarColumn(ColumnName = "created_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime CreateTime { get; set; }

        [DisplayName("最后登录时间")]
        [SugarColumn(ColumnName = "last_login_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime lastLoginTime { get; set; }
    }
}
