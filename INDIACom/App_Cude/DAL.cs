using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INDIACom.Models;
using System.Reflection;

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


       
        #region Feedback
       

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


        //#region PaperSubmission

        //public bool SubmitPapers(PaperSubmissionModel model)
        //{
        //    string message = "";
        //    OpenConnection();
        //    SqlCommand cmd = new SqlCommand();
        //    SqlTransaction transaction = con.BeginTransaction();

        //    try
        //    {
        //        cmd = new SqlCommand("sp_InsertPaperSubmission", con, transaction);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@title", model.title);
        //        cmd.Parameters.AddWithValue("@paper_path", model.paper_path);
        //        cmd.Parameters.AddWithValue("@plagiarism_path", model.plagiarism_path);
        //        cmd.Parameters.AddWithValue("@correspondance_id", Convert.ToInt32(model.correspondance_id));

        //        // Convert List<string> Authors to a comma-separated string
        //        string coAuthors = string.Join(",", model.co_authors_id);
        //        cmd.Parameters.AddWithValue("@co_authors_id", coAuthors);

        //        // Set EventID and TrackID as NULL
        //        cmd.Parameters.AddWithValue("@EventID", DBNull.Value);
        //        cmd.Parameters.AddWithValue("@TrackID", DBNull.Value);

        //        int rowsAffected = cmd.ExecuteNonQuery();
        //        transaction.Commit();
        //        return rowsAffected > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        Console.WriteLine("SQL Error: " + ex.Message);  // ✅ Log exact SQL error
        //        return false;
        //    }

        //    finally
        //    {
        //        con.Close();
        //    }
        //}


        //#endregion

        #region Event

        public string InsertEvent(EventModel model)
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
                cmd.CommandText = "ProcInsertEvent";

                cmd.Parameters.AddWithValue("@eventID", model.Event_Id);
                cmd.Parameters.AddWithValue("@eventName", model.Event_Name);
                cmd.Parameters.AddWithValue("@eventCreation", model.Event_Creation_date);
                cmd.Parameters.AddWithValue("@eventOpeningDate", model.Event_Opening_date);
                cmd.Parameters.AddWithValue("@eventClosingDate", model.Event_Closing_date);
                cmd.Parameters.AddWithValue("@description", model.Event_Description);
                cmd.Parameters.AddWithValue("@eventType", model.Event_Type);

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


       /* #region SpecialSession 
        public string InsertSession(SpecialSessionModel ss)
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
                cmd.CommandText = "ProcInsertSession";
                cmd.Parameters.AddWithValue("@SSName", ss.SSName);
                cmd.Parameters.AddWithValue("@Mobile", ss.Mobile);
                cmd.Parameters.AddWithValue("@Email", ss.Email);
                cmd.Parameters.AddWithValue("@Org", ss.Organization);
                cmd.Parameters.AddWithValue("@Topic", ss.Topic);
                cmd.Parameters.AddWithValue("@Request_Date", ss.Request_Date);
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
*/
        #region file
        public string SaveFilePath(MemberDocumentModel doc)
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
                cmd.CommandText = "ProcSaveFilePath";

                cmd.Parameters.AddWithValue("@UserID", doc.UserID);
                cmd.Parameters.AddWithValue("@FilePath", doc.FilePath);

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

        #region User
        public string InsertUserDetails(MembersModel model)
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
                cmd.CommandText = "Proc_InsertDetails";
                cmd.Parameters.AddWithValue("@Salutation", model.Salutation);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Address", model.Address);
                cmd.Parameters.AddWithValue("@Country", model.Country);
                cmd.Parameters.AddWithValue("@CountryID", model.CountryID);
                cmd.Parameters.AddWithValue("@State", model.State);
                cmd.Parameters.AddWithValue("@StateID", model.StateID);
                cmd.Parameters.AddWithValue("@City", model.City);
                cmd.Parameters.AddWithValue("@CityID", model.CityID);
                cmd.Parameters.AddWithValue("@Pincode", model.Pincode);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile);
                cmd.Parameters.AddWithValue("@Event", model.Event);
                cmd.Parameters.AddWithValue("@CSI_No", model.CSI_No);
                cmd.Parameters.AddWithValue("@IEEE_No", model.IEEE_No);
                cmd.Parameters.AddWithValue("@Organisation", model.OrganisationName);
                cmd.Parameters.AddWithValue("@Category", model.Category);
                cmd.Parameters.AddWithValue("@Password", model.Password);

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

        public long GetMemberID(string email, long mobile)
        {
            long memberID = 0;
            OpenConnection();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_GetMemberID";
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Mobile", mobile);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    memberID = (long)result;
                }
            }
            catch (Exception)
            {

                transaction.Rollback();

            }
            finally
            {
                DisposeConnection();
            }
            return memberID;
        }

        public DataTable checkCredentials(LoginUserModel model)
        {
            DataTable ds = new DataTable();
            OpenConnection();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_CheckCredentials";
                cmd.Parameters.AddWithValue("@EmailId", filter_bad_chars_rep(model.Email));
                cmd.Parameters.AddWithValue("@UserID", model.UserID);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception)
            {
                ds = new DataTable();
                transaction.Rollback();

            }
            finally
            {
                DisposeConnection();
            }
            return ds;
        }

        public DataTable GetUserById(long memberId)
        {
            DataTable ds = new DataTable();
            OpenConnection();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = con.BeginTransaction();
            try
            {
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_GetCredentials";
                cmd.Parameters.AddWithValue("@memberid", memberId);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception)
            {
                ds = new DataTable();
                transaction.Rollback();

            }
            finally
            {
                DisposeConnection();
            }
            return ds;
        }

        public bool UpdateUserProfile(MemberModel model)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_UpdateProfile";
                cmd.Parameters.AddWithValue("@memberId", model.MemberID);
                cmd.Parameters.AddWithValue("@Salutation", model.Salutation);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Address", model.Address);
                cmd.Parameters.AddWithValue("@Country", model.Country);
                cmd.Parameters.AddWithValue("@CountryID", model.CountryID);
                cmd.Parameters.AddWithValue("@State", model.State);
                cmd.Parameters.AddWithValue("@StateID", model.StateID);
                cmd.Parameters.AddWithValue("@City", model.City);
                cmd.Parameters.AddWithValue("@CityID", model.CityID);
                cmd.Parameters.AddWithValue("@Pincode", model.Pincode);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile);
                cmd.Parameters.AddWithValue("@Event", model.Event);
                cmd.Parameters.AddWithValue("@CSI_No", model.CSI_No);
                cmd.Parameters.AddWithValue("@IEEE_No", model.IEEE_No);
                cmd.Parameters.AddWithValue("@Organisation", model.OrganisationName);
                cmd.Parameters.AddWithValue("@Category", model.Category);
                cmd.Parameters.AddWithValue("@Password", model.Password);

                cmd.ExecuteNonQuery();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                DisposeConnection();
            }

        }

        #endregion

        #region Organisation
        public string AddOrganisation(MembersModel model)
        {
            string result = "";
            OpenConnection();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_SaveOrganisation";
                cmd.Parameters.AddWithValue("@OrgEmail", model.OrgEmail);
                cmd.Parameters.AddWithValue("@OrgName", model.OrgName);
                cmd.Parameters.AddWithValue("@OrgShortName", model.OrgShortName);
                cmd.Parameters.AddWithValue("@OrgAddress", model.OrgAddress);
                cmd.Parameters.AddWithValue("@OrgContactPerson", model.OrgContactPerson);
                cmd.Parameters.AddWithValue("@OrgPhone", model.OrgPhone);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                result = "Success";
            }
            catch (Exception)
            {

                transaction.Rollback();
                result = "Something went wrong";

            }
            finally
            {
                DisposeConnection();
            }
            return result;
        }

        #endregion





        #region Paper
        //// PAPER SUBMISSION

        //public string SubmitPapers(PaperSubmissionModel model)
        //{
        //    string message = "";
        //    OpenConnection();
        //    SqlCommand cmd = new SqlCommand();
        //    SqlTransaction transaction = con.BeginTransaction();

        //    try
        //    {
        //        cmd.Connection = con;
        //        cmd.Transaction = transaction;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "Proc_InsertPaperSubmission";

        //        cmd.Parameters.AddWithValue("@Title", model.Title);
        //        cmd.Parameters.AddWithValue("@DateOfSubmission", model.DateOfSubmission);
        //        cmd.Parameters.AddWithValue("@EventId", model.Event_Id);
        //        cmd.Parameters.AddWithValue("@TrackId", model.Track_Id);
        //        cmd.Parameters.AddWithValue("@SessionId", model.Session_Id);
        //        cmd.Parameters.AddWithValue("@EventName", model.Event_Name);
        //        cmd.Parameters.AddWithValue("@TrackName", model.Track_Name);
        //        cmd.Parameters.AddWithValue("@SessionName", model.Session_Name);
        //        cmd.Parameters.AddWithValue("@MemberId", model.Member_Id);
        //        cmd.Parameters.AddWithValue("@PaperPath", model.PaperPath ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@PlagiarismPath", model.PlagiarismPath ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@CorrespondanceId", model.CorrespondanceId);
        //        cmd.Parameters.AddWithValue("@CoAuthorsId", model.Co_Authors_Id ?? (object)DBNull.Value);

        //        cmd.ExecuteNonQuery();
        //        transaction.Commit();
        //        message = "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        message = "Error: " + ex.Message;
        //    }
        //    finally
        //    {
        //        DisposeConnection();
        //    }

        //    return message;
        //}


        ////REVISED PAPER SUBMISSION

        //public RevisedPaperModel GetRevisedPaper(int paperId)
        //{
        //    RevisedPaperModel model = new RevisedPaperModel();
        //    OpenConnection();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("Proc_GetRevisedPaperDetails", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@PaperId", paperId);

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        //if (reader.Read())
        //        //{
        //        //    model.Event_ID = reader["Event_Id"].ToString();
        //        //    model.Track = reader["Track"].ToString();
        //        //    model.Title = reader["Title"].ToString();
        //        //    model.Authors = reader["Authors"].ToString();
        //        //    model.Version = Convert.ToInt32(reader["Version"]);
        //        //    model.ReviewResult = reader["ReviewResult"].ToString();
        //        //    model.DetailedComments = reader["DetailedComments"].ToString();
        //        //    // If stored, also populate existing file paths
        //        //    model.MasterListPath = reader["MasterListPath"].ToString();
        //        //    model.RevisedPaperPath = reader["RevisedPaperPath"].ToString();
        //        //    model.CertificateOfOriginalityPath = reader["CertificateOfOriginalityPath"].ToString();
        //        //    model.CopyrightTransferFormPath = reader["CopyrightTransferFormPath"].ToString();
        //        //    model.PresentationPath = reader["PresentationPath"].ToString();
        //        //}


        //        if (reader.Read())
        //        {
        //            model.Event_ID = reader["Event_Id"]?.ToString() ?? "";
        //            model.Track = reader["Track"]?.ToString() ?? "";
        //            model.Title = reader["Title"]?.ToString() ?? "";
        //            model.Authors = reader["Authors"]?.ToString() ?? "";
        //            model.Version = reader["Version"] != DBNull.Value ? Convert.ToInt32(reader["Version"]) : 0;
        //            model.ReviewResult = reader["ReviewResult"]?.ToString() ?? "";
        //            model.DetailedComments = reader["DetailedComments"]?.ToString() ?? "";

        //            model.MasterListPath = reader["MasterListPath"]?.ToString() ?? "";
        //            model.RevisedPaperPath = reader["RevisedPaperPath"]?.ToString() ?? "";
        //            model.CertificateOfOriginalityPath = reader["CertificateOfOriginalityPath"]?.ToString() ?? "";
        //            model.CopyrightTransferFormPath = reader["CopyrightTransferFormPath"]?.ToString() ?? "";
        //            model.PresentationPath = reader["PresentationPath"]?.ToString() ?? "";
        //        }



        //        reader.Close();
        //    }
        //    catch (Exception)
        //    {
        //        // Handle error if needed
        //        return null;
        //    }
        //    finally
        //    {
        //        DisposeConnection();
        //    }

        //    return model;
        //}


        ////submit_to_Db
        //public string SubmitRevisedPaper(RevisedPaperModel model)
        //{
        //    string message = "";
        //    OpenConnection();
        //    SqlTransaction transaction = con.BeginTransaction();
        //    SqlCommand cmd = new SqlCommand();

        //    try
        //    {
        //        cmd.Connection = con;
        //        cmd.Transaction = transaction;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "Proc_SubmitRevisedPaper";

        //        cmd.Parameters.AddWithValue("@Event_Id", model.Event_ID);
        //        cmd.Parameters.AddWithValue("@Track", model.Track);
        //        cmd.Parameters.AddWithValue("@Title", model.Title);
        //        cmd.Parameters.AddWithValue("@Authors", model.Authors);
        //        cmd.Parameters.AddWithValue("@Version", model.Version);
        //        cmd.Parameters.AddWithValue("@ReviewResult", model.ReviewResult);
        //        cmd.Parameters.AddWithValue("@DetailedComments", model.DetailedComments ?? "");
        //        cmd.Parameters.AddWithValue("@AuthorsComments", model.AuthorsComments ?? "");
        //        cmd.Parameters.AddWithValue("@DoYouWishToSubmitRevision", model.DoYouWishToSubmitRevision);

        //        cmd.Parameters.AddWithValue("@MasterListPath", model.MasterListPath ?? "");
        //        cmd.Parameters.AddWithValue("@RevisedPaperPath", model.RevisedPaperPath ?? "");
        //        cmd.Parameters.AddWithValue("@CertificateOfOriginalityPath", model.CertificateOfOriginalityPath ?? "");
        //        cmd.Parameters.AddWithValue("@CopyrightTransferFormPath", model.CopyrightTransferFormPath ?? "");
        //        cmd.Parameters.AddWithValue("@PresentationPath", model.PresentationPath ?? "");

        //        cmd.ExecuteNonQuery();
        //        transaction.Commit();
        //        message = "Success";
        //    }
        //    catch (Exception)
        //    {
        //        transaction.Rollback();
        //        message = "Something went wrong";
        //    }
        //    finally
        //    {
        //        DisposeConnection();
        //    }

        //    return message;
        //}


        #endregion

        #region UserDetailsAdmin
        //FETCH USER INFO
        public List<UserInfoModel> GetAllMembers()
        {
            List<UserInfoModel> members = new List<UserInfoModel>();

            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand("Proc_GetAllMembers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    UserInfoModel m = new UserInfoModel
                    {
                        MemberId = Convert.ToInt64(dr["member_id"]),
                        Salutation = dr["Salutation"]?.ToString(),
                        Name = dr["Name"]?.ToString(),
                        Email = dr["Email"]?.ToString(),
                        Password = dr["Password"]?.ToString(),
                        Category = dr["Category"]?.ToString(),
                        Address = dr["Address"]?.ToString(),
                        Country = dr["Country"]?.ToString(),
                        CountryID = dr["CountryID"]?.ToString(),
                        State = dr["State"]?.ToString(),
                        City = dr["City"]?.ToString(),
                        Pincode = dr["Pincode"]?.ToString(),
                        Mobile = dr["Mobile"]?.ToString(),
                        Organisation = dr["Organisation"]?.ToString(),
                        BioDataPath = dr["Bio_data_path"]?.ToString()
                    };

                    members.Add(m);
                }

                dr.Close();
            }
            catch (SqlException ex)
            {
                // Log exception (could use a logging library)
                throw new Exception("Database operation failed: " + ex.Message, ex);
            }
            finally
            {
                DisposeConnection();
            }

            return members;
        }

        #endregion

        #region UserDetailsAdmin – single record

        /// Returns a single user by MemberId; returns null if not found
        public UserInfoModel FetchUserById(long memberId)
        {
            UserInfoModel user = null;

            try
            {
                OpenConnection();                    

                SqlCommand cmd = new SqlCommand("Proc_GetUserById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberId", memberId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        user = new UserInfoModel
                        {
                            MemberId = Convert.ToInt64(dr["member_id"]),
                            Salutation = dr["Salutation"]?.ToString(),
                            Name = dr["Name"]?.ToString(),
                            Email = dr["Email"]?.ToString(),
                            Password = dr["Password"]?.ToString(),
                            Category = dr["Category"]?.ToString(),
                            Address = dr["Address"]?.ToString(),
                            Country = dr["Country"]?.ToString(),
                            CountryID = dr["CountryID"]?.ToString(),
                            State = dr["State"]?.ToString(),
                            City = dr["City"]?.ToString(),
                            Pincode = dr["Pincode"]?.ToString(),
                            Mobile = dr["Mobile"]?.ToString(),
                            Organisation = dr["Organisation"]?.ToString(),
                            BioDataPath = dr["Bio_data_path"]?.ToString()
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database operation failed: " + ex.Message, ex);
            }
            finally
            {
                DisposeConnection();                             
            }

            return user;
        }

        #endregion

       
        #region PaperAdmin
        //FETCH ALL PAPER SUBMISSIONS
        public List<PaperModel> GetAllPapers()
        {
            List<PaperModel> papers = new List<PaperModel>();

            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand("Proc_GetAllPapers", con)  // Replace with your stored procedure for fetching papers
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PaperModel p = new PaperModel
                    {
                        PaperId = dr["PaperId"] != DBNull.Value ? Convert.ToInt64(dr["PaperId"]) : 0,
                        Title = dr["Title"]?.ToString() ?? string.Empty,
                        DateOfSubmission = dr["DateOfSubmission"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfSubmission"]) : DateTime.MinValue,
                        EventName = dr["EventName"]?.ToString() ?? string.Empty,
                        TrackName = dr["TrackName"]?.ToString() ?? string.Empty,
                        SessionName = dr["SessionName"]?.ToString() ?? string.Empty,
                        CorrespondanceId = dr["CorrespondanceId"] != DBNull.Value ? Convert.ToInt32(dr["CorrespondanceId"]) : (int?)null,
                        Status = dr["Status"]?.ToString() ?? string.Empty,
                        PlagiarismPath = dr["PlagiarismPath"]?.ToString() ?? string.Empty
                    };

                    papers.Add(p);
                }

                dr.Close();
            }
            catch (SqlException ex)
            {
                // Log exception (could use a logging library)
                throw new Exception("Database operation failed: " + ex.Message, ex);
            }
            finally
            {
                DisposeConnection();
            }

            return papers;
        }
        #endregion

    }
}


