using ConfigTool.ConfigCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp1
{
    internal class AppConfig_001 : ConfigBase
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
                    _appName = value; OnPropertyChanged();
                }
            }
        }
        private ServerSettings _serverSettings = new ServerSettings();
        [Config("高级设置", "服务器设置")]
        public ServerSettings ServerSettings
        {
            get => _serverSettings; set
            {
                if (_serverSettings != value)
                {
                    _serverSettings = value; OnPropertyChanged();
                }
            }
        }
    } // class AppConfig_001

}
