using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.Auth;
using ZeroHunger.EF;
using ZeroHunger.EF.Models;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    //[ResturantAccess]
    public class ResturantController : Controller
    {
        // GET: Resturant
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateCollectRequest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCollectRequest(String FoodName, int FoodQuantity, string ResturantName, string ResturantAddress, int Hours, int Minutes, string Contact, string Status)
        {
            ZeroHungerContext db= new ZeroHungerContext();
            CollectRequest newCollectRequest= new CollectRequest();
            newCollectRequest.StartTime = DateTime.Now;
            newCollectRequest.EndTime = newCollectRequest.StartTime.AddHours(Hours).AddMinutes(Minutes);
            newCollectRequest.Status= Status;
            newCollectRequest.ResturantId = (int)Session["ResturantId"];
            db.CollectRequests.Add(newCollectRequest);
            db.SaveChanges();

            FoodItem foodItem= new FoodItem();
            foodItem.Name = FoodName;
            foodItem.Quantity = FoodQuantity;
            foodItem.CollectRequestId = db.CollectRequests.OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault();
            db.FoodItems.Add(foodItem);
            db.SaveChanges();

            return RedirectToAction("CollectRequestList", "Admin");
        }

        public ActionResult CollectRequestList()
        {
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from f in db.FoodItems
                          join c in db.CollectRequests on f.CollectRequestId equals c.Id
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where r.Id == 1
                          select new
                          {
                              FoodName = f.Name,
                              FoodQuantity = f.Quantity,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              Status = c.Status
                          };
            var collectRequests = new List<CollectRequestModel>();
            foreach (var item in reqList)
            {
                CollectRequestModel collectRequest = new CollectRequestModel();
                collectRequest.FoodName = item.FoodName;
                collectRequest.FoodQuantity = item.FoodQuantity;
                collectRequest.StartTime = item.StartTime;
                collectRequest.EndTime = item.EndTime;
                collectRequest.Status = item.Status;
                collectRequests.Add(collectRequest);
            }
            return View(collectRequests);  
        }
    }
}