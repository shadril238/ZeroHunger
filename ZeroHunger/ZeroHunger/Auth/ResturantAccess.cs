using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;

namespace ZeroHunger.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ResturantAccess : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var flag = base.AuthorizeCore(httpContext);
            if (flag)
            {
                ZeroHungerContext db = new ZeroHungerContext();
                //email based
                string role = (from r in db.Resturants where r.Email.Equals(httpContext.User.Identity.Name) select "resturant").SingleOrDefault();
                if (role == "resturant") return true;
            }
            return false;
        }
    }
}