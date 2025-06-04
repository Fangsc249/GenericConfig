using ConfigTool.ConfigCore;
using System.Windows.Forms;

namespace ConfigTool.ConfigUI
{
    public partial class TabedPanelForm : Form
    {
        private readonly IConfigService _configService;
        private readonly string _configPath;
        private readonly object _configObject;

        private TableLayoutPanel tableLayoutPanel1;
        private TabedPanel dynamicPanel2;
        private GroupBox groupBox1;
        private Button btnCancel;
        private Button btnSave;


        public TabedPanelForm(IConfigService configService, string configPath, object configObj)
        {
            InitializeComponent();
            
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            _configService = configService;
            _configPath = configPath;
            _configObject = configObj;
            configPath = System.IO.Path.GetFullPath(configPath);
            Text = $"Configuraion - {configPath}";
            dynamicPanel2.Bind(configObj);
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            dynamicPanel2.ApplyChanges();
            _configService.Save(_configPath, (ConfigBase)_configObject);
            this.DialogResult = DialogResult.OK; // 标准成功结果
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabedPanelForm));
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.dynamicPanel2 = new TabedPanel();
            this.groupBox1 = new GroupBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.dynamicPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 600F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(832, 666);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dynamicPanel2
            // 
            this.dynamicPanel2.AutoScroll = true;
            this.dynamicPanel2.BackColor = System.Drawing.Color.Azure;
            this.dynamicPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicPanel2.Location = new System.Drawing.Point(3, 3);
            this.dynamicPanel2.Name = "dynamicPanel2";
            this.dynamicPanel2.Size = new System.Drawing.Size(826, 594);
            this.dynamicPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 603);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(826, 60);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(432, 21);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(139, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(577, 21);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            this.ClientSize = new System.Drawing.Size(832, 666);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
