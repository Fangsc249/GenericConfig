using System;
using ConfigTool.ConfigCore;
using ConfigTool.ConfigUI;

namespace DemoApp1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            NestedPanelTest();
            //TabedPanelTest();
        }
        static void NestedPanelTest()
        {
            string configFile = "LabelPrinting.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<LabelPrintingConfig>(configFile);
            new NestedConfigPanelForm(configService, config, configFile, nameof(LabelPrintingConfig)).ShowDialog();

        }
        static void TabedPanelTest()
        {
            string configFile = "config.json";
            configFile = "config.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<AppConfig>(configFile);
            new TabedPanelForm(configService, configFile, config).ShowDialog();
        }
    }   
}
