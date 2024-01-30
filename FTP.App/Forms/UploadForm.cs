using FTP.Common;
using FTP.Model;
//using Limilabs.FTP.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace FTP
{
    public partial class UploadForm : Form
    {
        public UploadForm()
        {
            InitializeComponent();
            
            if (MainForm.JobState == (int)MainForm.eJobState.Off)
            {
                btnStop.Enabled = false;
                btnStart.Enabled = true;
                MainForm._instance.btnMainStop.Enabled = false;
                MainForm._instance.btnMainStart.Enabled = true;
                btnSave.Enabled = true;
            }
            else if(MainForm.JobState == (int)MainForm.eJobState.Running)
            {
                btnStop.Enabled = true;
                btnStart.Enabled = false;
                MainForm._instance.btnMainStop.Enabled = true;
                MainForm._instance.btnMainStart.Enabled = false;
                btnSave.Enabled = false;
            }            
        }

        private void UploadForm_Load(object sender, EventArgs e)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@type"] = 1
            };
            var _config = MainForm._repositoryMiddle.GetObject<Config>(@"sp_GetConfig", 4, paras);
            if (_config != null)
            {
                tbUser.Text = _config.UserName;
                tbPassword.Text = _config.Password;// Helper.Base64Decode(_config.Password);
                tbDestination.Text = _config.Destination;
                nudTimer.Value = _config.Timer;
                tbSource.Text = _config.Source;
            }

            //MainForm.Run();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var paras = new Dictionary<string, object>()
                {
                    ["@username"] = tbUser.Text,
                    ["@password"] = Helper.Base64Encode(tbPassword.Text),
                    ["@source"] = tbSource.Text,
                    ["@destination"] = tbDestination.Text,
                    ["@timer"] = (int)nudTimer.Value,
                };
                MainForm._repositoryMiddle.ExecuteSQLFromParameters(@"sp_SaveConfig", 4, paras);
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            MainForm.ReConfig(tbDestination.Text, tbUser.Text, tbPassword.Text, (int)nudTimer.Value, tbSource.Text);
            MainForm.JobState = (int)MainForm.eJobState.Running;
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            MainForm._instance.btnMainStop.Enabled = true;
            MainForm._instance.btnMainStart.Enabled = false;
            btnSave.Enabled = false;
            MainForm.myTimer.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            MainForm.JobState = (int)MainForm.eJobState.Off;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            MainForm._instance.btnMainStop.Enabled = false;
            MainForm._instance.btnMainStart.Enabled = true;
            btnSave.Enabled = true;
            MainForm.myTimer.Stop();
        }
    }
}
