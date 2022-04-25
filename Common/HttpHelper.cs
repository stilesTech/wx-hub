using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class HttpHelper
    {
        /// <summary>
        /// post同步請求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">數據</param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">請求頭</param> 
        /// <returns></returns>
        public static string HttpPost(string url, string postData = "", string contentType = null, Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
             
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }


                using HttpContent httpContent = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
                if (contentType != null)
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// post異步請求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">數據</param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="timeOut">請求超時時間</param> 
        /// <param name="headers">請求頭</param> 
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, string postData = "", string contentType = null, int timeOut = 30, Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, timeOut); 
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                using HttpContent httpContent = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
                if (contentType != null)
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                HttpResponseMessage response = await client.PostAsync(url, httpContent);
                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// get同步請求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="headers">請求頭</param>
        /// <returns></returns>
        public static string HttpGet(string url, Dictionary<string, string> headers = null)
        {
            using HttpClient client = new HttpClient();

            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            HttpResponseMessage response = client.GetAsync(url).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// get異步請求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, Dictionary<string, string> headers = null)
        {
            using HttpClient client = new HttpClient();

            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

    }
}
