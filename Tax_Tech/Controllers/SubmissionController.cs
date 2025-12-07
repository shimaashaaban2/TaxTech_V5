using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Filters;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    public class SubmissionController : BaseController
    {
        private readonly SubmissionRepository _SubmissionApiRepository;
        public SubmissionController()
        {
            _SubmissionApiRepository = new SubmissionRepository();
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

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult getSubmissionLogByDate(DateTime from, DateTime to,byte Status)
        {
            try
            { 
                long EntityID = Convert.ToInt64(Session["EntityID"]);
                return PartialView("Submission/_SubmissionLogList", SubmissionRepository.Get().GetSubmissionLogByDate(EntityID, from, to, Status).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult getSubmissionLogByInternalID(string InternalID)
        {
            try
            {
                long EntityID = Convert.ToInt64(Session["EntityID"]);
                return PartialView("Submission/_SubmissionLogList", SubmissionRepository.Get().GetSubmissionLogByInternalID(InternalID).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
    }
}