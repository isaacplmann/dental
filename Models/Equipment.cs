using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSUDental.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }
    }
}