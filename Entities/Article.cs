using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Entities
{
    [SugarTable("article")]
    public class Article
    {
        /// <summary>
        /// id标识
        /// 指定主键和自增列，当然数据库中也要设置主键和自增列才会有效
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true,ColumnName ="id")]
        [DisplayName("标识")]
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("标题")]
        [SugarColumn(ColumnName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DisplayName("作者")]
        [SugarColumn(ColumnName = "author")]
        public string Author { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [DisplayName("来源")]
        [SugarColumn(ColumnName = "source")]
        public string Source { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        [SugarColumn(ColumnName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        [DisplayName("文本内容")]
        [SugarColumn(ColumnName = "content")]
        public string Content { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [DisplayName("封面")]
        [UIHint("Picture")]
        [SugarColumn(ColumnName = "cover")]
        public string Cover { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [SugarColumn(ColumnName ="created_time")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        [SugarColumn(ColumnName = "updated_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// 1:已同步,0:未同步
        /// </summary>
        [DisplayName("已同步到公众号")]
        [UIHint("IsSync")]
        [SugarColumn(ColumnName = "is_sync")]
        public bool IsSync { get; set; }

        [DisplayName("MediaId")]
        [SugarColumn(ColumnName = "media_id")]
        public string MediaId { get; set; }


        [DisplayName("第三方来源连接")]
        [SugarColumn(ColumnName = "source_url")]
        public string SourceUrl { get; set; }

        [DisplayName("md5")]
        [SugarColumn(ColumnName = "md5")]
        public string Md5 { get; set; }

        [DisplayName("url")]
        [SugarColumn(ColumnName = "url")]
        public string Url { get; set; }

        [DisplayName("第三方来源连接")]
        [SugarColumn(ColumnName = "short_url")]
        public string ShortUrl { get; set; }

        [DisplayName("微信配置ID")]
        [SugarColumn(ColumnName = "wechat_config_id")]
        public int WechatConfigId { get; set; }


        [DisplayName("公众号名称")]
        [SugarColumn(IsIgnore = true)]
        public string WechatName { get; set; }
    }
}
