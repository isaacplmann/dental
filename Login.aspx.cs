using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OSUDental
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnLoggedIn(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.IsInRole("admin"))
            {
                Response.SetCookie(new HttpCookie("userRole", "4"));
            }
            else
            {
                Response.SetCookie(new HttpCookie("userRole", "2"));
            }
        }
    }
}