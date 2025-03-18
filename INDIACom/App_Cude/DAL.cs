using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
    using System.Web.Mvc;
using INDIACom.Models;

namespace INDIACom.App_Cude
{
    public class DAL
    {

        private static SqlConnection con = null;
        private static void OpenConnection()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            SqlConnection.ClearAllPools();
            con.Open();
        }
        private static void DisposeConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        public string filter_bad_chars_rep(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            string[] sL_Char = {
            "onfocus",
            "\"\"",
            "=",
            "onmouseover",
            "prompt",
            "%27",
            "alert#",
            "alert",
            "'or",
            "`or",
            "`or`",
            "'or'",
            "'='",
            "`=`",
            "'",
            "`",
            "9,9,9",
            "src",
            "onload",
            "select",
            "drop",
            "insert",
            "delete",
            "xp_",
            "having",
            "union",
            "group",
            "update",
            "script",
            "<script",
            "</script>",
            "language",
            "javascript",
            "vbscript",
            "http",
            "~",
            "$",
            "<",
            ">",
            "%",
            "\\",
            ";",
            "",
            "_",
            "{",
            "}",
            "^",
            "?",
            "[",
            "]",
            "!",
            "#",
            "&",
            "*"
        };
            int sL_Char_Length = sL_Char.Length - 1;
            int i = 0;
            while (i <= sL_Char_Length)
            {
                if (s.Contains(sL_Char[i]))
                {
                    s = s.Replace(sL_Char[i], "");
                    // break; // TODO: might not be correct. Was : Exit While
                }
                i++;
                sL_Char_Length -= 1;
            }
            return s.Trim();
        }


        #region Common Method to Fetch Dropdown Data
        private List<SelectListItem> FetchDropdownData(string actionType)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=SubmitPaper;Trusted_Connection=True; MultipleActiveResultSets=True"))
            {
                try
                {
                    con.Open();  // Open connection before executing query

                    using (SqlCommand cmd = new SqlCommand("Proc_Common", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ActionType", actionType);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new SelectListItem
                                {
                                    Value = reader["Value"].ToString(),
                                    Text = reader["Text"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error (Implement logging mechanism here)
                    throw new Exception("Error fetching dropdown data: " + ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();  // Close connection after execution
                    }
                }
            }
            return items;
        }
        #endregion

        #region Public Methods for Dropdowns
        public List<SelectListItem> GetTracks()
        {
            return FetchDropdownData("TRACK");
        }

        public List<SelectListItem> GetEvents()
        {
            return FetchDropdownData("EVENT");
        }

        public List<SelectListItem> GetSessions()
        {
            return FetchDropdownData("SESSION");
        }
        #endregion
        #region Event Methods

        
        public string DeleteExpiredEvents()
        {
            string message = "Expired events deleted successfully.";
            OpenConnection();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                using (SqlCommand cmd = new SqlCommand("Proc_Common", con, transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "DELETE_EXPIRED_EVENTS");
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
              
            }
            finally
            {
                DisposeConnection();
            }
            return message; 
        }
        #endregion
        #region Feedback
        //public bool InsertFeedback(FeedbackModel feedback)
        //{
        //    using (SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=SubmitPaper;Trusted_Connection=True; MultipleActiveResultSets=True"))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("Proc_InsertFeedback", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@Name", feedback.Name);
        //            cmd.Parameters.AddWithValue("@Email", feedback.Email);
        //            cmd.Parameters.AddWithValue("@Mobile", string.IsNullOrEmpty(feedback.Mobile) ? (object)DBNull.Value : feedback.Mobile);
        //            cmd.Parameters.AddWithValue("@Organization", string.IsNullOrEmpty(feedback.Organization) ? (object)DBNull.Value : feedback.Organization);
        //            cmd.Parameters.AddWithValue("@ConferenceInfo", feedback.ConferenceInfo);
        //            cmd.Parameters.AddWithValue("@PaperSubmission", feedback.PaperSubmission);
        //            cmd.Parameters.AddWithValue("@Registration", feedback.Registration);
        //            cmd.Parameters.AddWithValue("@ConferenceOrg", feedback.ConferenceOrg);
        //            cmd.Parameters.AddWithValue("@Proceedings", feedback.Proceedings);
        //            cmd.Parameters.AddWithValue("@Comments", string.IsNullOrEmpty(feedback.Comments) ? (object)DBNull.Value : feedback.Comments);

        //            con.Open();
        //            int rowsAffected = cmd.ExecuteNonQuery();
        //            return rowsAffected > 0;
        //        }
        //    }
        //}


        public string InsertFeedback(FeedbackModel model)
        {
            string message = "";
            OpenConnection();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_InsertFeedback";

                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile);
                cmd.Parameters.AddWithValue("@Organization", model.Organization);
                cmd.Parameters.AddWithValue("@ConferenceInfo", model.ConferenceInfo);
                cmd.Parameters.AddWithValue("@PaperSubmission", model.PaperSubmission);
                cmd.Parameters.AddWithValue("@Registration", model.Registration);
                cmd.Parameters.AddWithValue("@ConferenceOrg", model.ConferenceOrg);
                cmd.Parameters.AddWithValue("@Proceedings", model.Proceedings);
                cmd.Parameters.AddWithValue("@Comments", model.Comments);

                cmd.ExecuteNonQuery();
                transaction.Commit();
                message = "Success";
            }
            catch (Exception)
            {
                transaction.Rollback();
                message = "Something went wrong";
            }
            finally
            {
                DisposeConnection();
            }

            return message;
        }





        #endregion
    }
}
