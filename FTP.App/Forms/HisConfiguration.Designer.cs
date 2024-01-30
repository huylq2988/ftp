using Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FTP
{
    partial class HisConfiguration
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
            FormatGridView();
            ResizeColumns();
            base.OnResize(e);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HisConfiguration));
            this.lblMain = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.dgvConfiguration = new Core.IDataGridView();
            this.tbSearch = new Core.ITextBox();
            this.btnFreConfig = new System.Windows.Forms.Button();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInterval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMain
            // 
            this.lblMain.AutoSize = true;
            this.lblMain.Location = new System.Drawing.Point(25, 20);
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(156, 14);
            this.lblMain.TabIndex = 0;
            this.lblMain.Text = "Danh sách thông số từ HIS";
            // 
            // lblSearch
            // 
            this.lblSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(493, 20);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(56, 14);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Tìm kiếm";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Location = new System.Drawing.Point(284, 447);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(115, 28);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Nhập từ excel";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(419, 447);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(115, 28);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Xoá thông số";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ofdImport
            // 
            this.ofdImport.FileName = "openFileDialog1";
            this.ofdImport.Filter = "All Excel (*.xls, *.xlsx)|*.xls*|Excel 2007+ (*.xlsx)|*.xlsx|Excel 2003 (*.xls)|*" +
    ".xls";
            this.ofdImport.Title = "Chọn file";
            // 
            // btnTemplate
            // 
            this.btnTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTemplate.Location = new System.Drawing.Point(554, 447);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(115, 28);
            this.btnTemplate.TabIndex = 9;
            this.btnTemplate.Text = "Tải file mẫu";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(14, 447);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(115, 28);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.Location = new System.Drawing.Point(149, 447);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(115, 28);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
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
            this.colCode,
            this.colName,
            this.colUnit,
            this.colDevice,
            this.colInterval,
            this.colEnable});
            this.dgvConfiguration.IsCancelEdit = false;
            this.dgvConfiguration.Location = new System.Drawing.Point(0, 65);
            this.dgvConfiguration.MultiSelect = false;
            this.dgvConfiguration.Name = "dgvConfiguration";
            this.dgvConfiguration.RowHeadersVisible = false;
            this.dgvConfiguration.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConfiguration.ShowCheckBox = false;
            this.dgvConfiguration.Size = new System.Drawing.Size(909, 346);
            this.dgvConfiguration.TabIndex = 4;
            // 
            // tbSearch
            // 
            this.tbSearch.AllowTextChanged = true;
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tbSearch.ForeColor = System.Drawing.Color.Black;
            this.tbSearch.Location = new System.Drawing.Point(555, 17);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(342, 22);
            this.tbSearch.SuggestText = "Nhập từ khoá";
            this.tbSearch.SuggestType = Core.SuggestType.PlaceHolder;
            this.tbSearch.TabIndex = 3;
            this.tbSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearch_KeyPress);
            // 
            // btnFreConfig
            // 
            this.btnFreConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFreConfig.Location = new System.Drawing.Point(689, 447);
            this.btnFreConfig.Name = "btnFreConfig";
            this.btnFreConfig.Size = new System.Drawing.Size(178, 28);
            this.btnFreConfig.TabIndex = 10;
            this.btnFreConfig.Text = "Cấu hình tần suất lấy dữ liệu";
            this.btnFreConfig.UseVisualStyleBackColor = true;
            this.btnFreConfig.Click += new System.EventHandler(this.btnFreConfig_Click);
            // 
            // colSelect
            // 
            this.colSelect.Frozen = true;
            this.colSelect.HeaderText = "";
            this.colSelect.Name = "colSelect";
            this.colSelect.Width = 30;
            // 
            // colCode
            // 
            this.colCode.DataPropertyName = "TagName";
            this.colCode.HeaderText = "Tag Name";
            this.colCode.MaxInputLength = 100;
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Description";
            this.colName.HeaderText = "Mô tả";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 180;
            // 
            // colUnit
            // 
            this.colUnit.DataPropertyName = "Unit";
            this.colUnit.HeaderText = "Đơn vị tính";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            // 
            // colDevice
            // 
            this.colDevice.DataPropertyName = "Device";
            this.colDevice.HeaderText = "Tên thiết bị";
            this.colDevice.Name = "colDevice";
            this.colDevice.ReadOnly = true;
            this.colDevice.Width = 150;
            // 
            // colInterval
            // 
            this.colInterval.DataPropertyName = "Interval";
            this.colInterval.HeaderText = "Tần suất lấy dữ liệu (phút)";
            this.colInterval.Name = "colInterval";
            this.colInterval.ReadOnly = true;
            // 
            // colEnable
            // 
            this.colEnable.DataPropertyName = "Enable";
            this.colEnable.HeaderText = "Sử dụng";
            this.colEnable.Name = "colEnable";
            this.colEnable.ReadOnly = true;
            this.colEnable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEnable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // HisConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 483);
            this.Controls.Add(this.btnFreConfig);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.dgvConfiguration);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.lblMain);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(750, 440);
            this.Name = "HisConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý thông số HIS";
            this.Load += new System.EventHandler(this.HisConfiguration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblMain;
        private Label lblSearch;
        private ITextBox tbSearch;
        private Button btnImport;
        private Button btnDelete;
        private IDataGridView dgvConfiguration;
        private OpenFileDialog ofdImport;
        private Button btnTemplate;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnFreConfig;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewTextBoxColumn colCode;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colUnit;
        private DataGridViewTextBoxColumn colDevice;
        private DataGridViewTextBoxColumn colInterval;
        private DataGridViewCheckBoxColumn colEnable;
    }
}

