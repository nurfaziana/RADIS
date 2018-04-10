using System;
using System.Data;//Added
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;//Added
using System.Web.UI.WebControls;//Added (Dropdownlist)

namespace radisutm.ClassFunction
{
    public class ClsFunction
    {
        String conOraStr = System.Configuration.ConfigurationManager.ConnectionStrings["radisDbContext"].ConnectionString;
        public ClsFunction()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public DateTime getDateTime(String dbType, String SQLSTMT)
        {
            DateTime dtSysdate = DateTime.Today;

            DataSet ds = new DataSet();
            OracleDataAdapter daSql = new OracleDataAdapter();
            OracleDataAdapter daOra = new OracleDataAdapter();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(SQLSTMT);

            Dictionary<string, string> fields = new Dictionary<string, string>();
            OracleConnection conOra = new OracleConnection(conOraStr);
            OracleCommand cmdOra = new OracleCommand(SQLSTMT, conOra);
            cmdOra.CommandType = CommandType.Text;

            try
            {
                conOra.Open();

                OracleDataReader drOra = null;
                drOra = cmdOra.ExecuteReader();

                if (drOra.HasRows)
                {
                    while (drOra.Read())
                    {
                        dtSysdate = Convert.ToDateTime(drOra["SYSDATE"]);

                    }
                }

                drOra.Close();
                conOra.Close();
                conOra.Dispose();
                cmdOra.Dispose();
            }
            catch (Exception ex)
            {

            }


            return dtSysdate;
        }

        public DataSet getDataSet(String dbType, String SQLSTMT)
        {

            DataSet ds = new DataSet();
            OracleDataAdapter daSql = new OracleDataAdapter();
            OracleDataAdapter daOra = new OracleDataAdapter();

            OracleConnection conOra = new OracleConnection(conOraStr);
            OracleCommand cmdOra = new OracleCommand(SQLSTMT, conOra);
            cmdOra.CommandType = CommandType.Text;

            try
            {
                conOra.Open();

                //cmd.CommandText = cls_mysqlstatement;
                //cmd.Connection = cls_con;

                //OracleDataReader dr = null;
                //dr = cmd.ExecuteReader();

                //if (dr.HasRows){
                //     while (dr.Read())
                //    {
                //        string myField = (string)dr["ACCOUNTNO"];
                //        Console.WriteLine(myField);
                //    }
                //}

                ds = new DataSet();
                daOra = new OracleDataAdapter(cmdOra);
                daOra.Fill(ds);

                conOra.Close();

                conOra.Dispose();
            }
            catch (Exception ex)
            {

            }



            return ds;

        }
        public DataTable getOracleDT(String SQLSTMT)
        {
            var dtOra = new DataTable();
            OracleDataAdapter daOra = new OracleDataAdapter();

            try
            {

                using (OracleConnection con = new OracleConnection(conOraStr))
                {
                    con.Open();

                    // use a SqlAdapter to execute the query
                    using (daOra = new OracleDataAdapter(SQLSTMT, con))
                    {
                        // fill a data table

                        daOra.Fill(dtOra);

                    }
                }

            }
            catch (Exception ex)
            {

            }

            return dtOra;
        }


        //lblValue.Text = CF.getReturnValueSpecCol("oracle", "Select Created_Date,AccountNo From PKU_PT_MST_TEST where AccountNo = " + accNo, "Created_Date");
        public String getReturnValueSpecCol(String dbType, String SQLSTMT, String fieldName)
        {

            DataSet ds = new DataSet();
            OracleDataAdapter daSql = new OracleDataAdapter();
            OracleDataAdapter daOra = new OracleDataAdapter();
            String returnval = "";


            Dictionary<string, string> fields = new Dictionary<string, string>();
            OracleConnection conOra = new OracleConnection(conOraStr);
            OracleCommand cmdOra = new OracleCommand(SQLSTMT, conOra);
            cmdOra.CommandType = CommandType.Text;

            try
            {
                conOra.Open();

                OracleDataReader drOra = null;
                drOra = cmdOra.ExecuteReader();

                if (drOra.HasRows)
                {
                    while (drOra.Read())
                    {
                        if (drOra[fieldName] != null)
                        {
                            returnval = Convert.ToString(drOra[fieldName]);
                            Console.WriteLine(returnval);
                        }
                        else
                        {
                            returnval = "[NULL]";
                        }

                    }
                }

                drOra.Close();
                conOra.Close();
                conOra.Dispose();
                cmdOra.Dispose();
            }
            catch (Exception ex)
            {

            }



            return returnval;

        }





    }

    //CF.populateDropdown(DropDownList2, "oracle", "Select PT_MST_PK,ACCOUNTNO From PKU_PT_MST_TEST", "ACCOUNTNO", "PT_MST_PK");


    //CF.populateGridview(GridView1, "oracle", "Select ACCOUNTNO,PANEL_FK,STATUS_ACTIVE,CREATED_DATE From PKU_PT_MST_TEST");

}


    
