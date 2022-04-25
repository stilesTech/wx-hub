using System;
namespace OperateCenter.Service.Message
{
    public class NewsDetail
    {
        public String title { get; set; }

        public String author { get; set; }

        public String digest { get; set; }

        public String content { get; set; }

        public String content_source_url { get; set; }

        public String thumb_media_id { get; set; }

        public int show_cover_pic { get; set; }

        public String url { get; set; }

        public String thumb_url { get; set; }

        public int need_open_comment { get; set; }

        public int only_fans_can_comment { get; set; }
    }
}
