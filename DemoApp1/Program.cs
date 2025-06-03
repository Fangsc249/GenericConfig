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
            new NestedConfigPanelForm().ShowDialog();
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

    public class NestedConfigPanelForm : Form
    {
        IConfigService configService = new YamlConfigService();
        public NestedConfigPanelForm()
        {
            var panel = new NestedConfigPanel
            {
                Dock = DockStyle.Fill
            };
            var config = configService.Load<LabelPrintingConfig>("labelPrinting.yaml");
            panel.Bind(configService, config, "labelPrinting.yaml", "LabelPrinting");

            var saveButton = new Button
            {
                Text = "保存配置",
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.SteelBlue
            };
            saveButton.Click += (s, e) => panel.ApplyChanges();

            this.Controls.Add(panel);
            this.Controls.Add(saveButton);
            this.Size = new Size(1000, 600);
        }
    }

}
