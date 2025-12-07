using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.Filters;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class GlobalConfigController : Tax_Tech.Controllers.BaseController
    {
        public GlobalConfigController()
        {

        }

        #region Pages
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                string ErrorMsg = "";
                if (ex.InnerException != null)
                {
                    ErrorMsg = Convert.ToString(ex.InnerException.Message);
                    //ErrorLog.GetErrorLog().Insert("Balance", "Request Account Statement", ex.InnerException, ex.InnerException.Message);
                }
                else
                {
                    ErrorMsg = Convert.ToString(ex.Message);
                    //ErrorLog.GetErrorLog().Insert("Balance", "Request Account Statement", ex, ex.Message);
                }
                TempData["ErrorMsg"] = "ERROR: " + ErrorMsg;
                return RedirectToAction("error", "ConfigPublic");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult List()
        {
            try
            {
                // TODO: get list of Global Configurations
                return PartialView("~/Areas/Configuration/Views/Shared/GlobalConfig/_GlobalConfigList.cshtml");
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
        #endregion
    }
}
