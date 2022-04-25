using System;
using System.Text.Json;

namespace Common
{
    public class JsonHelper
    {
        public static string ToJson(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T ToEntity<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
