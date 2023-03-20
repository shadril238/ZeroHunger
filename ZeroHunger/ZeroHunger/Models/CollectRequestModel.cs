using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger.Models
{
    public class CollectRequestModel
    {
        [Required]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string FoodName { get; set; }
        [Required]
        public int FoodQuantity { get; set; }
        [Required, StringLength(50)]
        public string ResturantName { get; set; }
        [Required, StringLength(100)]
        public string ResturantAddress { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required, StringLength(11)]
        public string Contact { get; set; }
        [Required, StringLength(10)]
        public string Status { get; set; }
    }
}