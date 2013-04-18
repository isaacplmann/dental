using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSUDental.Models
{
    public class Result
    {
        public int Id { get; set; }
        public DateTime TestDate { get; set; }
        public DateTime EnterDate { get; set; }
        public Boolean TestResult { get; set; }
        public String EquipId { get; set; }
        public String Reference { get; set; }
    }
}