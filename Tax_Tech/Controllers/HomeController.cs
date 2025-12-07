using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ReportApiRepository _reportApiRepository;
        private readonly JobQueueApiRepository _jobQueueApiRepository;

        public HomeController()
        {
            _reportApiRepository = new ReportApiRepository();
            _jobQueueApiRepository = new JobQueueApiRepository();
        }

       [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetRejectedTotals()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);

                var rejectedTotals = _reportApiRepository.GetRejectedDocumentReport(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), entityId);

                return PartialView("Reports/_ReportsRejectedDocs", rejectedTotals);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetInvoiceTotals()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);

                var invoiceTotals = _reportApiRepository.GetDocumentValues(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), PublicConfig.InvoiceTypeID, entityId);

                return PartialView("Reports/_ReportsDocumentsValues", invoiceTotals);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetDebitTotals()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);

                var debitTotals = _reportApiRepository.GetDocumentValues(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), PublicConfig.DebitNoteTypeID, entityId);

                return PartialView("Reports/_ReportsDocumentsValues", debitTotals);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetCreditTotals()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);

                var creditTotals = _reportApiRepository.GetDocumentValues(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), PublicConfig.CreditNoteTypeID, entityId);

                return PartialView("Reports/_ReportsDocumentsValues", creditTotals);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult GetStatusStatistics()
        {
            try
            {
                var result = _jobQueueApiRepository.GetStatusStatistics().FirstOrDefault();
                return Json(new { msg = "success", CountOfRunning = result?.CountOfRunning ?? 0, CountOfClosed = result?.CountOfClosed ?? 0 });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetImportStatistics()
        {
            try
            {
                
                var runningJobs = _jobQueueApiRepository.GetRunningList().ToList();

                return PartialView("Home/_RunningJobsReportCard", runningJobs);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }


        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetCloseJop(long itemId)
        {
            try
            {
                var ResultModel = _reportApiRepository.GetCloseJob(itemId);
                if (ResultModel != null)
                {
                    string Result = "";
                    if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        if (Result == "1")
                        {
                            var jsonResult = Json(new
                            {
                                redirecturl = Url.Action("Index", "Home", new { area = "" })
                            });
                            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                            return jsonResult;
                        }
                        else
                            ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                    }
                }
                else
                    ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;

                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });



            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
        }
        //Job Invoice
        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetJopTotalPopup(int? itemId)
        {
            try
            {
                return PartialView("~/Areas/Configuration/Views/Shared/Items/_JobTotalLayout.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        #region Methods
        private string RenderRazorViewToString(string viewName, object model = null)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        #endregion
    }
}