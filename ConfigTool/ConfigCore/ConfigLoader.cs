using ConfigTool.ConfigUI;
using System;
using System.Windows.Forms;

namespace ConfigTool.ConfigCore
{
    public static class ConfigLoader // 2025-9-25
    {
        public static string ConfigFolder = AppDomain.CurrentDomain.BaseDirectory;
        public static T YamlConfig<T>() where T : ConfigBase, new()
        {
            string configFile = $"{ConfigFolder}\\{typeof(T).Name}.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<T>(configFile);
            return config;
        }
        public static void TabedPanelYamlConfig<T>() where T : ConfigBase, new()
        {
            string configFile = $"{ConfigFolder}\\{typeof(T).Name}.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<T>(configFile);
            new TabedPanelForm(configService, configFile, config).ShowDialog();
            
        }
        public static void NestedPanelYamlConfig<T>() where T : ConfigBase, new()
        {
            string configFile = $"{ConfigFolder}\\{typeof(T).Name}.yaml";
            IConfigService configService = new YamlConfigService();
            var config = configService.Load<T>(configFile);
            new NestedConfigPanelForm(configService, config, configFile, typeof(T).Name).ShowDialog();
        }

        //***********************************
        public static T JsonConfig<T>() where T : ConfigBase, new()
        {
            string configFile = $"{ConfigFolder}\\{typeof(T).Name}.json";
            IConfigService configService = new JsonConfigService();
            var config = configService.Load<T>(configFile);
            return config;
        }

        public static void TabedPanelJsonConfig<T>() where T : ConfigBase, new()
        {
            string configFile = $"{ConfigFolder}\\{typeof(T).Name}.json";
            IConfigService configService = new JsonConfigService();
            var config = configService.Load<T>(configFile);
            new TabedPanelForm(configService, configFile, config).ShowDialog();
        }
        public static void NestedPanelJsonConfig<T>() where T : ConfigBase, new()
        {
            string configFile = $"{ConfigFolder}\\{typeof(T).Name}.json";
            IConfigService configService = new JsonConfigService();
            var config = configService.Load<T>(configFile);
            new NestedConfigPanelForm(configService, config, configFile, typeof(T).Name).ShowDialog();
        }
    }

}
