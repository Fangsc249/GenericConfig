using System;
using ConfigTool.ConfigCore;
using ConfigTool.ConfigUI;

/*
 * 涉及嵌套的配置对象或者对象集合的情况使用NestedPanel
 * 其它情况用TabedPanel可以用Tab实现更好的组织
 * 2025-6-6
 */

namespace DemoApp1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLower().Trim())
                {
                    case "appconfig": TabedPanelTest(); break;
                    case "labelprintingconfig": NestedPanelTest(); break;
                }
            }
            else
            {
                TabedPanelAppConfig_001();
            }
            
        }
        static void NestedPanelTest()
        {
            string configFile = $"{nameof(LabelPrintingConfig)}.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<LabelPrintingConfig>(configFile);
            new NestedConfigPanelForm(configService, config, configFile, nameof(LabelPrintingConfig)).ShowDialog();

        }
        static void TabedPanelTest()
        {
            string configFile = $"{nameof(AppConfig)}.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<AppConfig>(configFile);
            new TabedPanelForm(configService, configFile, config).ShowDialog();
        }
        static void TabedPanelAppConfig_001()
        {
            string configFile = $"{nameof(AppConfig_001)}.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<AppConfig_001>(configFile);
            new TabedPanelForm(configService, configFile, config).ShowDialog();
        }
    }
}
