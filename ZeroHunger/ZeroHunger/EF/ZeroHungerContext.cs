using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ZeroHunger.EF.Models;

namespace ZeroHunger.EF
{
    public class ZeroHungerContext:DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Resturant> Resturants { get; set; }
        public DbSet<CollectRequest> CollectRequests { get; set; }
        public DbSet<AssignedRequest> AssignedRequests { get; set; }
    }
}