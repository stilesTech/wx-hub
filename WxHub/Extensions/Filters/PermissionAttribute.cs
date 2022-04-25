using System;
using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace OperateCenter.Extensions.Filters
{
    public class PermissionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; private set; }

        public PermissionAttribute(string name)
        {
            this.Name = name;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AuthManager authManager = context.HttpContext.RequestServices.GetService(typeof(AuthManager)) as AuthManager;
            IMemoryCache cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;

            var authorizedUser = authManager.GetAuthUser(context.HttpContext.Request.Cookies);

            //获取不到cookies,跳转到登录页面
            if (authorizedUser == null)
            {
                context.Result = new RedirectResult("/Account/Login");
                return;
            }
            else
            {
                return;
            }
           
            //string cacheKey = CacheKeys.GetPermissionKey(authorizedUser.RoleId);

            //IList<string> permissionList = cache.Get<IList<string>>(cacheKey);
            //// 权限为空，需要重新登录，可能是缓存失效
            //if (permissionList == null || permissionList.Count == 0)
            //{
            //    //context.Result = new JsonResult(ResponseResultFactory.Create(ErrorCode.用户没有权限));
            //    context.Result = new RedirectResult("/Account/Login");
            //    return;
            //}

            //// *星号表示最高权限
            //bool enabled = permissionList.Contains("*") || permissionList.Contains(this.Name);
            //if (enabled)
            //{
            //    return;
            //}

            //context.Result = new JsonResult(ResponseResultFactory.Create(ErrorCode.用户没有权限));
        }


        /// <summary>
        /// 角色权限缓存
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        private static string GetPermissionKey(int roleId)
        {
            return $"permission:{roleId}";
        }
    }
}
