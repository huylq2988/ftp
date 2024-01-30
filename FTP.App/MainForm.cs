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
            ManualUpload form = new ManualUpload();
            //form.MdiParent = this;
            //form.Show();
            form.Dock = DockStyle.Fill;
            form.ShowDialog();
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
        public static string sqlRuntime = @"select DateTime,TagName,Value
                    from History                    
                    where TagName =@TagName and DateTime>@Datetime and DATEPART(MI,DateTime)%@Interval=0
                    order by DateTime";

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
            backGroundTimer.Interval = TimeInterval * 1000;
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
        static void PushData(object sender, ElapsedEventArgs e)
        {
            System.Timers.Timer myTimer = (System.Timers.Timer)sender;
            myTimer.Stop();
            
            //Select Data
            var sqlParam = @"select ID,TagName,Description,Unit,Device,Ameterid,Interval,Enable
                    from Params                    
                    where Interval is not null and Enable=1";
            

            var lstParams = MainForm._repositoryMiddle.GetListFromParameters<HisConfig>(sqlParam, 1, null);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            if (lstParams != null && lstParams.Count > 0)
            {
                //for (int i = 0; i < lstParams.Count; i++)
                //{
                //    DoQueryAndPushFtp(lstParams[i]);
                //}
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 5
                };
                int n = 10;
                Parallel.For(0, lstParams.Count, options, i =>
                {
                    DoQueryAndPushFtp(lstParams[i]);
                    Thread.Sleep(10);
                });

            }
            stopWatch.Stop();
            MessageBox.Show($"Time Taken to Execute Parallel For Loop in miliseconds {stopWatch.ElapsedMilliseconds}");
            
            //MessageBox.Show($"Time Taken to Sequentially in miliseconds {stopWatch.ElapsedMilliseconds}");
            myTimer.Start();
        }

        private static void DoQueryAndPushFtp(HisConfig config)
        {
            var lstHisRuntime = new List<HisRuntime>();
            int iPush = 0;
            bool success = true;
            LastRead lastRead = getLastRead(config.TagName);
            var parameters = new Dictionary<string, object>()
            {
                ["@TagName"] = config.TagName,
                ["@Datetime"] = lastRead != null ? lastRead.LastReadTime : DateTime.Now.AddHours(-1).Date,//lastReadTime(lstParams[k].TagName),
                ["@Interval"] = config.Interval
            };

            lstHisRuntime.AddRange(MainForm._repositoryRuntime.GetListFromParameters<HisRuntime>(sqlRuntime, 1, parameters));
            //if(lstHis==null || lstHis.Count == 0)
            //{
            //    lstHis = MainForm._repositoryRuntime.GetListFromParameters<HisRuntime>(sqlRuntime, 1, parameters);
            //}
            //if (lstHis != null && lstHis.Count > 0)
            //{
            //Boolean bExists = false;
            //DateTime currDate = DateTime.Now;
            //DateTime lastDate = lastRead!=null ? lastRead.LastReadTime.AddMinutes(lstParams[k].Interval): DateTime.Now.AddHours(-1).Date;
            //while (lastDate < currDate)
            //{
            //    bExists = false;                            
            //    for (int j = 0; j < lstHis.Count; j++)
            //    {
            //        if(DateTime.Compare(lastDate, lstHis[j].DateTime) == 0)
            //        {
            //            bExists = true;
            //            lstHisRuntime.Add(lstHis[j]);
            //            lstHis.Remove(lstHis[j]);
            //            break;
            //        }
            //    }
            //    if (!bExists)
            //    {
            //        HisRuntime hisRuntime = getLastRuntime(lstParams[k].TagName, lastRead != null ? lastRead.LastReadTime : DateTime.Now.AddHours(-1).Date, lastDate);
            //        if (hisRuntime != null)
            //            lstHisRuntime.Add(hisRuntime);
            //    }
            //    lastDate = lastDate.AddMinutes(lstParams[k].Interval);
            //}                        
            //    //lstHisRuntime.AddRange(lstHis);
            //}

            if (lstHisRuntime != null && lstHisRuntime.Count > 0)
            {
                var sb = new StringBuilder();
                int rowPerPage = 500;
                int pageNum = (lstHisRuntime.Count % rowPerPage) > 0 ? (lstHisRuntime.Count / rowPerPage) + 1 : (lstHisRuntime.Count / rowPerPage);
                for (int page = 1; page <= pageNum; page++)
                {
                    string folderPath = _localFolder;//AppDomain.CurrentDomain.BaseDirectory;
                    string fileName = DateTime.Now.ToString("HHmmssddMMyyyy") + "_" + Guid.NewGuid().ToString().Substring(0, 8) + ".txt";
                    sb = new StringBuilder();
                    sb.AppendLine("Tagname,TimeStamp,Value");
                    for (int i = (page - 1) * rowPerPage; i < lstHisRuntime.Count; i++)
                    {
                        if (i > (page * rowPerPage - 1))
                            break;
                        //if (i % rowPerPage == 0)
                        //    sb.AppendLine(lstHisRuntime[i].TagName + "," + lstHisRuntime[i].DateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "," + lstHisRuntime[i].Value.ToString());
                        //else
                        sb.AppendLine(lstHisRuntime[i].TagName + "," + lstHisRuntime[i].DateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "," + lstHisRuntime[i].Value.ToString());
                    }
                    string originalContent = sb.ToString();

                    //string folderPath = _localFolder;//AppDomain.CurrentDomain.BaseDirectory;
                    //string fileName = DateTime.Now.ToString("HHmmssddMMyyyy") + "_" + Guid.NewGuid().ToString().Substring(0, 8) + ".txt";
                    //sb = new StringBuilder();
                    //sb.AppendLine("Tagname,TimeStamp,Value");
                    //for (int i = 0; i < lstHisRuntime.Count; i++)
                    //{
                    //    //NOX,2018-06-13 11:05:00.000,95.900002
                    //    if (i == lstHisRuntime.Count - 1)
                    //        sb.Append(lstHisRuntime[i].TagName + "," + lstHisRuntime[i].DateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "," + lstHisRuntime[i].Value.ToString());
                    //    else
                    //        sb.AppendLine(lstHisRuntime[i].TagName + "," + lstHisRuntime[i].DateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "," + lstHisRuntime[i].Value.ToString());
                    //}
                    //string originalContent = sb.ToString();

                    //Ma hoa du lieu
                    //string encryptData = Helper.EncryptRSA(Helper.PassCode, originalContent);
                    //string decryptData = new RSA().Decrypt(RSA.PassCode, encryptData);

                    //var fullFileName = folderPath + fileName;
                    var fullFileName = Path.Combine(folderPath, fileName);

                    // Ghi ra file
                    File.WriteAllText(System.IO.Path.Combine(folderPath, fileName), originalContent);

                    for (int i = (page - 1) * rowPerPage; i < lstHisRuntime.Count; i++)
                    {
                        if (i > (page * rowPerPage - 1))
                            break;
                        updateLastReadTime(lstHisRuntime[i]);
                    }

                //Day file
                Label_Push:
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(_userName, _password);
                        try
                        {
                            CreateFTPDirectory(_ftpLink + config.TagName, _userName, _password);
                            client.UploadFile(_ftpLink + config.TagName + "//" + fileName, WebRequestMethods.Ftp.UploadFile, fullFileName);
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
            }
            else
            {
                //LogPush((int)eStatusError.NoData, "");
            }
        }

        static bool CreateFTPDirectory(string directory, string _username, string _password)
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
        static HisRuntime getLastRuntime(string tagName, DateTime fromDt, DateTime toDt)
        {
            var sqlRuntime = @"select DateTime,TagName,Value
                    from History                    
                    where TagName =@TagName and DateTime>=@FromDt and DateTime<@toDt
                    order by DateTime desc";
            var parameters = new Dictionary<string, object>()
            {
                ["@TagName"] = tagName,
                ["@FromDt"] = fromDt,
                ["@ToDt"] = toDt
            };
            var lstHis = MainForm._repositoryRuntime.GetListFromParameters<HisRuntime>(sqlRuntime, 1, parameters);
            if (lstHis != null && lstHis.Count > 0)
                return lstHis[0];
            return null;
        }

        static LastRead getLastRead(String tagName)
        {
            string sql = @"select top 1 LastReadTime from LastRead where TagName=@TagName";
            var parameters = new Dictionary<string, object>()
            {
                ["@TagName"] = tagName
            };
            var result = _repositoryMiddle.GetObject<LastRead>(sql, 1, parameters);
            return result;
        }

        static DateTime lastReadTime(String tagName)
        {
            string sql = @"select top 1 LastReadTime from LastRead where TagName=@TagName";
            var parameters = new Dictionary<string, object>()
            {
                ["@TagName"] = tagName
            };
            var result = _repositoryMiddle.GetObject(sql, 1, parameters);
            if (result == null)
                return DateTime.Now.AddHours(-1).Date;
            else
                return (DateTime)result;
        }

        static void updateLastReadTime(HisRuntime his)
        {
            string sql = @"IF (NOT EXISTS (SELECT 1 FROM LastRead WHERE TagName = @TagName)) 
                        BEGIN 
                            INSERT INTO LastRead (TagName,LastReadTime) values (@TagName,@Datetime) 
                        END 
                        ELSE 
                        BEGIN 
                            UPDATE LastRead SET LastReadTime = @Datetime WHERE TagName=@TagName
                        END";
            var parameters = new Dictionary<string, object>()
            {
                ["@TagName"] = his.TagName,
                ["@Datetime"] = his.DateTime
            };
            var result = _repositoryMiddle.ExecuteSQLFromParameters(sql, 1, parameters);
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
                    log = string.Format("{0}: " + "Tạo file {1} thành công! Gửi FTP lỗi!", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), fileName);
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
                string[] files = GetFileList();
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        Download(file);
                    }
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

        static string[] GetFileList()
        {
            string[] downloadFiles;
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
                return result.ToString().Split('\n');
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

        static void Download(string file)
        {
            bool downSuccess = true;
            int iDown = 0;
            var now = DateTime.Today.ToString("yyyy.MM.dd");
            var currentFolder = _localFolder + "\\" + now;
            bool isExistDir = Directory.Exists(currentFolder);
            if (!isExistDir)
                Directory.CreateDirectory(currentFolder);

            Label_Down:
            try
            {
                string uri = _ftpLink + file;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    return;
                }
                FtpWebRequest  downReqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpLink + file));
                downReqFTP.Credentials = new NetworkCredential(_userName, _password);
                downReqFTP.KeepAlive = false;
                downReqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                downReqFTP.UseBinary = true;
                downReqFTP.Proxy = null;
                downReqFTP.UsePassive = false;
                FtpWebResponse downResponse = (FtpWebResponse)downReqFTP.GetResponse();
                Stream responseStream = downResponse.GetResponseStream();
                using (FileStream writeStream = new FileStream(currentFolder + "\\" + file, FileMode.Create))
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
                using (StreamReader sr = new StreamReader(currentFolder + "\\" + file))
                {
                    content = sr.ReadToEnd();
                }



                string decryptData = Helper.DecryptRSA(Helper.PassCode, content.Replace("\0", string.Empty));
                var arrHis = decryptData.Split('\n');
                if (arrHis != null && arrHis.Length > 1)
                {
                    string tableName = "";
                    DateTime appstartday = DateTime.Now;
                    LocalDAL dal = new LocalDAL();
                    dal.checkExistTableInDB(ref tableName, ref appstartday);
                    // Bo qua dong tieu de
                    for (int i = 1; i < arrHis.Length; i++)
                    {
                        string hisContent = arrHis[i];
                        var his = hisContent.Split(',');
                        if (his.Length >= 3)
                        {
                            // GHEP DAL
                            string tagName = his[0];
                            string dateTime = his[1];
                            string value = his[2];

                            if (value != null)
                            {
                                value = value.Replace("\r", "").Replace("\n", "");
                            }

                            String ameterid = getAmeterid(tagName);
                            if (ameterid != null && !ameterid.Trim().Equals(""))
                            {
                                dal.insertToDB(tableName, appstartday, ameterid, DateTime.Parse(dateTime), value);
                            }
                        }
                    }
                }
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
                if (File.Exists(currentFolder + "\\" + file))
                    LogPull((int)eStatusError.SuccessAll, currentFolder + "\\" + file);
                else
                    LogPull((int)eStatusError.CreateFileSuccessSendFtpError, currentFolder + "\\" + file);
            }
            else
            {
                if (iDown >= 3)
                    LogPull((int)eStatusError.ErrorAll, currentFolder + "\\" + file);
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


                    MainForm.ReConfig(_config.Destination, _config.UserName, _config.Password/*Helper.Base64Decode(_config.Password)*/, _config.Timer, _config.Source);
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
