using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tax_Tech.Controllers
{
    public class CustomToolbarController : BaseController
    {   
        public ActionResult ShowFilterForm(int formId)
        {
            ViewBag.formId = formId;
            return PartialView("~/Areas/Configuration/Views/Shared/CustomToolbar/_CustomToolbarCustomFilter.cshtml");
        }
    }

}