using System;
using System.Collections.Generic;
using System.Text;
using Business;
using Common;
using Entities;
using Entities.Ext;
using Entities.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OperateCenter.Extensions.Filters;
using SqlSugar;

namespace OperateCenter.Controllers
{
    [Permission("admin")]
    public class SysUserController: BaseController
    {
        private BaseService<SysUser> sysUserService = new BaseService<SysUser>();

        public ActionResult List(int pg = 1, int pgSize = 8, String key = "")
        {
            int pageCount;

            List<SysUser> model = sysUserService.GetPageList((p => p.UserName.Contains(key)), pg, pgSize, (p => p.CreatedTime), OrderByType.Desc);
            int recordCounts = sysUserService.Count((p => p.UserName.Contains(key)));
            pageCount = (int)Math.Ceiling(recordCounts / (pgSize * 1.0));
            StringBuilder searchKey = new StringBuilder();
            if (key != "")
                searchKey.Append(string.Format("&key={0}", key));
            PagerQuery pagerQuery = new PagerQuery(pg, pgSize, searchKey.ToString(), pageCount, recordCounts);
            ViewBag.pagerModel = pagerQuery;
            return View(model);
        }


        public ActionResult Update(int id)
        {
            SysUser model = sysUserService.GetById(id);
            return View(model);
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Update(SysUser model)
        {
            var dbSysUser = sysUserService.GetById(model.Id);
            dbSysUser.UpdatedTime = TimeHelper.GetNow();
            dbSysUser.Account = model.Account;
            dbSysUser.Password = MD5Helper.ComputeHash(model.Password);
            bool result = sysUserService.Update(dbSysUser);
            if (result)
            {
                return RedirectToAction("list");
            }
            return View(new Article());
        }


        public ActionResult Create()
        {
            return View(new SysUser());
        }

        [HttpPost]
        public ActionResult Create(SysUser model)
        {
            model.UpdatedTime = TimeHelper.GetNow();
            model.Status = 1;
            model.RoleId = 1;
            model.IsLocked = false;
            model.Password = MD5Helper.ComputeHash(model.Password);
            bool result = sysUserService.Insert(model);
            if (result)
            {
                return RedirectToAction("list");
            }
            return View(new SysUser());
        }

        public ActionResult Delete(int id)
        {
            sysUserService.DeleteById(id);
            return RedirectToAction("list");
        }
    }
}
