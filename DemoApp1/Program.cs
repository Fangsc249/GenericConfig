using System;
using ConfigTool.ConfigCore;
using ConfigTool.ConfigUI;
using System.Windows.Forms;
using System.Drawing;

namespace DemoApp1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new DemoForm().ShowDialog();
            //DynamicPanelTest();
        }
        static void DynamicPanelTest()
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

    public class DemoForm : Form
    {
        public DemoForm()
        {
            var configService = new JsonConfigService();
            var config = configService.Load<AppConfig>("config.json");

            config.Servers = new System.Collections.Generic.List<ServerConfig>
                {
                    new ServerConfig { Name = "主服务器", IP = "10.0.0.1" },
                    new ServerConfig { Name = "备份服务器", IP = "10.0.0.2" }
                };


            var panel = new NestedConfigPanel
            {
                Dock = DockStyle.Fill
            };
            panel.Bind(config);

            var saveButton = new Button
            {
                Text = "保存配置",
                Dock = DockStyle.Bottom
            };
            saveButton.Click += (s, e) => panel.ApplyChanges();

            this.Controls.Add(panel);
            this.Controls.Add(saveButton);
            this.Size = new Size(900, 600);
        }
    }

}
