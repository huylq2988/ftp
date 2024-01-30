using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FTP.Common
{
    class SQLDataAccess
    {
        public static ConnectionStringSettings ConnString
           = ConfigurationManager.ConnectionStrings["Local"];
        static SqlConnection conn = null;
        public static void OpenConnection()
        {
            conn = new SqlConnection(ConnString.ConnectionString);
            conn.Open();
        }

        public static void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        public static SqlDataAdapter DataAdapter = null;
        public static int Execute(string strCommandText, SqlParameter[] param)
        {
            OpenConnection();
            SqlCommand objCommand = new SqlCommand();
            objCommand.Connection = conn;
            objCommand.CommandText = strCommandText;
            objCommand.CommandType = CommandType.Text;

            if (param != null)
                objCommand.Parameters.AddRange(param);
            int result = 0;
            try
            {
                result = objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Thao tác với CSDL không thực hiện được: " + ex.Message);
            }
            CloseConnection();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCommandText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string strCommandText, SqlParameter[] param)
        {
            OpenConnection();
            SqlCommand objCommand = new SqlCommand();
            objCommand.Connection = conn;
            objCommand.CommandText = strCommandText;
            objCommand.CommandType = CommandType.Text;
            if (param != null)
                objCommand.Parameters.AddRange(param);

            DataAdapter = new SqlDataAdapter(objCommand);
            DataSet objDataSet = new DataSet();
            try
            {
                DataAdapter.Fill(objDataSet);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kết nối đến CSDL khi lấy dữ liệu");
            }
            CloseConnection();
            return objDataSet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ameterid"></param>
        /// <param name="dtime"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int insertMeterData(string tablename, string ameterid, DateTime dtime, string val)
        {
            string str = "INSERT INTO " + tablename + " (AMETERID,DTIME,VAL_S,READMETHODID,USERID,USERDTIME) ";
            str += "VALUES (@AMETERID, @DTIME, @VAL_S, 'DCS','OCCREADING',getdate())";

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@AMETERID";
            para[0].SqlDbType = SqlDbType.NVarChar;
            para[0].Value = ameterid;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@DTIME";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = dtime;

            para[2] = new SqlParameter();
            para[2].ParameterName = "@VAL_S";
            para[2].SqlDbType = SqlDbType.NVarChar;
            para[2].Value = val;

            return SQLDataAccess.Execute(str, para);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ameterid"></param>
        /// <param name="dtime"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int updateMeterReadCurent(string tablename, string ameterid, DateTime dtime, string val)
        {
            string str = "UPDATE " + tablename + " set DTIME=@DTIME,VAL_S=@VAL_S,READMETHODID='DCS', USERID='OCCREADING', USERDTIME=getdate()";
            str += "where AMETERID=@AMETERID";

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@AMETERID";
            para[0].SqlDbType = SqlDbType.NVarChar;
            para[0].Value = ameterid;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@DTIME";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = dtime;

            para[2] = new SqlParameter();
            para[2].ParameterName = "@VAL_S";
            para[2].SqlDbType = SqlDbType.NVarChar;
            para[2].Value = val;

            return SQLDataAccess.Execute(str, para);
        }

        public static int updateMeterDataHis(string tablename, string ameterid, DateTime dtime, string val)
        {
            string str = "UPDATE " + tablename + " set VAL_S=@VAL_S,READMETHODID='DCS', USERID='OCCREADING', USERDTIME=getdate()";
            str += "where AMETERID=@AMETERID and DTIME=@DTIME";

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@AMETERID";
            para[0].SqlDbType = SqlDbType.NVarChar;
            para[0].Value = ameterid;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@DTIME";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = dtime;

            para[2] = new SqlParameter();
            para[2].ParameterName = "@VAL_S";
            para[2].SqlDbType = SqlDbType.NVarChar;
            para[2].Value = val;

            return SQLDataAccess.Execute(str, para);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="appstartday"></param>
        /// <returns></returns>
        public static bool getNameTableForWriteData(ref string tableName, ref DateTime appstartday, DateTime dtime)
        {
            string str = "Select o.* from Op_Meter_Read_Config o ";
            str += " where o.enable = 1 and o.appstartday < @appstartday order by o.appstartday desc";
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@appstartday";
            para[0].SqlDbType = SqlDbType.DateTime;
            para[0].Value = dtime.AddDays(1).Date;

            DataSet ds = GetDataSet(str, para);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string storeTypeID = dt.Rows[0]["STORETYPEID"].ToString();
                if (storeTypeID.ToUpper().Trim().Equals("ONE"))//Chung một bảng
                    tableName = dt.Rows[0]["TABLENAMEPREFIX"].ToString();
                if (storeTypeID.ToUpper().Trim().Equals("YEAR"))//Theo năm
                    tableName = dt.Rows[0]["TABLENAMEPREFIX"].ToString() + DateTime.Now.Year.ToString();
                if (storeTypeID.ToUpper().Trim().Equals("MONTH"))//Theo tháng
                    tableName = dt.Rows[0]["TABLENAMEPREFIX"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
                if (storeTypeID.ToUpper().Trim().Equals("DAY"))//Theo tháng
                    tableName = dt.Rows[0]["TABLENAMEPREFIX"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                appstartday = DateTime.Parse(dt.Rows[0]["appstartday"].ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public static void createTablePhysicalForOperData(string tableName)
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
            Execute(str, null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterid"></param>
        /// <returns></returns>
        public static bool checkExistMeterInMeterReadCurrent(string meterid)
        {
            string str = "Select o.AMETERID from OP_METER_READ_CURRENT o ";
            str += " where o.AMETERID = @AMETERID";
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@AMETERID";
            para[0].SqlDbType = SqlDbType.NVarChar;
            para[0].Value = meterid;
            DataSet ds = GetDataSet(str, para);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
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
        public static bool checkExistRecordInMeterReadData(string tableName, string meterid, DateTime dtime)
        {
            string str = "Select o.AMETERID from " + tableName + " o ";
            str += " where o.AMETERID = @AMETERID and DTIME=@DTIME";
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@AMETERID";
            para[0].SqlDbType = SqlDbType.NVarChar;
            para[0].Value = meterid;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@DTIME";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = dtime;
            DataSet ds = GetDataSet(str, para);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool checkExistTableInDB(string tableName)
        {
            String s = "";
            int i = tableName.LastIndexOf('.');
            if (i > 0)
                s = tableName.Substring(0, i + 1);
            //string str = "select id from QLKT_OP.dbo.sysobjects where UPPER(id) = object_id(N'"+tableName.ToUpper() + "')"+" and xtype=N'U'";
            string str = "select id from " + s + "sysobjects where UPPER(id) = object_id(N'" + tableName.ToUpper() + "')" + " and xtype=N'U'";
            DataSet ds = GetDataSet(str, null);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool checkExistAssetMeter(string meterid)
        {
            string str = "Select o.ADMETERID from A_ASSET_METER_DYN o ";
            str += " where o.ADMETERID = @ADMETERID";
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@ADMETERID";
            para[0].SqlDbType = SqlDbType.NVarChar;
            para[0].Value = meterid;
            DataSet ds = GetDataSet(str, para);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                return true;
            return false;
        }

        public static bool checkExistMeterInMeterReadCurrentNew(string meterid, DateTime dtime)
        {
            string str = "Select o.AMETERID from OP_METER_READ_CURRENT o ";
            str += " where o.AMETERID = @AMETERID and o.DTIME > @DTIME";
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@AMETERID";
            para[0].SqlDbType = SqlDbType.NVarChar;
            para[0].Value = meterid;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@DTIME";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = dtime;
            DataSet ds = GetDataSet(str, para);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool checkExistDateTimeNowInMeterDataday(DateTime dtime)
        {
            string str = "Select o.DATADAY from OP_METER_READ_DATADAY o ";
            str += " where o.DATADAY = @DATADAY";
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@DATADAY";
            para[0].SqlDbType = SqlDbType.DateTime;
            para[0].Value = dtime;
            DataSet ds = GetDataSet(str, para);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int updateMeterReadConfig()
        {
            string str = "UPDATE OP_METER_READ_CONFIG set HAVINGDATA=1 where appstartday =";
            str += " (Select max(o.appstartday) from Op_Meter_Read_Config o ";
            str += " where o.enable = 1 and o.appstartday < @appstartday)";

            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@appstartday";
            para[0].SqlDbType = SqlDbType.DateTime;
            para[0].Value = DateTime.Now.AddDays(1).Date;

            return SQLDataAccess.Execute(str, para);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appstartday"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int insertMeterReadDataDay(DateTime appstartday, string tableName, DateTime dtime)
        {
            string str = "INSERT INTO OP_METER_READ_DATADAY (DATADAY,TABLENAME,APPSTARTDAY) ";
            str += "VALUES (@DATADAY, @TABLENAME, @APPSTARTDAY)";

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@DATADAY";
            para[0].SqlDbType = SqlDbType.DateTime;
            para[0].Value = dtime.Date;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@TABLENAME";
            para[1].SqlDbType = SqlDbType.NVarChar;
            para[1].Value = tableName;

            para[2] = new SqlParameter();
            para[2].ParameterName = "@APPSTARTDAY";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = appstartday;

            return SQLDataAccess.Execute(str, para);
        }

        public static object[] checkExistTableInDB()
        {            
            string tableName = "";
            DateTime appstartday = new DateTime();
            try
            {                                
                //Lấy tên bảng
                SQLDataAccess.getNameTableForWriteData(ref tableName, ref appstartday, new DateTime());
                //Kiểm tra bảng tồn tại
                if (!SQLDataAccess.checkExistTableInDB(tableName))
                {
                    SQLDataAccess.createTablePhysicalForOperData(tableName);
                }                             
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return new object[] { tableName, appstartday };
        }

        public static int insertToDB(string tableName, DateTime appstartday, String ameterid, DateTime dt, string value)
        {
            int countSucc = 0;
            if (value == null || value.Trim().Equals(""))
            {
                return countSucc;
            }
            try
            {                
                //DateTime appstartday = new DateTime();
                ////Lấy tên bảng
                //SQLDataAccess.getNameTableForWriteData(ref tableName, ref appstartday, dt);
                ////Kiểm tra bảng tồn tại
                //if (!SQLDataAccess.checkExistTableInDB(tableName))
                //{
                //    SQLDataAccess.createTablePhysicalForOperData(tableName);
                //}
                //Cập nhật lại havingdata
                SQLDataAccess.updateMeterReadConfig();
                if (!SQLDataAccess.checkExistDateTimeNowInMeterDataday(dt))
                {
                    SQLDataAccess.insertMeterReadDataDay(appstartday, tableName, dt);
                }

                //kiem tra xem co du lieu hay ko, neu ko co bo qua				
                if (ameterid != null && SQLDataAccess.checkExistAssetMeter(ameterid))
                {
                    if (!SQLDataAccess.checkExistMeterInMeterReadCurrent(ameterid))
                        countSucc += SQLDataAccess.insertMeterData("OP_METER_READ_CURRENT", ameterid, dt, value.ToString());
                    else
                    {
                        //kiem tra da co gia tri nao lon hon thoi diem dang xet chua, neu co bo qua
                        if (!SQLDataAccess.checkExistMeterInMeterReadCurrentNew(ameterid, dt))
                            countSucc += SQLDataAccess.updateMeterReadCurent("OP_METER_READ_CURRENT", ameterid, dt, value.ToString());
                    }
                    if (!SQLDataAccess.checkExistRecordInMeterReadData(tableName, ameterid, dt))
                        countSucc += SQLDataAccess.insertMeterData(tableName, ameterid, dt, value.ToString());
                    else
                    {
                        countSucc += SQLDataAccess.updateMeterDataHis(tableName, ameterid, dt, value.ToString());
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
