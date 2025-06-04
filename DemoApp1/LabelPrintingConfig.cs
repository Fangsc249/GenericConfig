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
                if (_runMode != value)
                {
                    _runMode = value; OnPropertyChanged();
                }
            }
        }

        private string _shipPoint = "";
        public string ShipPoint
        {
            get => _shipPoint;
            set
            {
                if (_shipPoint != value)
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
            set
            {
                if (_repeatInterval != value)
                {
                    _repeatInterval = value; OnPropertyChanged();
                }
            }
        }

        //private Printer _defaultPrinter = new Printer();
        //public Printer DefaultPrinter
        //{
        //    get => _defaultPrinter;
        //    set => SetProperty(ref _defaultPrinter, value);
        //}
        private int _backwardDayOffset = 1;
        public int BackwardDayOffset
        {
            get => _backwardDayOffset;
            set => SetProperty(ref _backwardDayOffset, value);
        }

        private int _forwardDayOffset = 2;
        public int ForwardDayOffset
        {
            get => _forwardDayOffset;
            set => SetProperty(ref _forwardDayOffset, value);
        }

        private bool _debugOn = false;
        public bool DebugOn
        {
            get => _debugOn;
            set => SetProperty(ref _debugOn, value);
        }

        private string _dataProvider = "DBDataProvider";
        public string DataProvider
        {
            get => _dataProvider;
            set => SetProperty(ref _dataProvider, value);
        }

        private string _labelTemplateFile = @"Templates\LabelDeliveryNote.cs";
        public string LabelTemplateFile
        {
            get => _labelTemplateFile;
            set => SetProperty(ref _labelTemplateFile, value);
        }

        private string _labelTemplateName = "LabelPrinting.Templates.LabelDeliveryNote";
        public string LabelTemplateName
        {
            get => _labelTemplateName;
            set => SetProperty(ref _labelTemplateName, value);
        }

        private string _forwarderFile = @"C:\Temp.xlsx";
        [PathSelector(PathType.File)]
        public string ForwarderFile
        {
            get => _forwarderFile;
            set => SetProperty(ref _forwarderFile, value);
        }

        private int _marginMM = 3;
        public int MarginMM
        {
            get => _marginMM;
            set => SetProperty(ref _marginMM, value);
        }

        private int _widthMM = 100;
        public int WidthMM
        {
            get => _widthMM;
            set => SetProperty(ref _widthMM, value);
        }

        private int _heightMM = 100;
        public int HeightMM
        {
            get => _heightMM;
            set => SetProperty(ref _heightMM, value);
        }

        private int _qrTableWidthMM = 100;
        public int QRTableWidthMM
        {
            get => _qrTableWidthMM;
            set => SetProperty(ref _qrTableWidthMM, value);
        }

        private int _qrTablePackedPNLines = 20;
        public int QRTablePackedPNLines
        {
            get => _qrTablePackedPNLines;
            set => SetProperty(ref _qrTablePackedPNLines, value);
        }

        private string _qrTableFieldSeparator = ",";
        public string QRTableFieldSeparator
        {
            get => _qrTableFieldSeparator;
            set => SetProperty(ref _qrTableFieldSeparator, value);
        }

        private string _pdfBackupDir = @"C:\Temp";
        [PathSelector(PathType.Directory)]
        public string PDFBackupDir
        {
            get => _pdfBackupDir;
            set => SetProperty(ref _pdfBackupDir, value);
        }

        private int _pdfBackupKeepDays = 2;
        public int PDFBackupKeepDays
        {
            get => _pdfBackupKeepDays;
            set => SetProperty(ref _pdfBackupKeepDays, value);
        }

        private PDFReader _pdfReader = PDFReader.SumatraPDF;
        public PDFReader PDFReader
        {
            get => _pdfReader;
            set => SetProperty(ref _pdfReader, value);
        }

        private string _sumatraPDF = @"PortablePDFReader\SumatraPDF-3.5.2-64.exe";
        public string SumatraPDF
        {
            get => _sumatraPDF;
            set => SetProperty(ref _sumatraPDF, value);
        }

        private string _pdfToPrinter = @"PortablePDFReader\PDFtoPrinter.exe";
        public string PDFtoPrinter
        {
            get => _pdfToPrinter;
            set => SetProperty(ref _pdfToPrinter, value);
        }

        private string _ghostScript = "C:\\Program Files\\gs\\gs10.05.1\\bin\\gswin64c.exe";
        [PathSelector(PathType.File)]
        public string GhostScript
        {
            get => _ghostScript;
            set => SetProperty(ref _ghostScript, value);
        }

        private string _pdfXEdit = "C:\\Users\\u324926\\OneDrive - Danfoss\\Tools\\PDFXEdit9\\PDFXEdit.exe";
        [PathSelector(PathType.File)]
        public string PDFXEdit
        {
            get => _pdfXEdit;
            set => SetProperty(ref _pdfXEdit, value);
        }

        private bool _autoPrint = true;
        public bool AutoPrint
        {
            get => _autoPrint;
            set => SetProperty(ref _autoPrint, value);
        }

        private int _delayBetweenPrint = 5;
        public int DelayBetweenPrint
        {
            get => _delayBetweenPrint;
            set => SetProperty(ref _delayBetweenPrint, value);
        }


    }

    public enum PDFReader
    {
        SumatraPDF,
        PDFtoPrinter,
        GhostScript,
        PDFXEdit,
    }
}
