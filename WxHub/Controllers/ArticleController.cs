using System;
using System.Collections.Generic;
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
using OperateCenter.Extensions.Filters;
using OperateCenter.Service;
using SqlSugar;

namespace OperateCenter.Controllers
{
    [Permission("admin")]
    public class ArticleController : BaseController
    {
        private BaseService<Article> ArticleService = BaseService.Build<Article>();
        private BaseService<Category> CategoryService = new BaseService<Category>();
        private BaseService<WechatConfig> WechatConfigService = new BaseService<WechatConfig>();

        public ActionResult List(int pg = 1, int pgSize = 8, string key="",int wechatConfigId=0)
        {
            List<Article> model = ArticleService.GetPageList((p => p.Title.Contains(key??"") && p.WechatConfigId == (wechatConfigId==0?p.WechatConfigId: wechatConfigId)), pg, pgSize,(p=>p.UpdatedTime ),OrderByType.Desc);
            int recordCounts = ArticleService.Count((p => p.Title.Contains(key??"") && p.WechatConfigId == (wechatConfigId == 0 ? p.WechatConfigId : wechatConfigId)));
            int pageCount = (int)Math.Ceiling(recordCounts / (pgSize * 1.0));
            StringBuilder searchKey = new StringBuilder();
            if (key != "")
                searchKey.Append(string.Format("&key={0}", key));
            searchKey.Append(string.Format("&wechatConfigId={0}", wechatConfigId));
            PagerQuery pagerQuery = new PagerQuery(pg, pgSize, searchKey.ToString(), pageCount, recordCounts);
            ViewBag.pagerModel = pagerQuery;
            ViewBag.Key = key;
            ViewBag.Cats = GetWechatSelectList(wechatConfigId);
            ReloadWechatName(model);
            return View(model);
        }

        public List<Article> ReloadWechatName(List<Article> articles)
        {
            List<WechatConfig> list=  WechatConfigService.GetList();
            foreach(var item in articles)
            {
                item.WechatName = list.Where(p => p.Id == item.WechatConfigId).FirstOrDefault()?.Name;
            }
            return articles;
        }

        public ActionResult Create()
        {
            ViewBag.CategoriesList = GetCategorySelectList();
            Article model = new Article();
            model.CreatedTime = TimeHelper.GetNow();
            return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Create(Article model)
        {
            model.Cover = this.Request.Form["picturePath"];
            model.UpdatedTime = TimeHelper.GetNow();
            bool result = ArticleService.Insert(model);
            if (result)
            {
               return RedirectToAction("list");
            }
            ViewBag.CategoriesList = GetCategorySelectList();
            return View(new Article());
        }

        public ActionResult Category()
        {
            var categories = CategoryService.GetList();

            return View(categories);
        }

        public ActionResult Update(int id)
        {
            ViewBag.CategoriesList = GetCategorySelectList();
            Article model = ArticleService.GetById(id);
            return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Update(Article model)
        {
            var dbArticle = ArticleService.GetById(model.Id);

            dbArticle.Content = model.Content;
            dbArticle.IsSync = false;
            bool result = ArticleService.Update(dbArticle);
            if (result)
            {
                return RedirectToAction("list");
            }
            ViewBag.CategoriesList = GetCategorySelectList();
            return View(new Article());
        }

        public ActionResult Delete(int id)
        {
            ArticleService.DeleteById(id);
            return RedirectToAction("list");
        }

        [HttpPost]
        public JsonResult PushToWechat(int id)
        {
            Article article = ArticleService.GetById(id);
            return Json(PushService.UploadNews(article));
        }


        [NonAction]
        #region 辅助方法
        public IEnumerable<SelectListItem> GetCategorySelectList()
        {
            var categories = CategoryService.GetList()?.ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Insert(0, new SelectListItem() { Text = "根目录", Selected = false, Value = Guid.Empty.ToString() });
            if (categories != null && categories.Count < 1) return li;
            categories.Where(p => p.ParentId == 0).ToList().ForEach(x =>
            {
                li.Add(new SelectListItem() { Selected = false, Text = "├" + x.Name, Value = x.Id.ToString() });
                foreach (var z in categories.Where(y => y.ParentId == x.Id))
                {
                    li.Add(new SelectListItem() { Selected = false, Text = "├—" + z.Name, Value = z.Id.ToString() });
                }
            });
            return li;
        }
        #endregion
    }
}