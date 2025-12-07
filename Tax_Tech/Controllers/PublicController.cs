using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tax_Tech.Controllers
{
    public class PublicController : BaseController
    {
        public ActionResult error()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}