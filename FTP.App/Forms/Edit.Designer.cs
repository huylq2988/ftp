using Core;
using System.ComponentModel;

namespace FTP
{
    partial class Edit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Edit));
            this.btnUpdate = new System.Windows.Forms.Button();
            this.openMI = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.exitMI = new System.Windows.Forms.MenuItem();
            this.tbMaTS = new Core.ITextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbTenTS = new Core.ITextBox();
            this.tbDVT = new Core.ITextBox();
            this.tbTenTB = new Core.ITextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ckbUsed = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudTimer = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(116, 293);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(87, 25);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Cập nhật";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // openMI
            // 
            this.openMI.Index = -1;
            this.openMI.Text = "";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = -1;
            this.menuItem2.Text = "";
            // 
            // exitMI
            // 
            this.exitMI.Index = -1;
            this.exitMI.Text = "";
            // 
            // tbMaTS
            // 
            this.tbMaTS.AllowTextChanged = true;
            this.tbMaTS.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tbMaTS.ForeColor = System.Drawing.Color.Black;
            this.tbMaTS.Location = new System.Drawing.Point(108, 23);
            this.tbMaTS.Name = "tbMaTS";
            this.tbMaTS.Size = new System.Drawing.Size(308, 22);
            this.tbMaTS.SuggestText = "Mã thông số";
            this.tbMaTS.SuggestType = Core.SuggestType.PlaceHolder;
            this.tbMaTS.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(240, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Huỷ";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbTenTS
            // 
            this.tbTenTS.AllowTextChanged = true;
            this.tbTenTS.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tbTenTS.ForeColor = System.Drawing.Color.Black;
            this.tbTenTS.Location = new System.Drawing.Point(108, 69);
            this.tbTenTS.Name = "tbTenTS";
            this.tbTenTS.Size = new System.Drawing.Size(308, 22);
            this.tbTenTS.SuggestText = "Tên thông số";
            this.tbTenTS.SuggestType = Core.SuggestType.PlaceHolder;
            this.tbTenTS.TabIndex = 2;
            // 
            // tbDVT
            // 
            this.tbDVT.AllowTextChanged = true;
            this.tbDVT.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tbDVT.ForeColor = System.Drawing.Color.Black;
            this.tbDVT.Location = new System.Drawing.Point(108, 115);
            this.tbDVT.Name = "tbDVT";
            this.tbDVT.Size = new System.Drawing.Size(308, 22);
            this.tbDVT.SuggestText = "Đơn vị tính";
            this.tbDVT.SuggestType = Core.SuggestType.PlaceHolder;
            this.tbDVT.TabIndex = 3;
            // 
            // tbTenTB
            // 
            this.tbTenTB.AllowTextChanged = true;
            this.tbTenTB.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tbTenTB.ForeColor = System.Drawing.Color.Black;
            this.tbTenTB.Location = new System.Drawing.Point(108, 161);
            this.tbTenTB.Name = "tbTenTB";
            this.tbTenTB.Size = new System.Drawing.Size(308, 22);
            this.tbTenTB.SuggestText = "Tên thiết bị";
            this.tbTenTB.SuggestType = Core.SuggestType.PlaceHolder;
            this.tbTenTB.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "Mã thông số";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "Tên thông số";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "Đơn vị tính";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "Tên thiết bị";
            // 
            // ckbUsed
            // 
            this.ckbUsed.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckbUsed.Location = new System.Drawing.Point(108, 245);
            this.ckbUsed.Name = "ckbUsed";
            this.ckbUsed.Size = new System.Drawing.Size(186, 28);
            this.ckbUsed.TabIndex = 6;
            this.ckbUsed.Text = "checkBox1";
            this.ckbUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckbUsed.UseVisualStyleBackColor = true;
            this.ckbUsed.CheckedChanged += new System.EventHandler(this.ckbUsed_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 252);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 14);
            this.label6.TabIndex = 14;
            this.label6.Text = "Sử dụng";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 14);
            this.label5.TabIndex = 15;
            this.label5.Text = "Tần suất";
            // 
            // nudTimer
            // 
            this.nudTimer.Location = new System.Drawing.Point(108, 206);
            this.nudTimer.Name = "nudTimer";
            this.nudTimer.Size = new System.Drawing.Size(308, 22);
            this.nudTimer.TabIndex = 5;
            this.nudTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Edit
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(442, 337);
            this.Controls.Add(this.nudTimer);
            this.Controls.Add(this.ckbUsed);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTenTB);
            this.Controls.Add(this.tbDVT);
            this.Controls.Add(this.tbTenTS);
            this.Controls.Add(this.tbMaTS);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Edit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sửa";
            ((System.ComponentModel.ISupportInitialize)(this.nudTimer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private ITextBox tbMaTS;
        private System.Windows.Forms.MenuItem openMI;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem exitMI;
        private System.Windows.Forms.Button btnCancel;
        private ITextBox tbTenTS;
        private ITextBox tbDVT;
        private ITextBox tbTenTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckbUsed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudTimer;
    }
}

