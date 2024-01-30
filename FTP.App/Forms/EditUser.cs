using FTP.Common;
using System;
using System.Text;
using System.Windows.Forms;

namespace FTP
{
    public partial class EditUser : Form
    {
        public EditUser(string userid, string password, bool sudung)
        {
            InitializeComponent();
            LoadData(userid, password, sudung);
        }

        private void LoadData(string userid, string password, bool sudung)
        {
            tbUser.Text = userid;
            //tbPassword.Text = password;
            ckbStatus.Checked = sudung;
            ckbStatus.Text = ckbStatus.Checked ? "Có" : "Không";
        }

        public delegate void SendRowHandler(string userid, string password, bool sudung);
        public event SendRowHandler SendRow = null;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tbUser.Text))
            {
                sb.AppendLine("- User Id");
            }
            if (string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                sb.AppendLine("- Mật khẩu");
            }
            if (sb.Length > 0)
            {
                var sb2 = new StringBuilder().AppendLine("Bạn chưa điền các trường bắt buộc");
                sb = sb2.AppendLine(sb.ToString());
                MessageBox.Show(sb.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SendRow(tbUser.Text, Helper.CreateMD5(tbPassword.Text), ckbStatus.Checked);
            Close();
        }

        private void ckbUsed_CheckedChanged(object sender, EventArgs e)
        {
            ckbStatus.Text = ckbStatus.Checked ? "Có" : "Không";
        }
    }
}
