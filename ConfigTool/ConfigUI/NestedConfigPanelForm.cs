using ConfigTool.ConfigCore;
using System.Drawing;
using System.Windows.Forms;

namespace ConfigTool.ConfigUI
{
    public class NestedConfigPanelForm : Form
    {
        public NestedConfigPanelForm(IConfigService service, object config, string configFile, string rootNodeText)
        {

            var panel = new NestedConfigPanel
            {
                Dock = DockStyle.Fill
            };
            panel.Bind(service, config, configFile, rootNodeText);
            configFile = System.IO.Path.GetFullPath(configFile);
            Text = $"Configuraion - {configFile}";
            var saveButton = new Button
            {
                Text = "保存配置",
                Dock = DockStyle.Bottom,
                Height = 30,
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold),
                BackColor = Color.LightSeaGreen
            };
            saveButton.Click += (s, e) => panel.ApplyChanges();

            Controls.Add(panel);
            Controls.Add(saveButton);
            Size = new Size(1000, 600);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NestedConfigPanelForm));
            this.SuspendLayout();
            // 
            // NestedConfigPanelForm
            // 
            this.ClientSize = new Size(282, 253);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NestedConfigPanelForm";
            this.ResumeLayout(false);

        }
    }
}
