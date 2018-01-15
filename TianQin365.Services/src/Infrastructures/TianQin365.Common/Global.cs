using System;
using TianQin365.Common.Models;

namespace TianQin365.Common
{
    public class Global
    {
        public static Config Config { get; set; }
        public static string ConfigPath { get; set; } = "site.config";
        public static Config DefaultConfig = new Config()
        {
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now
        };
    }
}
