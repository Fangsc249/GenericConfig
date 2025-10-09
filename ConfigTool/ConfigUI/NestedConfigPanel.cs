using ConfigTool.ConfigCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;


namespace ConfigTool.ConfigUI
{
    public partial class NestedConfigPanel : UserControl
    {
        private TreeView configTree;
        private Panel configPanel;
        private object _configObject;
        private IConfigService _configService;
        private string _configFile;
        private string _currentPath;
        //private string _rootNodeText;
        private BindingSource _bindingSource = new BindingSource();
        private Dictionary<string, object> _objectCache = new Dictionary<string, object>();

        public NestedConfigPanel()
        {
            //InitializeComponent();
            MyInitializeComponent();
        }

        private void MyInitializeComponent()
        {
            this.Size = new Size(1100, 600);

            // 分割容器
            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 15
            };
            this.Controls.Add(splitContainer);

            // 左侧树形视图
            configTree = new TreeView
            {
                Dock = DockStyle.Fill,
                ShowRootLines = false,
                ShowPlusMinus = true,
                HideSelection = false,
                BackColor = Color.AliceBlue
            };
            configTree.AfterSelect += ConfigTree_AfterSelect;
            configTree.DrawNode += treeView_DrawNode;
            splitContainer.Panel1.Controls.Add(configTree);

            // 右侧配置面板
            configPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.AliceBlue,
            };
            splitContainer.Panel2.Controls.Add(configPanel);
        }

        //Main logic
        public void Bind(IConfigService configService, object configObj, string configFile, string rootNodeText = "Config")
        {
            _configObject = configObj;
            _configService = configService;
            _configFile = configFile;
            _currentPath = rootNodeText;
            //_rootNodeText = rootNodeText;
            _bindingSource.DataSource = configObj;
            BuildConfigTree();
        }
        // build TreeView structure
        private void BuildConfigTree()
        {
            configTree.Nodes.Clear();
            _objectCache.Clear();

            var rootNode = new TreeNode(_currentPath)
            {
                Tag = new NodeInfo { Path = _currentPath, Object = _configObject }
            };
            configTree.Nodes.Add(rootNode);

            BuildTreeNodes(rootNode, _configObject, _currentPath);
            rootNode.Expand();
        }
        private void BuildTreeNodes(TreeNode parentNode, object obj, string path)
        {
            if (obj == null) return;

            var type = obj.GetType();
            _objectCache[path] = obj;

            // 处理集合类型
            if (IsCollectionType(type))
            {
                int index = 0;
                foreach (var item in (IEnumerable)obj)
                {
                    string itemPath = $"{path}[{index}]";

                    var itemNode = new TreeNode($"Element {index + 1}")
                    {
                        Tag = new NodeInfo { Path = itemPath, Object = item }
                    };
                    parentNode.Nodes.Add(itemNode);

                    BuildTreeNodes(itemNode, item, itemPath);
                    index++;
                }
                return;
            }

            // 处理嵌套对象
            foreach (var prop in type.GetProperties())
            {
                if (IsSimpleType(prop.PropertyType)) continue;

                string propPath = $"{path}.{prop.Name}";
                var propValue = prop.GetValue(obj);

                if (propValue == null) continue;

                var propNode = new TreeNode(prop.Name)
                {
                    Tag = new NodeInfo { Path = propPath, Object = propValue }
                };
                parentNode.Nodes.Add(propNode);

                BuildTreeNodes(propNode, propValue, propPath);
            }
        }
        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // 确定绘制模式
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                // 选中状态
                e.Graphics.FillRectangle(Brushes.DarkBlue, e.Bounds);
                e.Graphics.DrawString(e.Node.Text, new Font("Segoe UI", 14, FontStyle.Bold), Brushes.White, e.Bounds.X + 2, e.Bounds.Y + 2);
            }
            else
            {
                // 未选中状态
                e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
                e.Graphics.DrawString(e.Node.Text, new Font("Segoe UI", 9), Brushes.Black, e.Bounds.X + 2, e.Bounds.Y + 2);
            }

            // 如果节点有图像，绘制图像
            //if (e.Node.ImageIndex != -1)
            //{
            //    Image image = imageList.Images[e.Node.ImageIndex];
            //    e.Graphics.DrawImage(image, e.Bounds.X, e.Bounds.Y, image.Width, image.Height);
            //}
        }

        private void AddDataBinding(Control ctrl, object dataSource, string propertyName)
        {
            if (ctrl is CheckBox checkBox)
            {
                checkBox.DataBindings.Add("Checked", dataSource, propertyName,
                    false, DataSourceUpdateMode.OnPropertyChanged);
            }
            else if (ctrl is TextBox textBox)
            {
                textBox.DataBindings.Add("Text", dataSource, propertyName,
                    false, DataSourceUpdateMode.OnPropertyChanged);
            }
            else if (ctrl is NumericUpDown numericUpDown)
            {
                numericUpDown.DataBindings.Add("Value", dataSource, propertyName,
                    false, DataSourceUpdateMode.OnPropertyChanged);
            }
            else if (ctrl is DateTimePicker dateTimePicker)
            {
                dateTimePicker.DataBindings.Add("Value", dataSource, propertyName,
                    false, DataSourceUpdateMode.OnPropertyChanged);
            }
            else if (ctrl is ComboBox comboBox)
            {
                comboBox.DataBindings.Add("SelectedItem", dataSource, propertyName,
                    false, DataSourceUpdateMode.OnPropertyChanged);
            }
            else if (ctrl is Panel pathPanel)
            {
                var textBox2 = pathPanel.Controls.OfType<TextBox>().FirstOrDefault();
                if (textBox2 != null)
                {
                    textBox2.DataBindings.Add("Text", dataSource, propertyName,
                        false, DataSourceUpdateMode.OnPropertyChanged);
                }
            }
        }
        // Config Panel refresh
        private void ConfigTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string treePath = (e.Node.Tag as NodeInfo).Path;
            //Console.WriteLine($"{treePath}");
            LoadNodeConfiguration(e.Node);
        }
        private void LoadNodeConfiguration(TreeNode node)
        {
            configPanel.Controls.Clear();

            if (node == null) return;
            NodeInfo nodeInfo = node.Tag as NodeInfo;
            if (nodeInfo == null) return;

            var obj = nodeInfo.Object;
            if (obj == null) return;

            int topPos = 10;

            // 添加标题
            var titleLabel = new Label
            {
                Text = $"{node.Text} 配置",
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold),
                AutoSize = true,
                Top = topPos,
                Left = 10
            };
            configPanel.Controls.Add(titleLabel);
            topPos += 40;

            // 处理集合类型
            if (IsCollectionType(obj.GetType()))
            {
                var addButton = new Button
                {
                    Text = "Add",
                    Top = topPos,
                    Left = 10,
                    Width = 120
                };
                addButton.Click += (s, e) => AddCollectionElement(node, obj);
                configPanel.Controls.Add(addButton);
                topPos += 40;
            }

            // 生成配置控件
            GenerateControlsForObject(obj, configPanel, ref topPos);
        }
        private void GenerateControlsForObject_bk(object obj, Control container, ref int topPos, int indentLevel = 0)
        {
            if (obj == null) return;

            var type = obj.GetType();
            int leftPos = 10 + indentLevel * 20;

            // 处理集合类型
            if (IsCollectionType(type))
            {
                int index = 0;
                foreach (var item in (IEnumerable)obj)
                {
                    var group = new GroupBox
                    {
                        Text = $"元素 {index}",
                        Top = topPos,
                        Left = leftPos,
                        Width = container.Width - leftPos - 25,
                        Height = 120
                    };

                    var innerPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true
                    };

                    int innerTop = 20;
                    GenerateControlsForObject(item, innerPanel, ref innerTop, indentLevel + 1);

                    var removeButton = new Button
                    {
                        Text = "删除",
                        Top = 10,
                        Height = 20,
                        Left = group.Width - 80,
                        Width = 70
                    };
                    removeButton.Click += (s, e) => RemoveCollectionElement(obj, index);

                    group.Controls.Add(removeButton);
                    group.Controls.Add(innerPanel);
                    container.Controls.Add(group);

                    topPos += group.Height + 10;
                    index++;
                }
                return;
            }

            // 处理嵌套对象
            foreach (var prop in type.GetProperties())
            {
                // 简单类型属性
                if (IsSimpleType(prop.PropertyType))
                {
                    var lbl = new Label
                    {
                        Text = GetDisplayName(prop),
                        Top = topPos,
                        Left = leftPos,
                        Width = 150
                    };

                    var ctrl = CreateControlForType(prop, prop.GetValue(obj));
                    ctrl.Top = topPos;
                    ctrl.Left = leftPos + 160;
                    //ctrl.Width = 250;
                    ctrl.Tag = prop;

                    //Add databinding
                    AddDataBinding(ctrl, obj, prop.Name);
                    container.Controls.Add(lbl);
                    container.Controls.Add(ctrl);

                    topPos += ctrl.Height + 10;
                }
                // 注释掉下面处理嵌套对象属性的代码，只在TreeNode被选择之后再去生成，否则界面很乱。2025-6-3
                else if (!IsCollectionType(prop.PropertyType))
                {
                    //var group = new GroupBox
                    //{
                    //    Text = prop.Name,
                    //    Top = topPos,
                    //    Left = leftPos,
                    //    Width = container.Width - leftPos - 25,
                    //    //Width = container.Width - leftPos,
                    //    Height = 150
                    //};

                    //var innerPanel = new Panel
                    //{
                    //    Dock = DockStyle.Fill,
                    //    AutoScroll = true
                    //};

                    //int innerTop = 10;
                    //GenerateControlsForObject(prop.GetValue(obj), innerPanel, ref innerTop, indentLevel + 1);

                    //group.Controls.Add(innerPanel);
                    //container.Controls.Add(group);
                    ////container.Name.Dump();
                    //topPos += group.Height + 10;
                }
            }
        }
        private void GenerateControlsForObject(object obj, Control container, ref int topPos, int indentLevel = 0)
        {
            if (obj == null) return;

            var type = obj.GetType();
            int leftPos = 10 + indentLevel * 20;

            // 处理集合类型
            if (IsCollectionType(type))
            {
                //var bindingList = obj as IBindingList;
                //if (bindingList != null)
                //{
                //    // 添加新元素按钮
                //    var addButton = new Button
                //    {
                //        Text = "添加新元素-Gen",
                //        Top = topPos,
                //        Left = 10,
                //        Width = 120
                //    };
                //    addButton.Click += (s, e) => AddCollectionElement(bindingList);
                //    container.Controls.Add(addButton);
                //    topPos += 40;
                //}

                int index = 0;
                foreach (var item in (IEnumerable)obj)
                {
                    var group = new GroupBox
                    {
                        Text = $"#{index + 1}",
                        Top = topPos,
                        Left = leftPos,
                        Width = container.Width - leftPos - 25,
                        Height = 120
                    };

                    var innerPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true
                    };

                    int innerTop = 20;
                    GenerateControlsForObject(item, innerPanel, ref innerTop, indentLevel + 1);

                    var removeButton = new Button
                    {
                        Text = "删除",
                        Top = 10,
                        Height = 20,
                        Left = group.Width - 80,
                        Width = 70
                    };
                    int removeIndex = index;
                    removeButton.Click += (s, e) => RemoveCollectionElement(obj, removeIndex);

                    group.Controls.Add(removeButton);
                    group.Controls.Add(innerPanel);
                    container.Controls.Add(group);

                    topPos += group.Height + 10;
                    index++;
                }
                return;
            }

            // 处理嵌套对象
            foreach (var prop in type.GetProperties())
            {
                // 简单类型属性
                if (IsSimpleType(prop.PropertyType))
                {
                    var lbl = new Label
                    {
                        Text = GetDisplayName(prop),
                        Top = topPos,
                        Left = leftPos,
                        Width = 150
                    };

                    var ctrl = CreateControlForType(prop, prop.GetValue(obj));
                    ctrl.Top = topPos;
                    ctrl.Left = leftPos + 160;
                    //ctrl.Width = 250;
                    ctrl.Tag = prop;

                    //Add databinding
                    AddDataBinding(ctrl, obj, prop.Name);
                    container.Controls.Add(lbl);
                    container.Controls.Add(ctrl);

                    topPos += ctrl.Height + 10;
                }

            }
        }
        private void AddCollectionElement(IBindingList list)
        {   // for databinding
            // 要在GenerateControlsForObject中使用，但是尚未启用2025-6-3
            Type elementType = GetCollectionElementType(list.GetType());
            object newItem = CreateDefaultInstance(elementType);
            list.Add(newItem);
        }
        private void AddCollectionElement(TreeNode parentNode, object collection)
        {
            if (collection is IList list)
            {
                Type elementType = GetCollectionElementType(collection.GetType());
                object newItem = CreateDefaultInstance(elementType);
                list.Add(newItem);
                // 刷新树形视图
                BuildConfigTree();
                // 展开父节点
                parentNode.Expand();
                // 选中新添加的元素
                //TreeNode newNode = parentNode.Nodes[list.Count - 1];
                configTree.SelectedNode = newItem as TreeNode;
                LoadNodeConfiguration(configTree.SelectedNode);
            }
        }
        private void RemoveCollectionElement(object collection, int index)
        {
            //collection.Dump();
            //index.Dump();
            if (collection is IList list && index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
                BuildConfigTree();
                LoadNodeConfiguration(configTree.SelectedNode);
            }
        }

        // 辅助方法
        private bool IsSimpleType(Type type)
        {
            return type.IsPrimitive ||
                    type == typeof(string) ||
                    type.IsEnum ||
                    type == typeof(DateTime);
        }
        private bool IsCollectionType(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }
        private Type GetCollectionElementType(Type collectionType)
        {
            if (collectionType.IsArray)
                return collectionType.GetElementType();

            if (collectionType.IsGenericType)
                return collectionType.GetGenericArguments()[0];

            return typeof(object);
        }
        private object CreateDefaultInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }
        private string GetDisplayName(PropertyInfo prop)
        {
            var attr = prop.GetCustomAttribute<DisplayNameAttribute>();
            return attr?.DisplayName ?? prop.Name;
        }

        private Control CreateControlForType(PropertyInfo prop, object value)
        {
            Type type = prop.PropertyType;
            //type.Dump($"{value}");
            if (type == typeof(bool))
                return new CheckBox { Checked = (bool)(value ?? false) };

            //if (type.IsEnum)
            //{
            //    var cmb = new ComboBox
            //    {
            //        DropDownStyle = ComboBoxStyle.DropDownList,
            //        DataSource = Enum.GetValues(type)
            //    };
            //    if (value != null) cmb.SelectedItem = value;
            //    return cmb;
            //}
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
            if (type == typeof(int) || type == typeof(double) || type == typeof(decimal))
                return new NumericUpDown { Value = Convert.ToDecimal(value ?? 0) };

            if (type == typeof(DateTime))
                return new DateTimePicker { Value = (DateTime)(value ?? DateTime.Now) };

            if (type == typeof(string))
            {
                if (prop.GetCustomAttribute<PathSelectorAttribute>() != null)
                {
                    //$"generating pathselector for: {value?.ToString()}".Dump();
                    return CreatePathSelector(prop, value?.ToString());
                }

            }

            return new TextBox { Text = value?.ToString(), Width = 450, };
        }
        private Control CreatePathSelector(PropertyInfo prop, string initialPath)
        {
            var panel = new Panel { Height = 20, Width = 600 };
            var txtPath = new TextBox
            {
                Text = initialPath,
                Width = 450,
                Dock = DockStyle.Left,
                Tag = prop,
            };
            txtPath.SelectionStart = txtPath.Text.Length;
            txtPath.ScrollToCaret();
            txtPath.TextChanged
                += (s, e) =>
                {
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
            WinFormFormatters.FormatButtonsAsBootstrapInfo(new Button[] { btnBrowse });
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

        public void ApplyChanges()
        {
            _configService.Save(_configFile, (ConfigBase)_configObject);
            MessageBox.Show("Saved.");
        }

        private class NodeInfo
        {
            public string Path { get; set; }
            public object Object { get; set; }
        }
    }
}
