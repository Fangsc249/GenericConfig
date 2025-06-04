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
            //DynamicPanelTest();
        }
        static void NestedPanelTest()
        {
            string configFile = "labelPrinting.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<LabelPrintingConfig>(configFile);
            new NestedConfigPanelForm(configService, config, configFile, nameof(LabelPrintingConfig)).ShowDialog();

        }
        static void DynamicPanelTest()
        {
            string configFile = "config.json";
            configFile = "config.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<AppConfig>(configFile);
            new TabedPanelForm(configService, configFile, config).ShowDialog();
        }
    }   
}
