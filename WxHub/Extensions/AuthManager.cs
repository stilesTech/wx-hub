using System;
using Common;
using Entities;
using Microsoft.AspNetCore.Http;

namespace OperateCenter.Extensions
{
    public class AuthManager
    {
        private readonly AuthOptions authOptions;
        public AuthManager(AuthOptions authOptions)
        {
            this.authOptions = authOptions;
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public AuthUser GetAuthUser(IRequestCookieCollection cookies)
        {
            string cookieValue = string.Empty;
            bool getResult = cookies.TryGetValue(authOptions.CookieName, out cookieValue);
            if (getResult)
            {
                string json = DESCryptoHelper.DecryptDes(cookieValue);
                return JsonHelper.ToEntity<AuthUser>(json);
            }
            return null;
        }

        /// <summary>
        /// 设置验证用户cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="authUser"></param>
        public void SetAuthUser(IResponseCookies cookies, AuthUser authUser)
        {
            string cryptStr = DESCryptoHelper.EncryptDes(JsonHelper.ToJson(authUser));

            cookies.Append(authOptions.CookieName, cryptStr, new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddMinutes(authOptions.ExpireDays * 1440)
            });

            cookies.Append("username", authUser.UserName, new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddMinutes(authOptions.ExpireDays * 1440)
            });
        }

        public void Logout(IResponseCookies cookies)
        {
            cookies.Delete(authOptions.CookieName);
        }
    }
}
