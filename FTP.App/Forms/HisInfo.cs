using FTP.Common;
using FTP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace FTP
{
    public partial class HisInfo : Form
    {
        public HisInfo(string pmisId)
        {
            InitializeComponent();
            _repositoryLocal = new Repository(connectionStringLocal);
            //_repositoryMiddle = new Repository(connectionStringMiddle);
            _Id = pmisId;
        }

        string _Id;
        private string connectionStringLocal = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
        private string connectionStringMiddle = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;
        private readonly Repository _repositoryLocal;
        //private readonly Repository _repositoryMiddle;

        private void HisConfiguration_Load(object sender, EventArgs e)
        {
            FormatGridView();
            LoadData();
        }

        private void LoadData()
        {
            var paras = new Dictionary<string, object>()
            {
                ["@admeterid"] = _Id
            };
            var lst = _repositoryLocal.GetListFromParameters<ConditionMonitor>(
                @"select UPPERVAL1, LOWERVAL1, UPPERVAL2, LOWERVAL2, METHOD from OP_CONDITION_MONITOR where ADMETERID = @admeterid", 
                1, 
                paras);
            dgvConfiguration.DataSource = lst;
            //dgvConfiguration.Columns["Id"].Visible = false;
            //dgvConfiguration.Columns["Interval"].Visible = false;
            //dgvConfiguration.Columns["Enable"].Visible = false;
            //dgvConfiguration.Columns[3].Visible = false;
            //dgvConfiguration.Columns[1].Width = 150;
            //dgvConfiguration.Columns[2].Width = 280;
            //dgvConfiguration.Columns[3].Width = 120;
        }

        private void FormatGridView()
        {
            dgvConfiguration.Location = new Point(-1, 50);
            dgvConfiguration.Size = new Size(Width - 15, Height - 140);
        }

        //private void ResizeColumns()
        //{
        //    colCode.Width = (int)(Width * 0.2);
        //    colName.Width = (int)(Width * 0.3);
        //    colDevice.Width = (int)(Width * 0.22);
        //}

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
