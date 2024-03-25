using FTP.Common;
using FTP.log;
using FTP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.CodeDom;
using static FTP.MainForm;
using System.Net.Http;
using System.Net.Http.Headers;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;
using DevExpress.XtraPrinting.Native;
using static DevExpress.Data.Helpers.ExpressiveSortInfo;

namespace FTP
{
    public partial class MainForm : Form
    {
        public static MainForm _instance;
        public MainForm()
        {
            //this.BackgroundImage = Properties.Resources.digital;
            InitializeComponent();
            InitContext();
            _instance = this;
        }

        private bool IsLogin = false;

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void miShowForm_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void miLogin_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            var dr = login.ShowDialog();
            IsLogin = login.LoginSuccessful;
            miLogin.Visible = !IsLogin;
            miLogin.Enabled = !IsLogin;
            miLogout.Visible = IsLogin;
            menuItem2.Visible = IsLogin;
        }

        private void miFTPConnection_Click(object sender, EventArgs e)
        {
            if (MdiChildren.Any(a => a.Name == "UploadForm"))
            {
                return;
            }
            UploadForm form = new UploadForm();
            //form.MdiParent = this;
            //form.Show();
            form.Dock = DockStyle.Fill;
            form.ShowDialog();
        }
        private void miManual_Click(object sender, EventArgs e)
        {
            if (MdiChildren.Any(a => a.Name == "ManualUpload"))
            {
                return;
            }
            //ManualUpload form = new ManualUpload();
            //form.Dock = DockStyle.Fill;
            //form.ShowDialog();
        }

        private void DisableControls()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].GetType() != typeof(MdiClient))
                    this.Controls[i].Enabled = false;
            }
            foreach (Form frm in MdiChildren)
                frm.Enabled = false;
            //if (callingsender != null)
            //    callingsender.Enabled = true;
        }
        private void miLogout_Click(object sender, EventArgs e)
        {
            IsLogin = false;
            miLogin.Visible = true;
            miLogin.Enabled = true;
            miLogout.Visible = false;
            menuItem2.Visible = false;

            foreach (Form frm in MdiChildren)
            {
                frm.Dispose();
                frm.Close();
            }
        }

        private void miHisConfiguration_Click(object sender, EventArgs e)
        {
            if (MdiChildren.Any(a => a.Name == "HisConfiguration"))
            {
                return;
            }
            var form = new HisConfiguration();
            //form.MdiParent = this;
            //form.Show();
            form.Dock = DockStyle.Fill;
            form.ShowDialog();
        }

        private void miPmisConfiguration_Click(object sender, EventArgs e)
        {
            if (MdiChildren.Any(a => a.Name == "PmisConfiguration"))
            {
                return;
            }
            var form = new PmisConfiguration();
            //form.MdiParent = this;
            //form.Show();
            form.Dock = DockStyle.Fill;
            form.ShowDialog();
        }

        private void miDatabaseConnection_Click(object sender, EventArgs e)
        {
            var form = new ConnectionConfig();
            //form.MdiParent = this;
            //form.Show();
            form.Dock = DockStyle.Fill;
            form.ShowDialog();
        }

        private void miUserManagement_Click(object sender, EventArgs e)
        {
            var form = new UserManager();
            //form.MdiParent = this;
            //form.Show();
            form.Dock = DockStyle.Fill;
            form.ShowDialog();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon.Visible = true;
            this.Visible = false;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                this.Visible = false;
            }
            if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
            {
                notifyIcon.Visible = false;
                this.Visible = true;
            }
        }


        /*
         * FTP Form 
         */
        #region FTP Form 
        public enum eJobState
        {
            Off = 0,
            Running = 1,
        }
        public enum eJobType
        {
            PUSH = 0,
            PULL = 1,
        }
        public enum eStatusError
        {
            ErrorAll = 0,
            CreateFileSuccessSendFtpError = 1,
            SuccessAll = 2,
            NoData = 3,
            DelFileErr = 4
        }
        public static int JobState = (int)eJobState.Off;


        public static Repository _repositoryMiddle;
        public static string connectionStringMiddle = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;
        public static string connectionStringRuntime = ConfigurationManager.ConnectionStrings["Runtime"].ConnectionString;
        public static Repository _repositoryRuntime;
        public static System.Timers.Timer myTimer = null;
        public static Task myTask = null;
        public static string _ftpLink = string.Empty;
        public static string _userName = string.Empty;
        public static string _password = string.Empty;
        public static string _localFolder = string.Empty;
        public static void Run()
        {
            MainForm._repositoryRuntime = new Repository(MainForm.connectionStringRuntime);
            if (MainForm.myTimer == null)
                MainForm.myTimer = new System.Timers.Timer();
            if (myTask == null)
            {
                var jobType = System.Configuration.ConfigurationManager.AppSettings["JobType"];
                if (!string.IsNullOrEmpty(jobType))
                {
                    if (jobType == MainForm.eJobType.PUSH.ToString())
                    {
                        MainForm.lblHeader.Text = "Phần mềm truyền file FTP và mã hóa dữ liệu";
                        myTask = Task.Factory.StartNew(() => ProcessPush());
                    }
                    else if (jobType == MainForm.eJobType.PULL.ToString())
                    {
                        MainForm.lblHeader.Text = "Phần mềm nhận file FTP, giải mã dữ liệu và ghi vào CSDL";
                        myTask = Task.Factory.StartNew(() => ProcessPull());
                    }
                    else
                    {

                    }
                }

            }

        }
        static void ProcessPush()
        {
            MainForm.myTimer.Elapsed += new ElapsedEventHandler(PushData);
            var backGroundTimer = new System.Timers.Timer();
            var TimeInterval = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["TimeInterval"]);
            backGroundTimer.Interval = TimeInterval * 60 * 1000;
            backGroundTimer.Elapsed += new ElapsedEventHandler(BackGroundJob);
            backGroundTimer.Start();
        }
        private static void BackGroundJob(object sender, ElapsedEventArgs e)
        {
            System.Timers.Timer backGroundTimer = (System.Timers.Timer)sender;
            backGroundTimer.Stop();

            if (!string.IsNullOrEmpty(_localFolder))
            {
                string[] filePaths = Directory.GetFiles(_localFolder);
                for (int i = 0; i < filePaths.Length; i++)
                {
                    int iPush = 0;
                    bool success = true;

                //Day file
                Label_Push:
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(_userName, _password);
                        try
                        {
                            var fi = new FileInfo(filePaths[i]);
                            client.UploadFile(_ftpLink + fi.Name, WebRequestMethods.Ftp.UploadFile, filePaths[i]);
                        }
                        catch (WebException wEx)
                        {
                            iPush++;
                            //MessageBox.Show(wEx.Message, "Push Error");
                            if (iPush < 3)
                                goto Label_Push;// Gui lai
                            else
                            {
                                success = false;
                                //Do Something
                                //Move file to another Folder
                                //fileInfo.MoveTo(FailedFolder + "\\" + fileInfo.Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            iPush++;
                            //MessageBox.Show(ex.Message, "Push Error");
                            if (iPush < 3)
                                goto Label_Push;// Gui lai
                            else
                            {
                                success = false;
                                //Do Something
                                //Move file to another Folder
                                //fileInfo.MoveTo(FailedFolder + "\\" + fileInfo.Name);
                            }
                        }

                        if (File.Exists(filePaths[i]))
                        {
                            if (success)
                            {
                                LogPush((int)eStatusError.SuccessAll, filePaths[i], true);
                                System.IO.File.Delete(filePaths[i]);
                            }
                            else
                                LogPush((int)eStatusError.CreateFileSuccessSendFtpError, filePaths[i], true);
                        }
                        else
                        {
                            if (iPush >= 3)
                                LogPush((int)eStatusError.ErrorAll, filePaths[i], true);
                        }
                    }
                }
            }

            backGroundTimer.Start();
        }
        public static void ProcessPull()
        {
            MainForm.myTimer.Elapsed += new ElapsedEventHandler(PullData);
        }
        public static void ReConfig(string link, string userName, string password, int interval, string folderPath)
        {
            _ftpLink = link;
            _userName = userName;
            _password = password;
            myTimer.Interval = interval * 1000;
            _localFolder = folderPath;
        }
     //   public static string sqlRuntime = @"select p.Folder, l.LogDate, l.LogTime, l.TagName, l.LastValue from BwAnalogTable l
					//INNER JOIN Params p ON l.TagName = p.TagName
					//where p.Interval is not null and p.Enable = 1
					//and l.LogDate = CONVERT(VARCHAR, DATEADD(MI, -1, @datetime), 11) 
					//and l.LogTime = SUBSTRING(CONVERT(VARCHAR, DATEADD(MI, -1, @datetime), 20), 12, 5) + ':00'
     //               and (DATEPART(MI, GETDATE())-1)%p.Interval=0";
        public static string sqlRuntime = @"sp_GetBwAnalogTable";
        private static Dictionary<string, Mapping> mappings;

        static void PushData(object sender, ElapsedEventArgs e)
        {
            System.Timers.Timer myTimer = (System.Timers.Timer)sender;
            myTimer.Stop();

            var now = DateTime.Now;
            var dtMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Local);
            if (now.Minute % 5 == 1)
            {
                DateTime? lastQuery = GetLastQuery();
                if (lastQuery == null || dtMinute > lastQuery)
                {
                    var parameters = new Dictionary<string, object>()
                    {
                        ["@datetime"] = now,
                    };
                    //var lstAnalogs = MainForm._repositoryRuntime.GetListFromParameters<AnalogTable>(sqlRuntime, 1, parameters);
                    var lstAnalogs = MainForm._repositoryRuntime.GetListFromParameters<AnalogTable>(sqlRuntime, 4, parameters);
                    var PushingDatas = lstAnalogs.GroupBy(a => a.Folder).Select(o => new
                    {
                        Folder = o.Key,
                        AnalogTables = o.ToArray()
                    }).ToArray();

                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    if (PushingDatas != null && PushingDatas.Length > 0)
                    {
                        var options = new ParallelOptions()
                        {
                            MaxDegreeOfParallelism = PushingDatas.Length
                        };
                        Parallel.For(0, PushingDatas.Length, options, i =>
                        {
                            PushFtp(now, (PushingDatas[i]?.Folder, PushingDatas[i]?.AnalogTables));
                            Thread.Sleep(10);
                        });
                        UpsertLastQuery(dtMinute);
                    }
                    stopWatch.Stop();
                    //MessageBox.Show($"Time Taken to Execute Parallel For Loop in miliseconds {stopWatch.ElapsedMilliseconds}");
                }
            }

            myTimer.Start();
        }

        private static DateTime? GetLastQuery()
        {
            string sql = @"select top 1 LogTime from LastQuery";
            var result = _repositoryMiddle.GetObject<LastQuery>(sql, 1, null);
            return result?.LogTime;
        }

        private static DateTime? GetLastLogDateTime(DateTime lastQuery)
        {
            string sql = @"sp_GetLastLogDate";
            var parameters = new Dictionary<string, object>()
            {
                ["@LastQuery"] = lastQuery,
            };
            var result = _repositoryMiddle.GetObject<LastQuery>(sql, 4, parameters);
            return result?.LogTime;
        }

        static void UpsertLastQuery(DateTime now)
        {
            string sql = @"IF (NOT EXISTS (SELECT 1 FROM LastQuery)) 
                    BEGIN 
                        INSERT INTO LastQuery (LogTime) values (@logtime) 
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE LastQuery SET LogTime = @logtime
                    END";

            var parameters = new Dictionary<string, object>()
            {
                ["@logtime"] = now,
            };
            var result = _repositoryMiddle.ExecuteSQLFromParameters(sql, 1, parameters);
        }

        private static void PushFtp(DateTime datetime, (string Folder, AnalogTable[] AnalogTables) data)
        {
            int iPush = 0;
            bool success = true;
            if (data.AnalogTables != null && data.AnalogTables.Length > 0)
            {
                var sb = new StringBuilder();

                string folderPath = _localFolder;//AppDomain.CurrentDomain.BaseDirectory;
                string fileName = datetime.ToString("HHmmssddMMyyyy") + "_" + Guid.NewGuid().ToString().Substring(0, 8) + ".txt";
                sb = new StringBuilder();
                sb.AppendLine("Tagname,TimeStamp,Value");
                for (int i =0; i < data.AnalogTables.Length; i++)
                {
                    string dtString = data.AnalogTables[i].LogDate.Replace("/", "-") + " " + data.AnalogTables[i].LogTime;
                    sb.AppendLine(data.AnalogTables[i].TagName + "," + dtString + "," + data.AnalogTables[i].LastValue.ToString());
                }
                string originalContent = sb.ToString();
                var fullFileName = Path.Combine(folderPath, fileName);
                // Ghi ra file
                File.WriteAllText(System.IO.Path.Combine(folderPath, fileName), originalContent);

            //Day file
            Label_Push:
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(_userName, _password);
                    try
                    {
                        CreateFTPDirectory(_ftpLink + "/" + data.Folder, datetime.ToString("yyyyMMdd"), _userName, _password);
                        client.UploadFile(_ftpLink + data.Folder + "//" + datetime.ToString("yyyyMMdd") + "//" + fileName, WebRequestMethods.Ftp.UploadFile, fullFileName);
                    }
                    catch (WebException wEx)
                    {
                        iPush++;
                        //MessageBox.Show(wEx.Message, "Push Error");
                        if (iPush < 3)
                            goto Label_Push;// Gui lai
                        else
                        {
                            success = false;
                            //Do Something
                            //Move file to another Folder
                            //fileInfo.MoveTo(FailedFolder + "\\" + fileInfo.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        iPush++;
                        //MessageBox.Show(ex.Message, "Push Error");
                        if (iPush < 3)
                            goto Label_Push;// Gui lai
                        else
                        {
                            success = false;
                            //Do Something
                            //Move file to another Folder
                            //fileInfo.MoveTo(FailedFolder + "\\" + fileInfo.Name);
                        }
                    }

                    if (File.Exists(fullFileName))
                    {
                        if (success)
                        {
                            LogPush((int)eStatusError.SuccessAll, fullFileName);
                            try
                            {
                                System.IO.File.Delete(fullFileName);
                            }
                            catch (Exception exc) { }
                        }
                        else
                            LogPush((int)eStatusError.CreateFileSuccessSendFtpError, fullFileName);
                    }
                    else
                    {
                        if (iPush >= 3)
                            LogPush((int)eStatusError.ErrorAll, fullFileName);
                    }
                }
            }
            else
            {
                //LogPush((int)eStatusError.NoData, "");
            }
        }
        static bool CreateFTPDirectory(string directory, string subFolder, string _username, string _password)
        {
            try
            {
                //create the directory
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(directory));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(_username, _password);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
                ///////////////////////////////////////////
                if (!string.IsNullOrEmpty(subFolder))
                {
                    FtpWebRequest requestSub = (FtpWebRequest)FtpWebRequest.Create(new Uri(directory + "//" + subFolder));
                    requestSub.Method = WebRequestMethods.Ftp.MakeDirectory;
                    requestSub.Credentials = new NetworkCredential(_username, _password);
                    requestSub.UsePassive = true;
                    requestSub.UseBinary = true;
                    requestSub.KeepAlive = false;
                    FtpWebResponse response2 = (FtpWebResponse)requestSub.GetResponse();
                    Stream ftpStream2 = response.GetResponseStream();
                    ftpStream2.Close();
                    response2.Close();
                }
                
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }
        public static void LogPush(int statusError, string fileName, bool background = false)
        {
            string log = string.Empty;
            StringBuilder sb;
            switch (statusError)
            {
                case (int)eStatusError.SuccessAll:
                    log = string.Format("{0}: " + "Tạo file {1} thành công! Gửi FTP thành công!", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), fileName);
                    Logging.Instance.Logger.Info(log);
                    sb = new StringBuilder();
                    if (background)
                        sb.AppendLine("Xử lý ngầm: " + log);
                    else
                        sb.AppendLine(log);

                    if (!string.IsNullOrEmpty(MainForm._instance.txtLog.Text))
                        sb.Append(MainForm._instance.txtLog.Text);
                    MainForm._instance.SetText(sb.ToString());
                    break;
                case (int)eStatusError.CreateFileSuccessSendFtpError:
                    //log = string.Format("{0}: " + "Tạo file {1} thành công! Gửi FTP lỗi!", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), fileName);
                    //Logging.Instance.Logger.Info(log);
                    //sb = new StringBuilder();
                    //if (background)
                    //    sb.AppendLine("Xử lý ngầm: " + log);
                    //else
                    //    sb.AppendLine(log);

                    //if (!string.IsNullOrEmpty(MainForm._instance.txtLog.Text))
                    //    sb.Append(MainForm._instance.txtLog.Text);
                    //MainForm._instance.SetText(sb.ToString());
                    break;
                case (int)eStatusError.ErrorAll:
                    log = string.Format("{0}: " + "Tạo file {1} lỗi!", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), fileName);
                    Logging.Instance.Logger.Info(log);
                    sb = new StringBuilder();
                    if (background)
                        sb.AppendLine("Xử lý ngầm: " + log);
                    else
                        sb.AppendLine(log);

                    if (!string.IsNullOrEmpty(MainForm._instance.txtLog.Text))
                        sb.Append(MainForm._instance.txtLog.Text);
                    MainForm._instance.SetText(sb.ToString());
                    break;
                case (int)eStatusError.NoData:
                    log = string.Format("{0}: " + "Không có dữ liệu!", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"));
                    Logging.Instance.Logger.Info(log);
                    sb = new StringBuilder();                   
                    sb.AppendLine(log);

                    if (!string.IsNullOrEmpty(MainForm._instance.txtLog.Text))
                        sb.Append(MainForm._instance.txtLog.Text);
                    MainForm._instance.SetText(sb.ToString());
                    break;
                default:
                    break;
            }
        }
        static void PullData(object sender, ElapsedEventArgs e)
        {
            System.Timers.Timer myTimer = (System.Timers.Timer)sender;
            myTimer.Stop();
            try
            {
                // get file from FTP Server
                var nowFolder = DateTime.Today.ToString("yyyyMMdd");
                var folderAndFileList = GetFolderAndFileList(nowFolder);
                if (folderAndFileList != null && folderAndFileList.Length > 0)
                {
                    var options = new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = folderAndFileList.Length
                    };
                    Parallel.For(0, folderAndFileList.Length, options, i =>
                    {
                        DownloadFromFolder(folderAndFileList[i]);
                        Thread.Sleep(100);
                    });
                }
            }
            catch (Exception ex)
            {
                myTimer.Start();
                return;
            }
            finally
            {
            }

            myTimer.Start();
        }

        static (string folder, string timeFolder, string[] lstFiles)[] GetFolderAndFileList(string nowFolder)
        {
            (string, string, string[])[] FolderAndFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpLink));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(_userName, _password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                var listFolders = result.ToString().Split('\n');
                
                FolderAndFiles = new(string, string, string[])[listFolders.Length];
                for (int i = 0; i < listFolders.Length; i++)
                {
                    string[] files = GetFileList(_ftpLink + "//" + listFolders[i] + "//" + nowFolder);
                    FolderAndFiles[i] = (listFolders[i], nowFolder, files);
                }

                return FolderAndFiles;
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                FolderAndFiles = null;
                return FolderAndFiles;
            }
        }

        static string[] GetFileList(string folderLink)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(folderLink));
                reqFTP.UseBinary = true;
                //reqFTP.Credentials = new NetworkCredential(_userName, _password);
                reqFTP.Credentials = new NetworkCredential(_userName, _password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                var listFolders = result.ToString().Split('\n');
                return listFolders;
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                downloadFiles = null;
                return downloadFiles;
            }
        }
        static async void DownloadFromFolder((string folder, string timeFolder, string[] lstFiles) folderAndFileList)
        {
            bool downSuccess = true;
            int iDown = 0;
            //var now = DateTime.Today.ToString("yyyyMMdd");
            //var currentFolder = _localFolder + "\\" + now;
            if (!Directory.Exists(_localFolder + "\\" + folderAndFileList.folder))
                Directory.CreateDirectory(_localFolder + "\\" + folderAndFileList.folder);
            if (!Directory.Exists(_localFolder + "\\" + folderAndFileList.folder + "\\" + folderAndFileList.timeFolder))
                Directory.CreateDirectory(_localFolder + "\\" + folderAndFileList.folder + "\\" + folderAndFileList.timeFolder);




            string token = string.Empty;
            string json = await Login(System.Configuration.ConfigurationManager.AppSettings["PushDataUrl"], "admin", "admin<<<<!@@//@@!>>>>123");
            var objectData = JsonSerializer.Deserialize<JsonObject>(json);
            if (objectData?["status"] != null && objectData["status"]?.ToString() == "1")
            {
                token = objectData["token"]?.ToString();
            }

            mappings = (await GetMapping(token)).ToDictionary(i => i.tagid);

            if (!string.IsNullOrEmpty(token))
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = folderAndFileList.lstFiles.Length
                };
                Parallel.For(0, folderAndFileList.lstFiles.Length, options, i =>
                {
                    Download(token, folderAndFileList.folder + "\\" + folderAndFileList.timeFolder, folderAndFileList.folder + "//" + folderAndFileList.timeFolder + "//" + folderAndFileList.lstFiles[i], folderAndFileList.lstFiles[i]);
                    Thread.Sleep(10);
                });
            }
        }

        static async void Download(string token, string folder, string file, string fileName)
        {
            bool downSuccess = true;
            int iDown = 0;
            string fullFileNameSave = _localFolder + "\\" + folder + "\\" + fileName;
            string fullFileNameFtpDown = _ftpLink + file;
        Label_Down:
            try
            {
                string uri = _ftpLink + file;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    return;
                }
                FtpWebRequest downReqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(fullFileNameFtpDown));
                downReqFTP.Credentials = new NetworkCredential(_userName, _password);
                downReqFTP.KeepAlive = false;
                downReqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                downReqFTP.UseBinary = true;
                downReqFTP.Proxy = null;
                downReqFTP.UsePassive = false;
                FtpWebResponse downResponse = (FtpWebResponse)downReqFTP.GetResponse();
                Stream responseStream = downResponse.GetResponseStream();
                using (FileStream writeStream = new FileStream(fullFileNameSave, FileMode.Create))
                {
                    int Length = 2048;
                    Byte[] buffer = new Byte[Length];
                    int bytesRead = responseStream.Read(buffer, 0, Length);
                    while (bytesRead > 0)
                    {
                        writeStream.Write(buffer, 0, bytesRead);
                        bytesRead = responseStream.Read(buffer, 0, Length);
                    }
                }

                // Lay content ra decode va luu vao DB
                //string content = System.Text.Encoding.UTF8.GetString(buffer);

                string content = string.Empty;
                using (StreamReader sr = new StreamReader(fullFileNameSave))
                {
                    content = sr.ReadToEnd();
                }

                await CallApiToSaveDB(token, content);

                //string decryptData = Helper.DecryptRSA(Helper.PassCode, content.Replace("\0", string.Empty));
                //var arrHis = decryptData.Split('\n');
                //if (arrHis != null && arrHis.Length > 1)
                //{
                //    string tableName = "";
                //    DateTime appstartday = DateTime.Now;
                //    LocalDAL dal = new LocalDAL();
                //    dal.checkExistTableInDB(ref tableName, ref appstartday);
                //    // Bo qua dong tieu de
                //    for (int i = 1; i < arrHis.Length; i++)
                //    {
                //        string hisContent = arrHis[i];
                //        var his = hisContent.Split(',');
                //        if (his.Length >= 3)
                //        {
                //            // GHEP DAL
                //            string tagName = his[0];
                //            string dateTime = his[1];
                //            string value = his[2];

                //            if (value != null)
                //            {
                //                value = value.Replace("\r", "").Replace("\n", "");
                //            }

                //            String ameterid = getAmeterid(tagName);
                //            if (ameterid != null && !ameterid.Trim().Equals(""))
                //            {
                //                dal.insertToDB(tableName, appstartday, ameterid, DateTime.Parse(dateTime), value);
                //            }
                //        }
                //    }
                //}



                //writeStream.Close();
                downResponse.Close();
            }
            catch (WebException wEx)
            {
                iDown++;
                if (iDown < 3)
                    goto Label_Down;
                else
                    downSuccess = false;
            }
            catch (Exception ex)
            {
                iDown++;
                //MessageBox.Show(ex.Message, "Download Error");
                if (iDown < 3)
                    goto Label_Down;
                else
                    downSuccess = false;
            }

            if (downSuccess)
            {
                if (File.Exists(fullFileNameSave))
                    LogPull((int)eStatusError.SuccessAll, fullFileNameSave);
                else
                    LogPull((int)eStatusError.CreateFileSuccessSendFtpError, fullFileNameSave);
            }
            else
            {
                if (iDown >= 3)
                    LogPull((int)eStatusError.ErrorAll, fullFileNameSave);
            }



            int iDel = 0;
        Label_Del:
            // Delete file
            if (downSuccess)
            {
                try
                {
                    FtpWebRequest delReqFTP;
                    delReqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpLink + file));
                    delReqFTP.Credentials = new NetworkCredential(_userName, _password);
                    delReqFTP.KeepAlive = false;
                    delReqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                    delReqFTP.UseBinary = true;
                    delReqFTP.Proxy = null;
                    delReqFTP.UsePassive = false;

                    using (FtpWebResponse delResponse = (FtpWebResponse)delReqFTP.GetResponse())
                    {
                        //MessageBox.Show(delResponse.StatusDescription); 
                    }
                }
                catch (WebException wEx)
                {
                    iDel++;
                    //MessageBox.Show(wEx.Message, "Delete Error");
                    if (iDel < 3)
                        goto Label_Del;
                }
                catch (Exception ex)
                {
                    iDel++;
                    //MessageBox.Show(ex.Message, "Delete Error");
                    if (iDel < 3)
                        goto Label_Del;
                }
            }
        }

        private static async Task CallApiToSaveDB(string token, string content)
        {
            var arrHis = content.Split('\n');
            if (arrHis != null && arrHis.Length > 1)
            {
                // Bo qua dong tieu de
                SaveData saveData = new SaveData();
                saveData.lstDataSave = new List<DataRaw>();
                for (int i = 1; i < arrHis.Length; i++)
                {
                    string hisContent = arrHis[i];
                    var his = hisContent.Split(',');
                    if (his.Length >= 3)
                    {
                        if (mappings != null && mappings.ContainsKey(his[0]))
                        {
                            DataRaw data = new DataRaw() { maThongSoNhap = mappings[his[0]]?.admeterid, thoiDiem = his[1], giaTri = his[2] };
                            saveData.lstDataSave.Add(data);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(token))
                    await PushDataApi(token, saveData);
            }
        }

        private static async Task PushDataApi(string token, SaveData saveData)
        { 
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string url = System.Configuration.ConfigurationManager.AppSettings["PushDataUrl"];
                var jsonData = System.Text.Json.JsonSerializer.Serialize (saveData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                var rerult = response.Content.ReadAsStringAsync().Result;
                var objectResult = System.Text.Json.JsonSerializer.Deserialize<JsonObject>(rerult);
                if (objectResult?["status"] != null && objectResult["status"]?.ToString() == "1")
                {
                    //ThanhCong;
                }
            }
        }

        private static async Task<string> Login(string url, string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var json = System.Text.Json.JsonSerializer.Serialize(new { userId = username, password = password });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                var rerult = response.Content.ReadAsStringAsync().Result;
                return rerult;
            }
        }

        private static async Task<Mapping[]> GetMapping(string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string url = System.Configuration.ConfigurationManager.AppSettings["GetMappingUrl"];
                //var jsonData = System.Text.Json.JsonSerializer.Serialize(saveData);
                //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, null);
                var rerult = response.Content.ReadAsStringAsync().Result;
                var objectResult = System.Text.Json.JsonSerializer.Deserialize<MappingList>(rerult);
                if (objectResult != null && objectResult.status == 1)
                {
                    return await Task.FromResult(objectResult.data);
                }
            }
            return null;
        }
            
        public static void LogPull(int statusError, string fileName)
        {
            string log = string.Empty;
            StringBuilder sb;
            switch (statusError)
            {
                case (int)eStatusError.SuccessAll:
                    log = string.Format("{0}: " + "Tải file {1} thành công! Ghi vào dữ liệu thành công!", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), fileName);
                    Logging.Instance.Logger.Info(log);
                    sb = new StringBuilder();
                    sb.AppendLine(log);
                    if (!string.IsNullOrEmpty(MainForm._instance.txtLog.Text))
                        sb.Append(MainForm._instance.txtLog.Text);
                    MainForm._instance.SetText(sb.ToString());
                    break;
                case (int)eStatusError.CreateFileSuccessSendFtpError:
                    log = string.Format("{0}: " + "Đăng nhập FTP thành công! Tải file {1} lỗi!", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), fileName);
                    Logging.Instance.Logger.Info(log);
                    sb = new StringBuilder();
                    sb.AppendLine(log);
                    if (!string.IsNullOrEmpty(MainForm._instance.txtLog.Text))
                        sb.Append(MainForm._instance.txtLog.Text);
                    MainForm._instance.SetText(sb.ToString());
                    break;
                case (int)eStatusError.ErrorAll:
                    log = string.Format("{0}: " + "Đăng nhập FTP thất bại!", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"));
                    Logging.Instance.Logger.Info(log);
                    sb = new StringBuilder();
                    sb.AppendLine(log);
                    if (!string.IsNullOrEmpty(MainForm._instance.txtLog.Text))
                        sb.Append(MainForm._instance.txtLog.Text);
                    MainForm._instance.SetText(sb.ToString());
                    break;
                default:
                    break;
            }
        }
        static string getAmeterid(string tagName)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@tagName"] = tagName
            };
            var _config = _repositoryMiddle.GetObject<HisConfig>(@"sp_GetAmeterid", 4, paras);
            if (_config != null)
            {
                return _config.Ameterid;
            }
            return null;
        }

        delegate void SetTextLog(string text);

        public void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtLog.InvokeRequired)
            {
                SetTextLog d = new SetTextLog(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtLog.Text = text;
            }
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            var paras = new Dictionary<string, object>()
            {
                ["@type"] = 1
            };
            MainForm._repositoryMiddle = new Repository(MainForm.connectionStringMiddle);
            var _config = MainForm._repositoryMiddle.GetObject<Config>(@"sp_GetConfig", 4, paras);

            btnMainStop.Enabled = false;
            btnMainStart.Enabled = true;

            if (_config != null)
            {
                if (!string.IsNullOrEmpty(_config.UserName) && !string.IsNullOrEmpty(_config.Password) && !string.IsNullOrEmpty(_config.Destination) && !string.IsNullOrEmpty(_config.Source) && _config.Timer > 0)
                {
                    MainForm.Run();


                    MainForm.ReConfig(_config.Destination, _config.UserName, FTP.Common.Helper.Base64Decode(_config.Password), _config.Timer, _config.Source);
                    MainForm.JobState = (int)MainForm.eJobState.Running;
                    btnMainStop.Enabled = true;
                    btnMainStart.Enabled = false;
                    MainForm.myTimer.Start();
                }
            }
        }

        private void btnMainStart_Click(object sender, EventArgs e)
        {
            MainForm.JobState = (int)MainForm.eJobState.Running;
            MainForm._instance.btnMainStop.Enabled = true;
            MainForm._instance.btnMainStart.Enabled = false;
            MainForm.myTimer.Start();
        }

        private void bntMainStop_Click(object sender, EventArgs e)
        {
            MainForm.JobState = (int)MainForm.eJobState.Off;
            MainForm._instance.btnMainStop.Enabled = false;
            MainForm._instance.btnMainStart.Enabled = true;
            MainForm.myTimer.Stop();
        }
    }
}
