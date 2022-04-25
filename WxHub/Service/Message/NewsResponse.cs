using System;
using System.Collections.Generic;

namespace OperateCenter.Service.Message
{
    public class NewsResponse : BaseResponse
    {
        public List<NewsDetail> news_item { get; set; }
    }
}
