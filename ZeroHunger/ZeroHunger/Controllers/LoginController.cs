﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZeroHunger.EF;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoginEmp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginEmp(LoginModel login)
        {
            if(ModelState.IsValid)
            {
                ZeroHungerContext db = new ZeroHungerContext();
                var emp = (from e in db.Employees where e.Email.Equals(login.Email) && e.Password.Equals(login.Password) select e).SingleOrDefault();
                if (emp != null)
                {
                    FormsAuthentication.SetAuthCookie(emp.Email, false);
                    if (emp.Role == "admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (emp.Role == "employee")
                    {
                        return RedirectToAction("Dashboard", "Employee");
                    }
                }
                TempData["Msg"] = "Invalid email or password.";
            }
            return View();
        }
        [HttpGet]
        public ActionResult LoginResturant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginResturant(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                ZeroHungerContext db = new ZeroHungerContext();
                var res = (from r in db.Resturants where r.Email.Equals(login.Email) && r.Password.Equals(login.Password) select r).SingleOrDefault();
                if (res != null)
                {
                    FormsAuthentication.SetAuthCookie(res.Email, false);
                    Session["ResturantId"] = res.Id;
                    Session["ResturantName"] = res.Name;
                    Session["ResturantAddress"] = res.Address;
                    Session["ResturantEmail"] = res.Email;
                    Session["ResturantContact"] = res.Contact;

                    return RedirectToAction("CreateCollectRequest", "Resturant");
                }
                TempData["Msg"] = "Invalid email or password.";
            }
            return View();
        }
    }
}