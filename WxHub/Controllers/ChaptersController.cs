using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Business;
using Common;
using Entities;
using Entities.Query;
using Entities.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OperateCenter.Constant;
using OperateCenter.Extensions.Filters;
using OperateCenter.Html;
using OperateCenter.Service;
using OperateCenter.Service.Message;
using SqlSugar;

namespace OperateCenter.Controllers
{
    [Permission("admin")]
    public class ChaptersController : BaseController
    {
        private BaseService<Chapters> ChaptersService = new BaseService<Chapters>();
        private BaseService<WechatConfig> WechatConfigService = new BaseService<WechatConfig>();

        public ActionResult List(int pg = 1, int pgSize = 8, String key = "",int wechatConfigId=0)
        {
            List<Chapters> model = ChaptersService.GetPageList((p => p.Title.Contains(key??"") && p.WechatConfigId == (wechatConfigId == 0 ? p.WechatConfigId : wechatConfigId)), pg, pgSize,(p=>p.Id),OrderByType.Desc);
            int recordCounts = ChaptersService.Count((p => p.Title.Contains(key??"") && p.WechatConfigId == (wechatConfigId == 0 ? p.WechatConfigId : wechatConfigId)));
            int pageCount = (int)Math.Ceiling(recordCounts / (pgSize * 1.0));
            StringBuilder searchKey = new StringBuilder();
            if (key != "")
                searchKey.Append(string.Format("&key={0}", key));
            searchKey.Append(string.Format("&wechatConfigId={0}", wechatConfigId.ToString()));
            PagerQuery pagerQuery = new PagerQuery(pg, pgSize, searchKey.ToString(), pageCount, recordCounts);
            ViewBag.pagerModel = pagerQuery;
            ViewBag.Cats = GetWechatSelectList(wechatConfigId);
            ViewBag.Key = key;
            ReloadWechatName(model);
            return View(model);
        }

        public List<Chapters> ReloadWechatName(List<Chapters> chapters)
        {
            List<WechatConfig> list = WechatConfigService.GetList();
            foreach (var item in chapters)
            {
                item.WechatName = list.Where(p => p.Id == item.WechatConfigId).FirstOrDefault()?.Name;
            }
            return chapters;
        }

        public ActionResult Collect()
        {
            Chapters model = new Chapters();
            model.Tag = MD5Helper.ComputeHash16(TimeHelper.GetNow().ToString("yyyyMMddHHmmssfff"));
            ViewBag.Cats = GetWechatSelectList(0);
            return View(model);
        }

        [HttpPost]
        public ActionResult Collect(string data)
        {
            ChaptersCollectQuery query = JsonHelper.ToEntity<ChaptersCollectQuery>(data);

            if(query==null || string.IsNullOrEmpty(query.title))
            {
                return Json("错误的参数");
            }

            List<Chapters> chaptersList= ConvertToChaptersList(query);

            SaveChaptersToMysql(chaptersList,query.tag);
            var waitSyncChapters = chaptersList.Where(p => !p.IsTitle).ToList();
            UploadChaptersToWechat(waitSyncChapters);

            InitMenuPage(query.title,chaptersList,query.wechatConfigId);

            return Json("成功");
        }


        private bool UploadChaptersToWechat(List<Chapters> collectList)
        {
            int number = collectList.Count;
            int chunkSize = 8;
            var chunkList = collectList.ChunkBy(chunkSize);
            foreach (var chunk in chunkList)
            {
                SyncListToWechat(chunk.ToList());
            }
            return true;
        }

        public bool SyncListToWechat(List<Chapters> chapters)
        {
            try
            {
                return PushService.UploadNewsList(chapters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LogHelper.GetLogger<ChaptersController>().Error(ex);
            }
            return false;
        }

        public void InitMenuPage(string title,List<Chapters> chapters,int wechatConfigId)
        {
            Article article = BaseService.Build<Article>().GetSingle(p => p.Title == title && string.IsNullOrEmpty(p.SourceUrl) && p.WechatConfigId== wechatConfigId);
            string defaultCover = WechatConfigService.GetById(chapters[0].WechatConfigId)?.ArticleMenuCover;
            defaultCover = PathHelper.GetCoverPath(defaultCover);
            if (article == null)
            {
                article = new Article();
                article.Title = title;
                article.Cover = defaultCover?? ConfigService.getValue(ConfigConstant.DEFAULT_ARTICLE_MENU_COVER);
                article.CreatedTime = TimeHelper.GetNow();
                article.UpdatedTime = TimeHelper.GetNow();
                article.WechatConfigId = wechatConfigId;
                int articleId = BaseService.Build<Article>().InsertReturnIdentity(article);
                article.Id = articleId;
            }

            WechatConfig wechatConfig = WechatConfigService.GetById(wechatConfigId);
            string content = HtmlPageHelper.GetRenderContent(chapters, wechatConfigId);
            article.Content = wechatConfig.TopHtml + content + wechatConfig.BottomHtml;
            SyncToWechat(article);
        }

        private MaterialResponse SyncToWechat(Article article)
        {
            return PushService.UploadNews(article);
        }

        private List<Chapters> ConvertToChaptersList(ChaptersCollectQuery query,int orderNumber = 1)
        {
            List<Chapters> chaptersList = new List<Chapters>();
            foreach(var item in query.collecs)
            {
                Chapters chapters = new Chapters();
                chapters.IsTitle = item.isTitle;
                chapters.Level = item.level;
                chapters.SourceUrl = item.url;
                chapters.Tag = query.tag;
                chapters.Title = item.title;
                chapters.OrderNumber = ++orderNumber;
                chapters.ParentId = 0;
                chapters.WechatConfigId = query.wechatConfigId;
                chaptersList.Add(chapters);
            }

            return chaptersList;
        }


        private bool SaveChaptersToMysql(List<Chapters> chaptersList,string tag)
        {

            foreach (var chapters in chaptersList)
            {
                if (!chapters.IsTitle)
                {
                    CollectService.TryCollectChaptersItem(chapters, tag);
                }
                else
                {
                    int chaptersid = BaseService.Build<Chapters>().InsertReturnIdentity(chapters);
                    chapters.Id = chaptersid;
                }
            }
            return true;
        }


        public ActionResult Delete(int id)
        {
            ChaptersService.DeleteById(id);
            return RedirectToAction("list");
        }
    }
}