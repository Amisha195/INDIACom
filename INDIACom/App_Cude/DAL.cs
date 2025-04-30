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


        public string SubmitPapers(PaperSubmissionModel model)
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
                cmd.CommandText = "Proc_InsertPaperSubmission";

                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@DateOfSubmission", model.DateOfSubmission);
                cmd.Parameters.AddWithValue("@EventId", model.Event_Id);
                cmd.Parameters.AddWithValue("@TrackId", model.Track_Id);
                cmd.Parameters.AddWithValue("@SessionId", model.Session_Id);
                cmd.Parameters.AddWithValue("@EventName", model.Event_Name);
                cmd.Parameters.AddWithValue("@TrackName", model.Track_Name);
                cmd.Parameters.AddWithValue("@SessionName", model.Session_Name);
                cmd.Parameters.AddWithValue("@MemberId", model.Member_Id);
                cmd.Parameters.AddWithValue("@PaperPath", model.PaperPath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PlagiarismPath", model.PlagiarismPath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CorrespondanceId", model.Correspondence_Id);
                cmd.Parameters.AddWithValue("@CoAuthorsId", model.Co_Authors_Id ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
                transaction.Commit();
                message = "Success";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                message = "Error: " + ex.Message;
            }
            finally
            {
                DisposeConnection();
            }

            return message;
        }


        #endregion
        //for verificationbutton

        #region Verifybutton
        public string VerifyMemberByID(string memberId, out string message)
        {
            string memberName = "";
            message = "";

            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand();
                SqlTransaction transaction = con.BeginTransaction();

                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_CheckCredentials";

                cmd.Parameters.AddWithValue("@UserID", memberId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    memberName = reader["Name"].ToString();
                    message = "Member found.";
                }
                else
                {
                    message = "Member not found.";
                }

                reader.Close();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }
            finally
            {
                DisposeConnection();
            }

            return memberName;
        }


        #endregion




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
                cmd.CommandText = "Proc_InsertSession";
                cmd.Parameters.AddWithValue("@MemberID", ss.MemberID);
                cmd.Parameters.AddWithValue("@SSName", ss.SSName);
                cmd.Parameters.AddWithValue("@Mobile", ss.Mobile);
                cmd.Parameters.AddWithValue("@Email", ss.Email);
                cmd.Parameters.AddWithValue("@Org", ss.Organization);
                cmd.Parameters.AddWithValue("@Topic", ss.Topic);
                cmd.Parameters.AddWithValue("@TrackID", ss.TrackID);
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


        public long GetMemberID(string email)

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

        public string AddOrganisation(MemberModel model)
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

        #region Category Verification

        public List<MembersDocumentsModel> GetMemberDocuments()
        {
            List<MembersDocumentsModel> documents = new List<MembersDocumentsModel>();
            OpenConnection();

            try
            {
                SqlCommand cmd = new SqlCommand("GetMemberDocuments", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        documents.Add(new MembersDocumentsModel
                        {
                            MemberID = Convert.ToInt32(reader["MemberID"]),
                            EventID = reader["EventID"].ToString(),
                            InstitutionCard = reader["InstitutionCard"].ToString(),
                            InstitutionCardPath = reader["InstitutionCardPath"].ToString(),
                            ProfBodyIDCardPath = reader["MembershipProofPath"].ToString(),
                            PassportPath = reader["OtherDocumentPath"].ToString(),
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong while fetching member documents.");
            }
            finally
            {
                DisposeConnection();
            }

            return documents;
        }

        public string InsertMemberDocument(MembersDocumentsModel model)
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
                cmd.CommandText = "InsertMemberDocument";

                cmd.Parameters.AddWithValue("@EventID", model.EventID);
                cmd.Parameters.AddWithValue("@InstitutionCard", model.InstitutionCard);
                cmd.Parameters.AddWithValue("@ProfBodyIDCard", model.ProfBodyIDCard);
                cmd.Parameters.AddWithValue("@Passport", model.Passport);
                cmd.Parameters.AddWithValue("@MemberCatVerificationStatus", model.MemberCatVerificationStatus);
                cmd.Parameters.AddWithValue("@ProfBodyVerificationStatus", model.ProfBodyVerificationStatus);
                cmd.Parameters.AddWithValue("@PassportVerification", model.PassportVerification);
                cmd.Parameters.AddWithValue("@DecisionDate", (object)model.DecisionDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Comment", model.Comment);
                cmd.Parameters.AddWithValue("@InstitutionCardPath", model.InstitutionCardPath);
                // Mapping model -> renamed DB columns
                cmd.Parameters.AddWithValue("@MembershipProofPath", model.ProfBodyIDCardPath);
                cmd.Parameters.AddWithValue("@OtherDocumentPath", model.PassportPath);

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


