using FTP.Common;
using FTP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FTP
{
    public partial class UserManager : Form
    {
        public UserManager()
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

        private void UserManager_Load(object sender, EventArgs e)
        {
            FormatGridView();
            LoadData(0);
        }

        private void LoadData(int rowIndex)
        {
            var lst = _repositoryMiddle.GetListFromParameters<User>("select UserId, '**********************' as [Password], Enable from [users]", 1, null);
            dgvConfiguration.DataSource = Helper.ToDataTable(lst);
            dgvConfiguration.CurrentCell = dgvConfiguration[0, rowIndex];
            bool isAdmin = (string)dgvConfiguration[1, rowIndex].Value == "admin";
            btnDelete.Enabled = !isAdmin;
        }

        private void FormatGridView()
        {
            dgvConfiguration.Location = new Point(-1, 50);
            dgvConfiguration.Size = new Size(Width - 15, Height - 140);
        }

        private void ResizeColumns()
        {
            colSelect.Width = (int)(Width * 0.1 - 5);
            colUser.Width = (int)(Width * 0.3);
            colPass.Width = (int)(Width * 0.4);
            colEnable.Width = (int)(Width * 0.2);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNewUser add = new AddNewUser();
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
            var user = dgvConfiguration["colUser", row.RowIndex].Value.ToString();
            var password = dgvConfiguration["colPass", row.RowIndex].Value.ToString();
            var enable = dgvConfiguration["colEnable", row.RowIndex].Value;

            EditUser edit = new EditUser(user, password, (bool)enable);
            edit.SendRow += Edit_SendRow;
            edit.ShowDialog();
        }

        private void Add_SendRow(string user, string password, bool used)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@userid"] = user,
                ["@password"] = password,
                ["@enable"] = used,
            };
            var result = _repositoryMiddle.ExecuteSQLFromParameters(@"INSERT INTO [dbo].[Users]
           ([Userid]
           ,[Password]
           ,[Enable])
     VALUES
           (@userid
           ,CONVERT(VARCHAR(32), HashBytes('MD5', @password), 2)
           ,@enable)", 1, paras);
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

        private void Edit_SendRow(string userid, string password, bool sudung)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@userid"] = userid,
                ["@password"] = password,
                ["@enable"] = sudung,
            };
            var result = _repositoryMiddle.ExecuteSQLFromParameters(@"update [users] 
set password = @password,
    enable = @enable
where userid = @userid", 1, paras);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var currentRow = dgvConfiguration.CurrentRow;
            if (currentRow == null || (string)currentRow.Cells[1].Value == "admin")
            {
                MessageBox.Show("Không thể xoá user admin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var confirmDelete = MessageBox.Show("Bạn có chắc chắn muốn xoá?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmDelete == DialogResult.No)
            {
                return;
            }
            var paras = new Dictionary<string, object>()
            {
                ["@userid"] = currentRow.Cells[1].Value
            };
            _repositoryMiddle.ExecuteSQLFromParameters("delete [users] where userid = @userid", 1, paras);
            dgvConfiguration.Rows.RemoveAt(dgvConfiguration.CurrentCell.RowIndex);
        }

        private void dgvConfiguration_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool isAdmin = (string)dgvConfiguration[1, e.RowIndex].Value == "admin";
            btnDelete.Enabled = !isAdmin;
        }
    }
}
