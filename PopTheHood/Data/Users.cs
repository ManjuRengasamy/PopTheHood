﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.AspNetCore.Mvc;
using PopTheHood.Models;

namespace PopTheHood.Data
{
    public class Users
    {
        public static DataTable Login(Login userlogin)
        {
            try
            {
                string ConnectionString = Common.GetConnectionString();
                //SqlParameter[] parameters =
                //{
                //    new SqlParameter("@Email", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 10, 0, "Email", DataRowVersion.Proposed, userlogin.Email),
                //    new SqlParameter("@Password", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 10, 0, "Password", DataRowVersion.Proposed, userlogin.Password)
                //};
                string Role = "User";
                if (userlogin.Role=="Admin")
                {
                    Role = "Admin";
                }
                var encryptPassword = Common.EncryptData(userlogin.Password);

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Email", userlogin.Email));
                parameters.Add(new SqlParameter("@Password", encryptPassword));
                parameters.Add(new SqlParameter("@Role", Role));
                DataTable dt = new DataTable();
                using (dt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spLoginUser", parameters.ToArray()).Tables[0])
                {
                    return dt;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static string UpdatePassword(Login userlogin)
        {
            //SqlParameter[] parameters =
            //{
            //    new SqlParameter("@Email", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 10, 0, "Email", DataRowVersion.Proposed, userlogin.Email),
            //    new SqlParameter("@Password", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 10, 0, "Password", DataRowVersion.Proposed, userlogin.Password)
            //};
            var encryptPassword = Common.EncryptData(userlogin.Password);

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", userlogin.Email));
            parameters.Add(new SqlParameter("@Password", encryptPassword));

            try
            {
                string ConnectionString = Common.GetConnectionString();
                //Execute the query
                //using (DataTable dt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spUpdatePassword", parameters).Tables[0])
                //{
                string rowsAffected = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "spUpdatePassword", parameters.ToArray()).ToString();
                return rowsAffected;
                //}
            }
            catch (Exception e)
            {
                //loggerErr.Error(e.Message + " - " + e.StackTrace);
                throw e;
            }
        }

        public static DataTable GetAllUserList()
        {
            try
            {
                string ConnectionString = Common.GetConnectionString();
                //Execute the query
                using (DataTable dt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetUserList").Tables[0])
                {
                    return dt;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static int DeleteUser(int UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@UserID", UserID));

            try
            {
                string ConnectionString = Common.GetConnectionString();

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spDeleteUser", parameters.ToArray());
                return rowsAffected;
            }
            catch (Exception e)
            {
                //loggerErr.Error(e.Message + " - " + e.StackTrace);
                throw e;
            }
        }

        public static int UpdateVerificationStatus(int UserId, bool Status, string Source)
        {
            try
            {
                string ConnectionString = Common.GetConnectionString();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserId", UserId));
                parameters.Add(new SqlParameter("@Status", Status));
                parameters.Add(new SqlParameter("@UpdateStatus", Source));
                            
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdatePhoneEmailStatus", parameters.ToArray());
                return rowsAffected;
            }
            catch (Exception e)
            {
                //loggerErr.Error(e.Message + " - " + e.StackTrace);
                throw e;
            }
        }

        public static DataTable GetUserById(int UserId)
        {
            try
            {
                string ConnectionString = Common.GetConnectionString();
                //Create the parameters in the SqlParameter array
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserId", UserId));

                //Execute the query
                using (DataTable dt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetUserById", parameters.ToArray()).Tables[0])
                {
                    return dt;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static string SaveUser(UsersLogin userlogin, string Action)
        {
            //int UserId, string Name, string PhoneNumber, string Email, string Password, string SourceofReg, bool IsEmailVerified, bool IsPhoneNumVerified,
            //bool IsPromoCodeApplicable, string Action,

            try
            {
                string ConnectionString = Common.GetConnectionString();

                var encryptPassword = Common.EncryptData(userlogin.Password);

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserId", userlogin.UserId));
                parameters.Add(new SqlParameter("@Name", userlogin.Name));
                parameters.Add(new SqlParameter("@PhoneNumber", userlogin.PhoneNumber));
                parameters.Add(new SqlParameter("@Email", userlogin.Email));
                parameters.Add(new SqlParameter("@Password", encryptPassword));
                parameters.Add(new SqlParameter("@SourceofReg", userlogin.SourceofReg));
                parameters.Add(new SqlParameter("@IsEmailVerified", userlogin.IsEmailVerified));
                parameters.Add(new SqlParameter("@IsPhoneNumVerified", userlogin.IsPhoneNumVerified));
                parameters.Add(new SqlParameter("@IsPromoCodeApplicable", userlogin.IsPromoCodeApplicable));
                parameters.Add(new SqlParameter("@Role", userlogin.Role));
                parameters.Add(new SqlParameter("@Action", Action));

                string rowsAffected = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "spSaveUser", parameters.ToArray()).ToString();
                //SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spSaveUser", parameters.ToArray());
                return rowsAffected;
                //using (DataTable dt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spSaveUser", parameters.ToArray()).Tables[0])
                //{
                //    int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdatePhoneEmailStatus", parameters.ToArray());
                //    return rowsAffected;
                //}
            }
            catch (Exception e)
            {
                //loggerErr.Error(e.Message + " - " + e.StackTrace);
                throw e;
            }
            
        }

        public static string PhoneOrEmailVerification(string PhoneEmail, string OTP, string Type)
        {
            try
            {
                string ConnectionString = Common.GetConnectionString();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PhoneOrEmail", PhoneEmail));
                parameters.Add(new SqlParameter("@OTP", OTP));
                parameters.Add(new SqlParameter("@Type", Type));

                using (DataSet dt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spPhoneEmailVerification", parameters.ToArray()))
                {
                    string rowsAffected = dt.Tables[0].Rows[0]["Status"].ToString();
                    return rowsAffected;
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataSet ExternalLogin([FromBody]externalLogin userlogin)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@name", userlogin.Name));
            parameters.Add(new SqlParameter("@email", userlogin.Email));
            parameters.Add(new SqlParameter("@loginProvider", userlogin.LoginProvider));
            parameters.Add(new SqlParameter("@providerKey", userlogin.ProviderKey));

            try
            {
                string ConnectionString = Common.GetConnectionString();

                DataSet ds = new DataSet();
                using (ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spAuthentication", parameters.ToArray()))
                {
                    return ds;
                }
                //int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAuthentication", parameters.ToArray());
                //return rowsAffected;
            }
            catch (Exception e)
            {
                //loggerErr.Error(e.Message + " - " + e.StackTrace);
                throw e;
            }
        }

        public static string ExternalRegistration([FromBody]externalReg userReg)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Name", userReg.Name));
            parameters.Add(new SqlParameter("@PhoneNumber", userReg.PhoneNumber));
            parameters.Add(new SqlParameter("@loginProvider", userReg.LoginProvider));
            parameters.Add(new SqlParameter("@providerKey", userReg.ProviderKey));

            try
            {
                string ConnectionString = Common.GetConnectionString();
                using (DataTable dt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spAuthRegistation", parameters.ToArray()).Tables[0])
                {
                    string rowsAffected = dt.Rows[0]["Status"].ToString();
                    return rowsAffected;
                }

                //string rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAuthRegistation", parameters.ToArray()).ToString();
                //return rowsAffected;
            }
            catch (Exception e)
            {
                //loggerErr.Error(e.Message + " - " + e.StackTrace);
                throw e;
            }
        }

    }
}
