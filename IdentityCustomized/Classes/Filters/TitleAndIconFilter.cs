using System;
using System.Linq;
using System.Web.Mvc;

namespace IdentityCustomized.Classes
{
    public class TitleAndIconFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var titleAtttribute =
                filterContext.ActionDescriptor.GetCustomAttributes(typeof(TitleAttribute), false).FirstOrDefault();

            var iconAtttribute =
                filterContext.ActionDescriptor.GetCustomAttributes(typeof(IconAttribute), false).FirstOrDefault();

            var viewBag = filterContext.Controller.ViewBag;

            if (titleAtttribute != null)
            {
                viewBag.Title = (titleAtttribute as TitleAttribute).DisplayTitle;
            }

            if(iconAtttribute != null)
            {
                viewBag.Icon = (iconAtttribute as IconAttribute).DisplayIcon;
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