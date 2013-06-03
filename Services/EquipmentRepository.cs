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
    public class EquipmentRepository
    {
        public EquipmentRepository()
        {
        }

        private bool HasId(int Id, Equipment equipment)
        {
            return equipment.Id == Id;
        }

        public int GetTotalEquipment()
        {
            String username = AuthenticationHelper.GetUserName();
            if (String.IsNullOrEmpty(username))
            {
                return 0;
            }

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SMS.dbo.SMEQPT AS e INNER JOIN SMS.dbo.SMCLNT AS c ON e.SMS_ID = c.SMS_NUM WHERE c.UserName = @UserName", cn);
            cmd.Parameters.AddWithValue("@UserName", username);
            cn.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();
            return count;
        }

        public List<Equipment> GetAllEquipment()
        {
            return GetAllEquipment(new PageDetails());
        }
        public List<Equipment> GetAllEquipment(PageDetails pageDetails)
        {
            return GetAllEquipment(0, pageDetails);
        }
        public List<Equipment> GetAllEquipment(String username)
        {
            return GetAllEquipment(username, new PageDetails());
        }
        public List<Equipment> GetAllEquipment(int clientId)
        {
            return GetAllEquipment(clientId, new PageDetails());
        }
        public List<Equipment> GetAllEquipment(int clientId, PageDetails pageDetails)
        {
            String orderBy = "Eqp_ID";
            //if (sortColumn.Equals("EnterDate"))
            //{
            //    orderBy = "REC_DATE";
            //}
            //else if (sortColumn.Equals("TestDate"))
            //{
            //    orderBy = "TEST_DATE";
            //}
            String dir = "ASC";
            //if (direction.ToLower().Equals("desc"))
            //{
            //    dir = "DESC";
            //}

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            if (!AuthenticationHelper.IsAdmin())
            {
                cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT e.SMS_ID,e.Type,e.Active,e.Eqp_ID,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.SMEQPT AS e WHERE e.SMS_ID = @SMSID) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                cmd.Parameters.AddWithValue("@SMSID", AuthenticationHelper.GetClientId());
                cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
            }
            else
            {
                if (clientId==0)
                {
                    cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT SMS_ID,Type,Active,Eqp_ID,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.SMEQPT AS e) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                }
                else
                {
                    cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT e.SMS_ID,e.Type,e.Active,e.Eqp_ID,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMS.dbo.SMEQPT AS e WHERE e.SMS_ID = @SMSID) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    cmd.Parameters.AddWithValue("@SMSID", clientId);
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                }
            }
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Equipment> equipments = new List<Equipment>();
            //equipments.Add(new Equipment
            //{
            //    Id = 0,
            //    TestDate = start,
            //    EnterDate = end,
            //    TestEquipment = false,
            //    EquipId = "Page: " + page + ", Size: " + pageSize,
            //    Reference = "Sort: " + orderBy + " " + dir
            //});
            while (dr.Read())
            {
                equipments.Add(new Equipment
                {
                    Id = (int)(dr["Eqp_ID"] == DBNull.Value ? -1 : dr["Eqp_ID"]),
                    ClientId = (int)(dr["SMS_ID"] == DBNull.Value ? -1 : dr["SMS_ID"]),
                    Type = (int)(dr["Type"] == DBNull.Value ? -1 : dr["Type"]),
                    IsActive = !dr["Active"].Equals(0),
                });
            }

            cn.Close();
            return equipments;
        }
        public List<Equipment> GetAllEquipment(String username, PageDetails pageDetails)
        {
            int clientId = AuthenticationHelper.GetClientId(username);
            return GetAllEquipment(clientId, pageDetails);
        }

        public Equipment GetEquipment(int Id) {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM SMS.dbo.SMEQPT WHERE Eqp_ID=@EquipmentID", cn);
            cmd.Parameters.AddWithValue("@EquipmentID", Id);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Equipment> equipments = new List<Equipment>();
            while (dr.Read())
            {
                equipments.Add(new Equipment
                {
                    Id = (int)dr["Eqp_ID"],
                    ClientId = (int)dr["SMS_ID"],
                    Type = (int)dr["Type"],
                    IsActive = !dr["Active"].Equals(0),
                });
            }

            cn.Close();
            if (equipments.Count > 0) {
                return equipments[0];
            }
            return null;
        }

        public bool SaveEquipment(Equipment equipment) {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE SMS.dbo.SMEQPT SET Type=@Type,Active=@IsActive WHERE Eqp_ID=@EquipmentID", cn);
            cmd.Parameters.AddWithValue("@Type", equipment.Type);
            cmd.Parameters.AddWithValue("@IsActive", equipment.IsActive?-1:0);
            cmd.Parameters.AddWithValue("@EquipmentID", equipment.Id);

            cn.Open();
            int rows = cmd.ExecuteNonQuery();
            cn.Close();

            return rows > 0;
        }

        public Equipment DeleteEquipment(int equipmentId)
        {
            // TODO
             throw new NotImplementedException("Delete Equipment not coded.");
            //return null;
        }
    }
}