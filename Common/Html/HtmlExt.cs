using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Common.Html;
using HtmlAgilityPack;

namespace Common
{
    public class HtmlExt
    {
        public static HtmlDocument LoadHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc;
        }


        public static List<MaterialFile> ReloadHtmlLabel(string html,string htmlLabel)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            IList<HtmlNode> htmlLabels = htmlDoc.QuerySelectorAll(htmlLabel);
            List<MaterialFile> materialFiles = new List<MaterialFile>(); 
            if(htmlLabels != null && htmlLabels.Count > 0)
            {
                foreach(var item in htmlLabels)
                {
                    string sourceUrl = item.GetAttributeValue("src","")?.Trim();
                    string fileName = Path.GetFileName(sourceUrl);
                    if (htmlLabel== "mpvoice")
                    {
                        fileName = Path.GetFileName(WebUtility.HtmlDecode(item.GetAttributeValue("name", "")));
                        sourceUrl = "https://res.wx.qq.com/voice/getvoice?mediaid="+item.GetAttributeValue("voice_encode_fileid","");
                    }

                    if (string.IsNullOrEmpty(sourceUrl))
                    {
                        continue;
                    }

                    if (IsRemoteURL(sourceUrl))
                    {
                        MaterialFile materialFile = new MaterialFile();
                        string extName = string.Empty;
                        string path = Path.Combine(PathHelper.GetImageSavePath(), fileName);
                        bool isDownloaded = WebHelper.DownLoadFile(sourceUrl, path, null,out extName);
                        if (isDownloaded)
                        {
                            if (!path.Contains(extName))
                            {
                                path +=  extName;
                                fileName +=  extName;
                            }
                            materialFile.SourceUrl = sourceUrl;
                            materialFile.FilePath = path;
                            materialFile.WxUrl = string.Empty;
                            materialFile.ExtName = extName;
                            materialFile.FileName = fileName; //Path.GetFileName(materialFile.FilePath);
                            materialFiles.Add(materialFile);
                        }
                    }
                    else if(!string.IsNullOrEmpty(sourceUrl))
                    {
                        MaterialFile materialFile = new MaterialFile();
                        materialFile.SourceUrl = sourceUrl;
                        materialFile.FilePath = PathHelper.GetAppcationPath() + sourceUrl;
                        materialFile.ExtName =  Path.GetExtension(sourceUrl);
                        materialFile.FileName = Path.GetFileName(sourceUrl);
                        materialFile.WxUrl = string.Empty;
                        materialFiles.Add(materialFile);
                    }
                }
            }

            return materialFiles;
        }

        public static HtmlDocument LoadHtmlWithCache(string url, bool useProxy)
        {
            string html = CacheHelper.Get<string>(url);
            if (string.IsNullOrEmpty(html))
            {
                html = LoadHtmlStr(url, useProxy);
                if (string.IsNullOrEmpty(html))
                {
                    return null;
                }
                CacheHelper.Set<string>(url, html, 10 * 60 * 60);
            }
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc;
        }

        public static string LoadHtmlStr(string url, bool useProxy)
        {
            string html = null;
            try
            {
                html = WebHelper.GetStr(url, useProxy, true);
                //html = ClientHelper.GetStr(url);
                if (string.IsNullOrEmpty(html))
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LogHelper.GetLogger(typeof(HtmlExt)).Error(ex.ToString());

                return null;
            }
            return html;
        }

        /// <summary>
        /// 是否是远程链接
        /// </summary>
        /// <returns></returns>
        private static bool IsRemoteURL(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            bool isRemoteURL = url.StartsWith("http://") || url.StartsWith("https://");
            return isRemoteURL;
        }
    }
}
