using System;
namespace OperateCenter.Service.Message
{
    public class MaterialResponse : BaseResponse
    {
        public String media_id { get; set; }
        public String url { get; set; }
        public String short_url { get; set; }
    }
}
