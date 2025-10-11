using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConfigTool.ConfigUI
{
    public class WinFormFormatters
    {
        /// <summary>
        /// 格式化Label控件以显示警告信息
        /// </summary>
        /// <param name="label">要格式化的Label控件</param>
        /// <param name="warningText">警告文本</param>
        public static void FormatLabelAsWarning(Label label, string warningText)
        {

            // 设置警告样式
            label.Text = warningText;
            label.ForeColor = Color.Red; // 白色文字更醒目
            //label.BackColor = Color.FromArgb(23, 162, 184); // 红色背景表示警告[3](@ref)[12](@ref)
            label.Font = new Font(label.Font.FontFamily, 12f, FontStyle.Regular); // 加粗字体[2](@ref)[15](@ref)
            //label.BorderStyle = BorderStyle.FixedSingle; // 添加边框[14](@ref)
            label.TextAlign = ContentAlignment.MiddleCenter; // 文本居中[5](@ref)
            label.AutoSize = true; // 禁用自动调整大小[15](@ref)
            label.Height = 50;
            //label.Size = new Size(300, 40); // 固定大小[15](@ref)
            //label.Padding = new Padding(10); // 添加内边距[5](@ref)
        }
        /// <summary>
        /// 将Label控件恢复为普通消息样式
        /// </summary>
        /// <param name="label">要复原的Label控件</param>
        public static void FormatLabelAsNormal(Label label, string normalText)
        {
            label.BackColor = Color.Transparent; // 透明背景[13](@ref)
            label.ForeColor = SystemColors.ControlText;
            label.Font = new Font("Microsoft Sans Serif", 8.25f);
            label.BorderStyle = BorderStyle.None;
            label.AutoSize = true;
            label.Text = normalText;
        }

        public static void FormatButtonsAsBootstrapPrimary(params Button[] buttons)
        {
            // 1. 定义Bootstrap Primary颜色（深蓝色，RGB: 0, 123, 255）
            Color bootstrapPrimary = Color.FromArgb(0, 123, 255); // 对应Bootstrap的#007bff

            // 2. 遍历按钮数组并设置样式
            foreach (Button btn in buttons)
            {
                // 基础样式
                btn.BackColor = bootstrapPrimary;
                btn.ForeColor = Color.White;
                btn.Font = new Font(btn.Font, FontStyle.Bold);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                // 扩展1：悬停效果（深色）
                //btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 86, 179); // 悬停时颜色加深[6](@ref)[8](@ref)
                //btn.MouseLeave += (s, e) => btn.BackColor = bootstrapPrimary;

                // 扩展2：禁用状态样式（灰色但保留文字可读性）
                btn.EnabledChanged += (s, e) =>
                {
                    if (!btn.Enabled)
                    {
                        btn.BackColor = Color.Gray;
                        btn.ForeColor = Color.White; // 保持文字可见[9](@ref)[11](@ref)
                    }
                    else
                    {
                        btn.BackColor = bootstrapPrimary;
                        btn.ForeColor = Color.White;
                    }
                };
            }
        }
        public static void FormatButtonsAsBootstrapSecondary(params Button[] buttons)
        {
            // 1. 定义Bootstrap Secondary风格颜色（参考Bootstrap官方色值[8](@ref)[9](@ref)）
            Color secondaryColor = Color.FromArgb(108, 117, 125);   // Bootstrap Secondary灰色 (#6c757d)
            Color hoverColor = Color.FromArgb(92, 99, 106);          // 悬停深灰色（原色加深10%）
            Color disabledColor = Color.FromArgb(233, 236, 239);    // Bootstrap禁用背景色 (#e9ecef)
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Regular);

            // 2. 遍历Button数组并设置样式
            foreach (Button button in buttons)
            {
                // 基础样式（扁平化设计[2](@ref)[5](@ref)）
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = secondaryColor;
                button.ForeColor = Color.White;                     // 白色文字提升对比度[9](@ref)
                button.Font = buttonFont;
                button.FlatAppearance.BorderSize = 0;               // 移除边框
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Padding = new Padding(12, 0, 12, 0);          // 水平内边距（模仿Bootstrap的左右padding）

                // 3. 动态交互效果（悬停/点击/聚焦[6](@ref)[12](@ref)）
                button.MouseEnter += (s, e) => button.BackColor = hoverColor;
                button.MouseLeave += (s, e) => button.BackColor = secondaryColor;
                button.MouseDown += (s, e) =>
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 255, 255, 255); // 半透明白色点击效果
                button.MouseUp += (s, e) => button.BackColor = hoverColor;
                button.GotFocus += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(150, 150, 150); // 聚焦时灰色边框
                button.LostFocus += (s, e) => button.FlatAppearance.BorderColor = Color.Transparent;

                // 4. 禁用状态样式（参考Bootstrap规范[3](@ref)[7](@ref)）
                button.EnabledChanged += (s, e) =>
                {
                    if (!button.Enabled)
                    {
                        button.BackColor = disabledColor;
                        button.ForeColor = Color.FromArgb(108, 117, 125); // 与原色一致的灰色文字
                    }
                    else
                    {
                        button.BackColor = secondaryColor;
                        button.ForeColor = Color.White;
                    }
                };

                // 5. 自定义绘制圆角（可选，模仿Bootstrap的轻微圆角[5](@ref)[12](@ref)）
                button.Paint += (s, e) =>
                {
                    // 绘制圆角背景（半径4px）
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 4;
                        path.AddArc(0, 0, radius, radius, 180, 90);
                        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
                        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
                        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
                        path.CloseFigure();
                        e.Graphics.FillPath(new SolidBrush(button.BackColor), path);
                    }

                    // 绘制文本（居中且带内边距）
                    TextRenderer.DrawText(
                        e.Graphics,
                        button.Text,
                        button.Font,
                        new Rectangle(button.Padding.Left, 0, button.Width - button.Padding.Horizontal, button.Height),
                        button.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                };
            }
        }
        public static void FormatButtonsAsBootstrapSuccess(params Button[] buttons)
        {
            // 1. 定义Bootstrap Success风格颜色
            Color successColor = Color.FromArgb(40, 167, 69);   // Bootstrap Success绿色 (#28a745)
            Color hoverColor = Color.FromArgb(33, 136, 56);      // 悬停深绿色
            Color disabledColor = Color.FromArgb(200, 200, 200); // 禁用灰色
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Bold);

            // 2. 遍历Button数组并设置样式
            foreach (Button button in buttons)
            {
                // 基础样式
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = successColor;
                button.ForeColor = Color.White;
                button.Font = buttonFont;
                button.FlatAppearance.BorderSize = 0;            // 移除边框
                button.TextAlign = ContentAlignment.MiddleCenter;

                // 3. 动态交互效果
                button.MouseEnter += (s, e) => button.BackColor = hoverColor;
                button.MouseLeave += (s, e) => button.BackColor = successColor;
                button.MouseDown += (s, e) => button.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 255, 255, 255); // 半透明白色点击效果
                button.MouseUp += (s, e) => button.BackColor = hoverColor;

                // 4. 禁用状态样式
                button.EnabledChanged += (s, e) =>
                {
                    if (!button.Enabled)
                    {
                        button.BackColor = disabledColor;
                        button.ForeColor = Color.FromArgb(100, 100, 100);
                    }
                    else
                    {
                        button.BackColor = successColor;
                        button.ForeColor = Color.White;
                    }
                };

                // 5. 自定义绘制圆角（可选）
                button.Paint += (s, e) =>
                {
                    // 绘制圆角背景
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 4; // 圆角半径
                        path.AddArc(0, 0, radius, radius, 180, 90);
                        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
                        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
                        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
                        path.CloseFigure();
                        e.Graphics.FillPath(new SolidBrush(button.BackColor), path);
                    }

                    // 绘制文本（居中）
                    TextRenderer.DrawText(
                        e.Graphics,
                        button.Text,
                        button.Font,
                        new Rectangle(0, 0, button.Width, button.Height),
                        button.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                };
            }
        }
        public static void FormatButtonsAsBootstrapDanger(params Button[] buttons)
        {
            // 1. 定义Bootstrap Danger风格颜色（参考Bootstrap官方色值）
            Color dangerColor = Color.FromArgb(220, 53, 69);    // Bootstrap Danger红色 (#dc3545)
            Color hoverColor = Color.FromArgb(200, 35, 51);     // 悬停深红色（原色加深10%）
            Color disabledColor = Color.FromArgb(233, 236, 239); // Bootstrap禁用背景色 (#e9ecef)
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Bold);

            // 2. 遍历Button数组并设置样式
            foreach (Button button in buttons)
            {
                // 基础样式（扁平化设计）
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = dangerColor;
                button.ForeColor = Color.White;                  // 白色文字提升对比度
                button.Font = buttonFont;
                button.FlatAppearance.BorderSize = 0;           // 移除边框
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Padding = new Padding(12, 0, 12, 0);      // 水平内边距（模仿Bootstrap的左右padding）

                // 3. 动态交互效果（悬停/点击/聚焦）
                button.MouseEnter += (s, e) => button.BackColor = hoverColor;
                button.MouseLeave += (s, e) => button.BackColor = dangerColor;
                button.MouseDown += (s, e) =>
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 255, 255, 255); // 半透明白色点击效果
                button.MouseUp += (s, e) => button.BackColor = hoverColor;
                button.GotFocus += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(200, 35, 51); // 聚焦时深红色边框
                button.LostFocus += (s, e) => button.FlatAppearance.BorderColor = Color.Transparent;

                // 4. 禁用状态样式（参考Bootstrap规范）
                button.EnabledChanged += (s, e) =>
                {
                    if (!button.Enabled)
                    {
                        button.BackColor = disabledColor;
                        button.ForeColor = Color.FromArgb(108, 117, 125); // 与原色一致的灰色文字
                    }
                    else
                    {
                        button.BackColor = dangerColor;
                        button.ForeColor = Color.White;
                    }
                };

                // 5. 自定义绘制圆角（可选，模仿Bootstrap的轻微圆角）
                button.Paint += (s, e) =>
                {
                    // 绘制圆角背景（半径4px）
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 4;
                        path.AddArc(0, 0, radius, radius, 180, 90);
                        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
                        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
                        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
                        path.CloseFigure();
                        e.Graphics.FillPath(new SolidBrush(button.BackColor), path);
                    }

                    // 绘制文本（居中且带内边距）
                    TextRenderer.DrawText(
                        e.Graphics,
                        button.Text,
                        button.Font,
                        new Rectangle(button.Padding.Left, 0, button.Width - button.Padding.Horizontal, button.Height),
                        button.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                };
            }
        }
        public static void FormatButtonsAsBootstrapWarning(params Button[] buttons)
        {
            // 1. 定义Bootstrap Warning风格颜色（参考Bootstrap官方色值）
            Color warningColor = Color.FromArgb(255, 193, 7);    // Bootstrap Warning黄色 (#ffc107)
            Color hoverColor = Color.FromArgb(230, 174, 6);       // 悬停深黄色（原色加深10%）
            Color disabledColor = Color.FromArgb(233, 236, 239);  // Bootstrap禁用背景色 (#e9ecef)
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Bold);

            // 2. 遍历Button数组并设置样式
            foreach (Button button in buttons)
            {
                // 基础样式（扁平化设计）
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = warningColor;
                button.ForeColor = Color.Black;                   // 黑色文字提升对比度（Bootstrap规范）
                button.Font = buttonFont;
                button.FlatAppearance.BorderSize = 0;            // 移除边框
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Padding = new Padding(12, 0, 12, 0);       // 水平内边距（模仿Bootstrap的左右padding）

                // 3. 动态交互效果（悬停/点击/聚焦）
                button.MouseEnter += (s, e) => button.BackColor = hoverColor;
                button.MouseLeave += (s, e) => button.BackColor = warningColor;
                button.MouseDown += (s, e) =>
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 0, 0, 0); // 半透明黑色点击效果
                button.MouseUp += (s, e) => button.BackColor = hoverColor;
                button.GotFocus += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(200, 174, 6); // 聚焦时深黄色边框
                button.LostFocus += (s, e) => button.FlatAppearance.BorderColor = Color.Transparent;

                // 4. 禁用状态样式（参考Bootstrap规范）
                button.EnabledChanged += (s, e) =>
                {
                    if (!button.Enabled)
                    {
                        button.BackColor = disabledColor;
                        button.ForeColor = Color.FromArgb(108, 117, 125); // 灰色文字
                    }
                    else
                    {
                        button.BackColor = warningColor;
                        button.ForeColor = Color.Black;
                    }
                };

                // 5. 自定义绘制圆角（模仿Bootstrap的轻微圆角）
                button.Paint += (s, e) =>
                {
                    // 绘制圆角背景（半径4px）
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 4;
                        path.AddArc(0, 0, radius, radius, 180, 90);
                        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
                        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
                        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
                        path.CloseFigure();
                        e.Graphics.FillPath(new SolidBrush(button.BackColor), path);
                    }

                    // 绘制文本（居中且带内边距）
                    TextRenderer.DrawText(
                        e.Graphics,
                        button.Text,
                        button.Font,
                        new Rectangle(button.Padding.Left, 0, button.Width - button.Padding.Horizontal, button.Height),
                        button.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                };
            }
        }
        public static void FormatButtonsAsBootstrapInfo(params Button[] buttons)
        {
            // 1. 定义Bootstrap Info风格颜色（参考Bootstrap 5官方色值）
            Color infoColor = Color.FromArgb(23, 162, 184);    // Bootstrap Info青色 (#17a2b8)
            Color hoverColor = Color.FromArgb(19, 132, 150);   // 悬停深青色（原色加深10%）
            Color disabledColor = Color.FromArgb(233, 236, 239); // Bootstrap禁用背景色 (#e9ecef)
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Regular);

            // 2. 遍历Button数组并设置样式
            foreach (Button button in buttons)
            {
                // 基础样式（扁平化设计）
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = infoColor;
                button.ForeColor = Color.White;                  // 白色文字提升对比度
                button.Font = buttonFont;
                button.FlatAppearance.BorderSize = 0;           // 移除边框
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Padding = new Padding(12, 0, 12, 0);      // 水平内边距（模仿Bootstrap的左右padding）

                // 3. 动态交互效果（悬停/点击/聚焦）
                button.MouseEnter += (s, e) => button.BackColor = hoverColor;
                button.MouseLeave += (s, e) => button.BackColor = infoColor;
                button.MouseDown += (s, e) =>
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 255, 255, 255); // 半透明白色点击效果
                button.MouseUp += (s, e) => button.BackColor = hoverColor;
                button.GotFocus += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(100, 200, 220); // 聚焦时浅蓝色边框
                //button.LostFocus += (s, e) => button.FlatAppearance.BorderColor = Color.Transparent;

                // 4. 禁用状态样式（参考Bootstrap规范）
                button.EnabledChanged += (s, e) =>
                {
                    if (!button.Enabled)
                    {
                        button.BackColor = disabledColor;
                        button.ForeColor = Color.FromArgb(108, 117, 125); // 灰色文字
                    }
                    else
                    {
                        button.BackColor = infoColor;
                        button.ForeColor = Color.White;
                    }
                };

                // 5. 自定义绘制圆角（模仿Bootstrap的轻微圆角）
                button.Paint += (s, e) =>
                {
                    // 绘制圆角背景（半径4px）
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 4;
                        path.AddArc(0, 0, radius, radius, 180, 90);
                        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
                        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
                        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
                        path.CloseFigure();
                        e.Graphics.FillPath(new SolidBrush(button.BackColor), path);
                    }

                    // 绘制文本（居中且带内边距）
                    TextRenderer.DrawText(
                        e.Graphics,
                        button.Text,
                        button.Font,
                        new Rectangle(button.Padding.Left, 0, button.Width - button.Padding.Horizontal, button.Height),
                        button.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                };
            }
        }
        public static void FormatButtonsAsBootstrapLight(params Button[] buttons)
        {
            // 1. 定义Bootstrap Light风格颜色（参考Bootstrap 5官方色值）
            Color lightColor = Color.FromArgb(248, 249, 250);   // Bootstrap Light浅灰色 (#f8f9fa)
            Color hoverColor = Color.FromArgb(233, 236, 239);   // 悬停深灰色（Bootstrap禁用背景色 #e9ecef）
            Color disabledColor = Color.FromArgb(222, 226, 230); // 禁用状态灰色 (#dee2e6)
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Regular);

            // 2. 遍历Button数组并设置样式
            foreach (Button button in buttons)
            {
                // 基础样式（扁平化设计）
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = lightColor;
                button.ForeColor = Color.FromArgb(73, 80, 87);    // Bootstrap深灰色文字 (#495057)
                button.Font = buttonFont;
                button.FlatAppearance.BorderSize = 1;           // 浅灰色边框（模仿Bootstrap Light的边框）
                button.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230); // #dee2e6
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Padding = new Padding(12, 0, 12, 0);      // 水平内边距（模仿Bootstrap的左右padding）

                // 3. 动态交互效果（悬停/点击/聚焦）
                button.MouseEnter += (s, e) =>
                {
                    button.BackColor = hoverColor;
                    button.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218); // 悬停时边框加深 (#ced4da)
                };
                button.MouseLeave += (s, e) =>
                {
                    button.BackColor = lightColor;
                    button.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
                };
                button.MouseDown += (s, e) =>
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 0, 0, 0); // 半透明黑色点击效果
                button.MouseUp += (s, e) => button.BackColor = hoverColor;
                button.GotFocus += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(108, 117, 125); // 聚焦时灰色边框 (#6c757d)
                button.LostFocus += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);

                // 4. 禁用状态样式（参考Bootstrap规范[7](@ref)）
                button.EnabledChanged += (s, e) =>
                {
                    if (!button.Enabled)
                    {
                        button.BackColor = disabledColor;
                        button.ForeColor = Color.FromArgb(173, 181, 189); // 浅灰色文字 (#adb5bd)
                    }
                    else
                    {
                        button.BackColor = lightColor;
                        button.ForeColor = Color.FromArgb(73, 80, 87);
                    }
                };

                // 5. 自定义绘制圆角（模仿Bootstrap的轻微圆角）
                button.Paint += (s, e) =>
                {
                    // 绘制圆角背景（半径4px）
                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    //using ()
                    //{
                    int radius = 4;
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();
                    e.Graphics.FillPath(new SolidBrush(button.BackColor), path);
                    //}

                    // 绘制边框（独立于背景）
                    using (var borderPen = new Pen(button.FlatAppearance.BorderColor, 1))
                    {
                        e.Graphics.DrawPath(borderPen, path);
                    }

                    // 绘制文本（居中且带内边距）
                    TextRenderer.DrawText(
                        e.Graphics,
                        button.Text,
                        button.Font,
                        new Rectangle(button.Padding.Left, 0, button.Width - button.Padding.Horizontal, button.Height),
                        button.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                };
            }
        }
        public static void FormatButtonsAsBootstrapDark(params Button[] buttons)
        {
            // 1. 定义Bootstrap Dark风格颜色（参考Bootstrap 5官方色值）
            Color darkColor = Color.FromArgb(33, 37, 41);      // Bootstrap Dark深灰色 (#212529)
            Color hoverColor = Color.FromArgb(52, 58, 64);     // 悬停更深的灰色 (#343a40)
            Color disabledColor = Color.FromArgb(108, 117, 125); // Bootstrap禁用灰色 (#6c757d)
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Regular);

            // 2. 遍历Button数组并设置样式
            foreach (Button button in buttons)
            {
                // 基础样式（扁平化设计）
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = darkColor;
                button.ForeColor = Color.White;                // 白色文字提升对比度
                button.Font = buttonFont;
                button.FlatAppearance.BorderSize = 0;         // 移除边框
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Padding = new Padding(12, 0, 12, 0);    // 水平内边距（模仿Bootstrap的左右padding）

                // 3. 动态交互效果（悬停/点击/聚焦）
                button.MouseEnter += (s, e) => button.BackColor = hoverColor;
                button.MouseLeave += (s, e) => button.BackColor = darkColor;
                button.MouseDown += (s, e) =>
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 255, 255, 255); // 半透明白色点击效果
                button.MouseUp += (s, e) => button.BackColor = hoverColor;
                button.GotFocus += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(108, 117, 125); // 聚焦时灰色边框 (#6c757d)
                button.LostFocus += (s, e) => button.FlatAppearance.BorderColor = Color.Transparent;

                // 4. 禁用状态样式（参考Bootstrap规范）
                button.EnabledChanged += (s, e) =>
                {
                    if (!button.Enabled)
                    {
                        button.BackColor = disabledColor;
                        button.ForeColor = Color.FromArgb(222, 226, 230); // 浅灰色文字 (#dee2e6)
                    }
                    else
                    {
                        button.BackColor = darkColor;
                        button.ForeColor = Color.White;
                    }
                };

                // 5. 自定义绘制圆角（模仿Bootstrap的轻微圆角）
                button.Paint += (s, e) =>
                {
                    // 绘制圆角背景（半径4px）
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 4;
                        path.AddArc(0, 0, radius, radius, 180, 90);
                        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
                        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
                        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
                        path.CloseFigure();
                        e.Graphics.FillPath(new SolidBrush(button.BackColor), path);
                    }

                    // 绘制文本（居中且带内边距）
                    TextRenderer.DrawText(
                        e.Graphics,
                        button.Text,
                        button.Font,
                        new Rectangle(button.Padding.Left, 0, button.Width - button.Padding.Horizontal, button.Height),
                        button.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                };
            }
        }

        public static void FormatDataGridView(DataGridView[] dataGridViews)
        {
            foreach (DataGridView dgv in dataGridViews)
            {
                // 关闭系统默认视觉样式（必须设置，否则颜色可能不生效）
                dgv.EnableHeadersVisualStyles = false;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToOrderColumns = true;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dgv.BackgroundColor = Color.Gray;
                // 设置列标题背景色为浅蓝色
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                // 设置列标题文字颜色为白色
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Purple;
                // 设置列标题字体样式
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Regular);
                // 设置列标题文字居中对齐
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // 设置行标题背景色为橙色
                dgv.RowHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                // 设置行标题文字颜色为黑色
                dgv.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgv.RowHeadersWidth = 25;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
                // 设置默认行背景色（偶数行）
                dgv.RowsDefaultCellStyle.BackColor = Color.White;
                // 设置交替行背景色（奇数行）
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            }
        }
        public static void FormatGroupBoxesAsBootstrap(GroupBox[] groupBoxes)
        {
            // 1. 定义Bootstrap风格颜色
            Color borderColor = Color.FromArgb(206, 212, 218); // Bootstrap默认边框色 (#ced4da)[8](@ref)
            Color titleColor = Color.FromArgb(0, 123, 255);    // Bootstrap Primary蓝色 (#007bff)[8](@ref)
            Font titleFont = new Font("Segoe UI", 10, FontStyle.Bold); // 接近Bootstrap标题字体

            // 2. 遍历GroupBox数组并设置样式
            foreach (GroupBox groupBox in groupBoxes)
            {
                // 基础样式
                groupBox.FlatStyle = FlatStyle.Flat; // 扁平化风格[8](@ref)
                groupBox.BackColor = Color.White;
                groupBox.ForeColor = titleColor;     // 标题颜色
                //groupBox.Font = titleFont;

                // 自定义边框（需继承GroupBox重写OnPaint）[7](@ref)[9](@ref)
                groupBox.Paint += (s, e) =>
                {
                    // 绘制圆角边框（模仿Bootstrap的柔和边框）
                    using (Pen borderPen = new Pen(borderColor, 1))
                    {
                        e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 8, groupBox.Width - 1, groupBox.Height - 10));
                    }
                    // 绘制标题背景（可选）
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(10, 0, TextRenderer.MeasureText(groupBox.Text, titleFont).Width + 10, 20));
                    e.Graphics.DrawString(groupBox.Text, titleFont, new SolidBrush(titleColor), new PointF(15, 0));
                };

                // 扩展：动态调整内部控件边距（模仿Bootstrap的间距）
                foreach (Control control in groupBox.Controls)
                {
                    control.Margin = new Padding(10, 5, 10, 5); // 统一内边距[10](@ref)
                }
            }
        }
        public static void FormatComboBoxesAsBootstrap(ComboBox[] comboBoxes)
        {
            // 1. 定义Bootstrap风格颜色
            Color borderColor = Color.FromArgb(206, 212, 218); // Bootstrap默认边框色 (#ced4da)[12](@ref)
            Color focusBorderColor = Color.Gray;//Color.FromArgb(0, 123, 255); // Bootstrap Primary蓝色 (#007bff)[4](@ref)
            Color disabledColor = Color.FromArgb(233, 236, 239); // Bootstrap禁用背景色 (#e9ecef)[12](@ref)

            // 2. 遍历ComboBox数组并设置样式
            foreach (ComboBox comboBox in comboBoxes)
            {
                // 基础样式
                comboBox.FlatStyle = FlatStyle.Flat; // 扁平化风格[7](@ref)
                comboBox.BackColor = Color.White;
                comboBox.ForeColor = Color.FromArgb(73, 80, 87); // Bootstrap文字深灰色 (#495057)[12](@ref)
                comboBox.Font = new Font("Segoe UI", 9); // 接近Bootstrap默认字体[5](@ref)
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList; // 禁止用户输入[5](@ref)

                // 扩展1：动态边框颜色（聚焦/悬停效果）
                comboBox.GotFocus += (s, e) => comboBox.BackColor = Color.White;
                comboBox.LostFocus += (s, e) => comboBox.BackColor = Color.White;
                comboBox.MouseEnter += (s, e) => comboBox.BackColor = Color.FromArgb(240, 240, 240); // 悬停浅灰色背景
                comboBox.MouseLeave += (s, e) => comboBox.BackColor = Color.White;

                // 扩展2：禁用状态样式
                comboBox.EnabledChanged += (s, e) =>
                {
                    if (!comboBox.Enabled)
                    {
                        comboBox.BackColor = disabledColor;
                        comboBox.ForeColor = Color.FromArgb(108, 117, 125); // 禁用文字色 (#6c757d)[12](@ref)
                    }
                    else
                    {
                        comboBox.BackColor = Color.White;
                        comboBox.ForeColor = Color.FromArgb(73, 80, 87);
                    }
                };

                // 3. 自定义下拉项绘制（Bootstrap风格列表）
                comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                comboBox.DrawItem += (s, e) =>
                {
                    try
                    {
                        /*
                         * 已经被此程序引用过的SAP窗口，如果被用户关闭，有两个事件可能会受影响：
                         * 1，这里的CombBox重绘事件。
                         * 2，Sap Session的Disconnected事件。
                         * 如果Session的Disconnected事件先发生，则会重新加载窗口列表，此处的重绘事件不会再触发异常。
                         * 如果重绘事件先发生，则会抛出异常，这是为什么此处要捕获异常的原因。
                         * 2025-9-9
                         */
                        e.DrawBackground();
                        if (e.Index >= 0)
                        {
                            // 选中项高亮（Bootstrap Primary蓝色）
                            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(focusBorderColor), e.Bounds);
                                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds);
                            }
                            else
                            {
                                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font, new SolidBrush(comboBox.ForeColor), e.Bounds);
                            }
                        }
                        e.DrawFocusRectangle();
                    }
                    catch (COMException) // 重绘处理，SAP 关闭之后，可能会抛出异常。
                    {
                        //MessageBox.Show(cex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //Serilog.Log.Error("{0}", cex.Message);
                        //throw;
                    }
                };
            }
        }
        public static void FormatTextBoxesAsBootstrap(TextBox[] textBoxes)
        {
            // 1. 定义Bootstrap风格颜色
            Color borderColor = Color.FromArgb(206, 212, 218); // Bootstrap默认边框色 (#ced4da)[1](@ref)[7](@ref)
            Color focusBorderColor = Color.FromArgb(0, 123, 255); // Bootstrap Primary蓝色 (#007bff)[1](@ref)[5](@ref)
            Color disabledColor = Color.FromArgb(233, 236, 239); // Bootstrap禁用背景色 (#e9ecef)[7](@ref)

            // 2. 遍历TextBox数组并设置样式
            foreach (TextBox textBox in textBoxes)
            {
                // 基础样式
                textBox.BorderStyle = BorderStyle.FixedSingle; // 扁平化边框[7](@ref)[9](@ref)
                textBox.BackColor = Color.White;
                textBox.ForeColor = Color.FromArgb(73, 80, 87); // Bootstrap文字深灰色 (#495057)[1](@ref)
                textBox.Font = new Font("Segoe UI", 9); // 接近Bootstrap默认字体[5](@ref)

                // 扩展1：动态边框颜色（聚焦/悬停效果）
                textBox.Enter += (s, e) => textBox.BackColor = Color.White;
                textBox.Leave += (s, e) => textBox.BackColor = Color.White;
                textBox.MouseEnter += (s, e) =>
                {
                    textBox.BackColor = Color.FromArgb(240, 240, 240); // 悬停浅灰色背景
                    textBox.BorderStyle = BorderStyle.FixedSingle; // 确保边框可见
                };
                textBox.MouseLeave += (s, e) =>
                {
                    textBox.BackColor = Color.White;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                };

                // 扩展2：禁用状态样式
                textBox.EnabledChanged += (s, e) =>
                {
                    if (!textBox.Enabled)
                    {
                        textBox.BackColor = disabledColor;
                        textBox.ForeColor = Color.FromArgb(108, 117, 125); // 禁用文字色 (#6c757d)[7](@ref)
                    }
                    else
                    {
                        textBox.BackColor = Color.White;
                        textBox.ForeColor = Color.FromArgb(73, 80, 87);
                    }
                };

                // 扩展3：文本对齐（模仿Bootstrap表单控件）
                textBox.TextAlign = HorizontalAlignment.Left; // 默认左对齐，可改为Center[6](@ref)
            }
        }
        public static void FormatButtonsAsMaterialDesign(params Button[] buttons)
        {
            // 1. 定义Material Design风格颜色
            Color primaryColor = Color.FromArgb(33, 150, 243); // Material Blue 500 (#2196F3)
            Color hoverColor = Color.FromArgb(25, 118, 210);   // Darker Blue (#1976D2)
            Color rippleColor = Color.FromArgb(255, 255, 255, 128); // 半透明白色涟漪效果
            Color disabledColor = Color.FromArgb(189, 189, 189); // Material Grey 400 (#BDBDBD)

            // 2. 遍历Button数组并设置样式
            foreach (Button button in buttons)
            {
                // 基础样式
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = primaryColor;
                button.ForeColor = Color.White;
                button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                button.FlatAppearance.BorderSize = 0; // 隐藏边框
                button.TextAlign = ContentAlignment.MiddleCenter; // 文本居中

                // 动态效果：悬停和点击
                button.MouseEnter += (s, e) => button.BackColor = hoverColor;
                button.MouseLeave += (s, e) => button.BackColor = primaryColor;
                button.MouseDown += (s, e) => button.FlatAppearance.MouseDownBackColor = rippleColor;
                button.MouseUp += (s, e) => button.BackColor = hoverColor;

                // 禁用状态样式
                button.EnabledChanged += (s, e) =>
                {
                    if (!button.Enabled)
                    {
                        button.BackColor = disabledColor;
                        button.ForeColor = Color.FromArgb(97, 97, 97); // Material Grey 600 (#616161)
                    }
                    else
                    {
                        button.BackColor = primaryColor;
                        button.ForeColor = Color.White;
                    }
                };

                // 3. 自定义绘制（模拟Material Design的阴影和圆角）
                button.Paint += (s, e) =>
                {
                    // 绘制圆角背景（模拟Material Design卡片效果）
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 4; // 圆角半径
                        path.AddArc(0, 0, radius, radius, 180, 90);
                        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
                        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
                        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
                        path.CloseFigure();
                        e.Graphics.FillPath(new SolidBrush(button.BackColor), path);
                    }

                    // 绘制文本（居中且带轻微内边距）
                    TextRenderer.DrawText(
                        e.Graphics,
                        button.Text,
                        button.Font,
                        new Rectangle(0, 0, button.Width, button.Height),
                        button.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                };
            }
        }
        public static void FormatCheckedListBoxesAsBootstrap(params CheckedListBox[] checkedListBoxes)
        {
            // 1. 定义Bootstrap风格颜色
            Color borderColor = Color.FromArgb(206, 212, 218); // Bootstrap默认边框色 (#ced4da)
            Color focusColor = Color.FromArgb(0, 123, 255);     // Bootstrap Primary蓝色 (#007bff)
            Color disabledColor = Color.FromArgb(233, 236, 239); // 禁用背景色 (#e9ecef)
            Font itemFont = new Font("Segoe UI", 9);           // 接近Bootstrap默认字体

            // 2. 遍历CheckedListBox数组并设置样式
            foreach (CheckedListBox clb in checkedListBoxes)
            {
                // 基础样式
                clb.BorderStyle = BorderStyle.FixedSingle;      // 扁平化边框
                clb.BackColor = Color.White;
                clb.ForeColor = Color.FromArgb(73, 80, 87);    // 文字深灰色 (#495057)
                clb.Font = itemFont;
                clb.CheckOnClick = true;                       // 点击直接切换选中状态[3](@ref)[6](@ref)

                // 3. 自定义绘制（模仿Bootstrap的列表项和复选框）
                clb.DrawMode = DrawMode.OwnerDrawFixed;        // 启用自定义绘制[8](@ref)
                clb.DrawItem += (s, e) =>
                {
                    e.DrawBackground();
                    if (e.Index >= 0)
                    {
                        // 绘制复选框（模拟Bootstrap的方形复选框）
                        Rectangle checkBoxRect = new Rectangle(e.Bounds.X, e.Bounds.Y, 16, e.Bounds.Height);
                        ControlPaint.DrawCheckBox(e.Graphics, checkBoxRect,
                            clb.GetItemChecked(e.Index) ? ButtonState.Checked : ButtonState.Normal);

                        // 绘制文本（留出复选框空间）
                        Rectangle textRect = new Rectangle(e.Bounds.X + 20, e.Bounds.Y, e.Bounds.Width - 20, e.Bounds.Height);
                        TextRenderer.DrawText(e.Graphics, clb.Items[e.Index].ToString(), e.Font, textRect, e.ForeColor,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                    }
                };

                // 4. 动态效果（悬停和焦点）
                //clb.MouseEnter += (s, e) => clb.BackColor = Color.FromArgb(240, 240, 240); // 悬停浅灰色背景
                //clb.MouseLeave += (s, e) => clb.BackColor = Color.White;
                clb.GotFocus += (s, e) => clb.BorderStyle = BorderStyle.Fixed3D;           // 聚焦时边框高亮
                //clb.LostFocus += (s, e) => clb.BorderStyle = BorderStyle.FixedSingle;//报错

                // 5. 禁用状态样式
                clb.EnabledChanged += (s, e) =>
                {
                    if (!clb.Enabled)
                    {
                        clb.BackColor = disabledColor;
                        clb.ForeColor = Color.FromArgb(108, 117, 125); // 禁用文字色 (#6c757d)
                    }
                    else
                    {
                        clb.BackColor = Color.White;
                        clb.ForeColor = Color.FromArgb(73, 80, 87);
                    }
                };
            }
        }

    }
}
