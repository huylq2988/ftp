using Core;
using System.ComponentModel;

namespace FTP
{
    partial class ManualUpload
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualUpload));
            this.dtTuNgay = new System.Windows.Forms.DateTimePicker();
            this.lblTuNgay = new System.Windows.Forms.Label();
            this.lblDenNgay = new System.Windows.Forms.Label();
            this.dtDenNgay = new System.Windows.Forms.DateTimePicker();
            this.btnDocDuLieuLs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dtTuNgay
            // 
            this.dtTuNgay.CustomFormat = "dd-MM-yyyy";
            this.dtTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTuNgay.Location = new System.Drawing.Point(73, 28);
            this.dtTuNgay.Name = "dtTuNgay";
            this.dtTuNgay.Size = new System.Drawing.Size(115, 20);
            this.dtTuNgay.TabIndex = 0;
            // 
            // lblTuNgay
            // 
            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Location = new System.Drawing.Point(13, 32);
            this.lblTuNgay.Name = "lblTuNgay";
            this.lblTuNgay.Size = new System.Drawing.Size(46, 13);
            this.lblTuNgay.TabIndex = 1;
            this.lblTuNgay.Text = "Từ ngày";
            // 
            // lblDenNgay
            // 
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Location = new System.Drawing.Point(247, 32);
            this.lblDenNgay.Name = "lblDenNgay";
            this.lblDenNgay.Size = new System.Drawing.Size(53, 13);
            this.lblDenNgay.TabIndex = 1;
            this.lblDenNgay.Text = "Đến ngày";
            // 
            // dtDenNgay
            // 
            this.dtDenNgay.CustomFormat = "dd-MM-yyyy";
            this.dtDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDenNgay.Location = new System.Drawing.Point(319, 28);
            this.dtDenNgay.Name = "dtDenNgay";
            this.dtDenNgay.Size = new System.Drawing.Size(115, 20);
            this.dtDenNgay.TabIndex = 0;
            // 
            // btnDocDuLieuLs
            // 
            this.btnDocDuLieuLs.Location = new System.Drawing.Point(199, 96);
            this.btnDocDuLieuLs.Name = "btnDocDuLieuLs";
            this.btnDocDuLieuLs.Size = new System.Drawing.Size(75, 23);
            this.btnDocDuLieuLs.TabIndex = 2;
            this.btnDocDuLieuLs.Text = "Đọc dữ liệu";
            this.btnDocDuLieuLs.UseVisualStyleBackColor = true;
            this.btnDocDuLieuLs.Click += new System.EventHandler(this.btnDocDuLieuLs_Click);
            // 
            // ManualUpload
            // 
            this.ClientSize = new System.Drawing.Size(470, 161);
            this.Controls.Add(this.btnDocDuLieuLs);
            this.Controls.Add(this.lblDenNgay);
            this.Controls.Add(this.lblTuNgay);
            this.Controls.Add(this.dtDenNgay);
            this.Controls.Add(this.dtTuNgay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManualUpload";
            this.Text = "Đọc dữ liệu lịch sử";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtTuNgay;
        private System.Windows.Forms.Label lblTuNgay;
        private System.Windows.Forms.Label lblDenNgay;
        private System.Windows.Forms.DateTimePicker dtDenNgay;
        private System.Windows.Forms.Button btnDocDuLieuLs;
    }
}

