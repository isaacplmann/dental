using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace OSUDental.Messaging
{
    public class EmailAlertJob:Job
    {
        protected String FromAddress = "no-reply@smsosu.com", Host = "relay-hosting.secureserver.net", Subject = "", Body="";

        public EmailAlertJob():base("Email Alert Job",DateTime.Now)
        {
        }

        public void Execute()
        {
            base.Execute();
            CheckForEmails();
        }

        public void CheckForEmails()
        {
            TestEmail();
        }

        public void TestEmail()
        {
            Subject = "Test";
            Body = "This is a test email.";
            SendEmail("isaacplmann@gmail.com");
        }
        
        /// <summary>
        /// Send one email
        /// </summary>
        private void SendEmail(String toAddress)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(FromAddress);
                mail.To.Add(new MailAddress(toAddress));
                mail.Subject = Subject;
                mail.Body = Body;
                SmtpClient smtp = new SmtpClient(Host, 25);
                //smtp.Credentials = new System.Net.NetworkCredential(FromAddress, Password);
                smtp.EnableSsl = false;
                smtp.Send(mail);
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }
        }
    }
}