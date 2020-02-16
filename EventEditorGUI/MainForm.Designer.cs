namespace EventEditorGUI
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenuBar = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.载入主文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.载入增量文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新载入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空所有ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TopLine = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.属性界面 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.MainMenuBar.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenuBar
            // 
            this.MainMenuBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.MainMenuBar.Location = new System.Drawing.Point(0, 0);
            this.MainMenuBar.Name = "MainMenuBar";
            this.MainMenuBar.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.MainMenuBar.Size = new System.Drawing.Size(702, 25);
            this.MainMenuBar.TabIndex = 0;
            this.MainMenuBar.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.载入主文件ToolStripMenuItem,
            this.载入增量文件ToolStripMenuItem,
            this.重新载入ToolStripMenuItem,
            this.清空所有ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 载入主文件ToolStripMenuItem
            // 
            this.载入主文件ToolStripMenuItem.Name = "载入主文件ToolStripMenuItem";
            this.载入主文件ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.载入主文件ToolStripMenuItem.Text = "载入主文件";
            this.载入主文件ToolStripMenuItem.Click += new System.EventHandler(this.载入主文件ToolStripMenuItem_Click);
            // 
            // 载入增量文件ToolStripMenuItem
            // 
            this.载入增量文件ToolStripMenuItem.Enabled = false;
            this.载入增量文件ToolStripMenuItem.Name = "载入增量文件ToolStripMenuItem";
            this.载入增量文件ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.载入增量文件ToolStripMenuItem.Text = "载入增量文件";
            this.载入增量文件ToolStripMenuItem.Click += new System.EventHandler(this.载入增量文件ToolStripMenuItem_Click);
            // 
            // 重新载入ToolStripMenuItem
            // 
            this.重新载入ToolStripMenuItem.Enabled = false;
            this.重新载入ToolStripMenuItem.Name = "重新载入ToolStripMenuItem";
            this.重新载入ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.重新载入ToolStripMenuItem.Text = "重新载入";
            this.重新载入ToolStripMenuItem.Click += new System.EventHandler(this.重新载入ToolStripMenuItem_Click);
            // 
            // 清空所有ToolStripMenuItem
            // 
            this.清空所有ToolStripMenuItem.Enabled = false;
            this.清空所有ToolStripMenuItem.Name = "清空所有ToolStripMenuItem";
            this.清空所有ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.清空所有ToolStripMenuItem.Text = "清空所有";
            this.清空所有ToolStripMenuItem.Click += new System.EventHandler(this.清空所有ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // TopLine
            // 
            this.TopLine.AutoSize = true;
            this.TopLine.ForeColor = System.Drawing.Color.Silver;
            this.TopLine.Location = new System.Drawing.Point(106, 9);
            this.TopLine.Name = "TopLine";
            this.TopLine.Size = new System.Drawing.Size(41, 12);
            this.TopLine.TabIndex = 3;
            this.TopLine.Text = "......";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Controls.Add(this.SearchButton);
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(702, 347);
            this.splitContainer1.SplitterDistance = 232;
            this.splitContainer1.TabIndex = 4;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 49);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(207, 292);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(144, 22);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "搜索";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(126, 21);
            this.textBox1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.属性界面);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(466, 347);
            this.tabControl1.TabIndex = 0;
            // 
            // 属性界面
            // 
            this.属性界面.Location = new System.Drawing.Point(4, 22);
            this.属性界面.Name = "属性界面";
            this.属性界面.Padding = new System.Windows.Forms.Padding(3);
            this.属性界面.Size = new System.Drawing.Size(458, 321);
            this.属性界面.TabIndex = 0;
            this.属性界面.Text = "属性界面";
            this.属性界面.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(458, 321);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "关系界面";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(452, 315);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 372);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.TopLine);
            this.Controls.Add(this.MainMenuBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MainMenuBar;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "太吾事件编辑器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainMenuBar.ResumeLayout(false);
            this.MainMenuBar.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip MainMenuBar;
        public System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 载入主文件ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 载入增量文件ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 重新载入ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        public System.Windows.Forms.Label TopLine;
        public System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage 属性界面;
        public System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.ToolStripMenuItem 清空所有ToolStripMenuItem;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

