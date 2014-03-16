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
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SMEQPT AS e INNER JOIN SMCLNT AS c ON e.SMS_ID = c.SMS_NUM WHERE c.UserName = @UserName", cn);
            cmd.Parameters.AddWithValue("@UserName", username);
            cn.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();
            return count;
        }

        public List<Equipment> GetAllEquipment()
        {
            return GetAllEquipment(new PageDetails(),null);
        }
        public List<Equipment> GetAllEquipment(PageDetails pageDetails, String search)
        {
            return GetAllEquipment(0, pageDetails,search);
        }
        public List<Equipment> GetAllEquipment(String username)
        {
            return GetAllEquipment(username, new PageDetails(),null);
        }
        public List<Equipment> GetAllEquipment(int clientId)
        {
            return GetAllEquipment(clientId, new PageDetails(),null);
        }
        public List<Equipment> GetAllEquipment(int clientId, PageDetails pageDetails, String search)
        {
            String orderBy = "Eqp_ID";
            if (pageDetails.sortColumn.Equals("Id"))
            {
                orderBy = "Eqp_ID";
            }
            else if (pageDetails.sortColumn.Equals("Type"))
            {
                orderBy = "Type";
            }
            else if (pageDetails.sortColumn.Equals("IsActive"))
            {
                orderBy = "Active";
            }

            String dir = "ASC";
            if (pageDetails.direction.Equals(SortDirection.desc))
            {
                dir = "DESC";
            }

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd;
            if (!AuthenticationHelper.IsAdmin())
            { // Client viewing their equipment
                cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT e.SMS_ID,e.Type,e.Active,e.Eqp_ID,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMEQPT AS e WHERE e.SMS_ID = @SMSID) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                cmd.Parameters.AddWithValue("@SMSID", AuthenticationHelper.GetClientId());
                cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
            }
            else
            { // Admin viewing...
                if (clientId==0)
                { // ...all clients' equipment
                    cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT SMS_ID,Type,Active,Eqp_ID,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMEQPT AS e" + (search == null ? "" : " WHERE (Eqp_ID LIKE @Search OR Type LIKE @Search)") + ") AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
                    if (search != null)
                        cmd.Parameters.AddWithValue("@Search", '%' + search + '%');
                    cmd.Parameters.AddWithValue("@Start", pageDetails.GetStartingRow());
                    cmd.Parameters.AddWithValue("@End", pageDetails.GetEndingRow());
                }
                else
                { // ...one client's equipment
                    cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT e.SMS_ID,e.Type,e.Active,e.Eqp_ID,ROW_NUMBER() OVER (ORDER BY " + orderBy + " " + dir + ") AS RowNum FROM SMEQPT AS e WHERE e.SMS_ID = @SMSID) AS T WHERE T.RowNum BETWEEN @Start AND @End", cn);
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
        public List<Equipment> GetAllEquipment(String username, PageDetails pageDetails, String search)
        {
            int clientId = AuthenticationHelper.GetClientId(username);
            return GetAllEquipment(clientId, pageDetails, search);
        }

        public Equipment GetEquipment(int Id) {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM SMEQPT WHERE Eqp_ID=@EquipmentID", cn);
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

        public int CreateEquipment(Equipment equipment)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO SMEQPT (SMS_ID,Type,Active) OUTPUT Inserted.Eqp_ID VALUES (@ClientID,@Type,@IsActive)", cn);
            cmd.Parameters.AddWithValue("@ClientID", equipment.ClientId);
            cmd.Parameters.AddWithValue("@Type", equipment.Type);
            cmd.Parameters.AddWithValue("@IsActive", equipment.IsActive ? -1 : 0);

            cn.Open();
            int newId = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();

            return newId;
        }

        public bool SaveEquipment(Equipment equipment)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE SMEQPT SET Type=@Type,Active=@IsActive WHERE Eqp_ID=@EquipmentID", cn);
            cmd.Parameters.AddWithValue("@Type", equipment.Type);
            cmd.Parameters.AddWithValue("@IsActive", equipment.IsActive ? -1 : 0);
            cmd.Parameters.AddWithValue("@EquipmentID", equipment.Id);

            cn.Open();
            int rows = cmd.ExecuteNonQuery();
            cn.Close();

            return rows > 0;
        }

        public Equipment DeleteEquipment(int equipmentId)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("DELETE SMEQPT OUTPUT DELETED.* FROM SMEQPT WHERE Eqp_ID=@EquipmentID", cn);
            cmd.Parameters.AddWithValue("@EquipmentID", equipmentId);
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
            if (equipments.Count > 0)
            {
                return equipments[0];
            }
            return null;
        }
    }
}