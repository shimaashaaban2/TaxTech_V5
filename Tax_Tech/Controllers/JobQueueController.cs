using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    public class JobQueueController : BaseController
    {
        private readonly JobQueueApiRepository _jobQueueApiRepository;
        private readonly ReportApiRepository _reportApiRepository;
        public JobQueueController()
        {
            _jobQueueApiRepository = new JobQueueApiRepository();
            _reportApiRepository = new ReportApiRepository();
        }

        #region Pages
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult JobQueueLogSummary(int? jobId)
        {
            ViewBag.JobID = jobId;
            return View();
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult JobQueueLogSummaryList(int? jobId)
        {
            try
            {
                if(jobId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                var jobQueues = _jobQueueApiRepository.GetJobQueueSummary(jobId);
                return PartialView("JobQueue/_JobQueueLogDetails", jobQueues.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Details(string jobQueueID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jobQueueID))
                {
                    return RedirectToAction("error", "Public");
                }

                // TODO: get job details
                var jobDetailsResult = _jobQueueApiRepository.GetJobQueueTrackByJobID(jobQueueID);

                if (jobDetailsResult == null)
                {
                    return RedirectToAction("error", "Public");
                }

                return View(jobDetailsResult);
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
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult List()
        {
            try
            {
                var list = _jobQueueApiRepository.GetJobQueues();

                if (list == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("JobQueue/_JobQueueList", list.ToList());
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
        public ActionResult GetJobTypes()
        {
            try
            {
                var jobQueueTypes = _jobQueueApiRepository.GetJobQueueTypes();

                if (jobQueueTypes == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("JobQueue/_JobQueueTypes", jobQueueTypes);
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

        public ActionResult FilterByJobType(string jobType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jobType))
                {
                    var jobQueus = _jobQueueApiRepository.GetJobQueues();
                    return PartialView("JobQueue/_JobQueueList", jobQueus.ToList());
                }

                var filteredJobQueues = _jobQueueApiRepository.GetJobQueues(jobType);

                if (filteredJobQueues == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("JobQueue/_JobQueueList", filteredJobQueues.ToList());
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
        //[PermissionFilter(16, ReturnViewType.Normal)]
        //public ActionResult DownloadFile(string jobQueueID)
        //{
        //    try
        //    {
        //        string downloadLink = "";
        //        if (string.IsNullOrWhiteSpace(jobQueueID))
        //        {
        //            return RedirectToAction("error", "Public");
        //        }

        //        // TODO: get job details
        //        ApiResponse jobDetailsResult = _jobQueueApiRepository.GetJobsQueueByJobID(jobQueueID);

        //        if (jobDetailsResult == null || jobDetailsResult.CustomeRespons == null)
        //        {
        //            return RedirectToAction("error", "Public");
        //        }


        //        downloadLink = jobDetailsResult.CustomeRespons.FileLink;

        //        if (string.IsNullOrWhiteSpace(downloadLink))
        //        {
        //            return RedirectToAction("error", "Public");
        //        }
        //        DownloadExcelFile(downloadLink);
        //        return null;
        //        // return File(System.IO.File.ReadAllBytes(downloadLink), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //    }
        //    catch (Exception ex)
        //    {
        //        string ErrorMsg = "";
        //        if (ex.InnerException != null)
        //        {
        //            ErrorMsg = Convert.ToString(ex.InnerException.Message);
        //            //ErrorLog.GetErrorLog().Insert("Balance", "Request Account Statement", ex.InnerException, ex.InnerException.Message);
        //        }
        //        else
        //        {
        //            ErrorMsg = Convert.ToString(ex.Message);
        //            //ErrorLog.GetErrorLog().Insert("Balance", "Request Account Statement", ex, ex.Message);
        //        }
        //        TempData["ErrorMsg"] = "ERROR: " + ErrorMsg;
        //        return RedirectToAction("error", "Public");
        //    }
        //}

        private void DownloadExcelFile(string downloadLink)
        {
            //File to be downloaded.
            string fileName = methods.Getmethods().GetRand() + ".xlsx";

            //if(!System.IO.File.Exists(fileName))
            //{
            //    throw new FileNotFoundException(Resources.Resource.FileNotFound);
            //}

            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);

            //Writing the File to Response Stream.
            Response.WriteFile(downloadLink);

            //Flushing the Response.
            Response.Flush();

            //Deleting the File and ending the Response.
            System.IO.File.Delete(downloadLink);
            Response.End();
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult ExportToExecl()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Failed Imported Docs" + StringDate + ".xlsx";

                XLWorkbook wb = ExportHelper.ExportFailedImportedDocuments(_reportApiRepository.GetFailedDocumentImports().ToList(), $"FailedImportedDocs-{DateTime.Now}");

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

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult ExportGlobalLogToExecl(long? jobId, byte? logType)
        {
            try
            {
                string stringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string fileName = "GlobalLog" + stringDate + ".xlsx";

                XLWorkbook wb = ExportHelper.ExportGlobalLogDocuments(_jobQueueApiRepository.GetJobsTrackerDetails(jobId, logType).ToList(), $"FailedImportedDocs-{DateTime.Now}");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
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
        #endregion
    }
}
