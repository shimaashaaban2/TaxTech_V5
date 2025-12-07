using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Filters;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class LogsController : Tax_Tech.Controllers.BaseController
    {

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult MOF()
        {
            return View();
        }

        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult getMOFByDate(DateTime from, DateTime to)
        {
            try
            {
                long EntityID = Convert.ToInt64(Session["EntityID"]);
                return PartialView("~/Areas/Configuration/Views/Shared/Log/_MOFByDateList.cshtml", LogRepository.Get().GetMOFLogByDate(EntityID, from, to).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }

        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult getMOFLogByInternalID(string InternalID)
        {
            try
            {
                long EntityID = Convert.ToInt64(Session["EntityID"]);
                return PartialView("~/Areas/Configuration/Views/Shared/Log/_MOFByDateList.cshtml", LogRepository.Get().GetMOFLogByInternalID(InternalID).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }
    }
}