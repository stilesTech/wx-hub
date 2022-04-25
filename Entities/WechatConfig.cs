using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Entities
{
    [SugarTable("wechat_config")]
    public class WechatConfig
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
        [DisplayName("名称")]
        [SugarColumn(ColumnName="name")]
        public string Name { get; set; }

        [DisplayName("AppId")]
        [SugarColumn(ColumnName = "app_id")]
        public string AppId { get; set; }

        [DisplayName("AppSecret")]
        [SugarColumn(ColumnName = "app_secret")]
        public string AppSecret { get; set; }

        [DisplayName("目录封面")]
        [SugarColumn(ColumnName = "article_menu_cover")]
        public string ArticleMenuCover { get; set; }

        [DisplayName("内容封面")]
        [SugarColumn(ColumnName = "article_cover")]
        public string ArticleCover { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        [SugarColumn(ColumnName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        [SugarColumn(ColumnName = "update_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime UpdateTime { get; set; }

        [DisplayName("头部模版")]
        [SugarColumn(ColumnName = "top_html")]
        public String TopHtml { get; set; }

        [DisplayName("底部模版")]
        [SugarColumn(ColumnName = "bottom_html")]
        public string BottomHtml { get; set; }
    }
}
