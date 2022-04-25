using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Querys
{
    public class ChaptersCollectQuery
    {
        public string title { get; set; }

        public int wechatConfigId { get; set; }

        public string tag { get; set; }

        public List<CollecItem> collecs { get; set; }

    }

    public class CollecItem
    {
        public string title { get; set; }

        public string url { get; set; }

        public bool isTitle { get; set; }

        public int level { get; set; }
    }
}

