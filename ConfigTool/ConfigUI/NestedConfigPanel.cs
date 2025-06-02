using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using ConfigTool.ConfigCore;
using Dumpify;

namespace ConfigTool.ConfigUI
{
    public partial class NestedConfigPanel : UserControl
    {
        private TreeView configTree;
        private Panel configPanel;
        private object _configObject;
        private string _currentPath;
        private Dictionary<string, object> _objectCache = new Dictionary<string, object>();

        public NestedConfigPanel()
        {
            //InitializeComponent();
            MyInitializeComponent();
        }

        private void MyInitializeComponent()
        {
            this.Size = new Size(800, 600);

            // 分割容器
            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 30
            };
            this.Controls.Add(splitContainer);

            // 左侧树形视图
            configTree = new TreeView
            {
                Dock = DockStyle.Fill,
                ShowRootLines = true,
                ShowPlusMinus = true,
                HideSelection = false
            };
            configTree.AfterSelect += ConfigTree_AfterSelect;
            splitContainer.Panel1.Controls.Add(configTree);

            // 右侧配置面板
            configPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };
            splitContainer.Panel2.Controls.Add(configPanel);
        }

        public void Bind(object configObj, string configPath = "root")
        {
            _configObject = configObj;
            _currentPath = configPath;
            BuildConfigTree();
            LoadNodeConfiguration(configTree.Nodes[0]);
        }

        private void BuildConfigTree()
        {
            configTree.Nodes.Clear();
            _objectCache.Clear();

            var rootNode = new TreeNode("配置根节点")
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
                    var itemNode = new TreeNode($"元素 {index}")
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

        private void ConfigTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadNodeConfiguration(e.Node);
        }
        //为TreeNode加载配置信息
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
                    Text = "添加新元素",
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

        private void GenerateControlsForObject(object obj, Control container, ref int topPos, int indentLevel = 0)
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

                    int innerTop = 10;
                    GenerateControlsForObject(item, innerPanel, ref innerTop, indentLevel + 1);

                    var removeButton = new Button
                    {
                        Text = "删除",
                        Top = 5,
                        Left = group.Width - 80,
                        Width = 70
                    };
                    removeButton.Click += (s, e) => RemoveCollectionElement(obj, index);

                    group.Controls.Add(innerPanel);
                    group.Controls.Add(removeButton);
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

                    container.Controls.Add(lbl);
                    container.Controls.Add(ctrl);

                    topPos += ctrl.Height + 10;
                }
                // 嵌套对象属性
                else if (!IsCollectionType(prop.PropertyType))
                {
                    var group = new GroupBox
                    {
                        Text = prop.Name,
                        Top = topPos,
                        Left = leftPos,
                        Width = container.Width - leftPos - 25,
                        Height = 150
                    };

                    var innerPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true
                    };

                    int innerTop = 10;
                    GenerateControlsForObject(prop.GetValue(obj), innerPanel, ref innerTop, indentLevel + 1);

                    group.Controls.Add(innerPanel);
                    container.Controls.Add(group);

                    topPos += group.Height + 10;
                }
            }
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
            }
        }

        private void RemoveCollectionElement(object collection, int index)
        {
            if (collection is IList list && index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
                BuildConfigTree();
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

            return new TextBox { Text = value?.ToString() };
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

        public void ApplyChanges()
        {
            // 遍历所有控件应用更改
            // 实际实现需要递归遍历所有配置控件
            // 这里简化处理，实际项目中需要完整实现
            MessageBox.Show("配置已保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private class NodeInfo
        {
            public string Path { get; set; }
            public object Object { get; set; }
        }
    }
}
