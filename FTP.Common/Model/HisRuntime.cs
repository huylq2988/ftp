using System;

namespace FTP.Model
{
    //public class HisRuntime
    //{
    //    public DateTime DateTime { get; set; }
    //    public string TagName { get; set; }
    //    public double Value { get; set; }
    //}

    public class AnalogTable
    {
        public string Folder { get; set; }
        public string LogDate { get; set; }
        public string LogTime { get; set; }
        public string TagName { get; set; }
        public double LastValue { get; set; }
    }
}
