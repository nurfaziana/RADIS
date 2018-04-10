using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace radisutm.ClassFunction
{
    public class ClassImage : IHttpHandler
    {

      String conOraStr = System.Configuration.ConfigurationManager.ConnectionStrings["TismaOracle"].ConnectionString;

        public ClassImage()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void ProcessRequest(HttpContext context)
        {

            string roll_no = context.Request.QueryString["ImageId"].ToString();
            OracleConnection objConn = new OracleConnection(conOraStr);
            objConn.Open();
            //string sTSQL = "SELECT IMAGE FROM PKU_COLOR_BLIND_IMG WHERE CBIMG_PK=:roll_no";
            string sTSQL = "Select gambar from VIEWTABLE.VW_GAMBAR_STAF_UTM where No_PEKERJA=:roll_no";
            OracleCommand objCmd = new OracleCommand(sTSQL, objConn);
           // objCmd.CommandType = CommandType.Text;
            //objCmd.Parameters.AddWithValue(":roll_no", roll_no.ToString());
            object data = objCmd.ExecuteScalar();
            objConn.Close();
            objCmd.Dispose();
            context.Response.BinaryWrite((byte[])data);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}