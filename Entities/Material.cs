using System;
using System.ComponentModel;
using SqlSugar;

namespace Entities
{
    [SugarTable("material")]
    public class Material
    {
        /// <summary>
        /// id标识
        /// 指定主键和自增列，当然数据库中也要设置主键和自增列才会有效
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [DisplayName("标识")]
        public int Id { get; set; }
        [DisplayName("文件地址")]
        [SugarColumn(ColumnName = "file_path")]
        public String FilePath { get; set; }
        [DisplayName("标题")]
        [SugarColumn(ColumnName = "title")]
        public String Title { get; set; }
        [DisplayName("介绍")]
        [SugarColumn(ColumnName = "Introduction")]
        public String Introduction { get; set; }
        [DisplayName("类型")]
        [SugarColumn(ColumnName = "type")]
        public String Type { get; set; }
        [DisplayName("微信媒体id")]
        [SugarColumn(ColumnName = "media_id")]
        public String MediaId { get; set; }
        [SugarColumn(ColumnName = "url")]
        public String Url { get; set; }
        [SugarColumn(ColumnName = "source")]
        public string Source { get; set; }
        [DisplayName("创建时间")]
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }
        [DisplayName("微信配置ID")]
        [SugarColumn(ColumnName = "wechat_config_id")]
        public int WechatConfigId { get; set; }
    }
}
