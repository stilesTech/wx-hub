using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Business;
using Common;
using Common.Html;
using Entities;
using Entities.Enum;
using HtmlAgilityPack;
using OperateCenter.Constant;
using OperateCenter.Service.Message;

namespace OperateCenter.Service
{
    public class OldPushService
    {  
        public static MaterialResponse UploadNews(Article article)
        { 
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //标题
            keyValues.Add("title", article.Title);

            string media_id = GetCoverMetartId(article.WechatConfigId,article.Cover);

            //图文消息的封面图片素材id（必须是永久 media_ID）
            keyValues.Add("thumb_media_id", media_id);
            //作者
            keyValues.Add("author", article.Author);
            //图文消息的摘要，仅有单图文消息才有摘要，多图文此处为空
            keyValues.Add("digest", article.Description);
            //是否显示封面，0为false，即不显示，1为true，即显示
            keyValues.Add("show_cover_pic", "0");

            article.Content = ReplateContent(article.Content, MaterialTypeEnum.image);
            article.Content = removeVideo(article.Content);

            //图文消息的具体内容，支持HTML标签，必须少于2万字符，小于1M，且此处会去除JS
            keyValues.Add("content", HttpUtility.UrlEncode(article.Content));
            //图文消息的原文地址，即点击“阅读原文”后的URL
            keyValues.Add("content_source_url", "");
            //是否打开评论，0不打开，1打开
            keyValues.Add("need_open_comment", "0");
            //是否打开评论，0不打开，1打开
            keyValues.Add("only_fans_can_comment", "0");



            string requestParams= ConvertToUrlParams(keyValues);

            string jsonResult = HttpHelper.HttpPost(UrlConstant.UploadNewsURL, requestParams);

            if (string.IsNullOrEmpty(jsonResult))
            {
                return null;
            }

            var mediaIdRsponse = JsonHelper.ToEntity<MaterialResponse>(jsonResult);

            String md5 = string.Empty;
            if (!string.IsNullOrEmpty(mediaIdRsponse.media_id))
            {
                requestParams = "mediaId=" + mediaIdRsponse.media_id;
                jsonResult = HttpHelper.HttpPost(UrlConstant.GetNewsURL, requestParams);
                NewsResponse urlResponse = JsonHelper.ToEntity<NewsResponse>(jsonResult);

                if (urlResponse.errcode != null)
                {
                    MaterialResponse response = new MaterialResponse();
                    response.errcode = urlResponse.errcode;
                    response.errmsg = urlResponse.errmsg;
                    return response;
                }

                mediaIdRsponse.url = urlResponse.news_item[0].url;
                md5 = MD5Helper.ComputeHash16(mediaIdRsponse.url);
                mediaIdRsponse.short_url = UrlConstant.GetShortUrl(md5);

                article.IsSync = true;
                article.MediaId = mediaIdRsponse.media_id;
                article.Url = mediaIdRsponse.url;
                article.Md5 = md5;
                article.ShortUrl = mediaIdRsponse.short_url;
                UpdateArticle(article);
            }

            return mediaIdRsponse;
        }

        public static MaterialResponse UploadNewsList(List<Article> articles)
        { 

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //标题
            keyValues.Add("ids", String.Join(',',articles.Select(p=>p.Id)));

            string media_id = GetCoverMetartId(articles[0].WechatConfigId, articles[0].Cover);

            //图文消息的封面图片素材id（必须是永久 media_ID）
            keyValues.Add("thumb_media_id", media_id);
            //作者
            keyValues.Add("author", articles[0].Author);
            //图文消息的摘要，仅有单图文消息才有摘要，多图文此处为空
            keyValues.Add("digest", "");
            //是否显示封面，0为false，即不显示，1为true，即显示
            keyValues.Add("show_cover_pic", "0");
            //图文消息的原文地址，即点击“阅读原文”后的URL
            keyValues.Add("content_source_url", "");
            //是否打开评论，0不打开，1打开
            keyValues.Add("need_open_comment", "0");
            //是否打开评论，0不打开，1打开
            keyValues.Add("only_fans_can_comment", "0");

            foreach (var article in articles)
            {
                article.Content = ReplateContent(article.Content, MaterialTypeEnum.image);
                article.Content = removeVideo(article.Content);
                BaseService.Build<Article>().Update(article);
            }

            //图文消息的具体内容，支持HTML标签，必须少于2万字符，小于1M，且此处会去除JS
            //keyValues.Add("content", HttpUtility.UrlEncode(article.Content));

            string requestParams = ConvertToUrlParams(keyValues);

            string jsonResult = HttpHelper.HttpPost(UrlConstant.UploadNewsListURL, requestParams);

            if (string.IsNullOrEmpty(jsonResult))
            {
                return null;
            }

            var mediaIdRsponse = JsonHelper.ToEntity<MaterialResponse>(jsonResult);

            String md5 = string.Empty;
            if (!string.IsNullOrEmpty(mediaIdRsponse.media_id))
            { 
                requestParams = "mediaId=" + mediaIdRsponse.media_id;
                jsonResult = HttpHelper.HttpPost(UrlConstant.UploadNewsURL, requestParams);
                NewsResponse urlResponse = JsonHelper.ToEntity<NewsResponse>(jsonResult);

                if (urlResponse.errcode != null)
                {
                    MaterialResponse response = new MaterialResponse();
                    response.errcode = urlResponse.errcode;
                    response.errmsg = urlResponse.errmsg;
                    return response;
                }

                foreach(var item in urlResponse.news_item)
                {
                    md5 = MD5Helper.ComputeHash16(mediaIdRsponse.url);
                    
                    Article article = articles.Where(p => p.Title == item.title).First();
                    article.IsSync = true;
                    article.MediaId = mediaIdRsponse.media_id;
                    article.Url = item.url;
                    article.Md5 = md5;
                    article.ShortUrl = UrlConstant.GetShortUrl(md5);
                    UpdateArticle(article);
                }
            }


            return mediaIdRsponse;
        }

        public static String GetCoverMetartId(int wechatConfigId,string cover)
        {
            if (string.IsNullOrEmpty(cover))
            {
                return null;
            }

            Material material = MaterialService.Get(wechatConfigId,cover);

            if (material != null)
            {
                return material.MediaId;
            }

            string media_id = string.Empty;
            if (cover.StartsWith("http"))
            {
                var imageTab = DownloadToLocal(cover);
                MaterialResponse materialResponse = UploadMeterial(cover,imageTab.FilePath, imageTab.FileName, string.Empty);
                media_id = materialResponse?.media_id;
            }
            else
            {
                string fileName = Path.GetFileName(cover);
                MaterialResponse materialResponse = UploadMeterial(cover,cover, fileName, string.Empty);
                media_id = materialResponse?.media_id;
            }

            return media_id;
        }


        private static void UpdateArticle(Article article)
        {
            BaseService.Build<Article>().Update(article);
        }

        public static MaterialResponse UploadMeterial(string source,string filePath,string title,string introduction,string type= "image")
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("source", source);
            keyValues.Add("filePath", filePath);
            keyValues.Add("title", title);
            keyValues.Add("introduction", introduction);
            keyValues.Add("type", type);

            string requestParams = ConvertToUrlParams(keyValues);

            string result = HttpHelper.HttpPost(UrlConstant.AddMaterialFilePathURL, requestParams);
            return JsonHelper.ToEntity<MaterialResponse>(result);
        }

        private static string ConvertToUrlParams(Dictionary<string, string> keyValues)
        {
            StringBuilder requestParamsBuilder = new StringBuilder();
            foreach (var item in keyValues)
            {
                requestParamsBuilder.Append(item.Key + "=" + item.Value + "&");
            }
            string requestParams = requestParamsBuilder.ToString().TrimEnd('&');
            return requestParams;
        }

        private static MaterialFile DownloadToLocal(string imgurl)
        {
            MaterialFile imagetab = new MaterialFile();
            string extName = string.Empty;
            string fileName=Path.GetFileName(imgurl);
            string path = Path.Combine(PathHelper.GetImageSavePath(),MD5Helper.ComputeHash(imgurl))+ Path.GetExtension(imgurl); ;
            bool isDownloaded = WebHelper.DownLoadFile(imgurl, path, null, out extName);
            if (isDownloaded)
            {
                imagetab.SourceUrl = imgurl;
                imagetab.FilePath = path.EndsWith(extName)? path: path+ extName;
                imagetab.WxUrl = string.Empty;
                imagetab.ExtName = extName;
                imagetab.FileName = Path.GetFileName(imagetab.FilePath);
                return imagetab;
            }
            return null;
        }

        private static string ReplateContent(string content, MaterialTypeEnum materialType)
        {
            try
            {
                List<MaterialFile> materials =  HtmlExt.ReloadHtmlLabel(content, "img");
                List<MaterialFile> result = new List<MaterialFile>();
                if (materials.Count > 0)
                {
                    for(int i = 0;i<materials.Count;i++)
                    {
                        MaterialFile item = materials[i];
                        try
                        {
                            MaterialResponse materialResponse = UploadMeterial(item.SourceUrl,item.FilePath, item.FileName, item.FileName);
                            if (materialResponse != null && !string.IsNullOrEmpty(materialResponse.media_id))
                            {
                                item.WxUrl = materialResponse.url;
                                content = content.Replace(item.SourceUrl, item.WxUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            //LogHelper.GetLogger("")
                        }
                    }
                }

            }catch(Exception ex)
            {
                //LogHelper.GetLogger(ex.ToString());
            }
            return content;

        }

        private static string removeVideo(string content)
        {

            HtmlDocument htmlDoc = HtmlExt.LoadHtml(content);
            IList<HtmlNode> htmlMpvoice = htmlDoc.QuerySelectorAll("mpvoice");
            foreach(var item in htmlMpvoice)
            {
                string url = item.GetAttributeValue("src", "");

                if (url.StartsWith("http"))
                {
                    continue;
                }

                content = content.Replace(item.OuterHtml,"");
            }
            return content;
        }
    }
}
