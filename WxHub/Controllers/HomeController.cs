using System;
using Business;
using Entities;
using Microsoft.AspNetCore.Mvc;
using OperateCenter.Extensions.Filters;

namespace OperateCenter.Controllers
{
    [Permission("admin")]
    public class HomeController: BaseController
    {
        private BaseService<Article> ArticleService = new BaseService<Article>();


        // GET: Admin/Index
        public ActionResult Index()
        {
            ViewBag.articlesNum = BaseService.Build<Article>().Count(p=> p.IsSync);
            ViewBag.userNum = BaseService.Build<SysUser>().Count(p => p.Status == 1);
            ViewBag.chaptersNum = BaseService.Build<Chapters>().Count(p => true );
            return View();
        }
    }
}
