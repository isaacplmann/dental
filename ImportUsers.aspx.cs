using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OSUDental
{
    public partial class ImportUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from SMCLNT WHERE Status <> 0", cn);
            DataSet users = new DataSet();
            da.Fill(users);
            foreach (DataRow r in users.Tables[0].Rows)
            {
                String login = r["NAME"].ToString().ToLower();
                login = Regex.Replace(login,@"[^\w]","") + r["SMS_NUM"];
                if (login.Length > 10)
                {
                    login = login.Substring(0, 10);
                }
                Response.Write(r["NAME"] + " -> " + login + "<br/>\n");

                String email = Convert.ToString(r["EMail"]);
                //if (email.Length == 0)
                //{
                //    email = "";
                //}

                MembershipUser user;

                //try
                //{
                //    user = Membership.CreateUser(login, (String)r["ZIP"], email);
                //}
                //catch (MembershipCreateUserException ex) {
                    user = Membership.GetUser(login);
                    user.Email = email;
                    Membership.UpdateUser(user);
                //}
                
                try
                {
                    // Associate user with account details
                    SqlCommand cmd = new SqlCommand("UPDATE SMCLNT SET UserID = @UserID WHERE SMS_NUM=@SMS_NUM", cn);
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 50);
                    cmd.Parameters["@UserID"].Value = login;
                    cmd.Parameters.Add("@SMS_NUM", SqlDbType.Int);
                    cmd.Parameters["@SMS_NUM"].Value = r["SMS_NUM"];
                    cn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows != 1) {
                        Response.Write(r["SMS_NUM"]+" not updated with UserId.<br/>");
                    }
                    cn.Close();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message + "<br/>\n");
                }
            }

            SqlDataSource ds = new SqlDataSource(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, "SELECT * from SMCLNT WHERE Status <> 0");
            OldUsers.DataSource = ds;
            OldUsers.DataBind();
        }
    }
}