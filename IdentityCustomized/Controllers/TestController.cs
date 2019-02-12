using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityCustomized.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Title("تست ها")]    
    public class TestController : Controller
    {
        // GET: Test
        [Title("مدیریت تست ها")]
        [Icon("fas fa-clipboard-list")]
        public ActionResult Index()
        {
            return View();
        }
    }
}