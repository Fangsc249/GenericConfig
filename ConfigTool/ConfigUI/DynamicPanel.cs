using ConfigTool.ConfigCore;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace ConfigTool.ConfigUI
{
    public partial class DynamicPanel : UserControl
    {
        private object _configObject;
        private TabControl _tabControl;

        public DynamicPanel()
        {
            this.AutoScroll = true;
            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Multiline = true,
            };
            this.Controls.Add(_tabControl);
        }

        public void Bind(object configObj)
        {
            _configObject = configObj;
            GenerateControls();
        }

        private void GenerateControls()
        {
            _tabControl.TabPages.Clear();

            // 按Category分组属性
            var groupedProps = _configObject.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<ConfigAttribute>() != null)
                .GroupBy(p => p.GetCustomAttribute<ConfigAttribute>().Category)
                .OrderBy(g => g.First().GetCustomAttribute<ConfigAttribute>().Order);
            foreach (var group in groupedProps)
            {
                var tabPage = new TabPage(group.Key)
                {
                    Padding = new Padding(10),
                    AutoScroll = true,
                    BackColor = System.Drawing.Color.AliceBlue,
                };
                _tabControl.TabPages.Add(tabPage);
                int topPos = 10;
                foreach (var prop in group.OrderBy(p => p.GetCustomAttribute<ConfigAttribute>().Order))
                {
                    var attr = prop.GetCustomAttribute<ConfigAttribute>();
                    var lbl = new Label
                    {
                        Text = attr.DisplayName,
                        Top = topPos,
                        Left = 10,
                        Width = 150,
                    };
                    tabPage.Controls.Add(lbl);
                    var ctl = CreateControlForType(prop.PropertyType, prop.GetValue(_configObject), prop);
                    ctl.Top = topPos;
                    ctl.Left = 170;
                    ctl.Tag = prop;

                    if (ctl is TextBox txt) ctl.Width = 200;
                    if (ctl is TextBox && attr.DisplayName == "Connection String") ctl.Width = 600;
                    if (ctl is ComboBox cmb) cmb.Width = 200;
                    tabPage.Controls.Add(ctl);
                    if (!string.IsNullOrEmpty(attr.Description))
                    {
                        var toolTip = new ToolTip();
                        toolTip.SetToolTip(lbl, attr.Description);
                        toolTip.SetToolTip(ctl, attr.Description);
                    }
                    topPos += ctl.Height + 10;
                }
            }
        }

        private Control CreateControlForType(Type type, object value, PropertyInfo prop)
        {
            if (type == typeof(bool))
                return new CheckBox { Checked = (bool)(value ?? false) };

            if (type.IsEnum)
            {
                var cmb = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DataSource = Enum.GetValues(type)
                };
                if (value != null) cmb.SelectedItem = value;
                return cmb;
            }

            if (type == typeof(int) || type == typeof(double))
                return new NumericUpDown
                {
                    Value = Convert.ToDecimal(value ?? 0),
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue
                };

            if (type == typeof(DateTime))
                return new DateTimePicker { Value = DateTime.Today };

            if (type == typeof(string))
            {
                if (prop.GetCustomAttribute<PathSelectorAttribute>() != null)
                {
                    return CreatePathSelector(prop, value?.ToString());
                }

            }
            return new TextBox { Text = value?.ToString() };
        }

        public void ApplyChanges()
        {
            foreach (TabPage tabPage in _tabControl.TabPages)
            {
                foreach (Control ctrl in tabPage.Controls)
                {
                    if (ctrl.Tag is PropertyInfo prop)
                    {
                        object value = null;
                        if (ctrl is CheckBox cb) value = cb.Checked;
                        else if (ctrl is ComboBox cmb) value = cmb.SelectedItem;
                        else if (ctrl is NumericUpDown num) value = Convert.ChangeType(num.Value, prop.PropertyType);
                        else if (ctrl is DateTimePicker dtp) value = dtp.Value;
                        else if (ctrl is TextBox txt) value = Convert.ChangeType(txt.Text, prop.PropertyType);
                        else if (ctrl is Panel pathPanel && pathPanel.Tag is PropertyInfo prop2)
                        {
                            var txt2 = pathPanel.Controls.OfType<TextBox>().FirstOrDefault();
                            value = txt2.Text;
                        }
                        if (value != null)
                            prop.SetValue(_configObject, value);
                    }
                }
            }

        }
        private Control CreatePathSelector(PropertyInfo prop, string initialPath)
        {
            var panel = new Panel { Height = 20, Width = 600 };
            var txtPath = new TextBox
            {
                Text = initialPath,
                Width = 500,
                Dock = DockStyle.Left,
                Tag = prop,
            };

            var btnBrowse = new Button
            {
                Text = "浏览...",
                Dock = DockStyle.Right,
                Width = 80,
                Height = 20,
            };
            var attr = prop.GetCustomAttribute<PathSelectorAttribute>();
            btnBrowse.Click += (s, e) =>
            {
                if (attr?.Type == PathType.File)
                {
                    var dialog = new OpenFileDialog { Filter = attr.Filter, Multiselect = false };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        txtPath.Text = dialog.FileName;
                    }
                }
                else
                {
                    using (var dialog = new FolderBrowserDialog())
                    {
                        dialog.SelectedPath = txtPath.Text;
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            txtPath.Text = dialog.SelectedPath;
                        }
                    }
                }
            };

            panel.Controls.Add(txtPath);
            panel.Controls.Add(btnBrowse);
            return panel;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DynamicPanel
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Name = "DynamicPanel";
            this.ResumeLayout(false);

        }
    }
}
