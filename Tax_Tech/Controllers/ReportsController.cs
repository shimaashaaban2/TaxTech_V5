using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly ReportApiRepository _reportApiRepository;
        private readonly LookupApiRepository _lookupApiRepository;

        public ReportsController()
        {
            _reportApiRepository = new ReportApiRepository();
            _lookupApiRepository = new LookupApiRepository();
        }

        #region Vendors Report
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Vendors()
        {
            try
            {
                ViewBag.DocTypes = _lookupApiRepository.GetDocumentTypes();
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
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult VendorsList()
        {
            try
            {
                return PartialView("Reports/_ReportsVendorsList");
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
        public ActionResult FilterVendorsByDate(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                if (fromDate == null || toDate == null || docTypeID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                if (fromDate.Value > toDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return PartialView("_Result");
                }

                // filtering docs
                var vendorsReport = _reportApiRepository.GetVendorsReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID);

                return PartialView("Reports/_ReportsVendorsList", vendorsReport);
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
        #endregion

        #region Branches Report
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Branches()
        {
            try
            {
                ViewBag.DocTypes = _lookupApiRepository.GetDocumentTypes();
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
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult BranchesList()
        {
            try
            {
                return PartialView("Reports/_ReportsBranchesList");
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
        public ActionResult FilterBranchesByDate(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                if (fromDate == null || toDate == null || docTypeID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                if (fromDate.Value > toDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return PartialView("_Result");
                }

                // filtering docs
                var branchesReport = _reportApiRepository.GetBranchReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID);

                // TODO: filter logic goes here
                return PartialView("Reports/_ReportsBranchesList", branchesReport);
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
        #endregion

        #region Items Report
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Items()
        {
            try
            {
                ViewBag.DocTypes = _lookupApiRepository.GetDocumentTypes();
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
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult ItemsList()
        {
            try
            {
                return PartialView("Reports/_ReportsItemsList");
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
        public ActionResult FilterItemsByDate(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                if (fromDate == null || toDate == null || docTypeID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }
                if (fromDate.Value > toDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return PartialView("_Result");
                }

                // filtering docs
                var itemsReport = _reportApiRepository.GetItemsReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID);

                return PartialView("Reports/_ReportsItemsList", itemsReport);
                //// TODO: filter logic goes here
                //return PartialView("Reports/_ReportsItemsList");
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
        #endregion

        #region Document Reports
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult DocumentsValues()
        {
            try
            {
                ViewBag.DocTypes = _lookupApiRepository.GetDocumentTypes();
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
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult DocumentsValuesList()
        {
            try
            {
                return PartialView("Reports/_ReportsDocumentsValues");
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
        public ActionResult FilterDocumentValuesByDate(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                if (fromDate == null || toDate == null || docTypeID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                if (fromDate.Value > toDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return PartialView("_Result");
                }

                var documentValues = _reportApiRepository.GetDocumentValues(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID, entityId);
                return PartialView("Reports/_ReportsDocumentsValues", documentValues);
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

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult DocumentsCount()
        {
            try
            {
                ViewBag.DocTypes = _lookupApiRepository.GetDocumentTypes();
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
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult DocumentsCountList()
        {
            try
            {
                return PartialView("Reports/_ReportsDocumentCountList");
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

        public ActionResult GetDocTotalsJson()
        {
            try
            {
                string startDate = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
                string endDate = DateTime.Now.ToString("yyyy/MM/dd");
                var docsCount = _reportApiRepository.GetDocumentsCountReport(startDate, endDate, "1");

                if (docsCount == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    var errorResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    errorResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return errorResult;
                }

                var jsonResult = Json(new { msg = "success", data = docsCount.ToList() });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                var errorResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                errorResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return errorResult;
            }
        }

        public ActionResult GetRejectedDocsJson()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                string startDate = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
                string endDate = DateTime.Now.ToString("yyyy/MM/dd");
                var rejectedDocs = _reportApiRepository.GetRejectedDocumentReport(startDate, endDate, entityId);

                if (rejectedDocs == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    var errorResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    errorResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return errorResult;
                }

                var jsonResult = Json(new { msg = "success", data = rejectedDocs.ToList() });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                var errorResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                errorResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return errorResult;
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult FilterDocumentCountByDate(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                if (fromDate == null || toDate == null || docTypeID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                if (fromDate.Value > toDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return PartialView("_Result");
                }

                return PartialView("Reports/_ReportsDocumentCountList", _reportApiRepository.GetDocumentsCountReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID));
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

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult MOFResult()
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
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult DocumentsRejectedList()
        {
            try
            {
                return PartialView("Reports/_ReportsRejectedDocs");
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
        public ActionResult FilterRejectedDocsByDate(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                if (fromDate == null || toDate == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                if (fromDate.Value > toDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return PartialView("_Result");
                }

                return PartialView("Reports/_ReportsRejectedDocs", _reportApiRepository.GetRejectedDocumentReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), entityId));
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
        #endregion

        #region Job Queue Reports
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult FailedImportedDocs()
        {
            return View();
        }

        public ActionResult FailedImportedDocsList()
        {
            try
            {
                var failedImportedDocs = _reportApiRepository.GetFailedDocumentImports();

                if (failedImportedDocs == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("Reports/_ReportsFailedmportedDocslist", failedImportedDocs);
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

        public ActionResult FailedImportedDocsListJson()
        {
            try
            {
                var failedImportedDocs = _reportApiRepository.GetFailedDocumentImports();

                if (failedImportedDocs == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var jsonResult = Json(new { msg = "success", data = failedImportedDocs.ToList() });
                jsonResult.MaxJsonLength = int.MaxValue;
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
        }
        #endregion

        #region Export to Execl
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult ExportVendorsReport(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "VendorsTotals" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                wb = ExportHelper.ExportVendorsTotals(_reportApiRepository.GetVendorsReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID), $"VendorsTotals-{DateTime.Now}");

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
        public ActionResult ExportBranchesReport(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "BranchesTotals" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                wb = ExportHelper.ExportBranchesTotals(_reportApiRepository.GetBranchReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID), $"BranchesTotals-{DateTime.Now}");

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
        public ActionResult ExportItemsReport(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "ItemsTotals" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                wb = ExportHelper.ExportItemsTotals(_reportApiRepository.GetItemsReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID), $"ItemsTotals-{DateTime.Now}");

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
        public ActionResult ExportDocCountReport(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "DocsCount" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                wb = ExportHelper.ExportDocCount(_reportApiRepository.GetDocumentsCountReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID), $"DocsCount-{DateTime.Now}");

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
        public ActionResult ExportDocValuesReport(DateTime? fromDate, DateTime? toDate, string docTypeID)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "DocValues" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                long entityID = Convert.ToInt64(Session["EntityID"]);

                wb = ExportHelper.ExportDocValues(_reportApiRepository.GetDocumentValues(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), docTypeID, entityID), $"DocValues-{DateTime.Now}");

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
        public ActionResult ExportDocsRejectedReport(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "MOFResult" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                long entityID = Convert.ToInt64(Session["EntityID"]);

                wb = ExportHelper.ExportDocsRejectedValues(_reportApiRepository.GetRejectedDocumentReport(fromDate.Value.ToString("yyyy/MM/dd"), toDate.Value.ToString("yyyy/MM/dd"), entityID), $"DocsRejected-{DateTime.Now}");

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

        //Fatma 10-31-2023
        [AuthFilter(ReturnViewType.Normal)]
        [HttpGet]
        public ActionResult TaxDetailsReport()
        {
            return View();
        }


        //Fatma 10-31-2023
        [AuthFilter(ReturnViewType.Partial)]
        [HttpPost]
        public ActionResult TaxDetailsReportPartialView(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                if (fromDate == null || toDate == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                if (fromDate > toDate)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return PartialView("_Result");
                }

                if (fromDate > DateTime.Now)
                {
                    ViewBag.ErrorMsg = Resources.Resource.DateCannotBeInTheFuture;
                }
                var jsonResult = Json(new { msg = "success", exportType = RenderRazorViewToString("_ExcelExportLastWeek"), view = RenderRazorViewToString("Reports/_TaxDetailsReportList", _reportApiRepository.GetTaxDetailsReport(fromDate, toDate)) });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
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

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult ExportFailedImportedDocsReport()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "FailedImportedDocs" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                long entityID = Convert.ToInt64(Session["EntityID"]);

                wb = ExportHelper.ExportFailedImportedDocsValues(_reportApiRepository.GetFailedDocumentImports(), $"FailedImportedDocs-{DateTime.Now}");

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
        #endregion

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
