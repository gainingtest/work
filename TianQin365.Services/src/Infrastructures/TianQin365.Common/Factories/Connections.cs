using System;
using System.Configuration;
using System.Text;

namespace TianQin365.Common.Factories
{
    /// <summary>
    /// SQL Server 数据库连接字符串配置
    /// </summary>
    public class Connections
    {
        /// <summary>
        /// 获取读操作的数据库连接字符串
        /// </summary>
        /// <param name="type">ORM实体类型</param>
        /// <returns></returns>
        public static string GetReadString(Type type) => GetWriteString(type);

        /// <summary>
        /// 获取写操作的数据库连接字符串
        /// </summary>
        /// <param name="type">ORM实体类型</param>
        /// <returns></returns>
        public static string GetWriteString(Type type)
        {
            var connectionString = null as string;

            if (!string.IsNullOrEmpty(type.Namespace))
            {
                var name = new StringBuilder();
                switch (type.Namespace)
                {
                case "TianQin365.Organization.Models":
                    name.Append("Organization"); break;
                }

                name.Append($"_{Global.Config.DbPostion}");

                connectionString = ConfigurationManager.ConnectionStrings[name.ToString()].ConnectionString;
            }

            return connectionString;
        }
    }
}
