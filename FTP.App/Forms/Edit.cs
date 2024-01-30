using System;
using System.Text;
using System.Windows.Forms;

namespace FTP
{
    public partial class Edit : Form
    {
        public Edit(string id, string maTS, string tenTS, string dvt, string tenTB, object tansuat, object sudung)
        {
            InitializeComponent();
            LoadData(id, maTS, tenTS, dvt, tenTB, tansuat, sudung);
        }

        private string _id;

        private void LoadData(string id, string maTS, string tenTS, string dvt, string tenTB, object tansuat, object sudung)
        {
            tbMaTS.Text = maTS;
            tbTenTS.Text = tenTS;
            tbDVT.Text = dvt;
            tbTenTB.Text = tenTB;
            nudTimer.Value = Convert.ToDecimal(tansuat);
            ckbUsed.Checked = (bool)sudung;
            ckbUsed.Text = ckbUsed.Checked ? "Có" : "Không";
            _id = id;
        }

        public delegate void SendRowHandler(string id, string ma, string ten, string dvt, string tentb, decimal tansuat, bool sd);
        public event SendRowHandler SendRow = null;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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
            SendRow(_id, tbMaTS.Text, tbTenTS.Text, tbDVT.Text, tbTenTB.Text, nudTimer.Value, ckbUsed.Checked);
            Close();
        }

        private void ckbUsed_CheckedChanged(object sender, EventArgs e)
        {
            ckbUsed.Text = ckbUsed.Checked ? "Có" : "Không";
        }
    }
}
