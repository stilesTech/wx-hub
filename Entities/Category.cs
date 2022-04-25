using System;
using System.ComponentModel;
using SqlSugar;

namespace Entities
{
    [SugarTable("category")]
    public class Category
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
        [DisplayName("标识")]
        public int Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        [SugarColumn(ColumnName = "parent_id")]
        public int ParentId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }
    }
}
