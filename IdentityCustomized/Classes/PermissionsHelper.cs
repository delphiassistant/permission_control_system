using IdentityCustomized.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityCustomized
{
    public class PermissionsHelper
    {
        public static List<PermissionListItem> GetUserPermissions()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            string userID = HttpContext.Current.User.Identity.GetUserId();
            var Session = HttpContext.Current.Session;
            var Application = HttpContext.Current.Application;
            IdentityManager manager = new IdentityManager();
            List<PermissionListItem> userPermissionItems = new List<PermissionListItem>();
            List<string> AffectedUsers = new List<string>();

            if (Application["AffectedUsers"] != null)
            {
                AffectedUsers = Application["AffectedUsers"] as List<string>;
            }

            if (Session["Permissions"] == null || (AffectedUsers.Any() && AffectedUsers.Contains(userID)))
            {               

                // Get All User Role's
                var roles = manager.GetUserRoles(userID);

                // Get RoleID's of User Roles
                var roleIDs = db.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Id).ToList();

                // Get PermissionID's of those roles
                var permissionIDs =
                    db.RolePermissions.Where(rp => roleIDs.Contains(rp.RoleID))
                        .Select(rp => rp.PermissionID)
                        .Distinct()
                        .ToList();

                // Create A list of PermissionItems based of PermissionIDs
                userPermissionItems =
                    db.Permissions.Include("PermissionGroup").Where(pr => permissionIDs.Contains(pr.PermissionID))
                        .Select(pr => new PermissionListItem()
                        {
                            PersmissionName = pr.PermissionTitle,
                            PermissiongRequiresAuthorization = pr.RequiresAuthorization,
                            PermissiongGroupName = pr.PermissionGroup.PermissionGroupTitle,
                            PermissiongGroupNamespace = pr.PermissionGroup.PermissionGroupNamespace,
                            PermissiongGroupRequiresAuthorization = pr.PermissionGroup.RequiresAuthorization,
                            PermissionAllowAnonymous = pr.AllowAnonymous,
                            ActionFullName = pr.PermissionGroup.PermissionGroupTitle + pr.PermissionTitle
                        }).ToList();

                // Put PermissionItems Into Session
                Session["Permissions"] = userPermissionItems;

                // Remove UserID from AffectedUsers since user's permissions is reloaded
                if ((AffectedUsers.Any() && AffectedUsers.Contains(userID)))
                {
                    AffectedUsers.Remove(userID);
                    Application["AffectedUsers"] = AffectedUsers;
                }
            }
            else
            {
                // Extract PermissionItems From Session
                userPermissionItems = Session["Permissions"] as List<PermissionListItem>;
            }


            return userPermissionItems;
        }

        public static List<PermissionListItem> GetAllPermissions()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var Application = HttpContext.Current.Application;
            if (Application["AllPermissions"] == null)
            {
                List<PermissionListItem> allPermissions = db.Permissions.Include("PermissionGroup")
                    .Select(pr => new PermissionListItem()
                    {
                        PersmissionName = pr.PermissionTitle,
                        PermissiongRequiresAuthorization = pr.RequiresAuthorization,
                        PermissiongGroupName = pr.PermissionGroup.PermissionGroupTitle,
                        PermissiongGroupNamespace = pr.PermissionGroup.PermissionGroupNamespace,
                        PermissiongGroupRequiresAuthorization = pr.PermissionGroup.RequiresAuthorization,
                        PermissionAllowAnonymous = pr.AllowAnonymous,
                        ActionFullName =   pr.PermissionGroup.PermissionGroupTitle + pr.PermissionTitle
                            
                        }).ToList();
                Application["AllPermissions"] = allPermissions;
                return allPermissions;
            }
            else
            {
                List<PermissionListItem> allPermissions = Application["AllPermissions"] as List<PermissionListItem>;
                return allPermissions;
            }
        }

    }
}