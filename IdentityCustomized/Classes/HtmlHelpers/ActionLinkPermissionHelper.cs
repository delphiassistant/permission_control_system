using IdentityCustomized;
using IdentityCustomized.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class ActionLinkPermissionHelper
    {
        public static bool CheckActionPermission(string controllerName, string actionName, string requestType, string controllerNamespace = null)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                string userID = HttpContext.Current.User.Identity.GetUserId();


                // If User Is SuperAdmin, Do Nothing!
                var user = db.Users.FirstOrDefault(u => u.Id == userID);
                if (user.IsSuperAdmin)
                {
                    return true;
                }

                List<PermissionListItem> allPermissions = PermissionsHelper.GetAllPermissions();

                // Now Check If Permission Requires Authorization And User Has This Permission
                string currentActionFullName = controllerName + actionName;// + requestType;
                var requestedAction = allPermissions.FirstOrDefault(pr => pr.ActionFullName == currentActionFullName);
                if (requestedAction != null && (requestedAction.PermissiongRequiresAuthorization || requestedAction.PermissiongGroupRequiresAuthorization) && !requestedAction.PermissionAllowAnonymous)
                {
                    List<PermissionListItem> userPermissionItems = PermissionsHelper.GetUserPermissions();

                    if (!userPermissionItems.Any(upr =>
                            upr.ActionFullName == currentActionFullName
                            && upr.PermissiongGroupName == controllerName
                            && (controllerNamespace == null || upr.PermissiongGroupNamespace == controllerNamespace)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            string controllerNamespace = htmlHelper.ViewContext.Controller.GetType().Namespace;
            string controllername = (string)htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllername, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            string controllername = (string)htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            string controllerNamespace = htmlHelper.ViewContext.Controller.GetType().Namespace;
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllername, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            string controllername = (string)htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            string controllerNamespace = htmlHelper.ViewContext.Controller.GetType().Namespace;
            string actionname = (string)htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllername, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            string controllername = (string)htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            string controllerNamespace = htmlHelper.ViewContext.Controller.GetType().Namespace;
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllername, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
        {
            string controllername = (string)htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            string controllerNamespace = htmlHelper.ViewContext.Controller.GetType().Namespace;
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllername, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            string controllerNamespace = htmlHelper.ViewContext.Controller.GetType().Namespace;
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, object routeValues, object htmlAttributes)
        {
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment,
            object routeValues, object htmlAttributes)
        {
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, string protocol, string hostName, string fragment,
            RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        #region Action with Controller Namespace
        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string controllerNamespace)
        {

            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, string controllerNamespace, object routeValues, object htmlAttributes)
        {
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, string controllerNamespace, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string controllerNamespace, string protocol, string hostName, string fragment,
            object routeValues, object htmlAttributes)
        {
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPermission(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, string controllerNamespace, string protocol, string hostName, string fragment,
            RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string requestMethod = (string)htmlHelper.ViewContext.HttpContext.Request.HttpMethod;
            if (!CheckActionPermission(controllerName, actionName, requestMethod, controllerNamespace))
            {
                return null;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }
        #endregion
    }
}