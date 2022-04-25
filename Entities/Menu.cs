using System;
using System.ComponentModel;
using SqlSugar;

namespace Entities
{
    /// <summary>
    /// 菜单
    /// </summary>
    [SugarTable("menu")]
    public class Menu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true,ColumnName ="id")]
        [DisplayName("标识")]
        public int Id { get; set; }

        [DisplayName("菜单名称")]
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }

        [DisplayName("描述")]
        [SugarColumn(ColumnName = "summary")]
        public string Summary { get; set; }

        [DisplayName("标签")]
        [SugarColumn(ColumnName = "tag")]
        public string Tag { get; set; }

        [DisplayName("背景颜色")]
        [SugarColumn(ColumnName = "bg_color")]
        public string BgColor { get; set; }

        [DisplayName("圆型背景图")]
        [SugarColumn(ColumnName = "radius_bg_color")]
        public string RadiusBgColor { get; set; }

        [DisplayName("圆型图")]
        [SugarColumn(ColumnName = "radius_color")]
        public string RadiusColor { get; set; }

        [DisplayName("标题颜色")]
        [SugarColumn(ColumnName = "name_color")]
        public string NameColor { get; set; }

        [DisplayName("字体颜色")]
        [SugarColumn(ColumnName = "color")]
        public string Color { get; set; }
    }
}
