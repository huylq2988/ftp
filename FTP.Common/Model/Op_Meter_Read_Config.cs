using System;

namespace FTP.Model
{
    public class Op_Meter_Read_Config
    {
        public DateTime APPSTARTDAY { get; set; }
        public string STORETYPEID { get; set; }
        public string TABLENAMEPREFIX { get; set; }
        public bool ENABLE { get; set; }
        public bool HAVINGDATA { get; set; }        
    }
}
