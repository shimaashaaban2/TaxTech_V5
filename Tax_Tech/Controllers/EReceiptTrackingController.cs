using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.TaxUpdate;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Repository;
using static iTextSharp.text.pdf.PRTokeniser;

namespace Tax_Tech.Controllers
{
    public class EReceiptTrackingController : BaseController
    {
        private readonly EReceiptJobTrackingRepository _eReceiptJobTracking;

        public static string Logtype { get; set; }
        public EReceiptTrackingController()
        {
            _eReceiptJobTracking = new EReceiptJobTrackingRepository();
        }
        // GET: EReceiptTracking
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EreceiptJobsList()
        {
            try
            {
                IEnumerable<EreceiptJobTrackingModel> EReceiptTracking = _eReceiptJobTracking.GetEReceiptJobList();
                return PartialView("EReceiptTracking/_EReceiptTrackingList", EReceiptTracking);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }

        }
        [HttpGet]
        public ActionResult JobTrackingByJobID(int jobID)
        {
            try
            {
                var listJobs = _eReceiptJobTracking.GetEReceiptJobByID(jobID);

                return PartialView("EReceiptTracking/_EReceiptTrackingList", listJobs);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }

        }
        [HttpGet]
        public ActionResult JobTrackingByJobDate(string startDate, string endDate)
        {

            try
            {
                IEnumerable<EreceiptJobTrackingModel> EReceiptTrackingDate = _eReceiptJobTracking.GetEReceiptJobByDate(startDate, endDate);

                return PartialView("EReceiptTracking/_EReceiptTrackingList", EReceiptTrackingDate);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        [HttpGet]
        public ActionResult EReceiptFailedJobLogList(int jobId, string logType, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                if (!string.IsNullOrEmpty(logType) && logType != "undefined")
                {
                    Logtype = logType;
                }

                ViewBag.CurrentPage = pageNumber;

                EReceiptJobLogsModel result =_eReceiptJobTracking.GetEetEReceiptLogsByJobID(jobId, logType, pageNumber, pageSize);
                result.logtype = logType;
               
                ViewBag.logType = Logtype;
                // Return partial view for AJAX rendering
                //return View("JobTracking/_FailedJobList", result);
                return PartialView("EReceiptTracking/_EReceiptJobLogsList", result);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        [HttpGet]
        public ActionResult ExportEReceiptJobListToExcel()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "JobTracking" + StringDate + ".xlsx";
                XLWorkbook wb = null;
                IEnumerable<EreceiptJobTrackingModel> EReceiptJobTrackingList = _eReceiptJobTracking.GetEReceiptJobList();
                wb = ExportHelper.ExportEReceiptJobListToExcel(EReceiptJobTrackingList, $"JobTrackingList-{DateTime.Now}");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                wb.Dispose();
                return Content("", "application/ms-excel");
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("error", "Public");
            }

        }
        public ActionResult ExportJobByIdToExcel(int jobID)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "JobTracking" + StringDate + ".xlsx";
                XLWorkbook wb = null;

                IEnumerable<EreceiptJobTrackingModel> JobIDList = _eReceiptJobTracking.GetEReceiptJobByID(jobID);

               // var listJobs = new List<JobTrackingModel>();
               // listJobs.Add(JobID);
                wb = ExportHelper.ExportEReceiptJobIdToExcel(JobIDList, $"JobTrackingList-{DateTime.Now}");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                wb.Dispose();
                return Content("", "application/ms-excel");
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("error", "Public");
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