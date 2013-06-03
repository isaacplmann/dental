using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OSUDental
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cookies.Remove("userRole");
            HttpCookie currentUserCookie = new HttpCookie("userRole");
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            HttpContext.Current.Response.SetCookie(currentUserCookie);
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("/Login.aspx");
        }
    }
}