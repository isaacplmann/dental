using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSUDental.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime? DateReceived { get; set; }
        public double AmountDue { get; set; }
        public String PaymentType { get; set; }
        public String PlacedBy { get; set; }
        public String CheckNumber { get; set; }
        public String TakenBy { get; set; }
        public String Lot { get; set; }
        public String OrderType { get; set; }
        public int Units { get; set; }
    }
}