using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.Filters;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class DeliveryController : Tax_Tech.Controllers.BaseController
    {
        public DeliveryController()
        {

        }
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

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult CreateDelivery()
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
    }
}
