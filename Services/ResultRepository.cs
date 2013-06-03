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
    public class ResultRepository
    {
        public ResultRepository()
        {
        }

        private bool HasId(int Id, Result result)
        {
            return result.Id == Id;
        }

        public int GetTotalResults()
        {
            return GetTotalResults(null, null);
        }

        public int GetTotalResults(DateTime? startDate, DateTime? endDate)
        {
            String username = AuthenticationHelper.GetUserName();
            if (String.IsNullOrEmpty(username))
            {
                return 0;
            }

            DateTime start;
            if (startDate.HasValue)
            {
                start = (DateTime)startDate;
            }
            else
            {
                start = DateTime.Parse("1/1/1900");
            }
            DateTime end;
            if (endDate.HasValue)
            {
                end = (DateTime)endDate;
            }
            else
            {
                end = DateTime.Now;
            }

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SMS.dbo.SMRESLT AS r INNER JOIN SMS.dbo.SMCLNT AS c ON r.RSMS_NUM = c.SMS_NUM WHERE c.UserName = @UserName AND r.TEST_DATE BETWEEN @StartDate AND @EndDate", cn);
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@StartDate", start);
            cmd.Parameters.AddWithValue("@EndDate", end);
            cn.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();
            return count;
        }

        public List<Result> GetAllResults()
        {
            return GetAllResults(null, null, new PageDetails());
        }
        public List<Result> GetAllResults(DateTime? startDate, DateTime? endDate, PageDetails pageDetails)
        {
            return GetAllResults(0, startDate, endDate, pageDetails);
        }
        public List<Result> GetAllResults(int clientId)
        {
            return GetAllResults(clientId, null, null, new PageDetails());
        }
        public List<Result> GetAllResults(String username, DateTime? startDate, DateTime? endDate, PageDetails pageDetails)
        {
            int clientId = AuthenticationHelper.GetClientId(username);
            return GetAllResults(clientId, startDate, endDate, pageDetails);
        }
        public List<Result> GetAllResults(int clientId, DateTime? startDate, DateTime? endDate, PageDetails pageDetails)
        {
            String orderBy = "RESULT_ID";
            if (pageDetails.sortColumn.Equals("EnterDate"))
            {
                orderBy = "REC_DATE";
            }
            else if (pageDetails.sortColumn.Equals("TestDate"))
            {
                orderBy = "TEST_DATE";
            }
            else if (pageDetails.sortColumn.Equals("TestResult"))
            {
                orderBy = "RESULT";
            }
            else if (pageDetails.sortColumn.Equals("EquipId"))
            {
                orderBy = "REQUIPT";
            }
            else if (pageDetails.sortColumn.Equals("Reference"))
            {
                orderBy = "Hint";
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
            {
                cmd = new SqlCommand("SELECT TOP "+pageDetails.GetPageSize()+" * FROM (SELECT r.RSMS_NUM,RESULT_ID,TEST_DATE,REC_DATE,RESULT,REQUIPT,HINT,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.SMRESLT AS r WHERE r.RSMS_NUM = @RSMSNUM AND r.TEST_DATE BETWEEN @StartDate AND @EndDate) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                cmd.Parameters.AddWithValue("@RSMSNUM", AuthenticationHelper.GetClientId());
                cmd.Parameters.AddWithValue("@StartDate", start);
                cmd.Parameters.AddWithValue("@EndDate", end);
                cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
            }
            else
            {
                if (clientId==0)
                {
                    cmd = new SqlCommand("SELECT TOP " + pageDetails.GetPageSize() + " * FROM (SELECT r.RSMS_NUM,RESULT_ID,TEST_DATE,REC_DATE,RESULT,REQUIPT,HINT,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.SMRESLT AS r WHERE r.TEST_DATE BETWEEN @StartDate AND @EndDate) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    cmd.Parameters.AddWithValue("@StartDate", start);
                    cmd.Parameters.AddWithValue("@EndDate", end);
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                }
                else
                {
                    cmd = new SqlCommand("SELECT TOP " + pageDetails.GetPageSize() + " * FROM (SELECT r.RSMS_NUM,RESULT_ID,TEST_DATE,REC_DATE,RESULT,REQUIPT,HINT,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.SMRESLT AS r WHERE r.RSMS_NUM = @RSMSNUM AND r.TEST_DATE BETWEEN @StartDate AND @EndDate) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    cmd.Parameters.AddWithValue("@RSMSNUM", clientId);
                    cmd.Parameters.AddWithValue("@StartDate", start);
                    cmd.Parameters.AddWithValue("@EndDate", end);
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                }
            }
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Result> results = new List<Result>();
            //results.Add(new Result
            //{
            //    Id = 0,
            //    TestDate = start,
            //    EnterDate = end,
            //    TestResult = false,
            //    EquipId = "Page: " + page + ", Size: " + pageSize,
            //    Reference = "Sort: " + orderBy + " " + dir
            //});
            while (dr.Read())
            {
                results.Add(new Result
                {
                    Id = (int)dr["RESULT_ID"],
                    ClientId = Convert.ToInt32(dr["RSMS_NUM"] == DBNull.Value ? -1 : dr["RSMS_NUM"]),
                    TestDate = (DateTime)dr["TEST_DATE"],
                    EnterDate = (DateTime)dr["REC_DATE"],
                    TestResult = dr["RESULT"].Equals("+"),
                    EquipId = dr["REQUIPT"].ToString(),
                    Reference = dr["HINT"].ToString()
                });
            }

            cn.Close();
            return results;
        }

        public Result GetResult(int Id) {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM SMS.dbo.SMRESLT WHERE RESULT_ID=@ResultID", cn);
            cmd.Parameters.AddWithValue("@ResultID", Id);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Result> results = new List<Result>();
            while (dr.Read())
            {
                results.Add(new Result
                {
                    Id = (int)dr["RESULT_ID"],
                    ClientId = Convert.ToInt32(dr["RSMS_NUM"] == DBNull.Value ? -1 : dr["RSMS_NUM"]),
                    TestDate = (DateTime)dr["TEST_DATE"],
                    EnterDate = (DateTime)dr["REC_DATE"],
                    TestResult = dr["RESULT"].Equals("+"),
                    EquipId = dr["REQUIPT"].ToString(),
                    Reference = dr["HINT"].ToString()
                });
            }

            cn.Close();
            if (results.Count > 0) {
                return results[0];
            }
            return null;
        }

        public int CreateResult(Result result)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO SMS.dbo.SMRESLT (TEST_DATE,REC_DATE,RESULT,REQUIPT,HINT) OUTPUT Inserted.RESULT_ID VALUES(@TestDate,@RecDate,@Result,@Requipt,@Hint)", cn);
            cmd.Parameters.AddWithValue("@TestDate", result.TestDate);
            cmd.Parameters.AddWithValue("@RecDate", result.EnterDate);
            cmd.Parameters.AddWithValue("@Result", result.TestResult?"+":"-");
            cmd.Parameters.AddWithValue("@Requipt", result.EquipId);
            cmd.Parameters.AddWithValue("@Hint", result.Reference);

            cn.Open();
            int newId = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();

            return newId;
        }

        public bool SaveResult(Result result)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE SMS.dbo.SMRESLT SET TEST_DATE=@TestDate,REC_DATE=@RecDate,RESULT=@Result,REQUIPT=@Requipt,HINT=@Hint WHERE RESULT_ID=@ResultID", cn);
            cmd.Parameters.AddWithValue("@TestDate", result.TestDate);
            cmd.Parameters.AddWithValue("@RecDate", result.EnterDate);
            cmd.Parameters.AddWithValue("@Result", result.TestResult ? "+" : "-");
            cmd.Parameters.AddWithValue("@Requipt", result.EquipId);
            cmd.Parameters.AddWithValue("@Hint", result.Reference);
            cmd.Parameters.AddWithValue("@ResultID", result.Id);

            cn.Open();
            int rows = cmd.ExecuteNonQuery();
            cn.Close();

            return rows > 0;
        }

        public Result DeleteResult(int resultId)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("DELETE TOP 1 SMS.dbo.SMRESLT OUTPUT DELETED.* WHERE RESULT_ID=@ResultID", cn);
            cmd.Parameters.AddWithValue("@ResultID", resultId);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Result> results = new List<Result>();
            while (dr.Read())
            {
                results.Add(new Result
                {
                    Id = (int)dr["RESULT_ID"],
                    ClientId = Convert.ToInt32(dr["RSMS_NUM"] == DBNull.Value ? -1 : dr["RSMS_NUM"]),
                    TestDate = (DateTime)dr["TEST_DATE"],
                    EnterDate = (DateTime)dr["REC_DATE"],
                    TestResult = dr["RESULT"].Equals("+"),
                    EquipId = dr["REQUIPT"].ToString(),
                    Reference = dr["HINT"].ToString()
                });
            }

            cn.Close();
            if (results.Count > 0)
            {
                return results[0];
            }
            return null;
        }
    }
}