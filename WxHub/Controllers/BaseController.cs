using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using OperateCenter.Extensions;

namespace OperateCenter.Controllers
{
    public class BaseController: Controller
    {
        private AuthUser authUser;
        protected AuthUser CurrentUser
        {
            get
            {
                if (authUser == null)
                {
                    authUser = AuthManager.GetAuthUser(this.Request.Cookies);
                }

                return authUser;
            }
        }

        private IMemoryCache cache;
        protected IMemoryCache Cache
        {
            get
            {
                if (cache == null)
                {
                    this.cache = HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
                }

                return cache;
            }
        }

        private AuthManager authManager;
        protected AuthManager AuthManager
        {
            get
            {
                if (authManager == null)
                {
                    this.authManager = HttpContext.RequestServices.GetService(typeof(AuthManager)) as AuthManager;
                }

                return authManager;
            }
        }


        [NonAction]
        #region 辅助方法
        public List<SelectListItem> GetWechatSelectList(int wechatConfigId=0)
        {
            List<WechatConfig> wechatConfigs= BaseService.Build<WechatConfig>().GetList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem() { Selected = false, Text = "├请选择", Value = "0" });
            if (wechatConfigs.Count < 1) return li;
            wechatConfigs.ForEach(x =>
            {
                li.Add(new SelectListItem() { Selected = false, Text = "├" + x.Name, Value = x.Id.ToString() });
            });
            TrySettingDefaultValue(li, wechatConfigId);
            return li;
        }

        private void TrySettingDefaultValue(List<SelectListItem> selectItems, int wechatConfigId)
        {
            for (int i = 0; i < selectItems.Count(); i++)
            {
                if (selectItems[i].Value == wechatConfigId.ToString())
                {
                    selectItems[i].Selected = true;
                }
            }
        }
        #endregion
    }
}
