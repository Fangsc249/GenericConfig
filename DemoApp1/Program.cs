using System;
using ConfigTool.ConfigCore;
using ConfigTool.ConfigUI;
using System.Windows.Forms;

namespace DemoApp1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var configService = new JsonConfigService();
            var config = configService.Load<AppConfig>("config.json");

            using (var form = new ConfigForm(configService, "config.json", config))
            {
                var rlt = form.ShowDialog();
                Console.WriteLine(rlt.ToString());
                if (rlt == DialogResult.OK)
                {
                    // 应用新配置
                    Console.WriteLine($"{config.AppName}");
                    Console.WriteLine($"{config.ConnectionString}");
                }
            }

        }
    }
}
