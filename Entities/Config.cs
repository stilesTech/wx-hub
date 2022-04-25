using System;
using System.ComponentModel;
using SqlSugar;

namespace Entities
{
    [SugarTable("config")]
    public class Config
    {
        [SugarColumn(IsPrimaryKey = true,ColumnName ="config_key")]
        [DisplayName("配置Key")]
        public string ConfigKey { get; set; }
        [SugarColumn(ColumnName = "config_value")]
        [DisplayName("配置Value")]
        public string ConfigValue { get; set; }
    }
}
