using FTP.Common;
using FTP.Model;
//using Limilabs.FTP.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static FTP.MainForm;

namespace FTP
{
    public partial class ManualUpload : Form
    {
        public ManualUpload()
        {
            InitializeComponent();
        }
        public static string sqlRuntime = @"sp_GetBwAnalogTable";


        private void ManualUpload_Load(object sender, EventArgs e)
        {
            //_repositoryMiddle = new Repository(MainForm.connectionStringMiddle);

            //_ftpLink = link;
            //_userName = userName;
            //_password = password;
            //myTimer.Interval = interval * 1000;
            //_localFolder = folderPath;
        }

        private void btnDocDuLieuLs_Click(object sender, EventArgs e)
        {
            if (dtTuNgay.Value == null)
            {
                MessageBox.Show("Chưa nhập dữ liệu Từ ngày!");
            }
            else if (dtDenNgay.Value == null)
            {
                MessageBox.Show("Chưa nhập dữ liệu Đến ngày!");
            }
            else if (dtTuNgay.Value >= dtDenNgay.Value)
            {
                MessageBox.Show("Từ ngày không được lớn hơn hoặc bằng đến ngày!");
            }
            else if ((dtDenNgay.Value - dtTuNgay.Value).Hours > 6)
            {
                MessageBox.Show("Không được lọc khoảng thời gian quá dài (> 6 giờ)!");
            }
            else
            {
                var tuNgay = dtTuNgay.Value;
                var denNgay = dtDenNgay.Value;
                // Lấy dữ liệu theo điều kiện lọc này
                var time = tuNgay;
                while (time <= denNgay) 
                {
                    PushDataManual(time);
                    time = time.AddMinutes(5);
                }

                /* oldCode
                int iPush = 0;
                bool success = true;
                //Select Data
                var sqlParam = @"select ID,TagName,Description,Unit,Device,Ameterid,Interval,Enable
                    from Params                    
                    where Interval is not null and Enable=1";
                var sqlRuntime = @"select DateTime,TagName,Value
                    from History                    
                    where TagName =@TagName and DateTime>@Datetime and DATEPART(MI,DateTime)%@Interval=0
                    order by DateTime";

                var lstParams = MainForm._repositoryMiddle.GetListFromParameters<HisConfig>(sqlParam, 1, null);

                if (lstParams != null && lstParams.Count > 0)
                {
                    var lstHisRuntime = new List<AnalogTable>();
                    for (int k = 0; k < lstParams.Count; k++)
                    {
                        LastRead lastRead = getLastRead(lstParams[k].TagName);
                        var parameters = new Dictionary<string, object>()
                        {
                            ["@TagName"] = lstParams[k].TagName,
                            ["@Datetime"] = lastRead != null ? lastRead.LastReadTime : DateTime.Now.AddHours(-1).Date,//lastReadTime(lstParams[k].TagName),
                            ["@Interval"] = lstParams[k].Interval
                        };

                        var lstHis = MainForm._repositoryRuntime.GetListFromParameters<AnalogTable>(sqlRuntime, 1, parameters);
                        //if(lstHis==null || lstHis.Count == 0)
                        //{
                        //    lstHis = MainForm._repositoryRuntime.GetListFromParameters<HisRuntime>(sqlRuntime, 1, parameters);
                        //}
                        if (lstHis != null && lstHis.Count > 0)
                        {
                            Boolean bExists = false;
                            DateTime currDate = DateTime.Now;
                            DateTime lastDate = lastRead != null ? lastRead.LastReadTime.AddMinutes(lstParams[k].Interval) : DateTime.Now.AddHours(-1).Date;
                            while (lastDate < currDate)
                            {
                                bExists = false;
                                for (int j = 0; j < lstHis.Count; j++)
                                {
                                    //if (DateTime.Compare(lastDate, lstHis[j].DateTime) == 0)
                                    //{
                                    //    bExists = true;
                                    //    lstHisRuntime.Add(lstHis[j]);
                                    //    lstHis.Remove(lstHis[j]);
                                    //    break;
                                    //}
                                }
                                if (!bExists)
                                {
                                    AnalogTable hisRuntime = getLastRuntime(lstParams[k].TagName, lastRead.LastReadTime, lastDate);
                                    if (hisRuntime != null)
                                        lstHisRuntime.Add(hisRuntime);
                                }
                                lastDate = lastDate.AddMinutes(lstParams[k].Interval);
                            }
                            //lstHisRuntime.AddRange(lstHis);
                        }
                    }

                    if (lstHisRuntime != null && lstHisRuntime.Count > 0)
                    {
                        var sb = new StringBuilder();
                        int rowPerPage = 500;
                        int pageNum = (lstHisRuntime.Count % rowPerPage) > 0 ? (lstHisRuntime.Count / rowPerPage) + 1 : (lstHisRuntime.Count / rowPerPage);
                        for (int page = 1; page <= pageNum; page++)
                        {
                            string folderPath = MainForm._localFolder;//AppDomain.CurrentDomain.BaseDirectory;
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
                            }
                            string originalContent = sb.ToString();

                            //Ma hoa du lieu
                            string encryptData = Helper.EncryptRSA(Helper.PassCode, originalContent);
                            //string decryptData = new RSA().Decrypt(RSA.PassCode, encryptData);


                            var fullFileName = folderPath + fileName;

                            // Ghi ra file
                            File.WriteAllText(System.IO.Path.Combine(folderPath, fileName), encryptData);

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
                                client.Credentials = new NetworkCredential(MainForm._userName, MainForm._password);
                                try
                                {
                                    client.UploadFile(MainForm._ftpLink + fileName, WebRequestMethods.Ftp.UploadFile, fullFileName);
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
                                        MainForm.LogPush((int)MainForm.eStatusError.SuccessAll, fullFileName);
                                        try
                                        {
                                            System.IO.File.Delete(fullFileName);
                                        }
                                        catch (Exception exc) { }
                                    }
                                    else
                                        MainForm.LogPush((int)MainForm.eStatusError.CreateFileSuccessSendFtpError, fullFileName);
                                }
                                else
                                {
                                    if (iPush >= 3)
                                        MainForm.LogPush((int)MainForm.eStatusError.ErrorAll, fullFileName);
                                }
                            }
                        }
                    }
                    else
                    {
                    }
                }
                */
            }
        }

        private void PushDataManual(DateTime datetime)
        {
            var dtMinute = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, 0, DateTimeKind.Local);
            var parameters = new Dictionary<string, object>()
            {
                ["@datetime"] = dtMinute,
            };
            //var lstAnalogs = MainForm._repositoryRuntime.GetListFromParameters<AnalogTable>(sqlRuntime, 1, parameters);
            var lstAnalogs = MainForm._repositoryRuntime.GetListFromParameters<AnalogTable>(sqlRuntime, 4, parameters);
            var PushingDatas = lstAnalogs.GroupBy(a => a.Folder).Select(o => new
            {
                Folder = o.Key,
                AnalogTables = o.ToArray()
            }).ToArray();

            if (PushingDatas != null && PushingDatas.Length > 0)
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = PushingDatas.Length
                };
                Parallel.For(0, PushingDatas.Length, options, i =>
                {
                    PushFtp(dtMinute, (PushingDatas[i]?.Folder, PushingDatas[i]?.AnalogTables));
                    Thread.Sleep(10);
                });
            }
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
                for (int i = 0; i < data.AnalogTables.Length; i++)
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


        /* OldCode
        private AnalogTable getLastRuntime(string tagName, DateTime fromDt, DateTime toDt)
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
            var lstHis = MainForm._repositoryRuntime.GetListFromParameters<AnalogTable>(sqlRuntime, 1, parameters);
            if (lstHis != null && lstHis.Count > 0)
                return lstHis[0];
            return null;
        }

        private LastRead getLastRead(String tagName)
        {
            string sql = @"select top 1 LastReadTime from LastRead where TagName=@TagName";
            var parameters = new Dictionary<string, object>()
            {
                ["@TagName"] = tagName
            };
            var result = MainForm._repositoryMiddle.GetObject<LastRead>(sql, 1, parameters);
            return result;
        }

        private DateTime lastReadTime(String tagName)
        {
            string sql = @"select top 1 LastReadTime from LastRead where TagName=@TagName";
            var parameters = new Dictionary<string, object>()
            {
                ["@TagName"] = tagName
            };
            var result = MainForm._repositoryMiddle.GetObject(sql, 1, parameters);
            if (result == null)
                return DateTime.Now.AddHours(-1).Date;
            else
                return (DateTime)result;
        }
        private void updateLastReadTime(AnalogTable his)
        {
            string sql = @"IF (NOT EXISTS (SELECT 1 FROM LastRead WHERE TagName = @TagName)) 
                        BEGIN 
                            INSERT INTO LastRead (TagName,LastReadTime) values (@TagName,@Datetime) 
                        END 
                        ELSE 
                        BEGIN 
                            UPDATE LastRead SET LastReadTime = @Datetime WHERE TagName=@TagName
                        END";

            var strDt = "20" + his.LogDate + " " + his.LogTime;
            var dt = Convert.ToDateTime(strDt);
            var parameters = new Dictionary<string, object>()
            {
                ["@TagName"] = his.TagName,
                ["@Datetime"] = strDt
            };
            var result = MainForm._repositoryMiddle.ExecuteSQLFromParameters(sql, 1, parameters);
        }
        */
    }
}
