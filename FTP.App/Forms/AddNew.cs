using System;
using System.Text;
using System.Windows.Forms;

namespace FTP
{
    public partial class AddNew : Form
    {
        public AddNew()
        {
            InitializeComponent();
        }

        public delegate void SendRowHandler(string ma, string ten, string dvt, string tentb, decimal value);
        public event SendRowHandler SendRow = null;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tbMaTS.Text))
            {
                sb.AppendLine("- Mã thông số");
            }
            if (string.IsNullOrWhiteSpace(tbTenTS.Text))
            {
                sb.AppendLine("- Tên thông số");
            }
            if (string.IsNullOrWhiteSpace(tbTenTB.Text))
            {
                sb.AppendLine("- Tên thiết bị");
            }
            if (sb.Length > 0)
            {
                var sb2 = new StringBuilder().AppendLine("Bạn chưa điền các trường bắt buộc");
                sb = sb2.AppendLine(sb.ToString());
                MessageBox.Show(sb.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SendRow(tbMaTS.Text, tbTenTS.Text, tbDVT.Text, tbTenTB.Text, nudTimer.Value);
            Close();
        }
    }
}
