using System;
namespace Entities.Ext
{
    /// <summary>
    /// 显示消息提示对话框信息类
    /// 黄泽庭
    /// 2015/1/15
    /// </summary>
    [Serializable]
    public class MessageBox
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
    }
}
