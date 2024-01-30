using FTP.Common;
using FTP.Model;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace FTP
{
    public partial class HisSelect : Form
    {
        public HisSelect()
        {
            InitializeComponent();
            //_repositoryLocal = new Repository(connectionStringLocal);
            _repositoryMiddle = new Repository(connectionStringMiddle);
        }
        //private string connectionStringLocal = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
        private string connectionStringMiddle = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;
        //private readonly Repository _repositoryLocal;
        private readonly Repository _repositoryMiddle;
        //private string connectionString = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;

        public delegate void SendCodeHandler(string id, string code);
        public event SendCodeHandler SendCode = null;

        private void HisConfiguration_Load(object sender, EventArgs e)
        {
            FormatGridView();
            LoadData();
        }

        private void LoadData()
        {
            var lst = _repositoryMiddle.GetListFromParameters<HisConfig>("sp_GetHisConfigurationSelect", 4, null);
            dgvConfiguration.DataSource = lst;
            dgvConfiguration.Columns["Id"].Visible = false;
            dgvConfiguration.Columns["Interval"].Visible = false;
            dgvConfiguration.Columns["Enable"].Visible = false;
            dgvConfiguration.Columns[3].Visible = false;
            dgvConfiguration.Columns[1].Width = 150;
            dgvConfiguration.Columns[2].Width = 280;
            dgvConfiguration.Columns[3].Width = 120;
        }

        private void FormatGridView()
        {
            dgvConfiguration.Location = new Point(-1, 50);
            dgvConfiguration.Size = new Size(Width - 15, Height - 140);
        }

        private void ResizeColumns()
        {
            colCode.Width = (int)(Width * 0.2);
            colName.Width = (int)(Width * 0.3);
            colDevice.Width = (int)(Width * 0.22);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvConfiguration.CurrentCell == null)
            {
                MessageBox.Show("Chưa chọn thông số ánh xạ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = dgvConfiguration["Id", dgvConfiguration.CurrentCell.RowIndex].Value.ToString();
            var code = dgvConfiguration[1, dgvConfiguration.CurrentCell.RowIndex].Value.ToString();
            SendCode(id, code);
            Close();
        }
    }
}
