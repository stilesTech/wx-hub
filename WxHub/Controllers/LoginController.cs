using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using Common;
using Entities;
using Entities.Querys;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OperateCenter.Extensions;

namespace OperateCenter.Controllers
{
    public class LoginController: BaseController
    {
        private static readonly ILog log = LogHelper.GetLogger<LoginController>();

        #region Service
        private BaseService<SysUser> sysUserService = new BaseService<SysUser>();
        #endregion
        #region Private Variables
        private readonly IOptions<AuthOptions> authOptions;
        #endregion Private Variables
        public LoginController(IOptions<AuthOptions> authOptions)
        {
            this.authOptions = authOptions;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitLogin(LoginModel model)
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

                return Json(ResponseResultFactory.ErrorParams);
            }

            model.Password = MD5Helper.ComputeHash(model.Password);
            SysUser sysUser = sysUserService.GetSingle(p=>p.Account==model.UserName&&p.Password== model.Password);
            if (sysUser == null)
            {
                return Json(ResponseResultFactory.Create<Object>(ErrorCode.用户不存在, null));
            }

            if (sysUser.IsLocked)
            {
                return Json(ResponseResultFactory.Create<Object>(ErrorCode.用户被锁定, null));
            }
            AuthUser authUser = new AuthUser()
            {
                UserName = sysUser.UserName,
                UserId = sysUser.Id,
                RoleId = sysUser.RoleId
            };

            // 写入cookie
            AuthManager.SetAuthUser(this.Response.Cookies, authUser);

            // 写入权限, 主帐号roleId为-1
            string cacheKey = CacheKeys.GetPermissionKey(sysUser.RoleId);
            IList<string> permissionList;
            if (!Cache.TryGetValue(cacheKey, out permissionList))
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

            var returnInfo = new { userName = sysUser.UserName };

            return Json(ResponseResultFactory.Create<Object>(ErrorCode.请求成功, returnInfo));
        }

    }
}
