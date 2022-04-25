using Common.IP;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Html
{
    public static class WebHelper
    {
        public static List<IpSourceInfo> IpInfoGroup = new List<IpSourceInfo>();


        public static bool DownLoadFile(string url, string path, string[] allowExtGroup, out string extName)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "User-Agent: Fiddler/4.6.2.0 (.NET 4.0.30319.42000; WinNT 6.1.7601 SP1; zh-CN; 4xAMD64)";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                #region extName
                extName = Path.GetExtension(url);

                if (!string.IsNullOrEmpty(extName))
                {
                    extName = Regex.Match(Path.GetFileName(url), @"\.\w*").Value;
                }

                if (string.IsNullOrEmpty(extName))
                {
                    var contentTypeGroup = response.ContentType.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                    extName = contentTypeGroup.Length == 2 ? contentTypeGroup[1] : string.Empty;
                    if (extName == "png" || extName == "x-png")
                    {
                        extName = ".png";
                    }
                    else if (extName == "jpeg" || extName == "pjpeg" || extName == "x-jpg")
                    {
                        extName = ".jpg";
                    }
                    else if (extName == "gif")
                    {
                        extName = ".gif";
                    }
                    else if (extName == "bmp" || extName == "x-bmp")
                    {
                        extName = ".bmp";
                    }
                    else if( extName == "mp3")
                    {
                        extName = ".mp3";
                    }
                    if (!path.Contains(extName))
                    {
                        path = path + extName;
                    }
                }
                #endregion

                if (allowExtGroup != null && !allowExtGroup.Contains(extName.ToLower()))
                {
                    throw new FileLoadException(string.Format("{0}不是允许的扩展名！({1})", extName, url));
                }

                #region DownLoadFile
                using (var webStream = response.GetResponseStream())
                {
                    string dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (var fileStream = File.Create(path))
                    {
                        var byteGroup = new byte[102400];
                        var readCount = 0;
                        do
                        {
                            readCount = webStream.Read(byteGroup, 0, byteGroup.Length);
                            fileStream.Write(byteGroup, 0, readCount);
                        } while (readCount > 0);
                    }
                }
                return true;
                #endregion
            }
            catch (Exception exp)
            {
                LogHelper.GetLogger(typeof(WebHelper)).Error($"文件下载,{url + Environment.NewLine + path}");
                LogHelper.GetLogger(typeof(WebHelper)).Error(exp);
                //throw exp;
                extName = string.Empty;
                return false;
            }
        }

        public static string GetStr(string url, bool useProxy, bool isThrowExp = false)
        {
            string realUrl;
            try
            {
                return GetStr(url, out realUrl, useProxy);
            }
            catch (WebException ex)
            {
                if (!isThrowExp && ex.Status == WebExceptionStatus.Timeout)
                {
                    try
                    {
                        LogHelper.GetLogger(typeof(WebHelper)).Error($"请求超时,二次重试");
                        GetStr(url, out realUrl, useProxy);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (isThrowExp)
                {
                    throw ex;
                }
            }
            return string.Empty;
        }

        public static string GetStr(string url, out string realUrl, bool useProxy, int tryCount = 0)
        {
            Exception exp = null;
            DateTime startTime = DateTime.Now;
            int size = 0;
            IpSourceInfo ipSourceInfo = null;
            Guid guid = Guid.NewGuid();

            #region CanUserProxy
            Func<bool> canUseProxy = () => useProxy && IpInfoGroup.Count > 0;
            #endregion

            #region recordProxy
            Action recordProxy = () =>
            {
                if (canUseProxy())
                {
                    //LogHelper.Log("代理下载", string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    //    exp == null ? "sucess" : "fail",
                    //    (size / 1024.0).ToString("0.00"),
                    //    (DateTime.Now - startTime).TotalMilliseconds,
                    //    url,
                    //    exp != null ? exp.Message : string.Empty,
                    //    JsonSerializer.Serialize(ipSourceInfo)),
                    //    "代理下载.txt");
                }
            };
            #endregion
            //解决The SSL connection could not be established, see inner exception. Authentica
            #region 初始化Request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.DefaultConnectionLimit = 500;
            request.Timeout = 5000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
            if (canUseProxy())
            {
                ipSourceInfo = IpInfoGroup.Random();
                var proxy = new WebProxy(ipSourceInfo.Ip + ":" + ipSourceInfo.Port);
                request.Proxy = proxy;
            }
            #endregion

            try
            {
                //out can't use in lambda
                string innerRealUrl = null;
                string result = string.Empty;

                #region getData
                Action getData = () =>
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    innerRealUrl = request.Address.AbsoluteUri;

                    #region gzip
                    if ((response.ContentEncoding ?? "").ToLower().Contains("gzip"))
                    {
                        using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                    #endregion
                    #region deflate
                    else if ((response.ContentEncoding ?? "").ToLower().Contains("deflate"))
                    {
                        using (DeflateStream stream = new DeflateStream(
                            response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader =
                                new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                    #endregion
                    #region no compress
                    else
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                    #endregion

                    size = result.Length;
                };
                #endregion

                if (canUseProxy())
                {
                    TaskExt.RunWait(() =>
                    {
                        getData();
                    }, 30000, true);
                }
                else
                {
                    getData();
                }

                realUrl = innerRealUrl;
                recordProxy();

                return result;
            }
            catch (Exception e)
            {
                exp = e;
                tryCount++;

                recordProxy();

                if (canUseProxy() && tryCount < 10)
                {
                    return GetStr(url, out realUrl, useProxy, tryCount);
                }
                else
                {
                    throw e;
                }
            }
        }

    }
}
