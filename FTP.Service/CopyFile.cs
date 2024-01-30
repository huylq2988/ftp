using FTP.Common;
using FTP.Model;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Timers;

namespace FTP.Service
{
    public partial class CopyFile : ServiceBase
    {
        public CopyFile()
        {
            InitializeComponent();
            _repositoryMiddle = new Repository(connectionStringMiddle);
            config = GetConfig();
        }

        private readonly Repository _repositoryMiddle;
        private string connectionStringMiddle = ConfigurationManager.ConnectionStrings["Middle"].ConnectionString;
        Timer timer1 = new Timer(15000d);

        private Config config;
        private Config GetConfig()
        {
            return _repositoryMiddle.GetObject<Config>("sp_GetConfig", 4, null);
        }

        private void Method1()
        {
            using (WebClient client = new WebClient())
            {
                var files = Directory.GetFiles(config.Source);
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    string destinationPath = $"{config.Destination.TrimEnd('/')}/{fileInfo.Name}";
                    string sourcePath = $"{config.Source.TrimEnd('/')}\\{fileInfo.Name}";
                    client.Credentials = new NetworkCredential(config.UserName, config.Password /* Helper.Base64Decode(config.Password)*/ );
                    client.UploadFile(destinationPath, WebRequestMethods.Ftp.UploadFile, sourcePath);
                    File.Delete(file);
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Interval = 10000;// Convert.ToInt32(timer);
            timer1.Enabled = true;
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            Method1();
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
        }
    }
}
