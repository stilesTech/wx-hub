using System;
using System.Collections.Concurrent;
using log4net;

namespace Common
{
    public class LogHelper
    {
        private static readonly ConcurrentDictionary<Type, ILog> Loggers = new ConcurrentDictionary<Type, ILog>();

        public readonly static string RepositoryName = "NETCoreRepository";

        /// <summary>
        /// 获取记录器
        /// </summary>
        /// <param name="source">soruce</param>
        /// <returns></returns>
        public static ILog GetLogger<T>()
        {
            Type source = typeof(T);
            if (Loggers.ContainsKey(source))
            {
                return Loggers[source];
            }
            else
            {
                ILog logger = LogManager.GetLogger(RepositoryName, source);
                Loggers.TryAdd(source, logger);
                return logger;
            }
        }

        /// <summary>
        /// 获取记录器F:\Project\正在进行的项目\网关后台\LinkGate.Api\LinkGate.Api\Controllers\LoginController.cs
        /// </summary>
        /// <param name="source">soruce</param>
        /// <returns></returns>
        public static ILog GetLogger(Type source)
        {
            if (Loggers.ContainsKey(source))
            {
                return Loggers[source];
            }
            else
            {
                ILog logger = LogManager.GetLogger(RepositoryName, source);
                Loggers.TryAdd(source, logger);
                return logger;
            }
        }
    }
}
