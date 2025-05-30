namespace ConfigTool.ConfigCore
{
    public abstract class ConfigBase
    {
        [Config("通用设置", "应用名称")]
        public string AppName { get; set; } = "DemoApp";

        [Config("通用设置", "启用日志")]
        public bool EnableLogging { get; set; } = true;
    }
}
