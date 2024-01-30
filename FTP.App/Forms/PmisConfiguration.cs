using FTP.Common;
using FTP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FTP
{
    public partial class PmisConfiguration : Form
    {
        public PmisConfiguration()
        {
            InitializeComponent();
            _repositoryLocal = new Repository(connectionStringLocal);
            _repositoryMiddle = new Repository(connectionStringMiddle);
        }

        private string connectionStringLocal = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
        private string connectionStringMiddle = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;
        private readonly Repository _repositoryLocal;
        private readonly Repository _repositoryMiddle;

        private void PmisConfiguration_Load(object sender, EventArgs e)
        {
            FormatLayout();
            LoadTreeView();
            LoadGridView();
        }

        private void PopulateTreeView(string parentId, TreeNode parentNode, IEnumerable<Pmis_TreeConfig> lst)
        {
            var filteredItems = lst.Where(item => item.ASSETID_PARENT == parentId);

            TreeNode childNode;
            foreach (var item in filteredItems)
            {
                if (parentNode == null)
                    childNode = trvAssets.Nodes.Add(item.ASSETID, item.ASSETDESC);
                else
                    childNode = parentNode.Nodes.Add(item.ASSETID, item.ASSETDESC);

                PopulateTreeView(item.ASSETID, childNode, lst);
            }
        }

        private void LoadTreeView()
        {
            //var sql = @"select * from A_ASSET";
            //var trees = _repositoryLocal.GetListFromParameters<Pmis_TreeConfig>(sql, 1, null);
            //PopulateTreeView(null, null, trees);
            var sql = @"select a.ASSETID,a.ASSETID_PARENT,a.ASSETID_ORG,a.ASSETDESC,a.ASSETORD 
                        from A_ASSET a where exists (select assetid from A_ASSET_METER_DYN where ASSETID=a.ASSETID)
                        order by ASSETORD";
            var trees = _repositoryLocal.GetListFromParameters<Pmis_TreeConfig>(sql, 1, null);
            if (trees != null)
            {
                foreach (var item in trees)
                {                    
                      trvAssets.Nodes.Add(item.ASSETID, item.ASSETDESC);                    
                }
            }
        }

        private void LoadGridView()
        {
            if (trvAssets.SelectedNode == null)
            {
                return;
            }
            //var lstString = new List<string>();
            //var childNodes = GetAllChildren(trvAssets.SelectedNode, lstString);
            //List<Pmis_GridConfig> nodes;
            //if (childNodes.Count == 0)
            //{
            //    var sql = @"select a.ASSETDESC, m.METERDESC, m.UOMID, d.ADMETERID
            //                from A_ASSET_METER_DYN d
            //                inner join A_METER m on d.METERID = m.METERID
            //                inner join A_ASSET a on a.ASSETID = d.ASSETID
            //                where a.ASSETID = @assetId";
            //    var parameters = new Dictionary<string, object>()
            //    {
            //        ["@assetId"] = trvAssets.SelectedNode.Name
            //    };
            //    nodes = _repositoryLocal.GetListFromParameters<Pmis_GridConfig>(sql, 1, parameters);
            //}
            //else
            //{
            //    var sql = string.Format(@"select a.ASSETDESC, m.METERDESC, m.UOMID, d.ADMETERID
            //                                from A_ASSET_METER_DYN d
            //                                inner join A_METER m on d.METERID = m.METERID
            //                                inner join A_ASSET a on a.ASSETID = d.ASSETID
            //                                where a.ASSETID in ('{0}')", string.Join("', '", childNodes));
            //    nodes = _repositoryLocal.GetListFromParameters<Pmis_GridConfig>(sql, 1, null);
            //}
            List<Pmis_GridConfig> nodes;
            var sql = @"select m.METERDESC, m.UOMID, d.ADMETERID
                            from A_ASSET_METER_DYN d
                            inner join A_METER m on d.METERID = m.METERID
                            inner join A_ASSET a on a.ASSETID = d.ASSETID
                            where a.ASSETID = @assetId";
            var parameters = new Dictionary<string, object>()
            {
                ["@assetId"] = trvAssets.SelectedNode.Name
            };
            nodes = _repositoryLocal.GetListFromParameters<Pmis_GridConfig>(sql, 1, parameters);

            if(nodes!=null && nodes.Count > 0)
            {
                foreach (var item in nodes)
                {
                    sql = @"SELECT top 1 TagName FROM Params where Ameterid = @AmeterId";
                    parameters = new Dictionary<string, object>()
                    {
                        ["@AmeterId"] = item.ADMETERID
                    };
                    var tagName = _repositoryMiddle.GetObject(sql, 1, parameters);
                    if (tagName != null)
                        item.TAGNAME = tagName.ToString();
                }
            }

            dgvConfiguration.DataSource = nodes;
            trvAssets.ExpandAll();
        }

        private void FormatLayout()
        {
            panel1.Location = new Point(20, 40);
            panel1.Size = new Size(240, Height - 150);
            panel2.Location = new Point(270, 40);
            panel2.Size = new Size(Width - 300, Height - 150);
            btnImport.Location = new Point(panel2.Location.X + panel2.Width - btnImport.Width, Height - 90);
            btnExport.Location = new Point(panel2.Location.X + panel2.Width - btnImport.Width * 2 - 20, Height - 90);
        }

        private List<string> GetAllChildren(TreeNode parentNode, List<string> nodes)
        {
            List<string> lstNodes = nodes;
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                nodes.Add(childNode.Name);
                var range = GetAllChildren(childNode, nodes);
                lstNodes.AddRange(range);
            }
            return lstNodes;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ofdExport.ShowDialog();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ofdImport.ShowDialog();
        }

        private void trvAssets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadGridView();
        }

        private void dgvConfiguration_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvConfiguration.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var pmisId = dgvConfiguration["colId", e.RowIndex].Value.ToString();
                switch (e.ColumnIndex)
                {                    
                    case 2: // mapping
                        HisSelect select = new HisSelect();
                        select.SendCode += (hisId, code) =>
                        {
                            dgvConfiguration["colHisCode", e.RowIndex].Value = code;
                            SaveMapping(pmisId, hisId);
                        };
                        select.ShowDialog();
                        break;
                    case 3: // show info
                        HisInfo info = new HisInfo(pmisId);
                        info.ShowDialog();
                        break;
                    default:
                        break;
                }
            }
        }

        private void SaveMapping(string pmisId, string hisId)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@Id"] = hisId,
                ["@ameterId"] = pmisId
            };
            _repositoryMiddle.ExecuteSQLFromParameters("sp_SaveMapping", 4, paras);
        }
    }
}
