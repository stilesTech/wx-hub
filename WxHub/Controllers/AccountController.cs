using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using Common;
using Entities;
using Entities.Querys;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OperateCenter.Extensions;

namespace OperateCenter.Controllers
{
    public class AccountController: BaseController
    {
        private static readonly ILog log = LogHelper.GetLogger<AccountController>();
        private BaseService<SysUser> sysUserService = new BaseService<SysUser>();


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
           
            if (!ModelState.IsValid)
            {
                log.Info("登录数据验证失败");
                foreach (var errorList in ModelState.Values.Where(x => x.Errors.Any()).Select(x => x.Errors))
                {
                    foreach (var error in errorList)
                    {
                        log.Info($"验证失败, {error.ErrorMessage}");
                    }
                }
                ModelState.AddModelError("", "用户名或密码不正确");
                return View(model);
            }


            SysUser user = sysUserService.GetSingle(p=>p.Account==model.UserName&&p.Password==MD5Helper.ComputeHash(model.Password));
            if (user==null)
            {
                ModelState.AddModelError("", "用户名或密码不正确");
                return View(model);
            }
            AuthUser authUser = new AuthUser()
            {
                UserName = user.UserName,
                UserId = user.Id,
                RoleId = user.RoleId
            };

            // 写入cookie
            AuthManager.SetAuthUser(this.Response.Cookies, authUser);

            // 写入权限, 主帐号roleId为-1
            string cacheKey = CacheKeys.GetPermissionKey(user.RoleId);
            IList<string> permissionList;
            if (!this.Cache.TryGetValue(cacheKey, out permissionList))
            {
                permissionList = new List<string>();
                //result.Data.RoleId==1
                if (true)
                {
                    // 所有权限
                    permissionList.Add("*");
                }
                Cache.Set(cacheKey, permissionList);
            }

            return RedirectToLocal(returnUrl);

        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Logout()
        {
            AuthManager.Logout(this.Response.Cookies);
            return RedirectToAction("Login", "Account");
        }
    }
}
