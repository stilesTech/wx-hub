using System;
namespace Common
{
    public class ServiceProviderWrapper
    {
        private static IServiceProvider _serviceProvider;

        public static IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }

        public static void SetDefault(IServiceProvider svp)
        {
            ServiceProviderWrapper._serviceProvider = svp;
        }
    }
}
