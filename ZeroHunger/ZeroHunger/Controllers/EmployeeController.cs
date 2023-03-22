//shadril238
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.Auth;
using ZeroHunger.EF;
using ZeroHunger.EF.Models;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    [EmployeeAccess]
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult CollectFood()
        {
            int empId = (int)Session["EmpId"];
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from a in db.AssignedRequests
                          join e in db.Employees on a.EmployeeId equals e.Id
                          join c in db.CollectRequests on a.CollectRequestId equals c.Id
                          join f in db.FoodItems on c.Id equals f.CollectRequestId
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where c.Status.Equals("Processing") && e.Id==empId
                          select new
                          {
                              CollectReqId = c.Id,
                              FoodName = f.Name,
                              FoodQuantity = f.Quantity,
                              ResturantName = r.Name,
                              ResturantAddress = r.Address,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              ResturantContact = r.Contact,
                              Status = c.Status,
                          };
            var assignedRequests = new List<AssignedRequestModel>();
            foreach (var item in reqList)
            {
                AssignedRequestModel assignedRequest = new AssignedRequestModel
                {
                    CollectReqId= item.CollectReqId,
                    ResturantName = item.ResturantName,
                    ResturantAddress = item.ResturantAddress,
                    ResturantContact = item.ResturantContact,
                    FoodName = item.FoodName,
                    FoodQuantity = item.FoodQuantity,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = item.Status
                };
                assignedRequests.Add(assignedRequest);
            }
            return View(assignedRequests);
        }
        public ActionResult CollectFoodAction(int id)
        {
            ZeroHungerContext db = new ZeroHungerContext();
            var collReq=(from c in db.CollectRequests 
                        where c.Id.Equals(id) select c).SingleOrDefault();
            var req = collReq;
            collReq.Status = "Collected";
            db.Entry(req).CurrentValues.SetValues(collReq);
            db.SaveChanges();
            TempData["Msg"] = "Success!";
            return RedirectToAction("CollectFood");
        }

        public ActionResult DistributeFood()
        {
            int empId = (int)Session["EmpId"];
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from a in db.AssignedRequests
                          join e in db.Employees on a.EmployeeId equals e.Id
                          join c in db.CollectRequests on a.CollectRequestId equals c.Id
                          join f in db.FoodItems on c.Id equals f.CollectRequestId
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where c.Status.Equals("Collected") && e.Id==empId
                          select new
                          {
                              CollectReqId = c.Id,
                              FoodName = f.Name,
                              FoodQuantity = f.Quantity,
                              ResturantName = r.Name,
                              ResturantAddress = r.Address,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              ResturantContact = r.Contact,
                              Status = c.Status,
                          };
            var assignedRequests = new List<AssignedRequestModel>();
            foreach (var item in reqList)
            {
                AssignedRequestModel assignedRequest = new AssignedRequestModel
                {
                    CollectReqId = item.CollectReqId,
                    ResturantName = item.ResturantName,
                    ResturantAddress = item.ResturantAddress,
                    ResturantContact = item.ResturantContact,
                    FoodName = item.FoodName,
                    FoodQuantity = item.FoodQuantity,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = item.Status
                };
                assignedRequests.Add(assignedRequest);
            }
            return View(assignedRequests);
        }

        public ActionResult DistributeFoodAction(int id)
        {
            ZeroHungerContext db = new ZeroHungerContext();
            var collReq = (from c in db.CollectRequests
                           where c.Id.Equals(id)
                           select c).SingleOrDefault();
            var req = collReq;
            collReq.Status = "Completed";
            db.Entry(req).CurrentValues.SetValues(collReq);
            db.SaveChanges();
            TempData["Msg"] = "Success!";
            return RedirectToAction("DistributeFood");
        }

        public ActionResult DistributionHistory()
        {
            int empId = (int)Session["EmpId"];
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from a in db.AssignedRequests
                          join e in db.Employees on a.EmployeeId equals e.Id
                          join c in db.CollectRequests on a.CollectRequestId equals c.Id
                          join f in db.FoodItems on c.Id equals f.CollectRequestId
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where c.Status.Equals("Completed") && e.Id==empId
                          select new
                          {
                              CollectReqId = c.Id,
                              FoodName = f.Name,
                              FoodQuantity = f.Quantity,
                              ResturantName = r.Name,
                              ResturantAddress = r.Address,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              ResturantContact = r.Contact,
                              Status = c.Status,
                          };
            var compRequests = new List<AssignedRequestModel>();
            foreach (var item in reqList)
            {
                AssignedRequestModel compRequest = new AssignedRequestModel
                {
                    CollectReqId = item.CollectReqId,
                    ResturantName = item.ResturantName,
                    ResturantAddress = item.ResturantAddress,
                    ResturantContact = item.ResturantContact,
                    FoodName = item.FoodName,
                    FoodQuantity = item.FoodQuantity,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = item.Status
                };
                compRequests.Add(compRequest);
            }
            return View(compRequests);
        }

    }
}