using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OSUDental.Services
{
    public static class AuthenticationHelper
    {
        public static Boolean IsAdmin()
        {
            return HttpContext.Current.User.IsInRole("admin");
        }
        public static Boolean IsLoggedIn()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public static String GetUserName()
        {
            if (IsLoggedIn())
            {
                return HttpContext.Current.User.Identity.Name;
            }
            else
            {
                return null;
            }
        }

        public static String GetUserName(int clientId)
        {
            if (!IsAdmin())
            {
                return GetUserName();
            }
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 UserName FROM SMS.dbo.SMCLNT WHERE SMS_NUM=@SMS_NUM", cn);
            cmd.Parameters.AddWithValue("@SMS_NUM", clientId);
            cn.Open();
            return cmd.ExecuteScalar().ToString();
        }

        public static int GetClientId()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 SMS_NUM FROM SMS.dbo.SMCLNT WHERE UserName=@UserName", cn);
            cmd.Parameters.AddWithValue("@UserName", GetUserName());
            cn.Open();
            return (Int32)cmd.ExecuteScalar();
        }

        public static int GetClientId(String username)
        {
            if (!IsAdmin())
            {
                return GetClientId();
            }
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 SMS_NUM FROM SMS.dbo.SMCLNT WHERE UserName=@UserName", cn);
            cmd.Parameters.AddWithValue("@UserName", username);
            cn.Open();
            return (Int32)cmd.ExecuteScalar();
        }
    }
}