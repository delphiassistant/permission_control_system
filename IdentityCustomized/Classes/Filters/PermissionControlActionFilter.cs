using IdentityCustomized.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdentityCustomized.Classes
{
    public class PermissionControlActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool controllerMarkedToIgnorePermissionCheck =
                filterContext.ActionDescriptor.ControllerDescriptor
                    .GetCustomAttributes(typeof(IgnorePermissionCheckAttribute), false).Any();

            bool actionMarkedToIgnorePermissionCheck =
                filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnorePermissionCheckAttribute), false).Any();

            if (controllerMarkedToIgnorePermissionCheck)
            {
                return;  // Do nothing
            }

            if (actionMarkedToIgnorePermissionCheck)
            {
                return;  // Do nothing
            }


            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                List<PermissionListItem> allPermissions = PermissionsHelper.GetAllPermissions();
                string userID = HttpContext.Current.User.Identity.GetUserId();
                

                // If User Is SuperAdmin, Do Nothing!
                var user = db.Users.FirstOrDefault(u => u.Id == userID);
                if (user.IsSuperAdmin)
                {
                    return; // Do nothing
                }

                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string controllerNamespace = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace;
                string requestType = filterContext.HttpContext.Request.RequestType;

                // Now Check If Permission Requires Authorization And User Has This Permission                
                string currentActionFullName = controllerName + actionName;
                var requestedAction = allPermissions.FirstOrDefault(pr => pr.ActionFullName == currentActionFullName);
                if (
                    requestedAction != null
                    && (requestedAction.PermissiongRequiresAuthorization || requestedAction.PermissiongGroupRequiresAuthorization)
                    && !requestedAction.PermissionAllowAnonymous)
                {
                    List<PermissionListItem> userPermissionItems = PermissionsHelper.GetUserPermissions();

                    if (!userPermissionItems.Any(upr =>
                        upr.ActionFullName == currentActionFullName
                        && upr.PermissiongGroupName == controllerName
                        && upr.PermissiongGroupNamespace == controllerNamespace))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
                        filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                    }
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Log("OnActionExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //Log("OnResultExecuting", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // Log("OnResultExecuted", filterContext.RouteData);
        }
    }
}