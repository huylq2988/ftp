using FTP.Common;
using FTP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FTP
{
    public partial class HisConfiguration : Form
    {
        public HisConfiguration()
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

        private void HisConfiguration_Load(object sender, EventArgs e)
        {
            FormatGridView();
            LoadData(0);
        }

        private void ReloadData()
        {
            var lst = _repositoryMiddle.GetListFromParameters<HisConfig>("sp_GetHisConfiguration", 4, null);
            dgvConfiguration.DataSource = Helper.ToDataTable(lst);
            dgvConfiguration.Columns["Id"].Visible = false;
            dgvConfiguration.Columns["Ameterid"].Visible = false;
        }

        private void LoadData(int rowIndex)
        {
            var lst = _repositoryMiddle.GetListFromParameters<HisConfig>("sp_GetHisConfiguration", 4, null);
            dgvConfiguration.DataSource = Helper.ToDataTable(lst);
            dgvConfiguration.Columns["Id"].Visible = false;
            dgvConfiguration.CurrentCell = dgvConfiguration[0, rowIndex];

            dgvConfiguration.Columns["Ameterid"].Visible = false;
        }

        private void FormatGridView()
        {
            dgvConfiguration.Location = new Point(-1, 50);
            dgvConfiguration.Size = new Size(Width - 15, Height - 140);
        }

        private void ResizeColumns()
        {
            colSelect.Width = (int)(Width * 0.1 - 5);
            colCode.Width = (int)(Width * 0.2);
            colName.Width = (int)(Width * 0.3);
            colUnit.Width = (int)(Width * 0.18);
            colDevice.Width = (int)(Width * 0.22);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNew add = new AddNew();
            add.SendRow += Add_SendRow;
            add.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var row = dgvConfiguration.CurrentCell;
            if (row == null)
            {
                MessageBox.Show("Chưa chọn bản ghi cần cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var id = dgvConfiguration["ID", row.RowIndex].Value.ToString();
            var tagName = dgvConfiguration["colCode", row.RowIndex].Value.ToString();
            var description = dgvConfiguration["colName", row.RowIndex].Value.ToString();
            var unit = dgvConfiguration["colUnit", row.RowIndex].Value.ToString();
            var device = dgvConfiguration["colDevice", row.RowIndex].Value.ToString();
            var interval = dgvConfiguration["colInterval", row.RowIndex].Value;
            var enable = dgvConfiguration["colEnable", row.RowIndex].Value;

            Edit edit = new Edit(id, tagName, description, unit, device, interval, enable);
            edit.SendRow += Edit_SendRow;
            edit.ShowDialog();
        }

        private void Add_SendRow(string tagName, string description, string unit, string device, decimal interval)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@TagName"] = tagName,
                ["@Description"] = description,
                ["@Unit"] = unit,
                ["@Device"] = device,
                ["@Interval"] = interval,
            };
            var result = _repositoryMiddle.ExecuteSQLFromParameters("sp_InsertHisConfig", 4, paras);
            if (result > 0)
            {
                LoadData(dgvConfiguration.RowCount - 1);
                MessageBox.Show("Thêm mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result == 0)
            {
                MessageBox.Show("Không có bản ghi nào được thêm mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Thêm mới không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Edit_SendRow(string id, string tagName, string description, string unit, string device, decimal interval, bool enable)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@id"] = id,
                ["@tagName"] = tagName,
                ["@description"] = description,
                ["@unit"] = unit,
                ["@device"] = device,
                ["@interval"] = interval,
                ["@enable"] = enable,
            };
            var result = _repositoryMiddle.ExecuteSQLFromParameters("sp_UpdateHisConfig", 4, paras);
            if (result > 0)
            {
                LoadData(dgvConfiguration.CurrentCell.RowIndex);
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result == 0)
            {
                MessageBox.Show("Không có bản ghi nào được cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var dr = ofdImport.ShowDialog();
            if (dr == DialogResult.OK)
            {
                var dt = Helper.ImportExceltoDatatable(ofdImport.FileName);
                if (dt.Columns[0].ColumnName != "MA_TS"
                    || dt.Columns[1].ColumnName != "TEN_TS"
                    || dt.Columns[2].ColumnName != "DVT"
                    || dt.Columns[3].ColumnName != "TEN_TB"
                    )
                {
                    MessageBox.Show("File excel không đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    var paras = new Dictionary<string, object>()
                    {
                        ["@tagName"] = row.ItemArray[0],
                        ["@description"] = row.ItemArray[1],
                        ["@unit"] = row.ItemArray[2],
                        ["@device"] = row.ItemArray[3],
                        ["@interval"] = row.ItemArray[4],
                    };
                    var result = _repositoryMiddle.ExecuteSQLFromParameters("sp_InsertHisConfig", 4, paras);
                }
                var lst = _repositoryMiddle.GetListFromParameters<HisConfig>("sp_GetHisConfiguration", 4, null);
                dgvConfiguration.DataSource = Helper.ToDataTable(lst);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var confirmDelete = MessageBox.Show("Bạn có chắc chắn muốn xoá?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmDelete == DialogResult.No)
            {
                return;
            }
            var currentCell = dgvConfiguration.CurrentCell;
            if (currentCell == null || (currentCell.RowIndex == dgvConfiguration.RowCount - 1 && true))
            {
                return;
            }
            var paras = new Dictionary<string, object>()
            {
                ["@tagName"] = dgvConfiguration[currentCell.ColumnIndex, currentCell.RowIndex].Value
            };
            _repositoryMiddle.ExecuteSQLFromParameters("sp_RemoveConfig", 4, paras);
            dgvConfiguration.Rows.RemoveAt(dgvConfiguration.CurrentCell.RowIndex);
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            // FTP.Resources.template_his.xlsx
            Process.Start("explorer.exe", Application.StartupPath + "\\Resources\\template_his.xlsx");
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13) return;
            //sp_SearchHisConfiguration
            var parameters = new Dictionary<string, object>()
            {
                ["@txt"] = tbSearch.Text
            };
            var lst = _repositoryMiddle.GetListFromParameters<HisConfig>("sp_SearchHisConfiguration", 4, parameters);
            dgvConfiguration.DataSource = Helper.ToDataTable(lst);
        }

        private void btnFreConfig_Click(object sender, EventArgs e)
        {
            List<long> lstIDs = new List<long>();
            foreach (DataGridViewRow row in dgvConfiguration.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells[0].Value);
                if (isSelected) lstIDs.Add(Convert.ToInt64(row.Cells["Id"].Value));
            }

            if (lstIDs.Count == 0)
            {
                MessageBox.Show("Bắt buộc chọn ít nhất 1", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FrequenceConfig frequence = new FrequenceConfig(lstIDs.ToArray());
            frequence.LoadDataAfterClose += ReloadData;
            frequence.ShowDialog();
        }
    }
}
