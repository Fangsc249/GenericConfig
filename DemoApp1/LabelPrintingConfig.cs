using ConfigTool.ConfigCore;
using System.ComponentModel;

namespace DemoApp1
{
    public class OutputDir : ConfigBase
    {
        private string userId = "";
        [Config("高级设置", "OutputFolder", Description = "输出文件夹", Order = 5)]
        public string UserId
        {
            get => userId;
            set
            {
                if (userId != value)
                {
                    userId = value; OnPropertyChanged();
                }
            }
        }

        private string outputFolder = @"C:\Temp";
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
    }
    public class Printer : ConfigBase
    {
        private string outputFolder = @"C:\Temp";
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

        private string printerName = "OneNote (Desktop)";
        [Config("高级设置", "OutputFolder", Description = "输出文件夹", Order = 5)]
        public string PrinterName
        {
            get => printerName;
            set
            {
                if (printerName != value)
                {
                    printerName = value; OnPropertyChanged();
                }
            }
        }
    }

    public class LabelPrintingConfig : ConfigBase
    {
        private BindingList<OutputDir> _outFolders = new BindingList<OutputDir>();
        [Config("高级设置", "服务器配置", Order = 6)]
        public BindingList<OutputDir> OutFolders
        {
            get => _outFolders;
            set
            {
                if (_outFolders != value)
                {
                    _outFolders = value; OnPropertyChanged();
                }
            }
        }

        private BindingList<Printer> _printerNames = new BindingList<Printer>();
        [Config("高级设置", "服务器配置", Order = 6)]
        public BindingList<Printer> PrinterNames
        {
            get => _printerNames;
            set
            {
                if (_printerNames != value)
                {
                    _printerNames = value; OnPropertyChanged();
                }
            }
        }

        private string _connString = "";
        [Config("通用设置", "数据库连接字符串")]
        public string ConnectionString
        {
            get => _connString;
            set
            {
                if (_connString != value)
                {
                    _connString = value;
                    OnPropertyChanged();
                }
            }
        }

        private LogLevel _logLevel = LogLevel.Info;
        [Config("通用设置", "LogLevel")]
        public LogLevel LogLevel
        {
            get => _logLevel;
            set
            {
                if (_logLevel != value)
                {
                    _logLevel = value; OnPropertyChanged();
                }
            }
        }

        private int _runMode = 1;
        public int RunMode
        {
            get => _runMode;
            set
            {
                if(_runMode!=value)
                {
                    _runMode = value;OnPropertyChanged();
                }
            }
        }

        private string _shipPoint = "";
        public string ShipPoint
        {
            get => _shipPoint;
            set
            {
                if(_shipPoint!=value)
                {
                    _shipPoint = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _repeatInterval = 1;
        public int RepeatInterval
        {
            get => _repeatInterval;
            set {
                if(_repeatInterval!=value)
                {
                    _repeatInterval = value;OnPropertyChanged();
                }
            }
        }

        private Printer _defaultPrinter = new Printer();
        public Printer DefaultPrinter
        {
            get => _defaultPrinter;
            set
            {
                if (_defaultPrinter != value)
                {
                    _defaultPrinter = value;OnPropertyChanged();
                }
            }
        }
    }
}
