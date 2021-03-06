﻿using OSUDental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlTypes;
using System.Web.Security;

namespace OSUDental.Services
{
    public class ClientRepository
    {
        public ClientRepository()
        {
        }

        private bool HasId(int Id, Client client)
        {
            return client.Id == Id;
        }

        public List<Client> GetAllClients() {
            return GetAllClients(1, 50,"NAME","ASC",null);
        }

        public List<Client> GetAllClients(int Page,int PageSize,String SortColumn,String direction, String search) {
            if (!HttpContext.Current.User.IsInRole("admin"))
            {
                throw new UnauthorizedAccessException("You do not have permission to view all clients' information.");
            }

            String orderBy = "SMS_NUM";
            if (SortColumn.Equals("Name"))
            {
                orderBy = "NAME";
            }
            else if (SortColumn.Equals("Address2"))
            {
                orderBy = "ADDRESS2";
            }
            else if (SortColumn.Equals("State"))
            {
                orderBy = "STATE";
            }
            else if (SortColumn.Equals("DDSFirstName"))
            {
                orderBy = "DDS_FirstName";
            } else if(SortColumn.Equals("DDSLastName")) {
                orderBy = "DDS_LastName";
            }
            String dir = "ASC";
            if (direction.ToLower().Equals("desc")) {
                dir = "DESC";
            }

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 200 * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY "+orderBy+" "+dir+") AS RowNum FROM [SMCLNT] WHERE STATUS <> 0"+(search==null?"":" AND (Name LIKE @Search OR SMS_NUM LIKE @Search)")+") AS C WHERE STATUS <> 0 AND C.RowNum BETWEEN @Start AND @End", cn);
            if(search != null)
                cmd.Parameters.AddWithValue("@Search", '%'+search+'%');
            cmd.Parameters.AddWithValue("@Start", (Page - 1) * PageSize + 1);
            cmd.Parameters.AddWithValue("@End", (Page) * PageSize);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Client> clients = new List<Client>();
            while (dr.Read())
            {
                clients.Add(new Client {
                    Id = (int)dr["SMS_NUM"],
                    IsActive = !dr["STATUS"].Equals("0"),
                    TypeId = dr["TYPE"].ToString().Equals("") ? 0 : Int16.Parse(dr["TYPE"].ToString()),
                    Name = dr["NAME"].ToString(),
                    Address = dr["Address"].ToString(),
                    Address2 = dr["ADDRESS2"].ToString(),
                    City = dr["CITY"].ToString(),
                    State = dr["STATE"].ToString(),
                    Zip = dr["ZIP"].ToString(),
                    Phone = dr["TELE"].ToString(),
                    Extension = dr["Telephone_Ext"].ToString(),
                    DateAdded = dr["DTADDED"].ToString().Equals("") ? DateTime.MinValue : DateTime.Parse(dr["DTADDED"].ToString()),
                    DateDropped = dr["DTDROPPED"].ToString().Equals("") ? DateTime.MinValue : DateTime.Parse(dr["DTDROPPED"].ToString()),
                    Fax = dr["FAX"].ToString(),
                    ReferBy = dr["REFER_BY"].ToString(),
                    Certificate = !dr["certificate"].Equals("0"),
                    LifeMember = dr["Life_Member"].ToString().Equals("")?0:Int16.Parse(dr["Life_Member"].ToString()),
                    Email = dr["Email"].ToString(),
                    Graduate = !dr["Graduate"].Equals("0"),
                    AlumniAnnual = dr["Alumni_Annual"].ToString(),
                    AlumniID = dr["Alumni_ID"].ToString(),
                    DDSFirstName = dr["DDS_FirstName"].ToString(),
                    DDSLastName = dr["DDS_LastName"].ToString()
                });
            }

            cn.Close();
            return clients;
        }

        public Client GetClient() {
            String username = HttpContext.Current.User.Identity.Name;
            return GetClient(username);
        }

        private Client GetClient(String username)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM SMCLNT WHERE username=@username", cn);
            cmd.Parameters.AddWithValue("@username", username);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            Client cl = GetClient(dr);
            cn.Close();
            return cl;
        }
        public Client GetClient(int Id)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM SMCLNT WHERE SMS_NUM=@SMS_NUM", cn);
            cmd.Parameters.AddWithValue("@SMS_NUM", Id);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            Client cl = GetClient(dr);
            cn.Close();
            return cl;
        }
        private Client GetClient(IDataReader dr)
        {
            List<Client> clients = new List<Client>();
            while (dr.Read())
            {
                if (!HttpContext.Current.User.IsInRole("admin") && !HttpContext.Current.User.Identity.Name.Equals(dr["UserName"]))
                {
                    throw new UnauthorizedAccessException("You do not have permission to view other clients' information.");
                }

                clients.Add(new Client
                {
                    Id = (int)dr["SMS_NUM"],
                    IsActive = !dr["STATUS"].Equals("0"),
                    TypeId = (int)dr["TYPE"],
                    Name = dr["NAME"].ToString(),
                    Address = dr["Address"].ToString(),
                    Address2 = dr["ADDRESS2"].ToString(),
                    City = dr["CITY"].ToString(),
                    State = dr["STATE"].ToString(),
                    Zip = dr["ZIP"].ToString(),
                    Phone = dr["TELE"].ToString(),
                    Extension = dr["Telephone_Ext"].ToString(),
                    DateAdded = dr["DTADDED"].ToString().Equals("") ? (DateTime?)null : DateTime.Parse(dr["DTADDED"].ToString()),
                    DateDropped = dr["DTDROPPED"].ToString().Equals("") ? (DateTime?)null : DateTime.Parse(dr["DTDROPPED"].ToString()),
                    Fax = dr["FAX"].ToString(),
                    ReferBy = dr["REFER_BY"].ToString(),
                    Certificate = !dr["certificate"].Equals("0"),
                    LifeMember = dr["Life_Member"].ToString().Equals("") ? 0 : Int16.Parse(dr["Life_Member"].ToString()),
                    Email = dr["Email"].ToString(),
                    Graduate = !dr["Graduate"].Equals("0"),
                    AlumniAnnual = dr["Alumni_Annual"].ToString(),
                    AlumniID = dr["Alumni_ID"].ToString(),
                    DDSFirstName = dr["DDS_FirstName"].ToString(),
                    DDSLastName = dr["DDS_LastName"].ToString()
                });
            }

            if (clients.Count > 0) {
                return clients[0];
            }
            return null;
        }

        public int CreateClient(Client client)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO SMCLNT ([STATUS],[TYPE],[NAME],[Address],[ADDRESS2],[CITY],[STATE],[ZIP],[TELE],[Telephone_Ext],[DTADDED],[DTDROPPED],[FAX],[REFER_BY],[certificate],[Life_Member],[Email],[Graduate],[Alumni_Annual],[Alumni_ID],[DDS_LastName],[DDS_FirstName]) OUTPUT Inserted.[SMS_NUM] VALUES(@STATUS,@TYPE,@NAME,@Address,@ADDRESS2,@CITY,@STATE,@ZIP,@TELE,@Telephone_Ext,@DTADDED,@DTDROPPED,@FAX,@REFER_BY,@certificate,@Life_Member,@Email,@Graduate,@Alumni_Annual,@Alumni_ID,@DDS_LastName,@DDS_FirstName)", cn);
            //cmd.Parameters.AddWithValue("@SMS_NUM", client.Id);
            cmd.Parameters.AddWithValue("@STATUS", client.IsActive);
            cmd.Parameters.AddWithValue("@TYPE", client.TypeId);
            cmd.Parameters.AddWithValue("@NAME", client.Name);
            cmd.Parameters.AddWithValue("@Address", client.Address);
            cmd.Parameters.AddWithValue("@ADDRESS2", client.Address2);
            cmd.Parameters.AddWithValue("@CITY", client.City);
            cmd.Parameters.AddWithValue("@STATE", client.State);
            cmd.Parameters.AddWithValue("@ZIP", client.Zip);
            cmd.Parameters.AddWithValue("@TELE", client.Phone);
            cmd.Parameters.AddWithValue("@Telephone_Ext", client.Extension);
            if (client.DateAdded == null)
            {
                cmd.Parameters.Add("@DTADDED", SqlDbType.DateTime).Value = DateTime.Now;
            }
            else
            {
                cmd.Parameters.Add("@DTADDED", SqlDbType.DateTime).Value = client.DateAdded;
            }
            if (client.DateDropped == null)
            {
                cmd.Parameters.Add("@DTDROPPED", SqlDbType.DateTime).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@DTDROPPED", SqlDbType.DateTime).Value = client.DateDropped;
            }
            cmd.Parameters.AddWithValue("@FAX", client.Fax);
            cmd.Parameters.AddWithValue("@REFER_BY", client.ReferBy==null?"":client.ReferBy);
            cmd.Parameters.AddWithValue("@certificate", client.Certificate);
            cmd.Parameters.AddWithValue("@Life_Member", client.LifeMember);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@Graduate", client.Graduate);
            cmd.Parameters.AddWithValue("@Alumni_Annual", client.AlumniAnnual == null ? "" : client.AlumniAnnual);
            cmd.Parameters.AddWithValue("@Alumni_ID", client.AlumniID == null ? "" : client.AlumniID);
            cmd.Parameters.AddWithValue("@DDS_FirstName", client.DDSFirstName);
            cmd.Parameters.AddWithValue("@DDS_LastName", client.DDSLastName);

            cn.Open();
            int newId = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();

            return newId;
        }

        public bool SaveClient(Client client)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE SMCLNT SET [STATUS] = @STATUS,[TYPE] = @TYPE,[NAME] = @NAME,[Address] = @Address,[ADDRESS2] = @ADDRESS2,[CITY] = @CITY,[STATE] = @STATE,[ZIP] = @ZIP,[TELE] = @TELE,[Telephone_Ext] = @Telephone_Ext,[DTADDED] = @DTADDED,[DTDROPPED] = @DTDROPPED,[FAX] = @FAX,[REFER_BY] = @REFER_BY,[certificate] = @certificate,[Life_Member] = @Life_Member,[Email] = @Email,[Graduate] = @Graduate,[Alumni_Annual] = @Alumni_Annual,[Alumni_ID] = @Alumni_ID,[DDS_LastName] = @DDS_LastName,[DDS_FirstName] = @DDS_FirstName WHERE [SMS_NUM] = @SMS_NUM", cn);
            cmd.Parameters.AddWithValue("@SMS_NUM", client.Id);
            cmd.Parameters.AddWithValue("@STATUS", client.IsActive);
            cmd.Parameters.AddWithValue("@TYPE", client.TypeId);
            cmd.Parameters.AddWithValue("@NAME", client.Name);
            cmd.Parameters.AddWithValue("@Address", client.Address);
            cmd.Parameters.AddWithValue("@ADDRESS2", client.Address2);
            cmd.Parameters.AddWithValue("@CITY", client.City);
            cmd.Parameters.AddWithValue("@STATE", client.State);
            cmd.Parameters.AddWithValue("@ZIP", client.Zip);
            cmd.Parameters.AddWithValue("@TELE", client.Phone);
            cmd.Parameters.AddWithValue("@Telephone_Ext", client.Extension);
            if (client.DateAdded == null)
            {
                cmd.Parameters.Add("@DTADDED", SqlDbType.DateTime).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@DTADDED", SqlDbType.DateTime).Value = client.DateAdded;
            }
            if (client.DateDropped == null)
            {
                cmd.Parameters.Add("@DTDROPPED", SqlDbType.DateTime).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@DTDROPPED", SqlDbType.DateTime).Value = client.DateDropped;
            }
            cmd.Parameters.AddWithValue("@FAX", client.Fax);
            cmd.Parameters.AddWithValue("@REFER_BY", client.ReferBy);
            cmd.Parameters.AddWithValue("@certificate", client.Certificate);
            cmd.Parameters.AddWithValue("@Life_Member", client.LifeMember);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@Graduate", client.Graduate);
            cmd.Parameters.AddWithValue("@Alumni_Annual", client.AlumniAnnual);
            cmd.Parameters.AddWithValue("@Alumni_ID", client.AlumniID);
            cmd.Parameters.AddWithValue("@DDS_FirstName", client.DDSFirstName);
            cmd.Parameters.AddWithValue("@DDS_LastName", client.DDSLastName);

            cn.Open();
            int rows = cmd.ExecuteNonQuery();
            cn.Close();

            return rows > 0;
        }

        public Client DeleteClient(int clientId)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("DELETE SMCLNT OUTPUT DELETED.* FROM SMCLNT WHERE SMS_NUM=@SMS_NUM", cn);
            cmd.Parameters.AddWithValue("@SMS_NUM", clientId);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            Client cl = GetClient(dr);
            cn.Close();
            return cl;
        }
    }
}