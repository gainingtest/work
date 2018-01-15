using System;
using System.Configuration;
using System.Runtime.Serialization;
using TianQin365.Common.Extensions;

namespace TianQin365.Common.Models
{
    [DataContract]
    public class Config
    {
        #region Properties
        /// <summary>
        /// 是否发布到线上
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// 数据库位置
        /// </summary>
        public DbPostions DbPostion { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public DateTime UpdateTime { get; set; }
        #endregion

        #region Members
        /// <summary>
        /// 数据库位置枚举
        /// </summary>
        public enum DbPostions
        {
            Test = 0,
            HangTian,
            Online,
        }
        #endregion

        #region Methods
        private static object lockConfig = new object();

        public static void Initialize()
        {
            if (Global.Config == null)
                lock (lockConfig)
                    if (Global.Config == null)
                    {
                        // 加载配置文件
                        Global.Config = Config.Load(Global.ConfigPath);
                        //Common.Config.UpdateTime = DateTime.Now;
                        //Common.Config.Save(Common.ConfigPath);

                        //// 启动配置文件监听线程
                        //var watcher = new FileSystemWatcher(HttpContext.Current.Server.MapPath("/"), "Site.config");
                        //watcher.Changed += Watcher_Changed;
                        //watcher.EnableRaisingEvents = true;
                    }
        }

        public static Config Load(string path)
        {
            Config config = null;
            try
            {
                // 1.加载site.config文件
                //SharpSerializer serializer = new SharpSerializer();
                //config = serializer.Deserialize(path) as Config;

                // 2.加载site.config文件失败时，使用默认配置
                if (config == null)
                    config = Global.DefaultConfig.Clone() as Config;

                // 3.加载.net配置文件：web.config/app.config
                config.IsOnline = ConfigurationManager.AppSettings["online"].ConvertTo<bool>();
                if (ConfigurationManager.AppSettings["DbPostion"] != null)
                    config.DbPostion = ConfigurationManager.AppSettings["DbPostion"].ConvertTo<DbPostions>();
                else
                    config.DbPostion = config.IsOnline ? DbPostions.Online : DbPostions.Test;
            }
            catch { }
            return config;
        }

        //public bool Save() { return this.Save(Common.ConfigPath); }
        //public bool Save(string path)
        //{
        //    try
        //    {
        //        SharpSerializer serializer = new SharpSerializer();
        //        serializer.Serialize(this, path);
        //    }
        //    catch { return false; }
        //    return true;
        //}

        public object Clone() => this.MemberwiseClone();

        //private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    var config = Config.Load(Common.ConfigPath);
        //    if (config != null)
        //    {
        //        Common.Config = config;
        //        Common.UpdateTime = DateTime.Now;
        //        Common.Logs.WriteLine($"配置修改！");
        //    }
        //}
        #endregion
    }
}
