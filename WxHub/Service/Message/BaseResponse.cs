using System;
namespace OperateCenter.Service.Message
{
    public class BaseResponse
    {
        public int? errcode { get; set; }
        public string errmsg { get; set; }
    }
}
