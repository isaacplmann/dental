using OSUDental.Models;
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
    public class EmailAlert
    {
        protected String FromAddress = "no-reply@smsosu.com", Host = "relay-hosting.secureserver.net";
        public enum EmailType
        {
            FailedTest = 1,
            StripOverdue = 2,
            ReorderReminder = 3,
            MonthlyReport = 4
        };
        public EmailType getEmailTypeFromId(int id)
        {
            if (id == 1)
            {
                return EmailType.FailedTest;
            }
            else if (id == 2)
            {
                return EmailType.StripOverdue;
            }
            else if (id == 3)
            {
                return EmailType.ReorderReminder;
            }
            else if (id == 4)
            {
                return EmailType.MonthlyReport;
            }
            return EmailType.FailedTest;
        }

        public void SendAll(EmailType emailType)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand(getSelectStatement(emailType,0), cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                SendEmail(getToAddress(emailType,dr),getSubject(emailType,dr),getBody(emailType,dr));
                LogEmail(emailType.GetHashCode(), Convert.ToInt32(dr["SMS_NUM"]));
            }
        }
        private String getSelectStatement(EmailType emailType, int ClientId)
        {
            if (emailType.Equals(EmailType.FailedTest))
            { // SMS_NUM,NAME,Address (Contact),Email,RESULT_ID,REC_DATE,TEST_DATE,RESULT,REQUIPT(EquipId),Hint(Reference)
                String statement = "SELECT c.SMS_NUM,c.NAME,c.Address,c.Email,r.*" +
                " FROM SMCLNT AS c" +
                "  INNER JOIN SMRESLT AS r ON r.RSMS_NUM=c.SMS_NUM" +
                " WHERE c.STATUS<>0" +
                "  AND r.RESULT<>'+' AND r.REC_DATE>DATEADD(day,-8,GetDate())";
                if (ClientId == 0)
                {
                    statement += "  AND NOT EXISTS (" +
                    "    SELECT 1" +
                    "     FROM EmailTracking AS et" +
                    "     WHERE et.SMS_NUM=c.SMS_NUM" +
                    "      AND et.DateSent > DateAdd(day,-8,GetDate()) AND et.EmailTypeID=1" +
                    "  )";
                }
                if (ClientId > 0)
                {
                    statement += " AND c.SMS_NUM=" + ClientId;
                }
                return statement;
            }
            else if (emailType.Equals(EmailType.StripOverdue))
            { // SMS_NUM,NAME,Address (Contact),Email
                String statement = "SELECT c.SMS_NUM,c.NAME,c.Address,c.Email,COUNT(o.ORDER_ID) AS Orders" +
                " FROM SMCLNT AS c" +
                "  INNER JOIN CUST_ORDERS AS o ON o.SMS_NUM=c.SMS_NUM" +
                " WHERE c.STATUS<>0";
                if (ClientId == 0)
                {
                    statement += "  AND NOT EXISTS (" +
                    "    SELECT 1" +
                    "     FROM EmailTracking AS et" +
                    "     WHERE et.SMS_NUM=c.SMS_NUM" +
                    "      AND et.DateSent > DateAdd(day,-14,GetDate()) AND et.EmailTypeID=2" +
                    "  )" +
                    "  AND NOT EXISTS (SELECT 1 FROM SMRESLT WHERE RSMS_NUM=c.SMS_NUM AND REC_DATE>DateAdd(day,-14,GetDate()))" +
                    "  AND o.ORDER_SIZE='50 STRIPS'" +
                    "  AND o.REC_DATE>DATEADD(year,-1,GetDate())";
                }
                if (ClientId > 0)
                {
                    statement += " AND c.SMS_NUM=" + ClientId;
                }
                statement += " GROUP BY c.SMS_NUM,c.NAME,c.Address,c.Email";
                return statement;
            }
            else if (emailType.Equals(EmailType.ReorderReminder))
            { // SMS_NUM,NAME,Address (Contact),Email
                String statement = "SELECT c.SMS_NUM,c.NAME,c.Address,c.Email,COUNT(o.ORDER_ID) AS Orders" +
                " FROM SMCLNT AS c" +
                "  INNER JOIN CUST_ORDERS AS o ON o.SMS_NUM=c.SMS_NUM" +
                " WHERE c.STATUS<>0";
                if (ClientId == 0)
                {
                    statement += "  AND NOT EXISTS (" +
                    "    SELECT 1" +
                    "     FROM EmailTracking AS et" +
                    "     WHERE et.SMS_NUM=c.SMS_NUM" +
                    "      AND et.DateSent > DateAdd(month,-1,GetDate()) AND et.EmailTypeID=3" +
                    "  )" +
                    "  AND o.REC_DATE BETWEEN DATEADD(month, -12, GETDATE()) AND DATEADD(month, -11, GETDATE())" +
                    "  AND o.ORDER_SIZE='50 STRIPS'";
                }
                if (ClientId > 0)
                {
                    statement += " AND c.SMS_NUM=" + ClientId;
                }
                statement += " GROUP BY c.SMS_NUM,c.NAME,c.Address,c.Email";
                return statement;
            }
            else if (emailType.Equals(EmailType.MonthlyReport))
            { // SMS_NUM,NAME,Address (Contact),Email
                String statement = "SELECT c.SMS_NUM,c.NAME,c.Address,c.Email" +
                " FROM SMCLNT AS c" +
                "  INNER JOIN CUST_ORDERS AS o ON o.SMS_NUM=c.SMS_NUM" +
                " WHERE c.STATUS<>0";
                if (ClientId == 0)
                {
                    statement += "  AND NOT EXISTS (" +
                    "    SELECT 1" +
                    "     FROM EmailTracking AS et" +
                    "     WHERE et.SMS_NUM=c.SMS_NUM" +
                    "      AND et.DateSent > DateAdd(month,-1,GetDate()) AND et.EmailTypeID=4" +
                    "  )" +
                    "  AND o.ORDER_SIZE='50 STRIPS'" +
                    "  AND o.REC_DATE>DATEADD(year,-1,GetDate())";
                }
                if (ClientId > 0)
                {
                    statement += " AND c.SMS_NUM=" + ClientId;
                }
                statement += " GROUP BY c.SMS_NUM,c.NAME,c.Address,c.Email";
                return statement;
            }
            return null;
        }
        private String getToAddress(EmailType emailType, SqlDataReader dr)
        {
            return dr["Email"].ToString();
        }
        private String getSubject(EmailType emailType, SqlDataReader dr)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT TOP 1 * FROM EmailTemplate WHERE EmailTypeID="+emailType.GetHashCode()+" ORDER BY DateCreated DESC", cn);
            cn.Open();
            SqlDataReader templatedr = cmd.ExecuteReader();
            if (templatedr.Read())
            {
                String template = templatedr["Subject"].ToString();
                template = template.Replace("{SMS_ID}", dr["SMS_NUM"].ToString());
                template = template.Replace("{PrimaryContact}", dr["Address"].ToString());
                template = template.Replace("{Email}", dr["Email"].ToString());
                return template.Replace("{ClientName}", dr["NAME"].ToString());
            }
            return null;
        }
        private String getBody(EmailType emailType, SqlDataReader dr)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT TOP 1 * FROM EmailTemplate WHERE EmailTypeID=" + emailType.GetHashCode() + " ORDER BY DateCreated DESC", cn);
            cn.Open();
            SqlDataReader templatedr = cmd.ExecuteReader();
            if (templatedr.Read())
            { // SMS_NUM,NAME,Address (Contact),Email
                String template = templatedr["BodyTemplate"].ToString();
                template = template.Replace("{SMS_ID}", dr["SMS_NUM"].ToString());
                template = template.Replace("{PrimaryContact}", dr["Address"].ToString());
                template = template.Replace("{Email}", dr["Email"].ToString());
                return template.Replace("{ClientName}", dr["NAME"].ToString());
            }
            return null;
        }

        public int Send(int EmailTypeId, int ClientId)
        {
            EmailType emailType = getEmailTypeFromId(EmailTypeId);

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand(getSelectStatement(emailType, ClientId), cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            int lastEmailId = 0;
            while (dr.Read())
            {
                SendEmail(getToAddress(emailType, dr), getSubject(emailType, dr), getBody(emailType, dr));
                lastEmailId = LogEmail(emailType.GetHashCode(), ClientId);
            }
            return lastEmailId;
        }
        public int Send(EmailTracking email)
        {
            return Send(email.EmailTypeId,email.ClientId);
        }

        public void SendEmail(String ToAddress, String Subject, String Body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(FromAddress);
                mail.To.Add(new MailAddress("isaacplmann@gmail.com"));
                mail.Subject = Subject;
                mail.Body = Body;
                SmtpClient smtp = new SmtpClient(Host, 25);
                smtp.Credentials = new System.Net.NetworkCredential("no-reply@smsosu.com", "smsWsd93fg");
                smtp.EnableSsl = false;
                smtp.Send(mail);
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }
        }

        public int LogEmail(int EmailTypeId, int ClientId)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO EmailTracking (EmailTypeID,SMS_NUM) OUTPUT Inserted.ID VALUES(@EmailTypeID,@SMS_NUM)", cn);
            cmd.Parameters.AddWithValue("@EmailTypeID", EmailTypeId);
            cmd.Parameters.AddWithValue("@SMS_NUM", ClientId);

            cn.Open();
            int newId = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();

            return newId;
        }
    }
}