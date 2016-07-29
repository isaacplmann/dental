using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSUDental.Models
{
    public class EmailTracking
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int EmailTypeId { get; set; }
        public DateTime DateSent { get; set; }
    }
}