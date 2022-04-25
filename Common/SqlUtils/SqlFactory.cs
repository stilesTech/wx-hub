using System;
namespace Common.SqlUtils
{
    public class SqlFactory
    {
        private static ConnectionStringWrapper _wrapper = null;
        private static readonly object SynObject = new object();

        public static ConnectionStringWrapper Wrapper
        {
            get
            {
                if (_wrapper == null)
                {
                    lock (SynObject)
                    {
                        if (_wrapper == null)
                        {
                            var factory = ServiceProviderWrapper.ServiceProvider.GetService(typeof(ConnectionStringWrapper));
                            _wrapper = (ConnectionStringWrapper)factory;
                        }
                        return _wrapper;
                    }
                }
                return _wrapper;
            }
        }

        public static void SetDefault(ConnectionStringWrapper wrapper)
        {
            _wrapper = wrapper;
        }


        private SqlFactory()
        {
        }


        public static string GetConnectionString(string name)
        {
            return Wrapper.GetConnectionStringEntry(name)?.ConnectionString;
        }

    }
}
