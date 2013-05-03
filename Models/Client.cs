using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSUDental.Models
{
    public class Client
    {
        public int Id { get; set; } // SMS_NUM
        public Boolean IsActive { get; set; } // STATUS
        public int TypeId { get; set; } // TYPE
        public String Name { get; set; } // NAME
        public String Address { get; set; } // ADDRESS Original or Address
        public String Address2 { get; set; } // ADDRESS2
        public String City { get; set; } //CITY
        public String State { get; set; } //STATE
        public String Zip { get; set; } //ZIP
        public String Phone { get; set; } //TELE
        public String Extension { get; set; } //Telephone_Ext
        public DateTime? DateAdded { get; set; } //DTADDED
        public DateTime? DateDropped { get; set; } //DTDROPPED
//      ,[SMS_Dropped]
        public String Fax { get; set; } // FAX
        public String ReferBy { get; set; } // REFER_BY
        public Boolean Certificate { get; set; } // certificate
        public int LifeMember { get; set; } // Life_Member
        public String Email { get; set; } // Email
        public Boolean Graduate { get; set; } // Graduate
//      ,[Alumni_Life]
        public String AlumniAnnual { get; set; } // Alumni_Annual
        public String AlumniID { get; set; } // Alumni_ID
        public String DDSFirstName { get; set; } // DDS_FirstName
        public String DDSLastName { get; set; } // DDS_LastName
      //,[Website]
      //,[DDS_Email]
    }
}