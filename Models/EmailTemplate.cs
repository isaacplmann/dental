using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSUDental.Models
{
    public class EmailTemplate
    {
        public int Id { get; set; }
        public int EmailTypeId { get; set; }
        public String Subject { get; set; }
        public String BodyTemplate { get; set; }
        public DateTime DateCreated { get; set; }
    }
}