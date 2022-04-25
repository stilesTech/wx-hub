using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class UrlHelper
    {
        public static string ConvertToUrlParams(Dictionary<string, string> keyValues)
        {
            StringBuilder requestParamsBuilder = new StringBuilder();
            foreach (var item in keyValues)
            {
                requestParamsBuilder.Append(item.Key + "=" + item.Value + "&");
            }
            string requestParams = requestParamsBuilder.ToString().TrimEnd('&');
            return requestParams;
        }
    }
}
