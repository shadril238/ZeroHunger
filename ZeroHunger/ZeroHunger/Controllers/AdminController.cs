using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CollectRequestList()
        {
            ZeroHungerContext db=new ZeroHungerContext();
            var reqList=from f in db.FoodItems
                        join c in db.CollectRequests on f.CollectRequestId equals c.Id
                        join r in db.Resturants on c.ResturantId equals r.Id
                        select new {
                            FoodName= f.Name,
                            FoodQuantity=f.Quantity,
                            ResturantName=r.Name,
                            ResturantAddress=r.Address,
                            StartTime=c.StartTime,
                            EndTime=c.EndTime,
                            Contact=r.Contact,
                            Status=c.Status
                        };
            var collectRequests=new List<CollectRequestModel>();
            foreach(var item in reqList)
            {
                CollectRequestModel collectRequest=new CollectRequestModel();
                collectRequest.FoodName= item.FoodName;
                collectRequest.FoodQuantity= item.FoodQuantity;
                collectRequest.ResturantName= item.ResturantName;
                collectRequest.ResturantAddress= item.ResturantAddress;
                collectRequest.StartTime= item.StartTime;
                collectRequest.EndTime= item.EndTime;
                collectRequest.Contact= item.Contact;
                collectRequest.Status= item.Status;
                collectRequests.Add(collectRequest);
            }
            return View(collectRequests);

        }
    }
}