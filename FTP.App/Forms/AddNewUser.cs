using System;
using System.Text;
using System.Windows.Forms;

namespace FTP
{
    public partial class AddNewUser : Form
    {
        public AddNewUser()
        {
            InitializeComponent();
        }

        public delegate void SendRowHandler(string user, string password, bool used);
        public event SendRowHandler SendRow = null;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
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
            SendRow(tbUser.Text, tbPassword.Text, ckbStatus.Checked);
            Close();
        }
    }
}
