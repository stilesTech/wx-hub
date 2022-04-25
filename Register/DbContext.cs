using System;
using System.Linq;
using Common.SqlUtils;
using Entities;
using SqlSugar;

namespace Register
{
    public class DbContext
    {
        public const string DefaultDatabaseName = "wx_hub";

        public DbContext(string databaseName = DefaultDatabaseName)
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = GetConnString(databaseName),
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
            });
            //调式代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }

        private static string GetConnString(string databaseName)
        {
            return SqlFactory.GetConnectionString(databaseName);
        }

        //注意：不能写成静态的，不能写成静态的
        //用来处理事务多表查询和复杂的操作
        public SqlSugarClient Db;
    }

    public class DbContext<T> where T : class, new()
    {
        public const string DefaultDatabaseName = "wx_hub";

        public DbContext(string databaseName= DefaultDatabaseName)
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = GetConnString(databaseName),
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了

            });
            //调式代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }


        private static string GetConnString(string databaseName)
        {
            return SqlFactory.GetConnectionString(databaseName);
        }

        //注意：不能写成静态的，不能写成静态的
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作

        public SimpleClient<T> TableDb { get { return new SimpleClient<T>(Db); } }
    }
}
