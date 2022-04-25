using System;
using System.Collections.Generic;
using System.ComponentModel;
using SqlSugar;

namespace Entities
{
    [SugarTable("chapters")]
    public class Chapters
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
        [DisplayName("标识")]
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName("内容")]
        public string Content { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnName = "order_number")]
        [DisplayName("排序")]
        public int OrderNumber { get; set; }

        /// <summary>
        /// 来源地址
        /// </summary>
        [SugarColumn(ColumnName = "source_url")]
        [DisplayName("来源地址")]
        public string SourceUrl { get; set; }

        /// <summary>
        /// 是章节标题
        /// </summary>
        [DisplayName("是否标题")]
        [SugarColumn(ColumnName = "is_title")]
        public bool IsTitle { get; set; }

        [SugarColumn(ColumnName = "parent_id")]
        [DisplayName("父标识")]
        public int ParentId { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [DisplayName("级别")]
        public int Level { get; set; }

        [SugarColumn(IsIgnore = true)]
        [DisplayName("子节点")]
        public List<Chapters> Childs { get; set; }

        [SugarColumn(ColumnName = "tag")]
        [DisplayName("标记")]
        public string Tag { get; set; }

        [SugarColumn(ColumnName = "wechat_config_id")]
        [DisplayName("微信配置ID")]
        public int WechatConfigId { get; set; }

        [DisplayName("公众号名称")]
        [SugarColumn(IsIgnore = true)]
        public string WechatName { get; set; }
    }
}
