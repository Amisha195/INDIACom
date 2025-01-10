using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace INDIACom.App_Cude
{
    public class CommonMethod
    {

        private static SqlConnection con = null;
        private static string _conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;


        // for fetch data with sql select
        public static DataTable ExecuteSelectProc(string Procname, List<SqlParameter> sp)
        {
            SqlConnection cn = new SqlConnection(_conString);
            using (SqlCommand cmd = new SqlCommand(Procname, cn))
            {
                DataTable objdt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;
                if (sp != null)
                {
                    foreach (SqlParameter it in sp)
                    {
                        cmd.Parameters.Add(it);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(objdt);
                return objdt;


            }
        }

        public static void bindDropDownHnGrid(string ProcName, List<SelectListItem> distNames, string parm1, string parm2 = "", string parm3 = "", string parm4 = "", string parm5 = "", string parm6 = "", string parm7 = "")
        {
            DataSet ds = new DataSet();
            SqlDataReader sdr;
            if (parm1.Length > 0 && parm1 != "")
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Parm1", parm1));
                if (!string.IsNullOrWhiteSpace(parm2))
                    parameters.Add(new SqlParameter("@Parm2", parm2));
                if (!string.IsNullOrWhiteSpace(parm3))
                    parameters.Add(new SqlParameter("@Parm3", parm3));
                if (!string.IsNullOrWhiteSpace(parm4))
                    parameters.Add(new SqlParameter("@Parm4", parm4));
                if (!string.IsNullOrWhiteSpace(parm5))
                    parameters.Add(new SqlParameter("@Parm5", parm5));
                if (!string.IsNullOrWhiteSpace(parm5))
                    parameters.Add(new SqlParameter("@Parm6", parm6));
                sdr = SqlHelper.ExecuteReader(_conString, CommandType.StoredProcedure, ProcName, parameters.ToArray());
            }
            else
            {
                sdr = SqlHelper.ExecuteReader(_conString, CommandType.StoredProcedure, ProcName);
            }
            if (!@parm7.Equals("Z"))
            {
                distNames.Insert(0, new SelectListItem { Text = @parm7 == "all" ? "-- All -- " : "--Select--", Value = "-1", Selected = true });
            }


            while (sdr.Read())
            {
                distNames.Add(new SelectListItem
                {

                    Text = sdr["ValueText"].ToString(),
                    Value = sdr["ValueId"].ToString().Trim()
                });
            }

            sdr.Close();
        }
        public static void bindDropDownHnGrid1(string ProcName, List<SelectListItem> distNames, string parm1, string parm2 = "", string parm3 = "", string parm4 = "", string parm5 = "", string parm6 = "", string parm7 = "")
        {
            DataSet ds = new DataSet();
            SqlDataReader sdr;
            if (parm1.Length > 0 && parm1 != "")
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Parm1", parm1));
                if (!string.IsNullOrWhiteSpace(parm2))
                    parameters.Add(new SqlParameter("@Parm2", parm2));
                if (!string.IsNullOrWhiteSpace(parm3))
                    parameters.Add(new SqlParameter("@Parm3", parm3));
                if (!string.IsNullOrWhiteSpace(parm4))
                    parameters.Add(new SqlParameter("@Parm4", parm4));
                if (!string.IsNullOrWhiteSpace(parm5))
                    parameters.Add(new SqlParameter("@Parm5", parm5));
                if (!string.IsNullOrWhiteSpace(parm5))
                    parameters.Add(new SqlParameter("@Parm6", parm6));
                sdr = SqlHelper.ExecuteReader(_conString, CommandType.StoredProcedure, ProcName, parameters.ToArray());
            }
            else
            {
                sdr = SqlHelper.ExecuteReader(_conString, CommandType.StoredProcedure, ProcName);
            }
            //if (!@parm7.Equals("Z"))
            //{
            //    distNames.Insert(0, new SelectListItem { Text = @parm7 == "all" ? "-- All -- " : "--Select--", Value = "-1", Selected = true });
            //}


            while (sdr.Read())
            {
                distNames.Add(new SelectListItem
                {

                    Text = sdr["ValueText"].ToString(),
                    Value = sdr["ValueId"].ToString().Trim()
                });
            }

            sdr.Close();
        }
        public static bool ValidateDt(DataTable dataTable)
        {
            if (dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}