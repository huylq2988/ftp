using System;
using System.Collections.Generic;

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

    public class LastQuery
    {
        public DateTime LogTime { get; set; }
    }
    public class SaveData
    {
        public List<DataRaw> lstDataSave { get; set; }
    }
    public class DataRaw
    {
        public string maThongSoNhap { get; set; }
        public string thoiDiem { get; set; }
        public string giaTri { get; set; }
    }
}
