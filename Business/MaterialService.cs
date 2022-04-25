using System;
using Common;
using Entities;

namespace Business
{
    public class MaterialService
    {
        public static Material Get(int wechatConfigId,string source)
        {
            string key = $"{source}_{wechatConfigId.ToString()}";

            Material material = CacheHelper.Get<Material>(key);

            if (material != null)
            {
                return material;
            }

            material = BaseService.Build<Material>().GetSingle(p => p.Source == source&& p.WechatConfigId == wechatConfigId);

            if (material != null)
            {
                CacheHelper.Set<Material>(key, material, 60 * 60);
            }
            return material;
        }
    }
}
