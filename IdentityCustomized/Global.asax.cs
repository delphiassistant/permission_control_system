using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IdentityCustomized.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityCustomized
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {            
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, CodeFirstConfiguration>());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var persianCulture = new PersianCulture
            {
                DateTimeFormat =
                {
                    ShortDatePattern = "yyyy/MM/dd",
                    LongDatePattern = "dddd d MMMM yyyy",
                    AMDesignator = "صبح",
                    PMDesignator = "عصر"
                }
            };
            Thread.CurrentThread.CurrentCulture = persianCulture;
            Thread.CurrentThread.CurrentUICulture = persianCulture;
        }
    }

    

    public class CodeFirstConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public CodeFirstConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //Debug.WriteLine("Seed method being invoked");
            #region Add User with SuperAdmin rights.
            if (!context.Users.Any(u => u.UserName == "Administrator"))
            {
                //Debug.WriteLine("Seed method being invoked");
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "Administrator",
                    IsSuperAdmin = true,
                    NationalCode = "000",
                    Fullname = "مدیر کل"
                };

                var result = manager.Create(user, "AdminPassword");
            }
            #endregion
        }

    }
}
