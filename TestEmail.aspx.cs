using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OSUDental
{
    public partial class TestEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("no-reply@smsosu.com");
            mail.To.Add(new MailAddress("isaacplmann@gmail.com"));
            mail.Subject = "Test";
            mail.Body = "Message was sent.";
            SmtpClient smtp = new SmtpClient("relay-hosting.secureserver.net", 25);
            smtp.Credentials = new System.Net.NetworkCredential("no-reply@smsosu.com", "smsWsd93fg");
            smtp.EnableSsl = false;
            smtp.Send(mail);
        }
    }
}