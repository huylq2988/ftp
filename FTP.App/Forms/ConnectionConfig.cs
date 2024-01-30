using FTP.Common;
using FTP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FTP
{
    public partial class ConnectionConfig : Form
    {
        public ConnectionConfig()
        {
            InitializeComponent();
            _repositoryMiddle = new Repository(connectionStringMiddle);
        }

        private readonly Repository _repositoryMiddle;
        private string connectionStringMiddle = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;

        private void UploadForm_Load(object sender, EventArgs e)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@type"] = 2
            };
            var config = _repositoryMiddle.GetObject<Config>(@"sp_GetConfig", 4, paras);
            if (config != null)
            {
                tbUserName.Text = config.UserName;
                tbPassword.Text = config.Password;// Helper.Base64Decode(config.Password);
                tbServer.Text = config.Server;
                tbDatabase.Text = config.Database;
            }
        }

        private List<string> GetDatabases()
        {
            string connectionString = $"Data Source={tbServer.Text}; Integrated Security=True;";
            var lstDatabases = new List<string>();
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    DataTable databases = con.GetSchema("Databases");
                    foreach (DataRow database in databases.Rows)
                    {
                        string databaseName = database.Field<string>("database_name");
                        lstDatabases.Add(databaseName);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            return lstDatabases;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var paras = new Dictionary<string, object>()
                {
                    ["@username"] = tbUserName.Text,
                    ["@password"] = Helper.Base64Encode(tbPassword.Text),
                    ["@server"] = tbServer.Text,
                    ["@database"] = tbDatabase.Text,
                };
                _repositoryMiddle.ExecuteSQLFromParameters(@"sp_SaveConfigConnection", 4, paras);
                MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lưu thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Method2();
                Console.WriteLine(ex.Message);
            }
        }

        private void exitMI_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            btnCheck.Enabled = false;
            btnCheck.Text = "Đang ktra";
            string connectionString = $"Data Source={tbServer.Text};Initial Catalog={tbDatabase.Text};User ID={tbUserName.Text};Password={tbPassword.Text}";
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    con.Close();
                }
                btnCheck.Enabled = true;
                btnCheck.Text = "Ktra kết nối";
                MessageBox.Show("Kết nối thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
