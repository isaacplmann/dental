using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace OSUDental.Messaging
{
    public class AutomatedEmailJob:Job
    {
        private EmailAlert.EmailType emailType;

        public AutomatedEmailJob(EmailAlert.EmailType emailType):base("Automated Email Job",DateTime.Now)
        {
            this.emailType = emailType;
        }

        public override void Execute()
        {
            PreExecute();
            EmailAlert alert = new EmailAlert();
            alert.SendAll(emailType);
        }
    }
}