//shadril238
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.Auth;
using ZeroHunger.EF;
using ZeroHunger.EF.Models;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    [AdminAccess]
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult CollectRequestList()
        {
            ZeroHungerContext db=new ZeroHungerContext();
            //Collect Request List (details)
            var reqList=from f in db.FoodItems
                        join c in db.CollectRequests on f.CollectRequestId equals c.Id
                        join r in db.Resturants on c.ResturantId equals r.Id
                        where c.Status.Equals("Open")
                        select new {
                            Id=c.Id,
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
                collectRequest.Id =item.Id;
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
            //Select Employees Name
            var emp = (from e in db.Employees
                       where e.Role.Equals("employee")
                       select new
                       {
                           Id = e.Id,
                           Name = e.Name
                       }).ToList();
            ViewBag.Employees = new SelectList(emp, "Id", "Name");
            return View(collectRequests);
        }
        [HttpPost]
        public ActionResult CollectRequestList(AssignedRequest model)
        {
            ZeroHungerContext db = new ZeroHungerContext();

            db.AssignedRequests.Add(model);
            db.SaveChanges();
            var collReq=(from c in db.CollectRequests
                         where c.Id == model.CollectRequestId && c.Status.Equals("Open")
                         select c).SingleOrDefault();
            var exst = collReq;
            collReq.Status = "Processing";
            db.Entry(exst).CurrentValues.SetValues(collReq);
            db.SaveChanges();
            return RedirectToAction("CollectRequestList");
        }
        
        public ActionResult AssignedRequestList()
        {
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from a in db.AssignedRequests
                          join e in db.Employees on a.EmployeeId equals e.Id
                          join c in db.CollectRequests on a.CollectRequestId equals c.Id
                          join f in db.FoodItems on c.Id equals f.CollectRequestId
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where (c.Status.Equals("Processing") || c.Status.Equals("Collected"))
                          select new
                          {
                              FoodName = f.Name,
                              FoodQuantity = f.Quantity,
                              ResturantName = r.Name,
                              ResturantAddress = r.Address,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              ResturantContact = r.Contact,
                              Status = c.Status,
                              EmpName=e.Name,
                              EmpContact=e.Contact
                          };
            var assignedRequests = new List<AssignedRequestModel>();
            foreach (var item in reqList)
            {
                AssignedRequestModel assignedRequest = new AssignedRequestModel
                {
                    ResturantName = item.ResturantName,
                    ResturantAddress = item.ResturantAddress,
                    ResturantContact = item.ResturantContact,
                    FoodName = item.FoodName,
                    FoodQuantity = item.FoodQuantity,
                    DeliveredBy=item.EmpName,
                    DeliveredByContact=item.EmpContact,
                    StartTime=item.StartTime,
                    EndTime=item.EndTime,
                    Status= item.Status
                };
                assignedRequests.Add(assignedRequest);
            }
            return View(assignedRequests);
        }

        public ActionResult CompleteRequestList()
        {
            ZeroHungerContext db = new ZeroHungerContext();
            var reqList = from a in db.AssignedRequests
                          join e in db.Employees on a.EmployeeId equals e.Id
                          join c in db.CollectRequests on a.CollectRequestId equals c.Id
                          join f in db.FoodItems on c.Id equals f.CollectRequestId
                          join r in db.Resturants on c.ResturantId equals r.Id
                          where c.Status.Equals("Completed")
                          select new
                          {
                              FoodName = f.Name,
                              FoodQuantity = f.Quantity,
                              ResturantName = r.Name,
                              ResturantAddress = r.Address,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              ResturantContact = r.Contact,
                              Status = c.Status,
                              EmpName = e.Name,
                              EmpContact = e.Contact
                          };
            var completedRequests = new List<AssignedRequestModel>();
            foreach (var item in reqList)
            {
                AssignedRequestModel completedRequest = new AssignedRequestModel
                {
                    ResturantName = item.ResturantName,
                    ResturantAddress = item.ResturantAddress,
                    ResturantContact = item.ResturantContact,
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

        public ActionResult EmployeeList()
        {
            ZeroHungerContext db = new ZeroHungerContext();
            var emp=(from e in db.Employees
                     where e.Role.Equals("employee")
                     select e).ToList();
            return View(emp);

        }
        [HttpGet]
        public ActionResult CreateEmployee() 
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateEmployee(Employee emp)
        {
            ZeroHungerContext db=new ZeroHungerContext();
            emp.Role = "employee";
            db.Employees.Add(emp);
            db.SaveChanges();
            TempData["Msg"] = "Success";
            return RedirectToAction("EmployeeList");
        }
    }
}