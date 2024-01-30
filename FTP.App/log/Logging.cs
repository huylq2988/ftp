using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP.log
{
    public class Logging
    {
        private static Logging instance;

        private Logging() { }

        public static Logging Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logging();
                    instance.Initialize4Report();
                }
                return instance;
            }
        }
        public void Initialize4Report()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders(); /*Remove any other appenders*/

            RollingFileAppender fileAppender = new RollingFileAppender();
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            var a = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            fileAppender.File = @"log\ftp.log";
            fileAppender.Encoding = Encoding.UTF8;
            fileAppender.MaxSizeRollBackups = 100;
            fileAppender.MaximumFileSize = "2MB";
            fileAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
            fileAppender.StaticLogFileName = true;
            fileAppender.ActivateOptions();
            PatternLayout pl = new PatternLayout();
            pl.ConversionPattern = "%-5p%d{yyyy-MM-dd HH:mm:ss} – %m%n";
            pl.ActivateOptions();
            fileAppender.Layout = pl;
            fileAppender.ActivateOptions();

            log4net.Config.BasicConfigurator.Configure(fileAppender);
        }

        public readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
