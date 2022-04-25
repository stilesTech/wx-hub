using System;
using System.ComponentModel;
using SqlSugar;

namespace Entities
{
    [SugarTable("file_storage")]
    public class FileStorage
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true,ColumnName ="id")]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "name")]
        [DisplayName("名称")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "source_url")]
        [DisplayName("来源地址")]
        public string SourceUrl { get; set; }

        [SugarColumn(ColumnName = "file_streams")]
        [DisplayName("文件流")]
        public byte[] FileStreams { get; set; }

        [SugarColumn(ColumnName = "type")]
        [DisplayName("类型")]
        public string Type { get; set; }

        [SugarColumn(ColumnName = "create_time")]
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "wechat_config_id")]
        [DisplayName("微信配置ID")]
        public int WechatConfigId { get; set; }
    }
}
