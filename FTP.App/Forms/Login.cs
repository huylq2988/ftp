using FTP.Common;
using FTP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace FTP
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            //_repositoryLocal = new Repository(connectionStringLocal);
            _repositoryMiddle = new Repository(connectionStringMiddle);
            //tbUser.Text = "";
            //tbPassword.Text = "";
        }
        //private string connectionStringLocal = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
        private string connectionStringMiddle = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;
        //private readonly Repository _repositoryLocal;
        private readonly Repository _repositoryMiddle;
        //private string connectionString = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;

        public bool LoginSuccessful { get; set; } = false;

        private User GetUser(string username)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@userid"] = username,
            };
            var user = _repositoryMiddle.GetObject<User>("select * from [users] where Userid=@userid", 1, paras);
            return user;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String password = Helper.CreateMD5("admin");
            var user = GetUser(tbUser.Text);
            if (string.IsNullOrWhiteSpace(tbUser.Text))
            {
                label1.Text = "Chưa nhập tên người dùng";
            }
            else if (string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                label1.Text = "Chưa nhập mật khẩu";
            }
            else if (user == null || user.Password != Helper.CreateMD5(tbPassword.Text))
            {
                label1.Text = "Sai tên người dùng hoặc mật khẩu";
            }
            else
            {
                Close();
                LoginSuccessful = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbUser_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "";
        }
    }
}
