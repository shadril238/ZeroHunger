using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.Models
{
    public class AssignedRequestModel
    {
        public int CollectReqId { get; set; }
        public string ResturantName { get; set; }
        public string ResturantAddress { get;set; }
        public string ResturantContact { get; set; }
        public string FoodName { get; set; }
        public int FoodQuantity { get; set;}
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string DeliveredBy { get; set; }
        public string DeliveredByContact { get;set; }
    }
}