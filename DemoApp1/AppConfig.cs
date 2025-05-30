using ConfigTool.ConfigCore;
using System;

namespace DemoApp1
{
    class AppConfig : ConfigBase
    {
        [Config("高级设置", "Connection String", Description = "数据库连接字符串", Order = 1)]
        public string ConnectionString { get; set; } = "";

        [Config("高级设置", "最大连接数", Description = "数据库连接池大小", Order = 2)]
        public int MaxConnections { get; set; } = 100;

        [Config("高级设置", "日志级别", Order = 3)]
        public LogLevel LogLevel { get; set; } = LogLevel.Info;

        [Config("高级设置", "日期", Order = 4)]
        public DateTime DateTime { get; set; }

        [PathSelector(PathType.Directory)]
        [Config("高级设置", "OutputFolder", Description = "输出文件夹", Order = 5)]
        public string OutputFolder { get; set; } = @"c:\temp";

        [PathSelector(PathType.File, Filter = "Excel file(*.xlsx)|*.xlsx")]
        [Config("高级设置", "Forwaders", Description = "货代清单文件，Excel", Order = 6)]
        public string ForwarderFile { get; set; }

    }
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
}
