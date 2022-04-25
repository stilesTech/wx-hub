using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Business;
using Common;
using Common.Html;
using Engines.Service;
using Entities;
using Entities.Enum;
using OperateCenter.Constant;
using OperateCenter.Html;
using OperateCenter.Service.Message;

namespace OperateCenter.Service
{
    public class PushService
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

            article.Content = ReplateContentImg(article.WechatConfigId,article.Content);
            article.Content = ReplateContentVoice(article.WechatConfigId, article.Content);
            article.Content = HtmlOperationExt.RemoveVideo(article.Content);
            article.Content = HtmlOperationExt.RemoveAbnormalHtml(article.Content);

            //图文消息的具体内容，支持HTML标签，必须少于2万字符，小于1M，且此处会去除JS
            keyValues.Add("content", HttpUtility.UrlEncode(article.Content));
            //图文消息的原文地址，即点击“阅读原文”后的URL
            keyValues.Add("content_source_url", "");
            //是否打开评论，0不打开，1打开
            keyValues.Add("need_open_comment", "0");
            //是否打开评论，0不打开，1打开
            keyValues.Add("only_fans_can_comment", "0");
            //微信配置ID
            keyValues.Add("wechatConfigId", article.WechatConfigId.ToString());

            string requestParams = UrlHelper.ConvertToUrlParams(keyValues);

            string jsonResult = HttpHelper.HttpPost(UrlConstant.UploadNewsURL, requestParams);

            if (string.IsNullOrEmpty(jsonResult))
            {
                return null;
            }

            var mediaIdRsponse = JsonHelper.ToEntity<MaterialResponse>(jsonResult);

            if (!string.IsNullOrEmpty(mediaIdRsponse.media_id))
            {
                requestParams = "mediaId=" + mediaIdRsponse.media_id+ "&wechatConfigId="+article.WechatConfigId;
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
                string md5 = MD5Helper.ComputeHash16(mediaIdRsponse.url+article.Id);
                mediaIdRsponse.short_url = UrlConstant.GetShortUrl(md5);

                article.IsSync = true;
                article.MediaId = mediaIdRsponse.media_id;
                article.Url = mediaIdRsponse.url;
                article.Md5 = md5;
                article.ShortUrl = mediaIdRsponse.short_url;
                BaseService.Build<Article>().Update(article);
            }

            return mediaIdRsponse;
        }


        public static bool UploadNewsList(List<Chapters> chaptersList)
        {
            List<Article> articles = GetArticles(chaptersList);

            //已经同步的文章无需同步
            if (IsSyncedArticles(articles))
            {
                return true;
            }

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //标题
            keyValues.Add("ids", String.Join(',', articles.Select(p => p.Id)));

            string media_id = GetCoverMetartId(articles[0].WechatConfigId,articles[0].Cover);
            int wechatConfigId = articles[0].WechatConfigId;
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
            //微信配置ID
            keyValues.Add("wechatConfigId", wechatConfigId.ToString());

            WechatConfig wechatConfig = BaseService.Build<WechatConfig>().GetById(wechatConfigId);

            foreach (var chapters in chaptersList)
            {
                Article article = articles.Where(p => p.SourceUrl == chapters.SourceUrl && p.WechatConfigId==wechatConfigId && chapters.Title == p.Title).First();
                if (string.IsNullOrEmpty(article.Content))
                {
                    string content= ReplateContentImg(chapters.WechatConfigId, chapters.Content);
                    content = ReplateContentVoice(chapters.WechatConfigId, content);
                    content = HtmlOperationExt.RemoveVideo(content);
                    content = HtmlOperationExt.RemoveAbnormalHtml(content);
                    article.Content = wechatConfig.TopHtml + content + wechatConfig.BottomHtml;
                    bool result = BaseService.Build<Article>().Update(article);
                    Console.WriteLine(result);
                }
            }

            string requestParams = UrlHelper.ConvertToUrlParams(keyValues);

            string jsonResult = HttpHelper.HttpPost(UrlConstant.UploadNewsListURL, requestParams);

            if (string.IsNullOrEmpty(jsonResult))
            {
                return false;
            }

            var mediaIdRsponse = JsonHelper.ToEntity<MaterialResponse>(jsonResult);

            if (!string.IsNullOrEmpty(mediaIdRsponse.media_id))
            { 
                requestParams = "mediaId=" + mediaIdRsponse.media_id + "&wechatConfigId=" + articles[0].WechatConfigId;
                jsonResult = HttpHelper.HttpPost(UrlConstant.GetNewsURL, requestParams);
                NewsResponse newsResponse = JsonHelper.ToEntity<NewsResponse>(jsonResult);

                if (newsResponse.errcode != null)
                {
                    return false;
                }

                foreach (var item in newsResponse.news_item)
                {
                    string md5 = MD5Helper.ComputeHash16(item.url);

                    Article article = articles.Where(p => p.Title == item.title).First();
                    article.IsSync = true;
                    article.MediaId = mediaIdRsponse.media_id;
                    article.Url = item.url;
                    article.Md5 = md5;
                    article.UpdatedTime = TimeHelper.GetNow();
                    article.ShortUrl = UrlConstant.GetShortUrl(md5);
                    BaseService.Build<Article>().Update(article);
                }
            }

            return true;
        }

        public static bool IsSyncedArticles(List<Article> articles)
        {
            return articles.Where(p => !p.IsSync).Count() == 0;
        }

        public static List<Article> GetArticles(List<Chapters> chaptersList)
        {
            List<Article> articles = new List<Article>();
            foreach(var chapters in chaptersList)
            {
                Article article =  GetArticle(chapters);
                articles.Add(article);
            }
            return articles;
        }

        public static Article GetArticle(Chapters chapters)
        {
            Article article=BaseService.Build<Article>().GetSingle(p => p.SourceUrl == chapters.SourceUrl
                                &&p.WechatConfigId ==chapters.WechatConfigId && p.Title==chapters.Title);
            if (article != null)
            {
                return article;
            }
            else
            {
                Article newArticle = GetNewArticle(chapters);
                return newArticle;
            }
        }

        private static Article GetNewArticle(Chapters chapters)
        {
            WechatConfig wechatConfig = BaseService.Build<WechatConfig>().GetById(chapters.WechatConfigId);
            string defaultCover = PathHelper.GetCoverPath(wechatConfig.ArticleCover);
            Article article = new Article();
            article.SourceUrl = chapters.SourceUrl;
            article.Title = chapters.Title;
            article.CreatedTime = TimeHelper.GetNow();
            article.UpdatedTime = TimeHelper.GetNow();
            article.WechatConfigId = chapters.WechatConfigId;
            article.Cover = defaultCover ?? ConfigService.getValue(ConfigConstant.DEFAULT_ARTICLE_COVER);
            int articleId = BaseService.Build<Article>().InsertReturnIdentity(article);
            article.Id = articleId;
            return article;
        }

        private static String GetCoverMetartId(int wechatConfigId,string cover)
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
                var imagePath = DownloadService.DownloadToLocal(cover);
                Material materialRequest = new Material();
                materialRequest.Source = cover;
                materialRequest.FilePath = imagePath.FilePath;
                materialRequest.Title = imagePath.FileName;
                materialRequest.WechatConfigId = wechatConfigId;
                materialRequest.Type = MaterialTypeEnum.image.ToString();

                MaterialResponse materialResponse = UploadMeterial(materialRequest);
                media_id = materialResponse?.media_id;
            }
            else
            {
                string fileName = Path.GetFileName(cover);
                Material materialRequest = new Material();
                materialRequest.Source = cover;
                materialRequest.FilePath = cover;
                materialRequest.Title = fileName;
                materialRequest.WechatConfigId = wechatConfigId;
                materialRequest.Type = MaterialTypeEnum.image.ToString();

                MaterialResponse materialResponse = UploadMeterial(materialRequest);
                media_id = materialResponse?.media_id;
            }

            return media_id;
        }

        private static string ReplateContentImg(int wechatConfigId,string content)
        {
            try
            {
                List<MaterialFile> materialFiles= HtmlExt.ReloadHtmlLabel(content, "img");
                List<MaterialFile> result = new List<MaterialFile>();
                if (materialFiles.Count > 0)
                {
                    for (int i = 0; i < materialFiles.Count; i++)
                    {
                        MaterialFile item = materialFiles[i];
                        try
                        {
                            Material materialRequest = new Material();
                            materialRequest.Source = item.SourceUrl;
                            materialRequest.FilePath = item.FilePath;
                            materialRequest.Title = item.FileName;
                            materialRequest.Type = MaterialTypeEnum.image.ToString();
                            materialRequest.WechatConfigId = wechatConfigId;
                            MaterialResponse materialResponse;
                            materialResponse = NewsUploadImg(materialRequest);
                             
                            if (materialResponse != null && !string.IsNullOrEmpty(materialResponse.url))
                            {
                                item.WxUrl = materialResponse.url;
                                content = content.Replace(item.SourceUrl, item.WxUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.GetLogger<PushService>().Error(ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //LogHelper.GetLogger(ex.ToString());
            }
            return content;
        }

        private static string ReplateContentVoice(int wechatConfigId, string content)
        {
            try
            {
                List<MaterialFile> materialFiles = HtmlExt.ReloadHtmlLabel(content, "mpvoice");
                List<MaterialFile> result = new List<MaterialFile>();
                if (materialFiles.Count > 0)
                {
                    for (int i = 0; i < materialFiles.Count; i++)
                    {
                        MaterialFile item = materialFiles[i];
                        try
                        {
                            Material materialRequest = new Material();
                            materialRequest.Source = item.SourceUrl;
                            materialRequest.FilePath = item.FilePath;
                            materialRequest.Title = item.FileName;
                            materialRequest.Type = MaterialTypeEnum.video.ToString();
                            materialRequest.WechatConfigId = wechatConfigId;
                            MaterialResponse materialResponse;
                            materialRequest.Introduction = item.FileName;
                            materialResponse = UploadMeterial(materialRequest);
                            if (materialResponse != null && !string.IsNullOrEmpty(materialResponse.media_id))
                            {
                                string oldMediaId = item.SourceUrl.Replace("https://res.wx.qq.com/voice/getvoice?mediaid=", "");
                                content = content.Replace(oldMediaId, materialResponse.media_id);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.GetLogger<PushService>().Error(ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //LogHelper.GetLogger(ex.ToString());
            }
            return content;
        }




        /// <summary>
        /// 上传素材
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filePath"></param>
        /// <param name="title"></param>
        /// <param name="introduction"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MaterialResponse UploadMeterial(Material material)
        {
            int fileStorageId = SaveFileStorage(material);
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("source", material.Source);
            keyValues.Add("filePath", material.FilePath);
            keyValues.Add("title", material.Title);
            keyValues.Add("introduction", material.Introduction);
            keyValues.Add("type", material.Type);
            keyValues.Add("wechatConfigId", material.WechatConfigId.ToString());
            keyValues.Add("fileStorageId", fileStorageId.ToString());

            string requestParams = UrlHelper.ConvertToUrlParams(keyValues);

            string result = HttpHelper.HttpPost(UrlConstant.AddMaterialFilePathURL, requestParams);
            return JsonHelper.ToEntity<MaterialResponse>(result);
        }

        /// <summary>
        /// 图文上传图片
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filePath"></param>
        /// <param name="title"></param>
        /// <param name="introduction"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MaterialResponse NewsUploadImg(Material material)
        {
            int fileStorageId = SaveFileStorage(material);
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            //keyValues.Add("source", material.Source);
            keyValues.Add("filePath", material.FilePath);
            keyValues.Add("title", material.Title);
            keyValues.Add("introduction", material.Introduction);
            keyValues.Add("type", material.Type);
            keyValues.Add("wechatConfigId", material.WechatConfigId.ToString());
            keyValues.Add("fileStorageId", fileStorageId.ToString());

            string requestParams = UrlHelper.ConvertToUrlParams(keyValues);

            string result = HttpHelper.HttpPost(UrlConstant.NewsUploadImgURL, requestParams);
            return JsonHelper.ToEntity<MaterialResponse>(result);
        }

        private static int SaveFileStorage(Material material)
        {
            var dbFileStorage= BaseService.Build<FileStorage>().GetSingle(p => p.SourceUrl == material.Source && p.WechatConfigId == material.WechatConfigId);

            FileStorage fileStorage = new FileStorage();
            fileStorage.CreateTime = TimeHelper.GetNow();
            fileStorage.Name = material.Title;
            fileStorage.SourceUrl = material.Source;
            fileStorage.Type = material.Type;
            fileStorage.WechatConfigId = material.WechatConfigId;
            fileStorage.FileStreams = FileHelper.ReaderFileStream(material.FilePath);

            if (dbFileStorage != null)
            {
                fileStorage.Id = dbFileStorage.Id;
                BaseService.Build<FileStorage>().Update(fileStorage);
                return fileStorage.Id;
            }
            else
            {
                return BaseService.Build<FileStorage>().InsertReturnIdentity(fileStorage);
            }
        }

    }
}
