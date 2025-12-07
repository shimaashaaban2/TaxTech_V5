using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;
using Tax_Tech.Areas.Configuration.Helpers;
using Newtonsoft.Json;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Tax_Tech.ApiModels;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using System.Reflection;
using System.Drawing.Printing;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Windows.Interop;
using System.Threading.Tasks;
using Tax_Tech.Helpers;
using ValidationsHelper = Tax_Tech.Areas.Configuration.Helpers.ValidationsHelper;

namespace Tax_Tech.Controllers
{
    public class InvoiceController : BaseController
    {
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly InvoiceApiRepository _invoiceApiRepository;
        private readonly VendorsApiRepository _vendorsApiRepository;
        private readonly BranchesApiRepository _branchesApiRepository;
        private readonly AccountLookupApiRepository _accountLookupApiRepository;
        private readonly ItemsApiRepository _itemsApiRepository;
        private readonly JobQueueApiRepository _jobQueueApiRepository;
        private readonly Areas.Configuration.Helpers.ValidationsHelper _validationsHelper;
        private readonly Tax_Tech.Areas.Configuration.Repository.PermissionRepository _permissionRepository;
       
        private readonly Logger _logger;
        private readonly Tax_Tech.Classes.ExeclValidator _execlValidator;

        public InvoiceController()
        {
            _lookupApiRepository = new LookupApiRepository();
            _invoiceApiRepository = new InvoiceApiRepository();
            _vendorsApiRepository = new VendorsApiRepository();
            _accountLookupApiRepository = new AccountLookupApiRepository();
            _branchesApiRepository = new BranchesApiRepository();
            _itemsApiRepository = new ItemsApiRepository();
            _jobQueueApiRepository = new JobQueueApiRepository();
            _validationsHelper = Areas.Configuration.Helpers.ValidationsHelper.GetValidations();
            _permissionRepository = new PermissionRepository();

            _logger = new Logger();
            _execlValidator = new Tax_Tech.Classes.ExeclValidator();
        }

        #region Pages
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                // All Docs
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;
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
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult AllDoc(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                // All Docs
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;
                IEnumerable<ApiModels.DocumentType> document = _lookupApiRepository.GetDocumentTypes();
                IEnumerable<ReceivedVendorListViewModel> vendor = _lookupApiRepository.ReceivedVendorsList();
                IEnumerable<ApiModels.ProcessStatus> ProcessStatus = _lookupApiRepository.GetProcessStatusList();
                ViewBag.ProcessStatusList = _lookupApiRepository.GetProcessStatusList();
                ReceivedDocumentListsModel model = new ReceivedDocumentListsModel();
                SendFileModel sendFileModel = new SendFileModel();
                model.DocumentType = document;
                model.ReceivedVendors = vendor;
                model.Status = ProcessStatus;
                model.fileModel = sendFileModel;
                return View(model);
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

        [AuthFilter(ReturnViewType.Json)]
        [HttpGet]
        //[AuthFilter(ReturnViewType.Partial)]
        public ActionResult List(int? status, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                IEnumerable<DocumentViewModel> list = null;

                if (status == null || status == 0)
                {
                    list = _invoiceApiRepository.GetInvoiceListLastWeek(entityId, pageNo, pageSize);
                }
                else
                {
                    list = _invoiceApiRepository.GetDocumentsOf(status, entityId, pageNo, pageSize);
                }

                if (list == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                int totalCount = list.FirstOrDefault()?.TotalCount ?? 0;

                ViewBag.PageModel = new PageModel
                {
                    PageNo = pageNo,
                    PageSize = pageSize
                };

                var jsonResult = Json(new { msg = "success", exportType = RenderRazorViewToString("_ExcelExportLastWeek"), view = RenderRazorViewToString("Invoice/_InvoiceList", list) });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;

                //return PartialView("Invoice/_InvoiceList", list);
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


        //Fatma 10-30-2023
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        //[AuthFilter(ReturnViewType.Partial)]
        public ActionResult AllDocList(MasterReportViewModel model)
        {
            try
            {
                model.ReportType = 3;
                model.InternalID = "";
                model.UUID = "";
                long UserId = Convert.ToInt64(Session["ID"]);
                long entityId = Convert.ToInt64(Session["EntityID"]);
                model.UserId = UserId;
                model.EntityID = entityId;
                var hasPermissionsResult = _permissionRepository.HasPermission(16, UserId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }
                #region DataManageent
                List<string> errors = new List<string>();
                if (model.DateFrom == null)
                    errors.Add(Resources.Resource.PleaseSpecifyFromdate);

                if (model.DateTo == null)
                    errors.Add(Resources.Resource.PleaseSpecifyTodate);

                if (model.DateFrom > model.DateTo)
                    errors.Add(Resources.Resource.FromDateShouldBeLessThanToDate);
                if (model.DateFrom > DateTime.Now)
                    errors.Add(Resources.Resource.DateCannotBeInTheFuture);


                if (model.DocumentCheckBox == false && model.DocumentType == null)//
                    errors.Add(Resources.Resource.ChooseDocumentType);
                else if (model.DocumentCheckBox == true)//0
                    model.DocumentType = 0;

                if (model.ProccessStatusOption == false && model.ProccessStatusID == null)
                    errors.Add(Resources.Resource.ProccessType);
                else if (model.ProccessStatusOption == true)//0
                    model.ProccessStatusID = 0;

                if (model.ItemOption == false && model.InputType == null)
                    errors.Add(Resources.Resource.InputType);
                else if (model.ItemOption == true)//0
                    model.InputType = null;

                if (model.accountOption == false && model.AccountID == null)
                    errors.Add(Resources.Resource.AccountOption);
                else if (model.accountOption == true)//0
                    model.AccountID = 0;
                #endregion

                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    if (model.ReturnPartial == 1)
                        return PartialView("_Result");

                    var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult1;
                }

                IEnumerable<DocumentViewModel> list = null;

                list = _invoiceApiRepository.GetMasterReportPagination(model);

                if (list == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    if (model.ReturnPartial == 1)
                        return PartialView("_Result");
                    var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult1;
                }

                int totalCount = list.FirstOrDefault()?.TotalCount ?? 0;
                ViewBag.PageNo = model.pageNo;
                ViewBag.PageSize = model.pageSize;
                ViewBag.PageModel = new PageModel
                {
                    PageNo = model.pageNo,
                    PageSize = model.pageSize
                };
                if (model.ReturnPartial == 1)
                    return PartialView("Invoice/_InvoiceMasterFilterList", list);

                var jsonResult = Json(new { msg = "success", exportType = RenderRazorViewToString("_ExcelExportLastWeek"), view = RenderRazorViewToString("Invoice/_InvoiceMasterFilterList", list) });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;

                if (model.ReturnPartial == 1)
                    return PartialView("_Result");
                var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult1;
            }
        }
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        //[AuthFilter(ReturnViewType.Partial)]
        public ActionResult AllDocListPaging1(MasterReportViewModel model)
        {
            try
            {
                model.ReportType = 3;
                model.InternalID = "";
                model.UUID = "";
                long UserId = Convert.ToInt64(Session["ID"]);
                long entityId = Convert.ToInt64(Session["EntityID"]);
                model.UserId = UserId;
                model.EntityID = entityId;
                var hasPermissionsResult = _permissionRepository.HasPermission(151, UserId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }
                List<string> errors = new List<string>();
                if (model.DateFrom == null)
                    errors.Add(Resources.Resource.PleaseSpecifyFromdate);

                if (model.DateTo == null)
                    errors.Add(Resources.Resource.PleaseSpecifyTodate);

                if (model.DateFrom > model.DateTo)
                    errors.Add(Resources.Resource.FromDateShouldBeLessThanToDate);
                if (model.DateFrom > DateTime.Now)
                    errors.Add(Resources.Resource.DateCannotBeInTheFuture);


                if (model.DocumentCheckBox == false && model.DocumentType == null)//
                    errors.Add(Resources.Resource.ChooseDocumentType);
                else if (model.DocumentCheckBox == true)//0
                    model.DocumentType = 0;

                if (model.ProccessStatusOption == false && model.ProccessStatusID == null)
                    errors.Add(Resources.Resource.ProccessType);
                else if (model.ProccessStatusOption == true)//0
                    model.ProccessStatusID = 0;

                if (model.ItemOption == false && model.InputType == null)
                    errors.Add(Resources.Resource.InputType);
                else if (model.ItemOption == true)//0
                    model.InputType = null;

                if (model.accountOption == false && model.AccountID == null)
                    errors.Add(Resources.Resource.AccountOption);
                else if (model.accountOption == true)//0
                    model.AccountID = 0;

                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    return PartialView("_Result");
                }

                IEnumerable<DocumentViewModel> list = null;

                list = _invoiceApiRepository.GetMasterReportPagination(model);

                if (list == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                int totalCount = list.FirstOrDefault()?.TotalCount ?? 0;
                ViewBag.PageNo = model.pageNo;
                ViewBag.PageSize = model.pageSize;
                ViewBag.PageModel = new PageModel
                {
                    PageNo = model.pageNo,
                    PageSize = model.pageSize
                };

                return PartialView("Invoice/_InvoiceMasterFilterList", list);


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
        [HttpGet]
        public ActionResult JobIvoices(long JobID)
        {
            ViewBag.JobID = JobID;
            //Session["JobID"] = JobID;
            return View();
        }

        //Fatma 10-3-2023
        [HttpPost]
        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetMasterReportList(long JobID)
        {
            try
            {
                ViewBag.JobID = JobID;
                IEnumerable<DocumentViewModel> list = _invoiceApiRepository.GetMasterReport(JobID);
                List<string> errors = new List<string>();
                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    return PartialView("_Result");
                }

                if (list == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }
                //return PartialView("Invoice/_InvoiceMasterFilterList",list);
                return PartialView("Invoice/_JobInvoiceList", list);
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

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetCancellInvoice(long JobID, string DocID, string processMsg)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                ApiResponse res = _invoiceApiRepository.CancellInvoice("8", DocID, processMsg, actionBy);
                if (res == null || res.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (res.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                ViewBag.Success = Resources.Resource.DataSavedSuccessfully;
                return Json(new { msg = "success", id = JobID, view = RenderRazorViewToString("_Result") });
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

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetCancellManyInvoices(long JobID, long InvoiceID)
        {
            try
            {
                //= ViewBag.JobID;
                var changeStatusResult = _invoiceApiRepository.CancellManyInvoice(InvoiceID, "");

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = string.IsNullOrWhiteSpace(changeStatusResult.CustomeRespons.ResponseMsg) ? Resources.Resource.ErrCreatingInvoiceHead : changeStatusResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                //return success msg
                ViewBag.Success = Resources.Resource.DataSavedSuccessfully;
                return Json(new { msg = "success", id = JobID, view = RenderRazorViewToString("_Result") });

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }



        #region Search By Doc Type and Start Date and End Date and Status
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(16, ReturnViewType.Normal)]
        public ActionResult FilterDocsPage(FilterViewModel model)
        {
            try
            {
                model.EntityID = Convert.ToInt64(Session["EntityID"]);
                ViewBag.FilterModel = model;
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
                    return View(new List<DocumentViewModel>());
                }

                if (string.IsNullOrWhiteSpace(Convert.ToString(model.ProcessStatusID)))
                {
                    model.ProcessStatusID = 0;
                }

                if (model.StartDate.Value >= model.EndDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return View(new List<DocumentViewModel>());
                }

                var filteredDocs = _invoiceApiRepository.FilterDocumentsWithPagination(model);

                return View(filteredDocs);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View(new List<DocumentViewModel>());
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        //[PermissionFilter(16, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult FilterDocs(FilterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (string.IsNullOrWhiteSpace(Convert.ToString(model.ProcessStatusID)))
                {
                    model.ProcessStatusID = 0;
                }

                if (model.StartDate.Value >= model.EndDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                model.EntityID = Convert.ToInt64(Session["EntityID"]);
                var filteredDocs = _invoiceApiRepository.FilterDocumentsWithPagination(model);
                ViewBag.FilterModel = model;
                return Json(new { msg = "success", exportType = RenderRazorViewToString("_ExcelExportBySearch", model), view = RenderRazorViewToString("Invoice/_SearchedInvoiceList", filteredDocs.ToList()) });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }
        #endregion

        #region Pending Docs
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult PendingDocs(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                // Pending Docs
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;
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
        public ActionResult PendingList(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                IEnumerable<DocumentViewModel> list = _invoiceApiRepository.GetDocumentsOf(1, entityId, pageNo, pageSize);
                ViewBag.PageModel = new PageModel
                {
                    PageNo = pageNo,
                    PageSize = pageSize
                };
                return PartialView("Invoice/_InvoiceListPending", list);
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


        [AuthFilter(ReturnViewType.Normal)]
        //[PermissionFilter(28, ReturnViewType.Normal)]
        public ActionResult RejectedLocally()
        {
            try
            {
                // Rejected Docs
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

        [AuthFilter(ReturnViewType.Normal)]
        //[PermissionFilter(29, ReturnViewType.Normal)]
        public ActionResult MOFReject()
        {
            try
            {
                // Rejected Docs
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

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(17, ReturnViewType.Normal)]
        public ActionResult Preview(int? invoiceId, int? invoiceType)
        {
            try
            {
                if (invoiceId == null || invoiceType == null)
                {
                    return RedirectToAction("error", "Public");
                }

                if (invoiceType == 2)
                {
                    return RedirectToAction("Preview", new { controller = "CreditNote", area = "", creditId = invoiceId });
                }
                else if (invoiceType == 3)
                {
                    return RedirectToAction("Preview", new { controller = "DebitNote", area = "", debitId = invoiceId });
                }

                var document = _invoiceApiRepository.GetSingleDocumentView(invoiceId.Value).FirstOrDefault();

                var docTotals = _invoiceApiRepository.GetDocumentTotals(invoiceId).FirstOrDefault() ?? new ApiModels.DocumentTotal
                {
                    DocumentID = invoiceId,
                    ItemsDiscount = 0,
                    NetTotal = 0,
                    SalesTotal = 0,
                    TaxTotals = 0
                };

                if (document != null)
                {
                    ViewBag.DocTotals = docTotals;
                    return View(document);
                }

                TempData["ErrorMsg"] = "Document Not Found";
                return RedirectToAction("error", "Public");
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

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(10, ReturnViewType.Normal)]
        public ActionResult Edit(int? invoiceId)
        {
            try
            {
                if (invoiceId == null)
                {
                    return RedirectToAction("error", "Public");
                }

                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true).ToList();
                ViewBag.Payments = _accountLookupApiRepository.GetAccounts(true).ToList();

                var document = _invoiceApiRepository.GetSingleDocumentView(invoiceId.Value).FirstOrDefault();
                var docTotals = _invoiceApiRepository.GetDocumentTotals(invoiceId).FirstOrDefault() ?? new ApiModels.DocumentTotal
                {
                    DocumentID = invoiceId,
                    ItemsDiscount = 0,
                    NetTotal = 0,
                    SalesTotal = 0,
                    TaxTotals = 0
                };
                if (document != null)
                {
                    ViewBag.DocTotals = docTotals;
                    return View(document);
                }
                return RedirectToAction("error", "Public");
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


        #region All Invoices
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult AllInvoices(int? pageNo = 1, int? pageSize = 100)
        {
            ViewBag.PageNo = pageNo;
            ViewBag.PageSize = pageSize;
            return View();
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult AllInvoicesList(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                var invoiceList = _invoiceApiRepository.GetLastWeekDocsByDocType("1", entityId, pageNo, pageSize);

                ViewBag.PageModel = new PageModel
                {
                    PageNo = pageNo,
                    PageSize = pageSize
                };
                return PartialView("Invoice/_InvoiceAllInvoicesList", invoiceList);
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

        [AuthFilter(ReturnViewType.Json)]
        [HttpGet]
        //[AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetInvoices(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                var invoiceList = _invoiceApiRepository.GetLastWeekDocsByDocType("1", entityId, pageNo, pageSize);

                ViewBag.PageModel = new PageModel
                {
                    PageNo = pageNo,
                    PageSize = pageSize
                };
                var jsonResult = Json(new { msg = "success", exportType = RenderRazorViewToString("_ExcelExportLastWeek"), view = RenderRazorViewToString("Invoice/_InvoiceList", invoiceList.ToList()) });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
                // return PartialView("Invoice/_InvoiceList", invoiceList.ToList());
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


        [AuthFilter(ReturnViewType.Partial)]
        //[PermissionFilter(16, ReturnViewType.Partial)]
        public ActionResult RejectedList(int? status)
        {
            try
            {
                IEnumerable<DocumentViewModel> list = null;
                long entityId = Convert.ToInt64(Session["EntityID"]);
                if (status == null || status == 0)
                {
                    list = _invoiceApiRepository.GetInvoiceListLastWeek(entityId);
                }
                else
                {
                    list = _invoiceApiRepository.GetDocumentsOf(status, entityId);
                }

                if (list == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("Invoice/_InvoiceListRejected", list.ToList());
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
        public ActionResult DocumentRejectedList(int? status, long DocID)
        {
            try
            {
                IEnumerable<DocumentViewModel> list = null;
                list = _invoiceApiRepository.GetDocumentOf(status, DocID);

                if (list == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("Invoice/_DocumentInvoiceListRejected", list.ToList());
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
        [PermissionFilter(17, ReturnViewType.Partial)]
        public ActionResult GetInvoiceLines(long? invoiceId)
        {
            try
            {
                if (invoiceId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDocID;
                    return PartialView("_Result");
                }

                var invoiceLinesList = _invoiceApiRepository.GetLines(invoiceId.Value);

                if (invoiceLinesList == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("Invoice/_InvoiceLines", invoiceLinesList.ToList());
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

        public ActionResult GetItemById(int? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidItemID;
                    return PartialView("_Result");
                }

                return PartialView("Invoice/_InvoiceItem", _itemsApiRepository.GetItemById(itemId.Value).FirstOrDefault());
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

        public ActionResult GetVendorById(int? vendorId)
        {
            try
            {
                if (vendorId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidVendorID;
                    return PartialView("_Result");
                }

                var jsonResult = Json(new { msg = "success", data = _vendorsApiRepository.GetVendorById(vendorId.Value).FirstOrDefault() });
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

        public ActionResult GetPaymentById(int? paymentID)
        {
            try
            {
                if (paymentID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidPaymentID;
                    return PartialView("_Result");
                }

                var jsonResult = Json(new { msg = "success", data = _accountLookupApiRepository.GetPaymentById(paymentID.Value).FirstOrDefault() });
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
        [PermissionFilter(10, ReturnViewType.Normal)]
        public ActionResult UploadExecl(string jobType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jobType))
                {
                    TempData["ErrorMsg"] = "ERROR: Invalid Job Type";
                    return RedirectToAction("error", "Public");
                }
                ViewBag.JobType = jobType;
                ViewBag.Title = Request.UrlReferrer;
                var List = _lookupApiRepository.GetTemplatesList(Convert.ToInt32(jobType));
                return View(List);
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



        #region Export to Execl
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult ExportToExecl(int? status)
        {
            try
            {
                string docName = "AllDocsList";

                // checking permissions
                var userId = Convert.ToInt64(Session["ID"]);
                if (status == null || status == 0)
                {
                    var hasPermissionsResult = _permissionRepository.HasPermission(16, userId);

                    if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                    {
                        TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                        return RedirectToAction("Forbidden", "Public");
                    }

                }

                long entityId = Convert.ToInt64(Session["EntityID"]);
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "AllDocs" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                if (status == null || status == 0)
                {
                    wb = ExportHelper.ExportAllDocs(_invoiceApiRepository.GetInvoiceListLastWeek(entityId).ToList(), $"{docName}-{DateTime.Now}");
                }
                else
                {
                    if (status == 6)
                    {
                        docName = "MOFRejected";
                        FileName = "Export" + "MOFRejected" + StringDate + ".xlsx";
                    }

                    else if (status == 7)
                    {
                        docName = "InternalRejected";
                        FileName = "Export" + "InternalRejected" + StringDate + ".xlsx";

                    }
                    wb = ExportHelper.ExportAllDocs(_invoiceApiRepository.GetDocumentsOf(status, entityId).ToList(), $"{docName}-{DateTime.Now}");
                }

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
        public ActionResult ExportToExeclBySearch(int? ProcessStatusID, DateTime? StartDate, DateTime? EndDate
            , int DocumentType)
        {
            try
            {
                string docName = "AllDocsList";

                // checking permissions
                var userId = Convert.ToInt64(Session["ID"]);
                //if (status == null || status == 0)
                //{
                //    var hasPermissionsResult = _permissionRepository.HasPermission(16, userId);

                //    if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                //    {
                //        TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                //        return RedirectToAction("Forbidden", "Public");
                //    }

                //}

                if (string.IsNullOrWhiteSpace(Convert.ToString(ProcessStatusID)))
                    ProcessStatusID = 0;
                long EntityID = Convert.ToInt64(Session["EntityID"]);


                FilterViewModel model = new FilterViewModel()
                {
                    EntityID = EntityID,
                    ProcessStatusID = ProcessStatusID,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    DocumentType = DocumentType,
                    PageNo = 0,
                    PageSize = 0
                };
                var filteredDocs = _invoiceApiRepository.FilterDocumentsWithPagination(model);

                long entityId = Convert.ToInt64(Session["EntityID"]);
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "AllDocsFrom" + StringDate + "To" + EndDate + ".xlsx";

                XLWorkbook wb = null;

                wb = ExportHelper.ExportAllDocs(_invoiceApiRepository.FilterDocumentsWithPagination(model), FileName);


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
        public ActionResult ExportAllInvoicesToExecl()
        {
            try
            {
                // checking permissions
                var userId = Convert.ToInt64(Session["ID"]);
                var hasPermissionsResult = _permissionRepository.HasPermission(17, userId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }

                long entityId = Convert.ToInt64(Session["EntityID"]);
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Invoice" + StringDate + ".xlsx";

                XLWorkbook wb = null;

                wb = ExportHelper.ExportAllDocs(_invoiceApiRepository.GetLastWeekDocsByDocType("1", entityId).ToList(), $"InvoiceList-{DateTime.Now}");

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
        public ActionResult ExportRecievedDocsToExecl(DateTime fromDate, DateTime toDate, string mOStatus, string accountName, byte mDocTypeID = 0,
      bool docTypeOption = false, bool mOFStatusOption = false, bool accountOption = false)
        {
            try
            {
                if (fromDate > toDate)
                {
                    TempData["ErrorMsg"] = Resources.Resource.FromDateShouldBeLessThanToDate;
                    return RedirectToAction("error", "Public");
                }

                // checking permissions
                var userId = Convert.ToInt64(Session["ID"]);
                var hasPermissionsResult = _permissionRepository.HasPermission(13, userId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }

                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "ReceivedDocs" + StringDate + ".xlsx";

                XLWorkbook wb = null;
                IEnumerable<RecentDocument> receivedDocs = _invoiceApiRepository.GetReceivedDocumentsByFilter
                    (new ReceivedDocumentSubmitFilterViewModel
                    {
                        From = Convert.ToDateTime(fromDate),
                        To = Convert.ToDateTime(toDate),
                        docTypeOption = docTypeOption
                    ,
                        mDocTypeID = mDocTypeID,
                        mOFStatusOption = mOFStatusOption,
                        mOStatus = mOStatus,
                        accountOption = accountOption,
                        accountName = accountName
                    });

                wb = ExportHelper.ExportRecievedDocs(receivedDocs, $"RecievedDocs-{DateTime.Now}");

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
        public ActionResult ExportSearchDocsToExecl(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                if (fromDate > toDate)
                {
                    TempData["ErrorMsg"] = Resources.Resource.FromDateShouldBeLessThanToDate;
                    return RedirectToAction("error", "Public");
                }

                // checking permissions
                var userId = Convert.ToInt64(Session["ID"]);
                var hasPermissionsResult = _permissionRepository.HasPermission(28, userId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }

                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "SearchedDocs_From_" + Convert.ToDateTime(fromDate).ToShortDateString().Replace("/", "-") + "_To_" + Convert.ToDateTime(toDate).ToShortDateString().Replace("/", "-") + ".xlsx";

                XLWorkbook wb = null;

                wb = ExportHelper.ExportSearchDocs(_invoiceApiRepository.GetSearchDocuments(fromDate, toDate, 0, 0).ToList(), $"RecievedDocs-{DateTime.Now}");

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



        #endregion

        #region Actions
        #region Filter Docs By Internal ID
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult FilterDocsByInternalIDPage(string internalID, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(internalID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InternalDCannotBeEmpty;
                    return View(new List<DocumentViewModel>());
                }

                var docsByInternalID = _invoiceApiRepository.GetInvoiceByInternalID(internalID);

                ViewBag.InternalID = internalID;
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;
                return View(docsByInternalID.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View(new List<DocumentViewModel>());
            }
        }

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(17, ReturnViewType.Json)]
        public ActionResult FilterDocsByInternalID(string internalID, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(internalID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InternalDCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var docsByInternalID = _invoiceApiRepository.GetInvoiceByInternalID(internalID);

                ViewBag.InternalID = internalID;
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;
                var jsonResult = Json(new { msg = "success", exportType = RenderRazorViewToString("_ExcelExportLastWeek"), view = RenderRazorViewToString("Invoice/_InvoiceFilterByInternalIDList", docsByInternalID) });
                jsonResult.MaxJsonLength = int.MaxValue;
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

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(17, ReturnViewType.Json)]
        public ActionResult FilterInvoicesByInternalID(MasterReportViewModel model, string internalID, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(internalID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InternalDCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var invoicesByInternalID = _invoiceApiRepository.GetDocByInternalIDAndType(internalID, 1);

                ViewBag.InternalID = internalID;
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceFilterByInternalIDList", invoicesByInternalID) });
                jsonResult.MaxJsonLength = int.MaxValue;
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


        //Fatma 10-30-2023
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(17, ReturnViewType.Json)]
        public ActionResult GetInvoicesByInternalID(MasterReportViewModel model)
        {
            try
            {

                model.ReportType = 1;
                model.UUID = "";
                model.InputType = "";
                model.DateFrom = null;
                model.DateTo = null;
                model.ProccessStatusID = 0;
                model.AccountID = 0;
                model.DocumentType = 0;

                long UserId = Convert.ToInt64(Session["ID"]);
                long EntityID = Convert.ToInt64(Session["EntityID"]);
                model.UserId = UserId;
                model.EntityID = EntityID;
                var hasPermissionsResult = _permissionRepository.HasPermission(16, UserId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }

                if (string.IsNullOrWhiteSpace(model.InternalID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.UUIDCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                // (short ReportType, string InternalID, string UUID, string InputType,DateTime? DateFrom, DateTime DateTo, short? ProccessStatusID, long? AccountID, long UserId,short? DocumentType, long EntityID, int pageNo, int pageSize)
                var invoicesByInternalID = _invoiceApiRepository.GetMasterReportPagination(model);

                ViewBag.InternalID = model.InternalID;
                ViewBag.PageNo = model.pageNo;
                ViewBag.PageSize = model.pageSize;

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceMasterFilterList", invoicesByInternalID) });
                jsonResult.MaxJsonLength = int.MaxValue;
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

        //Fatma 10-30-2023
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(17, ReturnViewType.Json)]
        public ActionResult GetInvoicesByUUID(MasterReportViewModel model)
        {
            try
            {
                model.ReportType = 2;
                model.InternalID = "";
                model.InputType = "";
                model.DateFrom = null;
                model.DateTo = null;
                model.ProccessStatusID = 0;
                model.AccountID = 0;
                model.DocumentType = 0;
                long UserId = Convert.ToInt64(Session["ID"]);
                long EntityID = Convert.ToInt64(Session["EntityID"]);
                model.UserId = UserId;
                model.EntityID = EntityID;
                var hasPermissionsResult = _permissionRepository.HasPermission(16, UserId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }
                if (string.IsNullOrWhiteSpace(model.UUID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InternalDCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var invoicesByInternalID = _invoiceApiRepository.GetMasterReportPagination(model);

                ViewBag.UUID = model.UUID;
                ViewBag.PageNo = model.pageNo;
                ViewBag.PageSize = model.pageSize;

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceMasterFilterList", invoicesByInternalID) });
                jsonResult.MaxJsonLength = int.MaxValue;
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

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UploadExecl(HttpPostedFileBase execlFile, string jobType, int? TemplateID)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);

                if (string.IsNullOrWhiteSpace(jobType))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidJobType;
                    return Json(new { msg = "failed", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
                }

                if (execlFile == null || execlFile.ContentType != "application/vnd.ms-excel" && execlFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    ViewBag.ErrorMsg = "Please upload a valid Execl file";
                    return Json(new { msg = "failed", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
                }
                string fileName = Guid.NewGuid().ToString("N") + System.IO.Path.GetExtension(execlFile.FileName);

                string UploadResult = UploadFile(execlFile, jobType, fileName);
                if (UploadResult != "")
                {
                    ViewBag.ErrorMsg = UploadResult;
                    return Json(new { msg = "failed", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
                }

                // inserting a new job queue record
                var insertResult = _jobQueueApiRepository.InsertJobQueue(fileName, actionBy, jobType, TemplateID);

                if (insertResult == null || insertResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "failed", view = RenderRazorViewToString("JobQueue/_ValidationResult", ViewBag.ErrorMsg) });
                }

                if (insertResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = insertResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "failed", view = RenderRazorViewToString("JobQueue/_ValidationResult", ViewBag.ErrorMsg) });
                }

                return Json(new { msg = "success", view = RenderRazorViewToString("JobQueue/_ValidationResult") });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                _logger.WriteToFile($"3.Exception Thrown: {ViewBag.ErrorMsg}");
                return Json(new { msg = "errors", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult CreateInvoiceHead(CreateInvoiceHead model)
        {
            try
            {
                var Vendors = _vendorsApiRepository.GetVendors(true);

                long UserID = Convert.ToInt64(Session["ID"]);
                int EntityID = Convert.ToInt16(Session["EntityID"]);
                var invoiceType = Convert.ToInt32(InvoiceType.Invoice);
                model.ActionBy = UserID;
                model.EntityId = EntityID;
                model.ActivityCode = "1200";
               
                //model.VendorID = Convert.ToString(Vendors.Where(a => a.VendorName == model.VendorName).Select(i => i.VendorID).FirstOrDefault());
                var Vendor = _vendorsApiRepository.GetVendorByName(model.VendorName);
                model.VendorID = Convert.ToString(Vendor.FirstOrDefault().VendorID);
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                int days = Convert.ToInt32(PublicConfig.CreateDocumentMinDays);
                if (Convert.ToDateTime(model.InvoiceIssueDate) < DateTime.Now.AddDays(days * -1))
                {
                    ViewBag.ErrorMsg = $"{Resources.Resource.DocumentIssueDateCannotBeBefore} {days} {Resources.Resource.Days}";
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                ApiModels.ApiResponse createInvoiceResult;

                if (PublicConfig.CreateDocumentVersion == "1")
                {
                    // small version
                    createInvoiceResult = _invoiceApiRepository.CreateInvoiceHead(model, invoiceType.ToString());
                }
                else
                {
                    // full version
                    createInvoiceResult = _invoiceApiRepository.CreateInvoiceHeadV2(model, invoiceType.ToString());
                }

                if (createInvoiceResult == null || createInvoiceResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                string redirecturl = string.Empty;
                if (PublicConfig.CreateDocumentVersion == "1")
                {
                    // small version document
                    redirecturl = Url.Action("InvoiceDetails", "InvoiceLine", new { invoiceId = createInvoiceResult.CustomeRespons.DocumentID, invoiceType = 1 });
                }
                else
                {
                    // full version
                    redirecturl = Url.Action("InvoiceDetailsV2", "InvoiceLine", new { invoiceId = createInvoiceResult.CustomeRespons.DocumentID, invoiceType = 1 });
                }

                return Json(new { msg = "success", redirecturl });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult ChangeInvoiceStatus(string docId, string status, string msg, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                if (string.IsNullOrWhiteSpace(status))
                {
                    ViewBag.SingleValid = Resources.Resource.PlsSelectDocStatus;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (string.IsNullOrWhiteSpace(docId))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                long actionBy = Convert.ToInt64(Session["ID"]);

                var changeStatusResult = _invoiceApiRepository.ChangeInvoiceStatus(status, docId, msg, actionBy);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = string.IsNullOrWhiteSpace(changeStatusResult.CustomeRespons.ResponseMsg) ? Resources.Resource.ErrCreatingInvoiceHead : changeStatusResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                IEnumerable<DocumentViewModel> list = _invoiceApiRepository.GetDocumentsOf(1, entityId, pageNo, pageSize);

                if (list == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                ViewBag.PageModel = new PageModel
                {
                    PageSize = pageSize,
                    PageNo = pageNo
                };

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceListPending", list) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult ApproveAll(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);

                long actionBy = Convert.ToInt64(Session["ID"]);

                var changeStatusResult = _invoiceApiRepository.InvoiceApproveAll(actionBy);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = string.IsNullOrWhiteSpace(changeStatusResult.CustomeRespons.ResponseMsg) ? Resources.Resource.ErrCreatingInvoiceHead : changeStatusResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                IEnumerable<DocumentViewModel> list = _invoiceApiRepository.GetDocumentsOf(1, entityId, pageNo, pageSize);
                //int totalCount = list.FirstOrDefault()?.TotalCount ?? 0;
                //var pagedList = new StaticPagedList<DocumentViewModel>(list, pageNo.Value, pageSize.Value, totalCount);

                ViewBag.PageModel = new PageModel
                {
                    PageNo = pageNo,
                    PageSize = pageSize
                };

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceListPending", list) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult ChangeInvoiceStatusInPlace(string DocumentId, string ProcessStatusID, string msg, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                if (string.IsNullOrWhiteSpace(ProcessStatusID))
                {
                    ViewBag.SingleValid = Resources.Resource.PlsSelectDocStatus;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (string.IsNullOrWhiteSpace(DocumentId))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                long actionBy = Convert.ToInt64(Session["ID"]);

                var changeStatusResult = _invoiceApiRepository.ChangeInvoiceStatus(ProcessStatusID, DocumentId, msg, actionBy);

                var list = _invoiceApiRepository.GetInvoiceListLastWeek(entityId, pageNo, pageSize);
                var modifiedDoc = _invoiceApiRepository.GetSingleDocumentView(Convert.ToInt64(DocumentId)).FirstOrDefault();
                if (modifiedDoc == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                ViewBag.PageModel = new PageModel
                {
                    PageNo = pageNo,
                    PageSize = pageSize
                };
                var jsonResult = Json(new { msg = "success", data = modifiedDoc, view = RenderRazorViewToString("Invoice/_InvoiceList", list), invoiceId = $"#invoice-{DocumentId}" });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult SendInvoiceToSubmit(long DocumentId)
        {
            try
            {
                long? EntityID = 0;// Convert.ToInt64(Session["EntityID"]);
                var EntityResult = _invoiceApiRepository.GetDocumentEntity(DocumentId);
                if (EntityResult == null || EntityResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                if (EntityResult.CustomeRespons.ResponseID == "0")
                {
                    EntityID = Convert.ToInt64(EntityResult.CustomeRespons.EntityID);
                }

                ApiModels.ApiResponse submitDocumentResult;

                if (PublicConfig.CreateDocumentVersion == "1")
                {
                    // small version
                    submitDocumentResult = _invoiceApiRepository.SubmitDocument(DocumentId, EntityID);
                }
                else
                {
                    // full version
                    submitDocumentResult = _invoiceApiRepository.SubmitDocumentV2(DocumentId, EntityID);
                }

                if (submitDocumentResult == null || submitDocumentResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                //if fail
                if (submitDocumentResult.CustomeRespons.ResponseID != "1" || submitDocumentResult.CustomeRespons.ResponseID == "405")
                {
                    ViewBag.ErrorMsg = string.IsNullOrWhiteSpace(submitDocumentResult.CustomeRespons.ResponseMsg) ? Resources.Resource.ErrCreatingInvoiceHead : submitDocumentResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                //if success
                //check version
                if (submitDocumentResult.CustomeRespons.Version != "0.9")
                {
                    ViewBag.Success = Resources.Resource.SentToSubmissionLog;
                    return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });

                }
                else
                {
                    ViewBag.Success = string.IsNullOrWhiteSpace(submitDocumentResult.CustomeRespons.ResponseMsg) ? Resources.Resource.InvoiceSubmittedSuccess : submitDocumentResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "success", submitDocumentResult.CustomeRespons.UUID, view = RenderRazorViewToString("_Result") });
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        [PermissionFilter(17, ReturnViewType.Partial)]
        public ActionResult UpdateInvoiceHead(long? invoiceID, UpdateInvoiceHead model)
        {
            try
            {
                if (invoiceID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDocID;
                    return PartialView("_Result");
                }

                model.ActionBy = Convert.ToInt64(Session["ID"]);
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return PartialView("_Result");
                }

                int days = Convert.ToInt32(PublicConfig.CreateDocumentMinDays);
                if (Convert.ToDateTime(model.InvoiceIssueDate) < DateTime.Now.AddDays(days * -1))
                {
                    ViewBag.ErrorMsg = $"{Resources.Resource.DocumentIssueDateCannotBeBefore} {days} {Resources.Resource.Days}";
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                ApiModels.ApiResponse updateInvoiceHeadResult;

                if (PublicConfig.CreateDocumentVersion == "1")
                {
                    // small version
                    updateInvoiceHeadResult = _invoiceApiRepository.UpdateInvoiceHead(model, invoiceID);
                }
                else
                {
                    // full version
                    updateInvoiceHeadResult = _invoiceApiRepository.UpdateInvoiceHeadV2(model, invoiceID);
                }

                if (updateInvoiceHeadResult == null || updateInvoiceHeadResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (updateInvoiceHeadResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = updateInvoiceHeadResult.CustomeRespons.ResponseMsg;
                    return PartialView("_Result");
                }

                ViewBag.Success = Resources.Resource.DataSavedSuccessfully;
                return PartialView("_Result");
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

        [AuthFilter(ReturnViewType.Json)]
        public ActionResult FilterReceivedDocs(RecievedDocsFilterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (string.IsNullOrWhiteSpace(Convert.ToString(model.ProcessStatusID)))
                {
                    model.ProcessStatusID = 0;
                }

                if (model.StartDate.Value >= model.EndDate.Value)
                {
                    ViewBag.ErrorMsg = Resources.Resource.StartDateMustBeLessThanEndDate;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var filteredDocs = _invoiceApiRepository.FilterReceivedDocs(model);

                if (filteredDocs == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                return Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceReceivedList", filteredDocs) });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }
        public ActionResult ExportAllDocsToExecl(DateTime fromDate, DateTime toDate, short? DocumentType = 0, short ProccessStatusID = 0, string InputType = ""
          , long? AccountID = 0, bool ItemOption = false, bool accountOption = false, bool ProccessStatusOption = false, bool DocumentCheckBox = false)
        {
            try
            {
                if (fromDate > toDate)
                {
                    TempData["ErrorMsg"] = Resources.Resource.FromDateShouldBeLessThanToDate;
                    return RedirectToAction("error", "Public");
                }

                // checking permissions
                var userId = Convert.ToInt64(Session["ID"]);
                long EntityID = Convert.ToInt64(Session["EntityID"]);
                var hasPermissionsResult = _permissionRepository.HasPermission(151, userId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }

                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Invoices" + StringDate + ".xlsx";

                XLWorkbook wb = null;
                IEnumerable<DocumentViewModel> AllDocs = _invoiceApiRepository.GetMasterReportExcel
                    (new MasterReportViewModel
                    {
                        ReportType = 3,
                        DateFrom = Convert.ToDateTime(fromDate),
                        DateTo = Convert.ToDateTime(toDate),
                        InternalID = "",
                        UUID = "",
                        InputType = InputType,
                        ProccessStatusID = ProccessStatusID,
                        AccountID = AccountID,
                        accountOption = accountOption,
                        ItemOption = ItemOption,
                        DocumentType = DocumentType,
                        ProccessStatusOption = ProccessStatusOption,
                        DocumentCheckBox = DocumentCheckBox,
                        UserId = Convert.ToInt16(userId),
                        EntityID = EntityID,
                        pageSize = 100,
                        pageNo = 1
                    });

                wb = ExportHelper.ExportAllDocs(AllDocs, $"Invoices-{DateTime.Now}");

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


        #region Invoicing Api
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        [PermissionFilter(12, ReturnViewType.Json)]
        public ActionResult CancelDocument(long? invoiceId, string reason)
        {
            try
            {
                if (invoiceId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDocID;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                // sending a put request to cancel a document by uuid
                var cancelDocResult = _invoiceApiRepository.CancelDocument(invoiceId, reason);

                ViewBag.Success = cancelDocResult.CustomeRespons.ResponseMsg;
                return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
        }

        [AuthFilter(ReturnViewType.Normal)]
       [PermissionFilter(19, ReturnViewType.Normal)]
        public ActionResult DownloadDocumentPdf(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                {
                    TempData["ErrorMsg"] = Resources.Resource.InvalidUUID;
                    return RedirectToAction("error", "Public");
                }

                long? entityId = Convert.ToInt64(Session["EntityID"]);

                var downloadDocPdfResult = _invoiceApiRepository.DownloadDocumentPdf(uuid);

                if (downloadDocPdfResult == null || downloadDocPdfResult?.Stream == null)
                {
                    TempData["ErrorMsg"] = Resources.Resource.NoPdfFileFoundforThisDocument;
                    return RedirectToAction("error", "Public");
                }
                if (downloadDocPdfResult.Success == "0")
                {
                    TempData["ErrorMsg"] = downloadDocPdfResult.MSG;
                    return RedirectToAction("error", "Public");
                }
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename={$"{downloadDocPdfResult?.FileName}" ?? $"invoice-{uuid}.pdf"}");
                using (MemoryStream MyMemoryStream = new MemoryStream(downloadDocPdfResult?.Stream))
                {
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return Content("", "application/pdf");
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("error", "Public");
            }
        }
        //Fatma 10-26-23
        #region Recent Received Docs
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult GetRecentDocs()
        {
            try
            {
                IEnumerable<ApiModels.DocumentType> document = _lookupApiRepository.GetDocumentTypes();
                IEnumerable<MOFStatusListViewModel> MOF = _lookupApiRepository.MOFStatusList();
                IEnumerable<ReceivedVendorListViewModel> vendor = _lookupApiRepository.ReceivedVendorsList();
                ReceivedDocumentListsModel model = new ReceivedDocumentListsModel();
                model.DocumentType = document;
                model.MOFStatus = MOF;
                model.ReceivedVendors = vendor;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction(Url.Action("error", "Public"));
            }
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult GetSearchDocs(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                IEnumerable<ApiModels.DocumentType> document = _lookupApiRepository.GetDocumentTypes();
                IEnumerable<MOFStatusListViewModel> MOF = _lookupApiRepository.MOFStatusList();
                ReceivedDocumentListsModel model = new ReceivedDocumentListsModel();
                model.DocumentType = document;
                model.MOFStatus = MOF;
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction(Url.Action("error", "Public"));
            }
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult ReceivedDocs()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction(Url.Action("error", "Public"));
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult ReceivedDocsList()
        {
            try
            {
                var receivedDocsListResult = _invoiceApiRepository.GetReceivedDocuments().ToList();

                if (receivedDocsListResult == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("Invoice/_InvoiceReceivedList", receivedDocsListResult);
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
        //Fatma 11-8-2023
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(17, ReturnViewType.Json)]
        public ActionResult GetDocumentByInternalID(string InternalID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                IEnumerable<SearchDocument> DocsByInternalID = _invoiceApiRepository.GetDocumentByID(InternalID);

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceAPISearchDocs", DocsByInternalID) });
                jsonResult.MaxJsonLength = int.MaxValue;
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

        //Fatma 11-8-2023
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(17, ReturnViewType.Json)]
        public ActionResult GetDocumentByUUID(string UUID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                IEnumerable<SearchDocument> DocsByUUID = _invoiceApiRepository.GetDocumentByUUID(UUID);

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceAPISearchDocs", DocsByUUID) });
                jsonResult.MaxJsonLength = int.MaxValue;
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

        //Fatma 11-8-2023
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(16, ReturnViewType.Json)]
        public ActionResult GetDocumentByFilters(DocumentByFiltersModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                List<string> errors = new List<string>();
                if (model.From == null)
                    errors.Add(Resources.Resource.PleaseSpecifyFromdate);

                if (model.To == null)
                    errors.Add(Resources.Resource.PleaseSpecifyTodate);

                if (model.From > model.To)
                    errors.Add(Resources.Resource.FromDateShouldBeLessThanToDate);
                if (model.From > DateTime.Now)
                    errors.Add(Resources.Resource.DateCannotBeInTheFuture);


                if (model.AllDoc == false && model.DocumentType == null)
                    errors.Add(Resources.Resource.ChooseDocumentType);
                else if (model.AllDoc == true)
                    model.DocumentType = null;

                if (model.AllStatus == false && model.MOStatus == null)
                    errors.Add(Resources.Resource.mOFStatusOption);
                else if (model.AllStatus == true)
                    model.MOStatus = null;
                IEnumerable<SearchDocument> DocsByFilters = _invoiceApiRepository.GetDocumentByFilters(model);
                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    if (model.ReturnPartial == 1)
                        return PartialView("_Result");

                    var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult1;
                }

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceAPISearchDocs", DocsByFilters) });
                jsonResult.MaxJsonLength = int.MaxValue;
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

        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(13, ReturnViewType.Json)]
        public ActionResult GetRecentDocsList(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                List<string> errors = new List<string>();
                if (fromDate == null)
                    errors.Add(Resources.Resource.PleaseSpecifyFromdate);

                if (toDate == null)
                    errors.Add(Resources.Resource.PleaseSpecifyTodate);

                if (fromDate > toDate)
                    errors.Add(Resources.Resource.FromDateShouldBeLessThanToDate);
                if (fromDate > DateTime.Now)
                    errors.Add(Resources.Resource.DateCannotBeInTheFuture);
                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult1;
                }

                IEnumerable<RecentDocument> receivedDocs = _invoiceApiRepository.GetReceivedDocuments(fromDate, toDate);

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceAPIRecDocs", receivedDocs.ToList()), data = receivedDocs.ToList() });
                jsonResult.MaxJsonLength = int.MaxValue;
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                var jsonResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
        }

        //Fatma 26-10-23
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(13, ReturnViewType.Json)]
        public ActionResult GetRecentDocsByFilterList(ReceivedDocumentSubmitFilterViewModel model)
        {
            try
            {
                List<string> errors = new List<string>();
                if (model.From == null)
                    errors.Add(Resources.Resource.PleaseSpecifyFromdate);

                if (model.To == null)
                    errors.Add(Resources.Resource.PleaseSpecifyTodate);

                if (model.From > model.To)
                    errors.Add(Resources.Resource.FromDateShouldBeLessThanToDate);
                if (model.From > DateTime.Now)
                    errors.Add(Resources.Resource.DateCannotBeInTheFuture);

                if (model.docTypeOption == false && model.mDocTypeID == null)//
                    errors.Add(Resources.Resource.ChooseDocumentType);
                else if (model.docTypeOption == true)//0
                    model.mDocTypeID = 0;

                if (model.mOFStatusOption == false && model.mOStatus == null)
                    errors.Add(Resources.Resource.mOFStatusOption);
                else if (model.mOFStatusOption == true)//0
                    model.mOStatus = null;

                if (model.accountOption == false && model.accountName == null)
                    errors.Add(Resources.Resource.AccountOption);
                else if (model.accountOption == true)//0
                    model.accountName = null;

                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult1;
                }


                IEnumerable<RecentDocument> receivedDocs = _invoiceApiRepository.GetReceivedDocumentsByFilter(model);

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceAPIRecDocs", receivedDocs.ToList()), data = receivedDocs.ToList() });
                jsonResult.MaxJsonLength = int.MaxValue;
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                var jsonResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
        }

        [PermissionFilter(28, ReturnViewType.Json)]
        public ActionResult GetSearchDocsList(DateTime? fromDate, DateTime? toDate, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                List<string> errors = new List<string>();
                if (fromDate == null)
                    errors.Add(Resources.Resource.PleaseSpecifyFromdate);

                if (toDate == null)
                    errors.Add(Resources.Resource.PleaseSpecifyTodate);

                if (fromDate > toDate)
                    errors.Add(Resources.Resource.FromDateShouldBeLessThanToDate);
                if (fromDate > DateTime.Now)
                    errors.Add(Resources.Resource.DateCannotBeInTheFuture);

                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult1;
                }

                IEnumerable<SearchDocument> searchDocs = _invoiceApiRepository.GetSearchDocuments(fromDate, toDate, pageNo, pageSize);

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_InvoiceAPISearchDocs", searchDocs.ToList()) });
                jsonResult.MaxJsonLength = int.MaxValue;
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                var jsonResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
        }

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult InsertReceivedDocument(string docsStr)
        {
            try
            {
                List<RecentDocument> docs = JsonConvert.DeserializeObject<List<RecentDocument>>(docsStr);

                foreach (var doc in docs)
                {
                    if (doc.IssuerId != PublicConfig.IssuerID)
                    {
                        var insertReceivedDocResult = _invoiceApiRepository.InsertReceivedDocs(doc);

                        if (insertReceivedDocResult == null || insertReceivedDocResult.CustomeRespons == null)
                        {
                            ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                            return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                        }
                    }
                }

                ViewBag.Success = Resources.Resource.DataSavedSuccessfully;
                return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult RejectRecievedDocs(string reason, string docsStr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(docsStr))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                List<long?> docIds = JsonConvert.DeserializeObject<List<long?>>(docsStr);
                string message = Resources.Resource.DataSavedSuccessfully;

                foreach (var docId in docIds)
                {
                    var rejectDocResult = _invoiceApiRepository.RejectDocument(docId, reason);
                    message = rejectDocResult.CustomeRespons.ResponseMsg;
                }

                ViewBag.Success = message;
                return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult RejectRecievedDoc(string reason, string docId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reason))
                {
                    ViewBag.ErrorMsg = Resources.Resource.PleaseProvideReason;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (string.IsNullOrWhiteSpace(docId))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var rejectDocResult = _invoiceApiRepository.RejectDocument(Convert.ToInt64(docId), reason);

                ViewBag.Success = Resources.Resource.DocumenthasbeenrejectedSuccessfully;
                return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }
        #endregion
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

        private string UploadFile(HttpPostedFileBase File, string JobType, string fileName)
        {
            string Msg = "";
            byte[] FileBytes = null;
            if (File != null)
            {
                System.IO.MemoryStream target = new System.IO.MemoryStream();
                File.InputStream.CopyTo(target);
                FileBytes = target.ToArray();
            }

            PermissionRepository permissionRepository = new PermissionRepository();
            var hasPermission = permissionRepository.HasPermission(10, Convert.ToInt64(Session["ID"]));

            if (hasPermission.CustomeRespons.HasPermission == "0")
            {
                Msg = Resources.Resource.NotAuthorized;
                return Msg;
            }

            CustomViewModel ResultModel = UploadAPIRepository.Get().UploadExcel(FileBytes, JobType, fileName);
            if (ResultModel != null)
            {
                string Result = "";
                if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                {
                    if (Result != "1")
                        Msg = Resources.Resource.NoDataReturned;
                }
            }
            else
                Msg = Resources.Resource.NoDataReturned;

            return Msg;
        }
        #endregion

        #region Download Excel

        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetTemplateURL(int? TemplateID)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(TemplateID))
                //{
                //    TempData["ErrorMsg"] = "ERROR: Invalid Job Type";
                //    return RedirectToAction("error", "Public");
                //}
                ViewBag.TemplateID = TemplateID;
                ViewBag.Title = Request.UrlReferrer;
                var List = _lookupApiRepository.GetTemplateURL(TemplateID);
                string URLPath = List.CustomeRespons.URL;
                var jsonResult = Json(new { msg = "success", Url = URLPath });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
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
                return Json(new { msg = "fail" });
            }
        }

        #endregion



       
        #region Vendors List
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public async Task<ActionResult> GetVendorsList(string VendorName)
        {
            var Vendors = _vendorsApiRepository.GetVendors(true);
            var VendorsName = Vendors.Select(a => a.VendorName);//get only list of names
            //TempData["VendorsId"] = Convert.ToString(Vendors.Where(a => a.VendorName == VendorName).Select(i => i.VendorID).FirstOrDefault());
            //ViewBag.VendorsId = Vendors.Where(a => a.VendorName == VendorName).Select(i => i.VendorID).FirstOrDefault();
            var Namesres = VendorName.ToLower();
            var NameSelect = VendorsName.Where(suggestion => suggestion.ToLower().Contains(Namesres)).ToList();
            var vendorsJson = JsonConvert.SerializeObject(NameSelect);
            return Json(vendorsJson);//send list here
        }

        #endregion

       
    }
}