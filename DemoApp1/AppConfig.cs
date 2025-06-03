using ConfigTool.ConfigCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DemoApp1
{
    public class ServerSettings : ConfigBase
    {
        private string _serverAddress = "127.0.0.1";
        [Config("高级设置", "服务器地址")]
        public string ServerAddress
        {
            get => _serverAddress; set
            {
                if (_serverAddress != value)
                {
                    _serverAddress = value; OnPropertyChanged();
                }
            }
        }

        private string _port = "8080";
        [Config("高级设置", "端口号")]
        public string Port
        {
            get => _port; set
            {
                if (_port != value)
                {
                    _port = value; OnPropertyChanged();
                }
            }
        }

        private ServerConfig _serverConfig = new ServerConfig();
        [Config("高级设置", "ServerConfig")]
        public ServerConfig ServerConfig
        {
            get => _serverConfig; set
            {
                if (_serverConfig != value)
                {
                    value = _serverConfig; OnPropertyChanged();
                }
            }
        }

    }
    public class ServerConfig : ConfigBase
    {
        private string _serverName = "Database Server";
        [Config("高级设置", "ServerName", Order = 4)]
        public string Name
        {
            get => _serverName; set
            {
                if (_serverName != value)
                {
                    _serverName = value; OnPropertyChanged();
                }
            }
        }

    }
    class AppConfig : ConfigBase
    {
        private string _appName = "DemoApp";
        [Config("通用设置", "应用名称")]
        public string AppName
        {
            get => _appName;
            set
            {
                if (_appName != value)
                {
                    _appName = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _enableLogging = true;
        [Config("通用设置", "启用日志")]
        public bool EnableLogging
        {
            get => _enableLogging;
            set
            {
                if (_enableLogging != value)
                {
                    _enableLogging = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _connString = "";
        [Config("高级设置", "Connection String", Description = "数据库连接字符串", Order = 1)]
        public string ConnectionString
        {
            get => _connString; set
            {
                if (_connString != value)
                {
                    _connString = value; OnPropertyChanged();
                }
            }
        }

        private int maxConn = 100;
        [Config("高级设置", "最大连接数", Description = "数据库连接池大小", Order = 2)]
        public int MaxConnections
        {
            get => maxConn; set
            {
                if (maxConn != value)
                {
                    maxConn = value;
                    OnPropertyChanged();
                }
            }
        }

        private LogLevel _logLevel = LogLevel.Info;
        [Config("高级设置", "日志级别", Order = 3)]
        public LogLevel LogLevel
        {
            get => _logLevel; set
            {
                if (_logLevel != value)
                {
                    _logLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime dateTime = DateTime.Today;
        [Config("高级设置", "日期", Order = 4)]
        public DateTime DateTime
        {
            get => dateTime; set
            {
                if (dateTime != value)
                {
                    dateTime = value; OnPropertyChanged();
                }
            }
        }

        private string outputFolder = @"c:\temp";
        [PathSelector(PathType.Directory)]
        [Config("高级设置", "OutputFolder", Description = "输出文件夹", Order = 5)]
        public string OutputFolder
        {
            get => outputFolder; set
            {
                if (outputFolder != value)
                {
                    outputFolder = value;
                    OnPropertyChanged();
                }
            }
        }

        private BindingList<ServerConfig> _servers = new BindingList<ServerConfig>();
        [Config("高级设置", "服务器配置", Order = 6)]
        public BindingList<ServerConfig> Servers
        {
            get => _servers; set
            {
                if (_servers != value)
                {
                    _servers = value;
                    OnPropertyChanged();
                }
            }
        }

        private ServerSettings serverSettings = new ServerSettings();
        [Config("高级设置", "服务器参数", Order = 6)]
        public ServerSettings ServerSetting
        {
            get => serverSettings; set
            {
                if (serverSettings != value)
                {
                    serverSettings = value; OnPropertyChanged();
                }
            }
        }

        private string forwarderFile = @"";
        [PathSelector(PathType.File, Filter = "Excel file(*.xlsx)|*.xlsx")]
        [Config("高级设置", "Forwaders", Description = "货代清单文件，Excel", Order = 7)]
        public string ForwarderFile
        {
            get => forwarderFile; set
            {
                if (forwarderFile != value)
                {
                    forwarderFile = value; OnPropertyChanged();
                }
            }
        }


    }
    public enum LogLevel
    {
        Verbose,
        Debug,
        Info,
        Warning,
        Error,
        Fatl,
    }
}
