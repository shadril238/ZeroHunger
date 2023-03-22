//shadril238
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
    [ResturantAccess]
    public class ResturantController : Controller
    {
        // GET: Resturant
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

            return RedirectToAction("CollectRequestList", "Resturant");
        }

        public ActionResult CollectRequestList()
        {
            int resId = (int)Session["ResturantId"];
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from f in db.FoodItems
                          join c in db.CollectRequests on f.CollectRequestId equals c.Id
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where r.Id ==  resId && c.Status.Equals("Open")
                          select new
                          {
                              CollReqId=c.Id,
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
                collectRequest.Id = item.CollReqId;
                collectRequest.FoodName = item.FoodName;
                collectRequest.FoodQuantity = item.FoodQuantity;
                collectRequest.StartTime = item.StartTime;
                collectRequest.EndTime = item.EndTime;
                collectRequest.Status = item.Status;
                collectRequests.Add(collectRequest);
            }
            return View(collectRequests);  
        }
        [HttpGet]
        public ActionResult RemoveCollectRequest(int id)
        {
            ZeroHungerContext db = new ZeroHungerContext();
            var collReq=(from c in db.CollectRequests
                         where c.Id == id
                         select c).SingleOrDefault();
            db.CollectRequests.Remove(collReq);
            db.SaveChanges();

            var foodItem=(from f in db.FoodItems
                          join c in db.CollectRequests on f.CollectRequestId equals c.Id
                          where c.Id == id
                          select f).SingleOrDefault();
            db.FoodItems.Remove(foodItem);
            db.SaveChanges();
            return RedirectToAction("CollectRequestList");
        }
        public ActionResult AssignedRequestList()
        {
            int resId = (int)Session["ResturantId"];
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from a in db.AssignedRequests
                          join e in db.Employees on a.EmployeeId equals e.Id
                          join c in db.CollectRequests on a.CollectRequestId equals c.Id
                          join f in db.FoodItems on c.Id equals f.CollectRequestId
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where (c.Status.Equals("Processing") || c.Status.Equals("Collected")) && r.Id==resId
                          select new
                          {
                              FoodName = f.Name,
                              FoodQuantity = f.Quantity,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              Status = c.Status,
                              EmpName = e.Name,
                              EmpContact = e.Contact
                          };
            var assignedRequests = new List<AssignedRequestModel>();
            foreach (var item in reqList)
            {
                AssignedRequestModel assignedRequest = new AssignedRequestModel
                {
                    FoodName = item.FoodName,
                    FoodQuantity = item.FoodQuantity,
                    DeliveredBy = item.EmpName,
                    DeliveredByContact = item.EmpContact,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = item.Status
                };
                assignedRequests.Add(assignedRequest);
            }
            return View(assignedRequests);
        }
        public ActionResult CompleteRequestList()
        {
            int resId = (int)Session["ResturantId"];
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from a in db.AssignedRequests
                          join e in db.Employees on a.EmployeeId equals e.Id
                          join c in db.CollectRequests on a.CollectRequestId equals c.Id
                          join f in db.FoodItems on c.Id equals f.CollectRequestId
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where c.Status.Equals("Completed") && r.Id == resId
                          select new
                          {

                              FoodName = f.Name,
                              FoodQuantity = f.Quantity,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              Status = c.Status,
                              EmpName = e.Name,
                              EmpContact = e.Contact
                          };
            var completedRequests = new List<AssignedRequestModel>();
            foreach (var item in reqList)
            {
                AssignedRequestModel completedRequest = new AssignedRequestModel
                {
                    FoodName = item.FoodName,
                    FoodQuantity = item.FoodQuantity,
                    DeliveredBy = item.EmpName,
                    DeliveredByContact = item.EmpContact,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = item.Status
                };
                completedRequests.Add(completedRequest);
            }
            return View(completedRequests);
        }
    }
}