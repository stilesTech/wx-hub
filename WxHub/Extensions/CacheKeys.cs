using System;
namespace OperateCenter.Extensions
{
    public class CacheKeys
    {
        /// <summary>
        /// 角色权限缓存
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public static string GetPermissionKey(int roleId)
        {
            return $"permission:{roleId}";
        }
    }
}
