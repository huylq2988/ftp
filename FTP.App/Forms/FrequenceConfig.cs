using FTP.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace FTP
{
    public partial class FrequenceConfig : Form
    {
        public FrequenceConfig(long[] selectedRows)
        {
            SelectedRows = selectedRows;
            InitializeComponent();
            //_repositoryLocal = new Repository(connectionStringLocal);
            _repositoryMiddle = new Repository(connectionStringMiddle);
            label1.Text = string.Format(label1.Text, selectedRows.Length);
        }

        public delegate void LoadData();
        public event LoadData LoadDataAfterClose = null;
        //private string connectionStringLocal = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
        private string connectionStringMiddle = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;
        //private readonly Repository _repositoryLocal;
        private readonly Repository _repositoryMiddle;

        private long[] SelectedRows = new long[1];

        private void btnSave_Click(object sender, EventArgs e)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@newInterval"] = nudFreq.Value,
            };
            var result = _repositoryMiddle.ExecuteSQLFromParameters($"update Params set Interval = @newInterval where id in ({string.Join(", ", SelectedRows)})", 1, paras);
            if (result > 0)
            {
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataAfterClose();
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            Close();
        }
    }
}
