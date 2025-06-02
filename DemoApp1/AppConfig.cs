using ConfigTool.ConfigCore;
using System;
using System.Collections.Generic;

namespace DemoApp1
{
    public class ServerSettings
    {
        [Config("高级设置", "服务器地址")]
        public string ServerAddress { get; set; } = "127.0.0.1";

        [Config("高级设置", "端口号")]
        public string Port { get; set; } = "8080";

        [Config("高级设置", "ServerConfig")]
        public ServerConfig ServerConfig { get; set; } = new ServerConfig();

    }
    public class ServerConfig
    {
        [Config("高级设置", "ServerName", Order = 4)]
        public string Name { get; set; } = "Server1";

        [Config("高级设置", "IP Address", Order = 4)]
        public string IP { get; set; } = "192.168.1.1";

        [Config("高级设置", "Port", Order = 4)]
        public string Port { get; set; } = "8080";
    }
    class AppConfig : ConfigBase
    {
        [Config("高级设置", "Connection String", Description = "数据库连接字符串", Order = 1)]
        public string ConnectionString { get; set; } = "";

        [Config("高级设置", "最大连接数", Description = "数据库连接池大小", Order = 2)]
        public int MaxConnections { get; set; } = 100;

        [Config("高级设置", "日志级别", Order = 3)]
        public LogLevel LogLevel { get; set; } = LogLevel.Info;

        [Config("高级设置", "日期", Order = 4)]
        public DateTime DateTime { get; set; } = DateTime.Today;

        [PathSelector(PathType.Directory)]
        [Config("高级设置", "OutputFolder", Description = "输出文件夹", Order = 5)]
        public string OutputFolder { get; set; } = @"c:\temp";

        [Config("高级设置", "服务器配置", Order = 6)]
        public List<ServerConfig> Servers { get; set; }

        [Config("高级设置", "服务器参数", Order = 6)]
        public ServerSettings ServerSetting { get; set; } = new ServerSettings();

        [PathSelector(PathType.File, Filter = "Excel file(*.xlsx)|*.xlsx")]
        [Config("高级设置", "Forwaders", Description = "货代清单文件，Excel", Order = 7)]
        public string ForwarderFile { get; set; } = @"";


    }
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
}
