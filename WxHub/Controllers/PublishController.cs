using System;
using Business;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperateCenter.Service;

namespace OperateCenter.Controllers
{
    public class PublishController: BaseController
    {
        private BaseService<Article> ArticleService = new BaseService<Article>();


        [AllowAnonymous]
        public JsonResult PushToWechat(int id)
        {
            Article article = ArticleService.GetById(id);
            return Json(OldPushService.UploadNews(article));
        }

    }
}
