using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IdentityCustomized.Models;
using Microsoft.AspNet.Identity;

namespace IdentityCustomized.Controllers
{
    [IgnorePermissionCheck]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[Authorize]        
        public ActionResult Map()
        {
            /*
             * var manager = new IdentityManager();
            ApplicationUser user = manager.GetUser(HttpContext.User.Identity.Name);
            manager.AddUserToRole(user.Id, "Test1");
            */
            return View();

        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        [ChildActionOnly]
        // When we marked this action using [ChildActionOnly] attribute, it means this action can not be invoked as:
        // Get: /Home/GetAdminMenus directrly from browser
        public ActionResult GetAdminMenus()
        {
            List<Permission> userPermissions = new List<Permission>();

            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            ApplicationDbContext db = new ApplicationDbContext();

            string userID = User.Identity.GetUserId();
            IdentityManager manager = new IdentityManager();
            ApplicationUser user = manager.GetUserByID(userID);

            // SuperAdmin users may see all admin menus:
            if (user.IsSuperAdmin)
            {
                userPermissions = db.Permissions.Include("PermissionGroup")
                    .Where(pr => pr.PermissionTitle == "Index").ToList();
                
                return PartialView("_AdminMenus", userPermissions);
            }

            // When user has not SuperAdmin flag, select his/her permissions based on RolePermissions
            var userRoles = manager.GetUserRoles(userID);
            
            userPermissions = db.RolePermissions.Include("Permissions")
                .Where(rp => userRoles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission).Include("PermissionGroup")
                .Where(pr => pr.PermissionTitle == "Index")
                .ToList();
            return PartialView("_AdminMenus", userPermissions);
        }
    }
}