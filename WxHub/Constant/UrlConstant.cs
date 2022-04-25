using Business;
using System;

namespace OperateCenter.Constant
{
    public class UrlConstant
    {
        public static string AddMaterialFilePathURL
        {
            get
            {
                return GetServiceUrl() + "/material/addMaterialFilePath";
            }
        }

        public static string NewsUploadImgURL
        {
            get
            {
                return GetServiceUrl() + "/news/uploadImg";
            }
        }

        public static string UploadNewsURL
        {
            get
            {
                return GetServiceUrl() + "/news/uploadNews";
            }
        }

        public static string UploadNewsListURL
        {
            get
            {
                return GetServiceUrl() + "/news/uploadNewsList";
            }
        }

        public static string GetNewsURL
        {
            get
            {
                return GetServiceUrl() + "/news/getNews";
            }
        }

        public static string GetShortUrl(string md5)
        {
            return $"{GetDomainUrl()}/r/to?p={md5}";
        }

        private static string service_url;

        private static string GetServiceUrl()
        {
            if (string.IsNullOrEmpty(service_url))
            {
                service_url = ConfigService.getValue(ConfigConstant.SERVICE_URL);
            }

            return service_url;
        }

        private static string domain_url;

        private static string GetDomainUrl()
        {
            if (string.IsNullOrEmpty(domain_url))
            {
                domain_url = ConfigService.getValue(ConfigConstant.DOMAIN_URL);
            }
            return domain_url;
        }
    }
}
