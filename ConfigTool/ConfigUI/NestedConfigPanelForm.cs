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
                Text = "Save",
                Dock = DockStyle.Bottom,
                Height = 30,
                Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular),
                BackColor = Color.LightSeaGreen
            };
            saveButton.Click += (s, e) => panel.ApplyChanges();
            WinFormFormatters.FormatButtonsAsBootstrapInfo(saveButton);
            Controls.Add(panel);
            Controls.Add(saveButton);
            Size = new Size(1000, 600);
        }

    }
}
