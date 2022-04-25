using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business;
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
    public class ConfigController : BaseController
    {
        private BaseService<Config> ConfigService = new BaseService<Config>();

        public ActionResult List(int pg = 1, int pgSize = 8, String key = "")
        {
            int pageCount;

            List<Config> model = ConfigService.GetPageList((p => p.ConfigKey.Contains(key)), pg, pgSize,(p=>p.ConfigKey),OrderByType.Desc);
            int recordCounts = ConfigService.Count((p => p.ConfigKey.Contains(key)));
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
            Config model = new Config();
            return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Create(Config model)
        {
            bool result = ConfigService.Insert(model);
            if (result)
            {
               return RedirectToAction("list");
            }
            return View(new Config());
        }

        public ActionResult Update(string configKey)
        {
            Config model = ConfigService.GetSingle(p=>p.ConfigKey==configKey);
            return View(model);
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Update(Config model)
        {
            Config dbConfig = ConfigService.GetSingle(p => p.ConfigKey == model.ConfigKey);
            dbConfig.ConfigValue = model.ConfigValue;
            bool result = ConfigService.Update(dbConfig);
            if (result)
            {
                return RedirectToAction("list");
            }
            return View(new Config());
        }

        public ActionResult Delete(string configKey)
        {
            Config dbConfig = ConfigService.GetSingle(p => p.ConfigKey == configKey);
            ConfigService.Delete(dbConfig);
            return RedirectToAction("list");
        }
    }
}