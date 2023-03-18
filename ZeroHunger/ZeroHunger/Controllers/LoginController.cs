using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                    if (emp.Role == "admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (emp.Role == "employee")
                    {
                        return RedirectToAction("Dashboard", "Employee");
                    }
                }
                TempData["Msg"] = "Invalid username or password.";
            }
            return View();
        }
    }
}