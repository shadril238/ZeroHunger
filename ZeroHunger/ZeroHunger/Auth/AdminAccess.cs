﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;

namespace ZeroHunger.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminAccess:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var flag=base.AuthorizeCore(httpContext);
            if (flag)
            {
                ZeroHungerContext db = new ZeroHungerContext();
                //email based
                string role = (from e in db.Employees where e.Email.Equals(httpContext.User.Identity.Name) select e.Role).SingleOrDefault();
                if(role=="admin") return true;
            }
            return false;
        }
    }
}