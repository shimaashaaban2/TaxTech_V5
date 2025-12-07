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

namespace Tax_Tech.Controllers
{
    public class JobTrackingController : BaseController
    {
        private readonly JobTrackingRepository _jobTrackingRepo;

        public static string  Logtype { get; set; }
        public JobTrackingController()
        {
            _jobTrackingRepo = new JobTrackingRepository();
        }
        // GET: JobTracking
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult JobsList()
        {
            try
            {
                IEnumerable<JobTrackingModel> JobTrackingList = _jobTrackingRepo.GetJobList();
                return PartialView("JobTracking/_JobTrackingList", JobTrackingList);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
           
        }
        [HttpGet]
        public ActionResult JobTrackingByJobID(int JobId)
        {
            try
            {
                JobTrackingModel JobID = _jobTrackingRepo.GetJobListByJobId(JobId);

                var listJobs = new List<JobTrackingModel>() {
                
                };
                listJobs.Add(JobID);
                return PartialView("JobTracking/_JobTrackingList", listJobs);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
           
        }
        public ActionResult JobTrackingByJobDate(string From,string To)
        {
           
            try
            {
                List<JobTrackingModel> JobTrackingList = _jobTrackingRepo.GetJobListByDate(From,To);

                return PartialView("JobTracking/_JobTrackingList", JobTrackingList);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        [HttpGet]
        public ActionResult FailedJobLogList(int jobId, string logType, int pageNo = 1, int pageSize = 50)
        {
            try
            {
                if (!string.IsNullOrEmpty(logType) && logType != "undefined")
                {
                    Logtype = logType;
                }

                ViewBag.CurrentPage = pageNo;

                JobLogPagedResult result = _jobTrackingRepo.GetFailedJobList(jobId, Logtype, pageNo, pageSize);
                result.logType = Logtype;
                ViewBag.logType = Logtype;
                // Return partial view for AJAX rendering
                return View("JobTracking/_FailedJobList", result);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        [HttpGet]
        public ActionResult ExportJobListToExcel()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "JobTracking" + StringDate + ".xlsx";
                XLWorkbook wb = null;
                IEnumerable<JobTrackingModel> JobTrackingList = _jobTrackingRepo.GetJobList();
                wb = ExportHelper.ExportJobListToExcel(JobTrackingList, $"JobTrackingList-{DateTime.Now}");
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
        public ActionResult ExportJobByIdToExcel(int JobId)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "JobTracking" + StringDate + ".xlsx";
                XLWorkbook wb = null;

                JobTrackingModel JobID = _jobTrackingRepo.GetJobListByJobId(JobId);

                var listJobs = new List<JobTrackingModel>()
                {

                };
                listJobs.Add(JobID);
                wb = ExportHelper.ExportJobIdToExcel(listJobs, $"JobTrackingList-{DateTime.Now}");
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