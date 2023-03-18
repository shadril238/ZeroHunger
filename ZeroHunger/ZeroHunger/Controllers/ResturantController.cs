using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;
using ZeroHunger.EF.Models;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
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
        public ActionResult CreateCollectRequest(CollectRequestModel collectRequest)
        {
            ZeroHungerContext db= new ZeroHungerContext();
            CollectRequest newCollectRequest= new CollectRequest();
            newCollectRequest.StartTime = collectRequest.StartTime;
            newCollectRequest.EndTime = collectRequest.EndTime;
            newCollectRequest.Status= collectRequest.Status;
            newCollectRequest.ResturantId = 1;
            db.CollectRequests.Add(newCollectRequest);
            db.SaveChanges();

            FoodItem foodItem= new FoodItem();
            foodItem.Name = collectRequest.FoodName;
            foodItem.Quantity = collectRequest.FoodQuantity;
            foodItem.CollectRequestId = db.CollectRequests.OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault();
            db.FoodItems.Add(foodItem);
            db.SaveChanges();

            return RedirectToAction("CollectRequestList", "Admin");

        }
    }
}