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
    public class WechatConfigController : BaseController
    {
        private BaseService<WechatConfig> wechatConfigService = new BaseService<WechatConfig>();

        public ActionResult List(int pg = 1, int pgSize = 8, String key = "")
        {
            int pageCount;

            List<WechatConfig> model = wechatConfigService.GetPageList((p => p.Name.Contains(key)), pg, pgSize,(p=>p.Id),OrderByType.Desc);
            int recordCounts = wechatConfigService.Count((p => p.Name.Contains(key)));
            pageCount = (int)Math.Ceiling(recordCounts / (pgSize * 1.0));
            StringBuilder searchKey = new StringBuilder();
            if (key != "")
                searchKey.Append(string.Format("&key={0}", key));
            PagerQuery pagerQuery = new PagerQuery(pg, pgSize, searchKey.ToString(), pageCount, recordCounts);
            ViewBag.pagerModel = pagerQuery;
            return View(model);
        }

        public ActionResult Create()
        {
            WechatConfig model = new WechatConfig();
            return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Create(WechatConfig model)
        {
            bool result = wechatConfigService.Insert(model);
            if (result)
            {
               return RedirectToAction("list");
            }
            return View(new WechatConfig());
        }

        public ActionResult Update(int id)
        {
            WechatConfig model = wechatConfigService.GetById(id);
            return View(model);
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Update(WechatConfig model)
        {
            WechatConfig dbWechatConfig = wechatConfigService.GetById(model.Id);
            model.CreateTime = dbWechatConfig.CreateTime;
            model.Id = dbWechatConfig.Id;
            model.UpdateTime = TimeHelper.GetNow();
            bool result = wechatConfigService.Update(model);
            if (result)
            {
                return RedirectToAction("list");
            }
            return View(new Config());
        }

        public ActionResult Delete(int id)
        {
            wechatConfigService.DeleteById(id);
            return RedirectToAction("list");
        }
    }
}