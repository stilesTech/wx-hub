using Common;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class ConfigService
    {
        public static string getValue(string configKey)
        {
            string configValue = CacheHelper.Get<string>(configKey);

            if (!string.IsNullOrEmpty(configValue))
            {
                return configValue;
            }


            Config config = BaseService.Build<Config>().GetSingle(p => p.ConfigKey == configKey);

            if (config != null)
            {
                CacheHelper.Set<string>(configKey, config.ConfigValue, 10 * 60 * 60);
                return config.ConfigValue;
            }

            return string.Empty;
        }
    }
}
