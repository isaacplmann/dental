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
    public class OrderRepository
    {
        public OrderRepository()
        {
        }

        public List<Order> GetAllOrders()
        {
            return GetAllOrders(new PageDetails());
        }
        public List<Order> GetAllOrders(PageDetails pageDetails)
        {
            return GetAllOrders(null, pageDetails);
        }
        public List<Order> GetAllOrders(String username)
        {
            return GetAllOrders(username, new PageDetails());
        }
        public List<Order> GetAllOrders(int clientId)
        {
            return GetAllOrders(clientId, new PageDetails());
        }
        public List<Order> GetAllOrders(int clientId, PageDetails pageDetails)
        {
            String username = AuthenticationHelper.GetUserName(clientId);
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Not a valid client ID.", "clientId");
            }
            return GetAllOrders(username,pageDetails);
        }
        public List<Order> GetAllOrders(String username, PageDetails pageDetails)
        {
            String orderBy = "ORDER_ID";
            if (pageDetails.sortColumn.Equals("DateReceived"))
            {
                orderBy = "REC_DATE";
            }
            //else if (sortColumn.Equals("TestDate"))
            //{
            //    orderBy = "TEST_DATE";
            //}
            String dir = "ASC";
            if (pageDetails.direction == SortDirection.desc)
            {
                dir = "DESC";
            }

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            if (!AuthenticationHelper.IsAdmin())
            {
                cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT o.ORDER_ID,o.SMS_NUM,o.REC_DATE,o.AMT_DUE,o.PMT_TYPE,o.PLACED_BY,o.CHECK_NO,o.TAKEN_BY,o.LOT,o.ORDER_SIZE,o.NO_OF_UNITS,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.CUST_ORDERS AS o INNER JOIN SMS.dbo.SMCLNT AS c ON o.SMS_NUM = c.SMS_NUM WHERE c.UserName = @UserName) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                cmd.Parameters.AddWithValue("@UserName", AuthenticationHelper.GetUserName());
                cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
            }
            else
            {
                if(String.IsNullOrEmpty(username)) {
                    cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT o.ORDER_ID,o.SMS_NUM,o.REC_DATE,o.AMT_DUE,o.PMT_TYPE,o.PLACED_BY,o.CHECK_NO,o.TAKEN_BY,o.LOT,o.ORDER_SIZE,o.NO_OF_UNITS,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.CUST_ORDERS AS o) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                } else {
                    cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT o.ORDER_ID,o.SMS_NUM,o.REC_DATE,o.AMT_DUE,o.PMT_TYPE,o.PLACED_BY,o.CHECK_NO,o.TAKEN_BY,o.LOT,o.ORDER_SIZE,o.NO_OF_UNITS,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.CUST_ORDERS AS o INNER JOIN SMS.dbo.SMCLNT AS c ON o.SMS_NUM = c.SMS_NUM WHERE c.UserName = @UserName) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                }
            }
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Order> orders = new List<Order>();
            //orders.Add(new Order
            //{
            //    Id = 0,
            //    TestDate = start,
            //    EnterDate = end,
            //    TestOrder = false,
            //    EquipId = "Page: " + page + ", Size: " + pageSize,
            //    Reference = "Sort: " + orderBy + " " + dir
            //});
            while (dr.Read())
            {
                orders.Add(new Order {
                    Id = (int)(dr["ORDER_ID"] == DBNull.Value ? -1 : dr["ORDER_ID"]),
                    ClientId = (int)(dr["SMS_NUM"] == DBNull.Value ? -1 : dr["SMS_NUM"]),
                    DateReceived = (DateTime?)(dr["REC_DATE"] == DBNull.Value ? null : dr["REC_DATE"]),
                    AmountDue = Convert.ToDouble(dr["AMT_DUE"] == DBNull.Value ? -1 : dr["AMT_DUE"]),
                    PaymentType = dr["PMT_TYPE"].ToString(),
                    PlacedBy = dr["PLACED_BY"].ToString(),
                    CheckNumber = dr["CHECK_NO"].ToString(),
                    TakenBy = dr["TAKEN_BY"].ToString(),
                    Lot = dr["LOT"].ToString(),
                    OrderType = dr["ORDER_SIZE"].ToString(),
                    Units = (int)(dr["NO_OF_UNITS"] == DBNull.Value ? -1 : dr["NO_OF_UNITS"]),
                });
            }

            cn.Close();
            return orders;
        }

        public Order GetOrder(int Id) {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM SMS.dbo.CUST_ORDERS WHERE ORDER_ID=@OrderID", cn);
            cmd.Parameters.AddWithValue("@OrderID", Id);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Order> orders = new List<Order>();
            while (dr.Read())
            {
                orders.Add(new Order
                {
                    Id = (int)(dr["ORDER_ID"] == DBNull.Value ? -1 : dr["ORDER_ID"]),
                    ClientId = (int)(dr["SMS_NUM"] == DBNull.Value ? -1 : dr["SMS_NUM"]),
                    DateReceived = (DateTime?)(dr["REC_DATE"] == DBNull.Value ? null : dr["REC_DATE"]),
                    AmountDue = Convert.ToDouble(dr["AMT_DUE"] == DBNull.Value ? -1 : dr["AMT_DUE"]),
                    PaymentType = dr["PMT_TYPE"].ToString(),
                    PlacedBy = dr["PLACED_BY"].ToString(),
                    CheckNumber = dr["CHECK_NO"].ToString(),
                    TakenBy = dr["TAKEN_BY"].ToString(),
                    Lot = dr["LOT"].ToString(),
                    OrderType = dr["ORDER_SIZE"].ToString(),
                    Units = (int)(dr["NO_OF_UNITS"] == DBNull.Value ? -1 : dr["NO_OF_UNITS"]),
                });
            }

            cn.Close();
            if (orders.Count > 0) {
                return orders[0];
            }
            return null;
        }

        public bool SaveOrder(Order order) {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE SMS.dbo.CUST_ORDERS SET REC_DATE=@RecDate,AMT_DUE=@AmtDue,PMT_TYPE=@PmtType,PLACED_BY=@PlacedBy,CHECK_NO=@CheckNo,TAKEN_BY=@TakenBy,LOT=@Lot,ORDER_SIZE=@OrderSize,NO_OF_UNITS=@Units WHERE ORDER_ID=@OrderID", cn);
            cmd.Parameters.AddWithValue("@RecDate", order.DateReceived);
            cmd.Parameters.AddWithValue("@AmtDue", order.AmountDue);
            cmd.Parameters.AddWithValue("@PmtType", order.PaymentType);
            cmd.Parameters.AddWithValue("@PlacedBy", order.PlacedBy);
            cmd.Parameters.AddWithValue("@CheckNo", order.CheckNumber);
            cmd.Parameters.AddWithValue("@TakenBy", order.TakenBy);
            cmd.Parameters.AddWithValue("@Lot", order.Lot);
            cmd.Parameters.AddWithValue("@OrderSize", order.OrderType);
            cmd.Parameters.AddWithValue("@Units", order.Units);
            cmd.Parameters.AddWithValue("@OrderID", order.Id);

            cn.Open();
            int rows = cmd.ExecuteNonQuery();
            cn.Close();

            return rows > 0;
        }

        public Order DeleteOrder(int orderId)
        {
            // TODO
             throw new NotImplementedException("Delete Order not coded.");
            //return null;
        }
    }
}