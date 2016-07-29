using OSUDental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace OSUDental.Services
{
    public class EmailRepository
    {
        public EmailRepository()
        {
        }

        private bool HasId(int Id, EmailTracking email)
        {
            return email.Id == Id;
        }

        public List<EmailTracking> GetAllEmails()
        {
            return GetAllEmails(null, null, new PageDetails());
        }
        public List<EmailTracking> GetAllEmails(DateTime? startDate, DateTime? endDate, PageDetails pageDetails)
        {
            return GetAllEmails(0, startDate, endDate, pageDetails);
        }
        public List<EmailTracking> GetAllEmails(int clientId)
        {
            return GetAllEmails(clientId, null, null, new PageDetails());
        }
        public List<EmailTracking> GetAllEmails(String username, DateTime? startDate, DateTime? endDate, PageDetails pageDetails)
        {
            int clientId = AuthenticationHelper.GetClientId(username);
            return GetAllEmails(clientId, startDate, endDate, pageDetails);
        }
        public List<EmailTracking> GetAllEmails(int clientId, DateTime? startDate, DateTime? endDate, PageDetails pageDetails)
        {
            String orderBy = "DateSent";
            if (pageDetails.sortColumn.Equals("Id"))
            {
                orderBy = "ID";
            }
            else if (pageDetails.sortColumn.Equals("ClientId"))
            {
                orderBy = "SMS_NUM";
            }
            else if (pageDetails.sortColumn.Equals("EmailTypeId"))
            {
                orderBy = "EmailTypeId";
            }
            String dir = "ASC";
            if (pageDetails.direction == SortDirection.desc)
            {
                dir = "DESC";
            }

            DateTime start;
            DateTime minDate = DateTime.Parse("1/1/1900");
            if (startDate.HasValue)
            {
                start = (DateTime)startDate;
                if (DateTime.Compare(start, minDate) < 0)
                {
                    start = minDate;
                }
            }
            else
            {
                start = minDate;
            }
            DateTime end;
            if (endDate.HasValue)
            {
                end = (DateTime)endDate;
                if (DateTime.Compare(end, minDate) < 0)
                {
                    end = minDate;
                }
            }
            else
            {
                end = DateTime.Now;
            }

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            if (!AuthenticationHelper.IsAdmin())
            { // User viewing their results
                cmd = new SqlCommand("SELECT TOP " + pageDetails.GetPageSize() + " * FROM (SELECT et.ID,et.SMS_NUM,et.EmailTypeID,et.DateSent,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM EmailTracking AS et WHERE et.SMS_NUM = @SMS_NUM AND et.DateSent BETWEEN @StartDate AND @EndDate) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                cmd.Parameters.AddWithValue("@SMS_NUM", AuthenticationHelper.GetClientId());
                cmd.Parameters.AddWithValue("@StartDate", start);
                cmd.Parameters.AddWithValue("@EndDate", end);
                cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
            }
            else
            { // Admin viewing...
                if (clientId==0)
                { // ...all results.
                    cmd = new SqlCommand("SELECT TOP " + pageDetails.GetPageSize() + " * FROM (SELECT et.ID,et.SMS_NUM,et.EmailTypeID,et.DateSent,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM EmailTracking AS et WHERE et.DateSent BETWEEN @StartDate AND @EndDate) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    cmd.Parameters.AddWithValue("@StartDate", start);
                    cmd.Parameters.AddWithValue("@EndDate", end);
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                }
                else
                { // ...one client's results.
                    cmd = new SqlCommand("SELECT TOP " + pageDetails.GetPageSize() + " * FROM (SELECT et.ID,et.SMS_NUM,et.EmailTypeID,et.DateSent,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM EmailTracking AS et WHERE et.SMS_NUM = @SMS_NUM AND et.DateSent BETWEEN @StartDate AND @EndDate) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    cmd.Parameters.AddWithValue("@SMS_NUM", clientId);
                    cmd.Parameters.AddWithValue("@StartDate", start);
                    cmd.Parameters.AddWithValue("@EndDate", end);
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                }
            }
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<EmailTracking> emails = new List<EmailTracking>();
            while (dr.Read())
            {
                emails.Add(new EmailTracking
                {
                    Id = (int)dr["ID"],
                    ClientId = Convert.ToInt32(dr["SMS_NUM"] == DBNull.Value ? -1 : dr["SMS_NUM"]),
                    EmailTypeId = Convert.ToInt32(dr["EmailTypeID"] == DBNull.Value ? -1 : dr["EmailTypeID"]),
                    DateSent = (DateTime)dr["DateSent"],
                });
            }

            cn.Close();
            return emails;
        }

        public EmailTracking GetEmailTracking(int Id) {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM EmailTracking WHERE ID=@EmailTrackingID", cn);
            cmd.Parameters.AddWithValue("@EmailTrackingID", Id);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<EmailTracking> emails = new List<EmailTracking>();
            while (dr.Read())
            {
                emails.Add(new EmailTracking
                {
                    Id = (int)dr["ID"],
                    ClientId = Convert.ToInt32(dr["SMS_NUM"] == DBNull.Value ? -1 : dr["SMS_NUM"]),
                    EmailTypeId = Convert.ToInt32(dr["EmailTypeID"] == DBNull.Value ? -1 : dr["EmailTypeID"]),
                    DateSent = (DateTime)dr["DateSent"],
                });
            }

            cn.Close();
            if (emails.Count > 0)
            {
                return emails[0];
            }
            return null;
        }

        public List<EmailType> GetAllEmailTypes()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT et.ID,et.Name FROM EmailType AS et", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<EmailType> types = new List<EmailType>();
            while (dr.Read())
            {
                types.Add(new EmailType
                {
                    Id = (int)dr["ID"],
                    Name = dr["Name"].ToString(),
                });
            }

            cn.Close();
            return types;
        }

        public List<EmailTemplate> GetAllEmailTemplates()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT et.ID,et.EmailTypeID,et.Subject,et.BodyTemplate,et.DateCreated FROM EmailTemplate AS et", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<EmailTemplate> templates = new List<EmailTemplate>();
            while (dr.Read())
            {
                templates.Add(new EmailTemplate
                {
                    Id = (int)dr["ID"],
                    EmailTypeId = Convert.ToInt32(dr["EmailTypeID"] == DBNull.Value ? -1 : dr["EmailTypeID"]),
                    Subject = dr["Subject"].ToString(),
                    BodyTemplate = dr["BodyTemplate"].ToString(),
                    DateCreated = (DateTime)dr["DateCreated"],
                });
            }

            cn.Close();
            return templates;
        }

        public EmailTemplate GetEmailTemplate(int Id) {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM EmailTemplate WHERE ID=@EmailTemplateID", cn);
            cmd.Parameters.AddWithValue("@EmailTemplateID", Id);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<EmailTemplate> templates = new List<EmailTemplate>();
            while (dr.Read())
            {
                templates.Add(new EmailTemplate
                {
                    Id = (int)dr["ID"],
                    EmailTypeId = Convert.ToInt32(dr["EmailTypeID"] == DBNull.Value ? -1 : dr["EmailTypeID"]),
                    Subject = dr["Subject"].ToString(),
                    BodyTemplate = dr["BodyTemplate"].ToString(),
                    DateCreated = (DateTime)dr["DateCreated"],
                });
            }

            cn.Close();
            if (templates.Count > 0)
            {
                return templates[0];
            }
            return null;
        }

        public int CreateTemplate(EmailTemplate template)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO EmailTemplate (EmailTypeID,Subject,BodyTemplate) OUTPUT Inserted.TEMPLATE_ID VALUES(@EmailTypeID,@Subject,@BodyTemplate)", cn);
            cmd.Parameters.AddWithValue("@EmailTypeID", template.EmailTypeId);
            cmd.Parameters.AddWithValue("@Subject", template.Subject);
            cmd.Parameters.AddWithValue("@BodyTemplate", template.BodyTemplate);

            cn.Open();
            int newId = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();

            return newId;
        }

        public bool SaveEmailTemplate(EmailTemplate template)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE EmailTemplate SET EmailTypeID=@EmailTypeID,Subject=@Subject,BodyTemplate=@BodyTemplate WHERE ID=@TemplateId", cn);
            cmd.Parameters.AddWithValue("@TemplateId", template.Id);
            cmd.Parameters.AddWithValue("@EmailTypeID", template.EmailTypeId);
            cmd.Parameters.AddWithValue("@Subject", template.Subject);
            cmd.Parameters.AddWithValue("@BodyTemplate", template.BodyTemplate);

            cn.Open();
            int rows = cmd.ExecuteNonQuery();
            cn.Close();

            return rows > 0;
        }

        //public Result DeleteResult(int resultId)
        //{
        //    SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand("DELETE SMRESLT OUTPUT Deleted.* FROM SMRESLT WHERE RESULT_ID=@ResultID", cn);
        //    cmd.Parameters.AddWithValue("@ResultID", resultId);

        //    cn.Open();
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    List<Result> results = new List<Result>();
        //    while (dr.Read())
        //    {
        //        results.Add(new Result
        //        {
        //            Id = (int)dr["RESULT_ID"],
        //            ClientId = Convert.ToInt32(dr["RSMS_NUM"] == DBNull.Value ? -1 : dr["RSMS_NUM"]),
        //            TestDate = (DateTime)dr["TEST_DATE"],
        //            EnterDate = (DateTime)dr["REC_DATE"],
        //            TestResult = dr["RESULT"].Equals("+"),
        //            EquipId = dr["REQUIPT"].ToString(),
        //            Reference = dr["HINT"].ToString()
        //        });
        //    }

        //    cn.Close();
        //    if (results.Count > 0)
        //    {
        //        return results[0];
        //    }
        //    return null;
        //}
    }
}