using ConfigTool.ConfigCore;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ConfigTool.ConfigUI
{
    public partial class TabedPanel : UserControl
    {
        private object _configObject;
        private TabControl _tabControl;

        public TabedPanel()
        {
            AutoScroll = true;
            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Multiline = true,
            };
            Controls.Add(_tabControl);
        }

        public void Bind(object configObj)
        {
            _configObject = configObj;
            _tabControl.TabPages.Clear();
            //GenerateControls_SingleLevel();
            GenerateControls_Recursive();//支持嵌套对象，Bug待排除
        }
        private void GenerateControls_Recursive()
        {
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
                    BackColor = System.Drawing.Color.AliceBlue
                };
                _tabControl.TabPages.Add(tabPage);

                // 递归生成控件
                //Console.WriteLine($"Generating for group {group.Key}");
                GenerateControlsForObject(_configObject, tabPage, group.Key);
            }
        }
        private void GenerateControls_SingleLevel()
        { // not used.
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
            //type.Dump($"{value}");
            if (type == typeof(bool))
                return new CheckBox { Checked = (bool)(value ?? false) };
            if (type.IsEnum) // Hunyuan AI 2025-10-2 解决了枚举类型SelectedIndex无法正确设置的问题
            {
                var cmb = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                //Console.WriteLine($"Creating ComboBox for enum: {type.Name}");

                // 手动添加枚举值到 Items
                var enumValues = Enum.GetValues(type);
                foreach (var enumValue in enumValues)
                {
                    cmb.Items.Add(enumValue);
                }

                //Console.WriteLine($"✅ ComboBox.Items.Count = {cmb.Items.Count}");

                if (value != null)
                {
                    if (type.IsAssignableFrom(value.GetType()))
                    {
                        //Console.WriteLine($"Selecting value: {value}");

                        for (int i = 0; i < cmb.Items.Count; i++)
                        {
                            if (cmb.Items[i].Equals(value))
                            {
                                //Console.WriteLine($"✅ Found at index {i}, setting SelectedIndex = {i}");
                                cmb.SelectedIndex = i;
                                break;
                            }
                        }

                        if (cmb.SelectedIndex == -1)
                        {
                            Console.WriteLine($"⚠️ Did not find a match. Defaulting to index 0.");
                            cmb.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"❌ Value type not compatible. Defaulting to index 0.");
                        cmb.SelectedIndex = 0;
                    }
                }
                else
                {
                    Console.WriteLine($"⚠️ value is null. Defaulting to index 0.");
                    cmb.SelectedIndex = 0;
                }

                return cmb;
            }

            //if (type.IsEnum) // DeepSeek提供的有Bug的版本
            //{
            //    var cmb = new ComboBox
            //    {
            //        DropDownStyle = ComboBoxStyle.DropDownList,
            //        DataSource = Enum.GetValues(type)
            //    };
            //    Console.WriteLine($"Creating ComboBox for enum {type.Name} with selected value: {value}");
            //    Console.WriteLine($"🔍 Debug: ComboBox.Items.Count = {cmb.Items.Count}");

            //    if (value != null)
            //    {
            //        //cmb.SelectedIndex = 1;
            //        if (type.IsAssignableFrom(value.GetType()))
            //        {
            //            Console.WriteLine($"Value is a valid enum of type {type.Name}: {value}");

            //            // ✅ 正确方式：遍历 Items，找到与 value 值相等的项（使用 Equals），然后设置 SelectedIndex
            //            var items = (Array)cmb.DataSource;
            //            for (int i = 0; i < items.Length; i++)
            //            {
            //                var item = items.GetValue(i);
            //                if (item.Equals(value))  // 使用 .Equals() 比较值，而不是引用
            //                {
            //                    Console.WriteLine($"Found match at index {i}: {item}");
            //                    //cmb.SelectedIndex = i;
            //                    break;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine($"Value type {value.GetType()} is not the same as enum type {type}");
            //        }
                    
            //    }

            //    return cmb;
            //}

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
        private void GenerateControlsForObject(object obj, Control parentContainer, string parentCategory)
        { // new

            var props = obj.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<ConfigAttribute>() != null && p.GetCustomAttribute<ConfigAttribute>().Category == parentCategory)
                .OrderBy(p => p.GetCustomAttribute<ConfigAttribute>().Order);
            //Console.WriteLine($"Processing {obj.GetType().Name} props.Count: {props.Count()}");

            int topPos = 20;
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<ConfigAttribute>();
                //var category = parentCategory ?? attr.Category;
                //Console.WriteLine($"prop {prop.Name}-{prop.PropertyType.Name} simple? : {IsSimpleType(prop.PropertyType)}");
                // 如果是嵌套对象（类类型）
                if (!IsSimpleType(prop.PropertyType))
                {
                    var nestedObj = prop.GetValue(obj);
                    if (nestedObj != null)
                    {
                        // 创建分组容器（如 GroupBox）
                        // Tag - Provides a convenient place to store any type of object.
                        // The Tag property is not used by the .NET Framework.
                        // Instead, you use it to store associated data(like a data
                        // object or a string with a unique ID).
                        var groupBox = new GroupBox
                        {
                            Text = attr.DisplayName,
                            Left = 10,
                            Top = topPos,
                            Padding = new Padding(10),
                            Width = 800,
                            Tag = nestedObj,
                        };
                        parentContainer.Controls.Add(groupBox);
                        // 递归生成嵌套对象的控件
                        GenerateControlsForObject(nestedObj, groupBox, parentCategory);
                        topPos += groupBox.Controls.Count * 30;
                    }
                }
                else
                {
                    // 生成简单类型的控件（原有逻辑）

                    var attr2 = prop.GetCustomAttribute<ConfigAttribute>();
                    var lbl = new Label
                    {
                        Text = attr2.DisplayName,
                        Top = topPos,
                        Left = 10,
                        Width = 150,
                    };
                    parentContainer.Controls.Add(lbl);
                    //Returns the property value of a specified object.
                    var ctl = CreateControlForType(prop.PropertyType, prop.GetValue(obj), prop);
                    ctl.Top = topPos;
                    ctl.Left = 170;
                    ctl.Tag = prop;

                    if (ctl is TextBox txt)
                    {
                        ctl.Width = 500;
                        //txt.TextAlign = HorizontalAlignment.Right;
                    }
                    if (ctl is TextBox && attr.DisplayName == "Connection String") ctl.Width = 600;
                    if (ctl is ComboBox cmb) cmb.Width = 400;
                    parentContainer.Controls.Add(ctl);
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

        private bool IsSimpleType(Type type)
        { //new
            return type.IsPrimitive ||
                   type == typeof(string) ||
                   type.IsEnum ||
                   type == typeof(DateTime);
        }

        public void ApplyChanges()
        {
            foreach (TabPage tabPage in _tabControl.TabPages)
            {
                ApplyChangesToObject(_configObject, tabPage);
            }
        }

        private void ApplyChangesToObject(object obj, Control parentContainer)
        {
            foreach (Control ctrl in parentContainer.Controls)
            {
                //Console.WriteLine($"{ctrl is GroupBox groupBox2}");
                if (ctrl is GroupBox groupBox)
                {
                    //Console.WriteLine($"{ctrl is GroupBox groupBox2}");
                    // 递归处理分组容器中的控件
                    ApplyChangesToObject(groupBox.Tag, groupBox);
                }
                else if (ctrl is Panel panel)
                {
                    // 处理路径选择器控件
                    var txtPath = panel.Controls.OfType<TextBox>().FirstOrDefault();
                    if (txtPath != null && txtPath.Tag is PropertyInfo prop)
                    {
                        object value = txtPath.Text;
                        //Console.WriteLine($"setting value {value}");
                        prop.SetValue(obj, value);
                    }
                }
                else if (ctrl.Tag is PropertyInfo prop)
                {
                    // 设置属性值（原有逻辑）
                    object value = GetControlValue(ctrl);
                    //Console.WriteLine($"setting value {value}");
                    prop.SetValue(obj, value);
                }
            }
        }

        private object GetControlValue(Control ctrl)
        {
            if (ctrl is CheckBox cb) return cb.Checked;
            if (ctrl is ComboBox cmb) return cmb.SelectedItem;
            if (ctrl is NumericUpDown num) return (int)num.Value;
            if (ctrl is DateTimePicker dtp) return dtp.Value;
            if (ctrl is TextBox txt) return txt.Text;
            return null;
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
            txtPath.SelectionStart = txtPath.Text.Length;
            txtPath.ScrollToCaret();
            txtPath.TextChanged
                += (s, e) =>
            {
                //Console.WriteLine($"Path changed: {txtPath.Text}");
                // You can add validation logic here if needed
                txtPath.SelectionStart = txtPath.Text.Length;
                txtPath.ScrollToCaret();
            };
            var btnBrowse = new Button
            {
                Text = "浏览...",
                Dock = DockStyle.Right,
                Width = 80,
                Height = 20,
            };
            WinFormFormatters.FormatButtonsAsBootstrapInfo(new[] { btnBrowse });
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
