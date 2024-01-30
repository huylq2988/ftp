using FTP.Common;
using FTP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP
{
    class LocalDAL
    {
        private readonly Repository _repositoryLocal;
        private string connectionStringLocal = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
        public LocalDAL()
        {
            _repositoryLocal = new Repository(connectionStringLocal);
        }

        public LocalDAL(string connectionStringLocal_)
        {
            _repositoryLocal = new Repository(connectionStringLocal_);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ameterid"></param>
        /// <param name="dtime"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int insertMeterData(string tablename, string ameterid, DateTime dtime, string val)
        {
            string str = "INSERT INTO " + tablename + " (AMETERID,DTIME,VAL_S,READMETHODID,USERID,USERDTIME) ";
            str += "VALUES (@AMETERID, @DTIME, @VAL_S, 'DCS','OCCREADING',getdate())";            

            var paras = new Dictionary<string, object>()
            {
                ["@AMETERID"] = ameterid,
                ["@DTIME"] = dtime,
                ["@VAL_S"] = val
            };
            return _repositoryLocal.ExecuteSQLFromParameters(str, 1, paras);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ameterid"></param>
        /// <param name="dtime"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int updateMeterReadCurent(string tablename, string ameterid, DateTime dtime, string val)
        {
            string str = "UPDATE " + tablename + " set DTIME=@DTIME,VAL_S=@VAL_S,READMETHODID='DCS', USERID='OCCREADING', USERDTIME=getdate()";
            str += "where AMETERID=@AMETERID";            

            var paras = new Dictionary<string, object>()
            {
                ["@AMETERID"] = ameterid,
                ["@DTIME"] = dtime,
                ["@VAL_S"] = val
            };
            return _repositoryLocal.ExecuteSQLFromParameters(str, 1, paras);
        }

        public int updateMeterDataHis(string tablename, string ameterid, DateTime dtime, string val)
        {
            string str = "UPDATE " + tablename + " set VAL_S=@VAL_S,READMETHODID='DCS', USERID='OCCREADING', USERDTIME=getdate()";
            str += "where AMETERID=@AMETERID and DTIME=@DTIME";            

            var paras = new Dictionary<string, object>()
            {
                ["@AMETERID"] = ameterid,
                ["@DTIME"] = dtime,
                ["@VAL_S"] = val
            };
            return _repositoryLocal.ExecuteSQLFromParameters(str, 1, paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="appstartday"></param>
        /// <returns></returns>
        public bool getNameTableForWriteData(ref string tableName, ref DateTime appstartday, DateTime dtime)
        {
            string str = "Select o.* from Op_Meter_Read_Config o ";
            str += " where o.enable = 1 and o.appstartday < @appstartday order by o.appstartday desc";            

            var paras = new Dictionary<string, object>()
            {
                ["@appstartday"] = dtime.AddDays(1).Date
            };
            List<Op_Meter_Read_Config> lst = _repositoryLocal.GetListFromParameters<Op_Meter_Read_Config>(str, 1, paras);
            
            if (lst != null && lst.Count > 0)
            {
                Op_Meter_Read_Config dt = lst.First();
                string storeTypeID = dt.STORETYPEID;
                if (storeTypeID.ToUpper().Trim().Equals("ONE"))//Chung một bảng
                    tableName = dt.TABLENAMEPREFIX;
                if (storeTypeID.ToUpper().Trim().Equals("YEAR"))//Theo năm
                    tableName = dt.TABLENAMEPREFIX + DateTime.Now.Year.ToString();
                if (storeTypeID.ToUpper().Trim().Equals("MONTH"))//Theo tháng
                    tableName = dt.TABLENAMEPREFIX + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
                if (storeTypeID.ToUpper().Trim().Equals("DAY"))//Theo tháng
                    tableName = dt.TABLENAMEPREFIX + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                appstartday = dt.APPSTARTDAY;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public void createTablePhysicalForOperData(string tableName)
        {
            string[] arr = tableName.Split('.');
            string str = "create table " + tableName.ToUpper() + " (";
            str += " AMETERID nvarchar(50) not null,";
            str += " DTIME datetime not null,";
            str += " VAL_S nvarchar(255) not null,";
            str += " READMETHODID nvarchar(25) not null,";
            str += " USERID nvarchar(25) null,";
            str += " USERDTIME datetime null,";
            str += " CONSTRAINT " + arr[arr.Length - 1].ToUpper() + "_IDPK primary key(AMETERID,DTIME)";
            str += " )";            
            _repositoryLocal.ExecuteSQLFromParameters(str, 1, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterid"></param>
        /// <returns></returns>
        public bool checkExistMeterInMeterReadCurrent(string meterid)
        {
            string str = "Select count(*) from OP_METER_READ_CURRENT o ";
            str += " where o.AMETERID = @AMETERID";            

            var paras = new Dictionary<string, object>()
            {
                ["@AMETERID"] = meterid                
            };
            object obj = _repositoryLocal.GetObject(str, 1, paras);

            if (obj != null && Convert.ToInt32(obj) > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="meterid"></param>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public bool checkExistRecordInMeterReadData(string tableName, string meterid, DateTime dtime)
        {
            string str = "Select count(*) from " + tableName + " o ";
            str += " where o.AMETERID = @AMETERID and DTIME=@DTIME";            

            var paras = new Dictionary<string, object>()
            {
                ["@AMETERID"] = meterid,
                ["@DTIME"] = dtime
            };
            object obj = _repositoryLocal.GetObject(str, 1, paras);

            if (obj != null && Convert.ToInt32(obj) > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool checkExistTableInDB(string tableName)
        {
            String s = "";
            int i = tableName.LastIndexOf('.');
            if (i > 0)
                s = tableName.Substring(0, i + 1);
            //string str = "select id from QLKT_OP.dbo.sysobjects where UPPER(id) = object_id(N'"+tableName.ToUpper() + "')"+" and xtype=N'U'";
            string str = "select count(*) from " + s + "sysobjects where UPPER(id) = object_id(N'" + tableName.ToUpper() + "')" + " and xtype=N'U'";
           
            object obj = _repositoryLocal.GetObject(str, 1, null);

            if (obj != null && Convert.ToInt32(obj) > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool checkExistAssetMeter(string meterid)
        {
            string str = "Select count(*) from A_ASSET_METER_DYN o ";
            str += " where o.ADMETERID = @AMETERID";            

            var paras = new Dictionary<string, object>()
            {
                ["@AMETERID"] = meterid                
            };
            object obj = _repositoryLocal.GetObject(str, 1, paras);

            if (obj != null && Convert.ToInt32(obj) > 0)
                return true;
            return false;
        }

        public bool checkExistMeterInMeterReadCurrentNew(string meterid, DateTime dtime)
        {
            string str = "Select count(*) from OP_METER_READ_CURRENT o ";
            str += " where o.AMETERID = @AMETERID and o.DTIME > @DTIME";            

            var paras = new Dictionary<string, object>()
            {
                ["@AMETERID"] = meterid,
                ["@DTIME"] = dtime
            };
            object obj = _repositoryLocal.GetObject(str, 1, paras);

            if (obj != null && Convert.ToInt32(obj) > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool checkExistDateTimeNowInMeterDataday(DateTime dtime)
        {
            string str = "Select count(*) from OP_METER_READ_DATADAY o ";
            str += " where o.DATADAY = @DATADAY";            

            var paras = new Dictionary<string, object>()
            {
                ["@DATADAY"] = dtime
            };
            object obj = _repositoryLocal.GetObject(str, 1, paras);
           
            if (obj != null && Convert.ToInt32(obj) > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int updateMeterReadConfig()
        {
            string str = "UPDATE OP_METER_READ_CONFIG set HAVINGDATA=1 where appstartday =";
            str += " (Select max(o.appstartday) from Op_Meter_Read_Config o ";
            str += " where o.enable = 1 and o.appstartday < @appstartday)";            

            var paras = new Dictionary<string, object>()
            {
                ["@appstartday"] = DateTime.Now.AddDays(1).Date             
            };
            return _repositoryLocal.ExecuteSQLFromParameters(str, 1, paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appstartday"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int insertMeterReadDataDay(DateTime appstartday, string tableName, DateTime dtime)
        {
            string str = "INSERT INTO OP_METER_READ_DATADAY (DATADAY,TABLENAME,APPSTARTDAY) ";
            str += "VALUES (@DATADAY, @TABLENAME, @APPSTARTDAY)";            

            var paras = new Dictionary<string, object>()
            {
                ["@DATADAY"] = dtime.Date,
                ["@TABLENAME"] = tableName,
                ["@APPSTARTDAY"] = appstartday
            };
            return _repositoryLocal.ExecuteSQLFromParameters(str, 1, paras);
        }

        public bool checkExistTableInDB(ref string tableName, ref DateTime appstartday)
        {
            //string tableName = "";
            //DateTime appstartday = new DateTime();
            try
            {
                //Lấy tên bảng
                getNameTableForWriteData(ref tableName, ref appstartday, DateTime.Now);
                //Kiểm tra bảng tồn tại
                if (!checkExistTableInDB(tableName))
                {
                    createTablePhysicalForOperData(tableName);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
            //return new object[] { tableName, appstartday };
            return true;
        }

        public int insertToDB(string tableName, DateTime appstartday, String ameterid, DateTime dt, string value)
        {
            int countSucc = 0;
            if (value == null || value.Trim().Equals(""))
            {
                return countSucc;
            }
            try
            {                
                //Cập nhật lại havingdata
                updateMeterReadConfig();
                if (!checkExistDateTimeNowInMeterDataday(dt))
                {
                    insertMeterReadDataDay(appstartday, tableName, dt);
                }

                //kiem tra xem co du lieu hay ko, neu ko co bo qua				
                if (ameterid != null && checkExistAssetMeter(ameterid))
                {
                    if (!checkExistMeterInMeterReadCurrent(ameterid))
                        countSucc += insertMeterData("OP_METER_READ_CURRENT", ameterid, dt, value.ToString());
                    else
                    {
                        //kiem tra da co gia tri nao lon hon thoi diem dang xet chua, neu co bo qua
                        if (!checkExistMeterInMeterReadCurrentNew(ameterid, dt))
                            countSucc += updateMeterReadCurent("OP_METER_READ_CURRENT", ameterid, dt, value.ToString());
                    }
                    if (!checkExistRecordInMeterReadData(tableName, ameterid, dt))
                        countSucc += insertMeterData(tableName, ameterid, dt, value.ToString());
                    else
                    {
                        countSucc += updateMeterDataHis(tableName, ameterid, dt, value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return countSucc;
        }
    }
}
