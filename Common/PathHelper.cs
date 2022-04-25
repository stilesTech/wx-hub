using System;
using System.IO;

namespace Common
{
    public class PathHelper
    {
        public static string GetImageSavePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "upload", "image", DateTime.Now.ToString("yyyyMMdd"));
        }

        public static string GetBasePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }


        public static string GetAppcationPath()
        {
            return Directory.GetCurrentDirectory();
        }

        public static string GetCoverPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            string basePath = GetBasePath();
            if (path.Contains(basePath))
            {
                return path;
            }
            
            Console.WriteLine("basePath1:" + Path.Combine(basePath,path));
            Console.WriteLine("basePath2:" + basePath + path);
            return basePath + path;
        }
    }
}
