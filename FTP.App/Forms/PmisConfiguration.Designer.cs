using Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FTP
{
    partial class PmisConfiguration
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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            FormatLayout();
            base.OnSizeChanged(e);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PmisConfiguration));
            this.panel1 = new System.Windows.Forms.Panel();
            this.trvAssets = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvConfiguration = new Core.IDataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.ofdExport = new System.Windows.Forms.OpenFileDialog();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCoefficient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHisCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMapping = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMonitor = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.trvAssets);
            this.panel1.Location = new System.Drawing.Point(20, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(244, 347);
            this.panel1.TabIndex = 0;
            // 
            // trvAssets
            // 
            this.trvAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvAssets.HideSelection = false;
            this.trvAssets.Location = new System.Drawing.Point(0, 0);
            this.trvAssets.Name = "trvAssets";
            this.trvAssets.Size = new System.Drawing.Size(242, 345);
            this.trvAssets.TabIndex = 0;
            this.trvAssets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvAssets_AfterSelect);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dgvConfiguration);
            this.panel2.Location = new System.Drawing.Point(270, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(452, 347);
            this.panel2.TabIndex = 1;
            // 
            // dgvConfiguration
            // 
            this.dgvConfiguration.AllowUserToAddRows = false;
            this.dgvConfiguration.AllowUserToDeleteRows = false;
            this.dgvConfiguration.AllowUserToResizeColumns = false;
            this.dgvConfiguration.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvConfiguration.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvConfiguration.ColumnHeadersHeight = 40;
            this.dgvConfiguration.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colName,
            this.colUnit,
            this.colCoefficient,
            this.colHisCode,
            this.colMapping,
            this.colId,
            this.colMonitor});
            this.dgvConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConfiguration.IsCancelEdit = false;
            this.dgvConfiguration.Location = new System.Drawing.Point(0, 0);
            this.dgvConfiguration.MultiSelect = false;
            this.dgvConfiguration.Name = "dgvConfiguration";
            this.dgvConfiguration.RowHeadersVisible = false;
            this.dgvConfiguration.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConfiguration.ShowCheckBox = false;
            this.dgvConfiguration.Size = new System.Drawing.Size(450, 345);
            this.dgvConfiguration.TabIndex = 5;
            this.dgvConfiguration.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConfiguration_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Danh sách thông số PMIS";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(491, 410);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 30);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Xuất excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(622, 410);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 30);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Nhập excel";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Visible = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // ofdExport
            // 
            this.ofdExport.FileName = "openFileDialog1";
            this.ofdExport.Filter = "Excel 2003 (*.xls)|*.xls|Excel 2007+ (*.xlsx)|*.xlsx|All Excel (*.xls, *.xlsx)|*." +
    "xls,*.xlsx";
            this.ofdExport.Title = "Chọn file";
            // 
            // ofdImport
            // 
            this.ofdImport.FileName = "openFileDialog1";
            this.ofdImport.Filter = "Excel 2003 (*.xls)|*.xls|Excel 2007+ (*.xlsx)|*.xlsx|All Excel (*.xls, *.xlsx)|*." +
    "xls,*.xlsx";
            this.ofdImport.Title = "Chọn file";
            // 
            // colSelect
            // 
            this.colSelect.Frozen = true;
            this.colSelect.HeaderText = "";
            this.colSelect.Name = "colSelect";
            this.colSelect.Width = 30;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "METERDESC";
            this.colName.HeaderText = "Tên thông số";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 180;
            // 
            // colUnit
            // 
            this.colUnit.DataPropertyName = "UOMID";
            this.colUnit.HeaderText = "Đơn vị tính";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            this.colUnit.Width = 120;
            // 
            // colCoefficient
            // 
            this.colCoefficient.HeaderText = "Hệ số nhân";
            this.colCoefficient.Name = "colCoefficient";
            // 
            // colHisCode
            // 
            this.colHisCode.DataPropertyName = "TAGNAME";
            this.colHisCode.HeaderText = "Tag Name";
            this.colHisCode.Name = "colHisCode";
            this.colHisCode.ReadOnly = true;
            this.colHisCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colMapping
            // 
            this.colMapping.HeaderText = "Ánh xạ HIS";
            this.colMapping.Name = "colMapping";
            this.colMapping.Width = 50;
            // 
            // colId
            // 
            this.colId.DataPropertyName = "ADMETERID";
            this.colId.HeaderText = "Mã thông số";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            // 
            // colMonitor
            // 
            this.colMonitor.HeaderText = "Giám sát thông số";
            this.colMonitor.Name = "colMonitor";
            this.colMonitor.ReadOnly = true;
            this.colMonitor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMonitor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colMonitor.Width = 150;
            // 
            // PmisConfiguration
            // 
            this.ClientSize = new System.Drawing.Size(734, 461);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(750, 500);
            this.Name = "PmisConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý thông số PMIS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PmisConfiguration_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private Button btnExport;
        private Button btnImport;
        private OpenFileDialog ofdExport;
        private OpenFileDialog ofdImport;
        private TreeView trvAssets;
        private IDataGridView dgvConfiguration;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colUnit;
        private DataGridViewTextBoxColumn colCoefficient;
        private DataGridViewTextBoxColumn colHisCode;
        private DataGridViewButtonColumn colMapping;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewButtonColumn colMonitor;
    }
}

