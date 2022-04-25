using Common;
using Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Business
{
    public class CollectService
    {
        protected static bool UseProxy = false;

        public static bool TryCollectChaptersItem(Chapters chapters,string tag)
        {
            chapters.Tag = tag;
            Chapters dbRecord = BaseService.Build<Chapters>().GetSingle(p => p.SourceUrl == chapters.SourceUrl
             && p.Level == chapters.Level && p.Title == chapters.Title && p.Tag == chapters.Tag);

            if (dbRecord != null)
            {
                chapters.Content = dbRecord.Content;
                return true;
            }

            HtmlDocument htmlDoc = HtmlExt.LoadHtmlWithCache(chapters.SourceUrl, UseProxy);

            if (htmlDoc == null)
            {
                return false;
            }

            if (htmlDoc.QuerySelector(".rich_media_content") == null)
            {
                return false;
            }

            string content = htmlDoc.QuerySelector(".rich_media_content").InnerHtml?.Trim();

            //string cover = htmlDoc.QuerySelector(".rich_media_content img")?.GetAttributeValue("data-src", "")?.ToString();
            chapters.Content = content;

            int chaptersid = BaseService.Build<Chapters>().InsertReturnIdentity(chapters);

            chapters.Id = chaptersid;
            return true;
        }

    }
}
