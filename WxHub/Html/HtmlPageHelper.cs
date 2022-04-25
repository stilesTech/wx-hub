using Business;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperateCenter.Html
{
    public class HtmlPageHelper
    {
        public static string GetRenderContent(List<Chapters> chaptersList,int wechatConfigId)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in chaptersList)
            {
                if (item.IsTitle)
                {
                    //if (item.Level == 1)
                    //{
                    //    builder.Append(HtmlTemplate.GetModule(item.Title));
                    //}
                    //else if (item.Level == 2)
                    //{
                    //    builder.Append(HtmlTemplate.GetSubModule(item.Title));
                    //}
                    builder.Append(HtmlTemplate.GetModule(item.Title));
                }
                else if (!string.IsNullOrEmpty(item.SourceUrl))
                {
                    Article article = BaseService.Build<Article>().GetSingle(p => p.WechatConfigId== wechatConfigId && p.Title == item.Title && p.IsSync && p.SourceUrl == item.SourceUrl);
                    if (article != null)
                    {
                        builder.Append(HtmlTemplate.GetLinkTitle(item.Title, article.ShortUrl));
                    }
                }

                if (item.Childs != null)
                {
                    builder.Append(GetRenderContent(item.Childs, wechatConfigId));
                }
            }
            return builder.ToString();
        }
    }
}
