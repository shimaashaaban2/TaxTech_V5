using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class ConfigPublicController : Tax_Tech.Controllers.BaseController
    {
        public ActionResult error()
        {
            return View();
        }

        public ActionResult forbidden()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}