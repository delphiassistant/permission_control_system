using System.Web;
using System.Web.Mvc;
using IdentityCustomized.Classes;

namespace IdentityCustomized
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TitleAndIconFilter());
            filters.Add(new PermissionControlActionFilter());
            
        }
    }
}
