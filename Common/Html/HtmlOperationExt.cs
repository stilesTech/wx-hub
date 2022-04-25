using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Html
{
    public class HtmlOperationExt
    {
        public static string RemoveVideo(string content)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            IList<HtmlNode> htmlMpvoice = htmlDoc.QuerySelectorAll("mpvoice");
            foreach (var item in htmlMpvoice)
            {
                string url = item.GetAttributeValue("src", "");

                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }

                content = content.Replace(item.OuterHtml, "");
            }
            return content;
        }

        /// <summary>
        /// 异常异常的html标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveAbnormalHtml(string content)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            IList<HtmlNode> htmlTag = htmlDoc.QuerySelectorAll("mpcpc");
            foreach (var item in htmlTag)
            {
                content = content.Replace(item.OuterHtml, "");
            }
            return content;
        }
    }
}
