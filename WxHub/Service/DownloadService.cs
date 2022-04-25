using System;
using System.IO;
using Common;
using Common.Html;

namespace Engines.Service
{
    public class DownloadService
    {
        public static MaterialFile DownloadToLocal(string imgurl)
        {
            MaterialFile imagePath = new MaterialFile();
            string extName = string.Empty;
            string fileName = Path.GetFileName(imgurl);
            string path = Path.Combine(PathHelper.GetImageSavePath(), MD5Helper.ComputeHash(imgurl)) + Path.GetExtension(imgurl); ;
            bool isDownloaded = WebHelper.DownLoadFile(imgurl, path, null, out extName);
            if (isDownloaded)
            {
                imagePath.SourceUrl = imgurl;
                imagePath.FilePath = path.EndsWith(extName) ? path : path + extName;
                imagePath.WxUrl = string.Empty;
                imagePath.ExtName = extName;
                imagePath.FileName = Path.GetFileName(imagePath.FilePath);
                return imagePath;
            }
            return null;
        }
    }
}
