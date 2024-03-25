using System.ComponentModel;

namespace FTP
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitContext()
        {
            notifyIcon.ContextMenu = contextMenu1;
            menuItem2.Visible = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.miLogin = new System.Windows.Forms.MenuItem();
            this.miLogout = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.miHisConfiguration = new System.Windows.Forms.MenuItem();
            this.miPmisConfiguration = new System.Windows.Forms.MenuItem();
            this.miFTPConnection = new System.Windows.Forms.MenuItem();
            this.miManual = new System.Windows.Forms.MenuItem();
            this.miDatabaseConnection = new System.Windows.Forms.MenuItem();
            this.miUserManagement = new System.Windows.Forms.MenuItem();
            this.miReadDataConfiguration = new System.Windows.Forms.MenuItem();
            this.miReadHistory = new System.Windows.Forms.MenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.miShowForm = new System.Windows.Forms.MenuItem();
            this.miExit = new System.Windows.Forms.MenuItem();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnMainStart = new System.Windows.Forms.Button();
            this.btnMainStop = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pntBottomR = new System.Windows.Forms.Panel();
            this.pnlBottomL = new System.Windows.Forms.Panel();
            MainForm.lblHeader = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pntBottomR.SuspendLayout();
            this.pnlBottomL.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miLogin,
            this.miLogout,
            this.menuItem6,
            this.menuItem7});
            this.menuItem1.Text = "Hệ thống";
            // 
            // miLogin
            // 
            this.miLogin.Index = 0;
            this.miLogin.Shortcut = System.Windows.Forms.Shortcut.AltF1;
            this.miLogin.Text = "Đăng nhập";
            this.miLogin.Click += new System.EventHandler(this.miLogin_Click);
            // 
            // miLogout
            // 
            this.miLogout.Index = 1;
            this.miLogout.Text = "Đăng xuất";
            this.miLogout.Visible = false;
            this.miLogout.Click += new System.EventHandler(this.miLogout_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "-";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            this.menuItem7.Text = "Thoát";
            this.menuItem7.Click += new System.EventHandler(this.miExit_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miHisConfiguration,
            this.miPmisConfiguration,
            this.miFTPConnection,
            this.miManual,
            this.miDatabaseConnection,
            this.miUserManagement,
            this.miReadDataConfiguration,
            this.miReadHistory});
            this.menuItem2.Text = "Chức năng";
            // 
            // miHisConfiguration
            // 
            this.miHisConfiguration.Index = 0;
            //this.miHisConfiguration.Shortcut = System.Windows.Forms.Shortcut.Alt1;
            this.miHisConfiguration.Text = "Danh sách Tagname";
            this.miHisConfiguration.Click += new System.EventHandler(this.miHisConfiguration_Click);
            // 
            // miPmisConfiguration
            // 
            this.miPmisConfiguration.Index = 1;
            this.miPmisConfiguration.Visible = false;
            //this.miPmisConfiguration.Shortcut = System.Windows.Forms.Shortcut.Alt2;
            this.miPmisConfiguration.Text = "Danh sách thông số PMIS";
            this.miPmisConfiguration.Click += new System.EventHandler(this.miPmisConfiguration_Click);
            // 
            // miFTPConnection
            // 
            this.miFTPConnection.Index = 2;
            //this.miFTPConnection.Shortcut = System.Windows.Forms.Shortcut.Alt3;
            this.miFTPConnection.Text = "Cấu hình kết nối FTP";
            this.miFTPConnection.Click += new System.EventHandler(this.miFTPConnection_Click);
            // 
            // miManual
            // 
            this.miManual.Index = 3;
            //this.miManual.Shortcut = System.Windows.Forms.Shortcut.Alt4;
            this.miManual.Text = "Gửi dữ liệu thủ công";
            this.miManual.Click += new System.EventHandler(this.miManual_Click);
            // 
            // miDatabaseConnection
            // 
            this.miDatabaseConnection.Index = 4;
            this.miDatabaseConnection.Visible = false;
            //this.miDatabaseConnection.Shortcut = System.Windows.Forms.Shortcut.Alt5;
            this.miDatabaseConnection.Text = "Cấu hình kết nối CSDL";
            this.miDatabaseConnection.Click += new System.EventHandler(this.miDatabaseConnection_Click);
            // 
            // miUserManagement
            // 
            this.miUserManagement.Index = 5;
            //this.miUserManagement.Shortcut = System.Windows.Forms.Shortcut.Alt6;
            this.miUserManagement.Text = "Quản lý người dùng";
            this.miUserManagement.Click += new System.EventHandler(this.miUserManagement_Click);
            // 
            // miReadDataConfiguration
            // 
            this.miReadDataConfiguration.Enabled = false;
            this.miReadDataConfiguration.Visible = false;
            this.miReadDataConfiguration.Index = 6;
            this.miReadDataConfiguration.Text = "Cấu hình đọc dữ liệu";
            // 
            // miReadHistory
            // 
            this.miReadHistory.Enabled = false;
            this.miReadHistory.Visible = false;
            this.miReadHistory.Index = 7;
            this.miReadHistory.Text = "Lịch sử đọc";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miShowForm,
            this.miExit});
            // 
            // miShowForm
            // 
            this.miShowForm.DefaultItem = true;
            this.miShowForm.Index = 0;
            this.miShowForm.Text = "Mở form";
            this.miShowForm.Click += new System.EventHandler(this.miShowForm_Click);
            // 
            // miExit
            // 
            this.miExit.Index = 1;
            this.miExit.Text = "Thoát";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Info;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1072, 372);
            this.txtLog.TabIndex = 1;
            // 
            // btnMainStart
            // 
            this.btnMainStart.Location = new System.Drawing.Point(50, 69);
            this.btnMainStart.Name = "btnMainStart";
            this.btnMainStart.Size = new System.Drawing.Size(94, 68);
            this.btnMainStart.TabIndex = 3;
            this.btnMainStart.Text = "START";
            this.btnMainStart.UseVisualStyleBackColor = true;
            this.btnMainStart.Click += new System.EventHandler(this.btnMainStart_Click);
            // 
            // btnMainStop
            // 
            this.btnMainStop.Location = new System.Drawing.Point(50, 210);
            this.btnMainStop.Name = "btnMainStop";
            this.btnMainStop.Size = new System.Drawing.Size(94, 68);
            this.btnMainStop.TabIndex = 3;
            this.btnMainStop.Text = "STOP";
            this.btnMainStop.UseVisualStyleBackColor = true;
            this.btnMainStop.Click += new System.EventHandler(this.bntMainStop_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FTP.Properties.Resources.image1;
            this.pictureBox1.Location = new System.Drawing.Point(28, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(131, 95);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // pnlTop
            // 
            this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTop.Controls.Add(MainForm.lblHeader);
            this.pnlTop.Controls.Add(this.pictureBox1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1074, 181);
            this.pnlTop.TabIndex = 7;
            // 
            // pntBottomR
            // 
            this.pntBottomR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pntBottomR.Controls.Add(this.txtLog);
            this.pntBottomR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pntBottomR.Location = new System.Drawing.Point(0, 181);
            this.pntBottomR.Name = "pntBottomR";
            this.pntBottomR.Size = new System.Drawing.Size(1074, 374);
            this.pntBottomR.TabIndex = 8;
            // 
            // pnlBottomL
            // 
            this.pnlBottomL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottomL.Controls.Add(this.btnMainStart);
            this.pnlBottomL.Controls.Add(this.btnMainStop);
            this.pnlBottomL.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBottomL.Location = new System.Drawing.Point(0, 181);
            this.pnlBottomL.Name = "pnlBottomL";
            this.pnlBottomL.Size = new System.Drawing.Size(200, 374);
            this.pnlBottomL.TabIndex = 9;
            // 
            // labelControl1
            // 
            MainForm.lblHeader.Appearance.Font = new System.Drawing.Font("Tahoma", 16F);
            MainForm.lblHeader.Appearance.Options.UseFont = true;
            MainForm.lblHeader.Location = new System.Drawing.Point(196, 62);
            MainForm.lblHeader.Name = "labelControl1";
            MainForm.lblHeader.Size = new System.Drawing.Size(122, 25);
            MainForm.lblHeader.TabIndex = 6;
            MainForm.lblHeader.Text = "labelControl1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1074, 555);
            this.Controls.Add(this.pnlBottomL);
            this.Controls.Add(this.pntBottomR);
            this.Controls.Add(this.pnlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thu thập dữ liệu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pntBottomR.ResumeLayout(false);
            this.pntBottomR.PerformLayout();
            this.pnlBottomL.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem miLogin;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem miShowForm;
        private System.Windows.Forms.MenuItem miExit;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem miHisConfiguration;
        private System.Windows.Forms.MenuItem miPmisConfiguration;
        private System.Windows.Forms.MenuItem miFTPConnection;
        private System.Windows.Forms.MenuItem miDatabaseConnection;
        private System.Windows.Forms.MenuItem miReadDataConfiguration;
        private System.Windows.Forms.MenuItem miReadHistory;
        private System.Windows.Forms.MenuItem miLogout;
        private System.Windows.Forms.MenuItem miUserManagement;
        public System.Windows.Forms.TextBox txtLog;
        public System.Windows.Forms.Button btnMainStart;
        public System.Windows.Forms.Button btnMainStop;
        private System.Windows.Forms.MenuItem miManual;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pntBottomR;
        private System.Windows.Forms.Panel pnlBottomL;
        public static DevExpress.XtraEditors.LabelControl lblHeader;
        //private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}