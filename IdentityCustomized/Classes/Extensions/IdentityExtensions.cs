using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using IdentityCustomized.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc.Html;
using System.Web.Routing;
using IdentityCustomized;

namespace System.Web.Mvc
{
    public static class MyExtensionMethods
    {
        public static string GetUserFullName(this IIdentity identity)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userid = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == userid);
            return user.Fullname;
        }

    }
}