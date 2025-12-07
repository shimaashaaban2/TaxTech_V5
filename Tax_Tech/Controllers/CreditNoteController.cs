using ClosedXML.Excel;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Helpers;
using Tax_Tech.Repository;
using Tax_Tech.Repository.InvoicingApi;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Controllers
{
    public class CreditNoteController : BaseController
    {
        private readonly BranchesApiRepository _branchesApiRepository;
        private readonly VendorsApiRepository _vendorsApiRepository;
        private readonly AccountLookupApiRepository _accountLookupApiRepository;
        private readonly InvoiceApiRepository _invoiceApiRepository;
        private readonly PermissionRepository _permissionRepository;
        private readonly Areas.Configuration.Helpers.ValidationsHelper _validationsHelper;

        public CreditNoteController()
        {
            _validationsHelper = new Areas.Configuration.Helpers.ValidationsHelper();

            _branchesApiRepository = new BranchesApiRepository();
            _vendorsApiRepository = new VendorsApiRepository();
            _accountLookupApiRepository = new AccountLookupApiRepository();
            _invoiceApiRepository = new InvoiceApiRepository();
            _permissionRepository = new PermissionRepository();
        }

        #region Pages
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
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

        [AuthFilter(ReturnViewType.Json)]
        [HttpGet]
        //[AuthFilter(ReturnViewType.Partial)]
        public ActionResult List(int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                // Credit Note ID: 2
                var creditNoteList = _invoiceApiRepository.GetLastWeekDocsByDocType(PublicConfig.CreditNoteTypeID, entityId, pageNo, pageSize);

                int totalCount = creditNoteList.FirstOrDefault()?.TotalCount ?? 0;
                var pagedList = new StaticPagedList<DocumentViewModel>(creditNoteList, pageNo.Value, pageSize.Value, totalCount);

                var jsonResult = Json(new { msg = "success", exportType = RenderRazorViewToString("_ExcelExportLastWeek"), view = RenderRazorViewToString("DebitNote/_DebitNoteList", pagedList) });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;

                // return PartialView("CreditNote/_CreditNoteList", pagedList);
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
        [PermissionFilter(17, ReturnViewType.Normal)]
        public ActionResult CreditNoteDetails(string creditId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(creditId))
                {
                    return RedirectToAction("Error404", "Public");
                }
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true,entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                ViewBag.Payments = _accountLookupApiRepository.GetAccounts(true);

                // TODO: get the credit note by id
                var singleDocResult = _invoiceApiRepository.GetSingleDocumentView(Convert.ToInt64(creditId));

                if (singleDocResult == null || singleDocResult.Count() == 0)
                {
                    return RedirectToAction("Error404", "Public");
                }

                var docTotals = _invoiceApiRepository.GetDocumentTotals(Convert.ToInt64(creditId));

                if (docTotals == null)
                {
                    return RedirectToAction("error", "Public");
                }

                ViewBag.DocTotals = docTotals.FirstOrDefault() ?? new ApiModels.DocumentTotal
                {
                    DocumentID = Convert.ToInt64(creditId),
                    ExtraDiscountAmount = 0,
                    ItemsDiscount = 0,
                    NetTotal = 0,
                    SalesTotal = 0,
                    TaxTotals = 0,
                    TotalAmount = 0
                };

                ViewBag.CreditNoteId = creditId;
                return View(singleDocResult.FirstOrDefault());
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
        public ActionResult CreditNoteDetailsV2(string creditId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(creditId))
                {
                    return RedirectToAction("Error404", "Public");
                }
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true, entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                ViewBag.Payments = _accountLookupApiRepository.GetAccounts(true);

                // TODO: get the credit note by id
                var singleDocResult = _invoiceApiRepository.GetSingleDocumentView(Convert.ToInt64(creditId));

                if (singleDocResult == null || singleDocResult.Count() == 0)
                {
                    return RedirectToAction("Error404", "Public");
                }

                var docTotals = _invoiceApiRepository.GetDocumentTotals(Convert.ToInt64(creditId));

                if (docTotals == null)
                {
                    return RedirectToAction("error", "Public");
                }

                ViewBag.DocTotals = docTotals.FirstOrDefault() ?? new ApiModels.DocumentTotal
                {
                    DocumentID = Convert.ToInt64(creditId),
                    ExtraDiscountAmount = 0,
                    ItemsDiscount = 0,
                    NetTotal = 0,
                    SalesTotal = 0,
                    TaxTotals = 0,
                    TotalAmount = 0
                };

                ViewBag.CreditNoteId = creditId;
                return View(singleDocResult.FirstOrDefault());
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
        [PermissionFilter(17, ReturnViewType.Partial)]
        public ActionResult CreditLinesWithActions(string docId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(docId))
                {
                    ViewBag.ErrorMsg = Tax_Tech.Resources.Resource.InvalidDocID;
                    return PartialView("_Result");
                }

                var linesResult = _invoiceApiRepository.GetLines(Convert.ToInt64(docId));

                if (linesResult == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("CreditNote/_CreditNoteLinesWithActions", linesResult.ToList());
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
        public ActionResult CreditLines(string docId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(docId))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDocID;
                    return PartialView("_Result");
                }

                var linesResult = _invoiceApiRepository.GetLines(Convert.ToInt64(docId));

                if (linesResult == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("CreditNote/_CreditNoteLines", linesResult.ToList());
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
        [HttpPost]
        public ActionResult GetLinkedInvoices(string docId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(docId))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var getLinkedInvoicesResult = _invoiceApiRepository.GetLinkedInvoices(docId);

                if (getLinkedInvoicesResult == null || getLinkedInvoicesResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (getLinkedInvoicesResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = getLinkedInvoicesResult.CustomeRespons.ResponseMsg ?? Resources.Resource.ErrorOccured;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                // getting list of invoices based on the returned ids
                var invoicesIds = getLinkedInvoicesResult.CustomeRespons.LinkedInvoices.Split(',');

                IEnumerable<DocumentViewModel> linkedInvoicesDetails = _invoiceApiRepository.GetInvoicesList(invoicesIds);

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("CreditNote/_CreditLinkedInvoiceList", linkedInvoicesDetails), data = linkedInvoicesDetails });
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

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(17, ReturnViewType.Normal)]
        public ActionResult Preview(string creditId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(creditId))
                {
                    TempData["ErrorMsg"] = "ERROR: Credit Note Not Found";
                    return RedirectToAction("error", "Public");
                }

                // TODO: get document totals and get the document details
                var singleDocResult = _invoiceApiRepository.GetSingleDocumentView(Convert.ToInt64(creditId));

                if (singleDocResult == null || singleDocResult.Count() == 0)
                {
                    return RedirectToAction("Error404", "Public");
                }

                var docTotals = _invoiceApiRepository.GetDocumentTotals(Convert.ToInt64(creditId));

                if (docTotals == null)
                {
                    return RedirectToAction("error", "Public");
                }

                ViewBag.DocTotals = docTotals.FirstOrDefault() ?? new ApiModels.DocumentTotal
                {
                    DocumentID = Convert.ToInt64(creditId),
                    ExtraDiscountAmount = 0,
                    ItemsDiscount = 0,
                    NetTotal = 0,
                    SalesTotal = 0,
                    TaxTotals = 0,
                    TotalAmount = 0
                };
                return View(singleDocResult.FirstOrDefault());
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

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetInvoiceInfo(string internalID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(internalID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InternalDCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var docsByInternalID = _invoiceApiRepository.GetInvoiceByInternalID(internalID);

                if (docsByInternalID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                // ensuring that the document has a valid status and has a uuid
                var docToInsert = docsByInternalID.FirstOrDefault();

                if (docToInsert == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.DocumentNoteFound;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (string.IsNullOrWhiteSpace(docToInsert.UUID) || docToInsert.ProcessStatusID != 5)
                {
                    ViewBag.ErrorMsg = Resources.Resource.DocIsInvalid;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var jsonResult = Json(new { msg = "success", data = docsByInternalID });
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

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult DownloadDocumentPdf(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                {
                    TempData["ErrorMsg"] = Resources.Resource.InvalidUUID;
                    return RedirectToAction("error", "Public");
                }

                // login as a taxpayer
                string entityId = Convert.ToString(Session["EntityID"]);
                var _authApiRepository = new AuthApiRepository(entityId);
                var _documentsApiRepository = new DocumentsApiRepository(entityId);

                var authResult = _authApiRepository.LoginAsTaxPayer();

                var documentPdfResult = _documentsApiRepository.GetDocumentAsPdf(uuid, authResult.AccessToken);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=CreditNote-{uuid}");
                using (MemoryStream MyMemoryStream = new MemoryStream(documentPdfResult))
                {
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return Content("", "application/pdf");
            }
            catch(UnauthorizedAccessException ex)
            {
                TempData["ErrorMsg"] = JsonConvert.DeserializeObject<ErrorModel>(ex.Message)?.Error;
                return RedirectToAction("error", "Public");
            }
            catch(HttpRequestException ex)
            {
                TempData["ErrorMsg"] = JsonConvert.DeserializeObject<ResultModel>(ex.Message).Error.Message;
                return RedirectToAction("error", "Public");
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult ExportToExecl()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "CreditNote" + StringDate + ".xlsx";

                // checking permissions
                var userId = Convert.ToInt64(Session["ID"]);
                var hasPermissionsResult = _permissionRepository.HasPermission(17, userId);

                if (hasPermissionsResult.CustomeRespons.HasPermission == "0")
                {
                    TempData["ErrorMsg"] = Resources.Resource.NotAuthorized;
                    return RedirectToAction("Forbidden", "Public");
                }

                XLWorkbook wb = ExportHelper.ExportAllDocs(_invoiceApiRepository.GetLastWeekDocsByDocType("2",entityId).ToList(), $"CreditNoteList-{DateTime.Now}");

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
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        #endregion

        #region Actions
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

                var changeStatusResult = _invoiceApiRepository.SubmitDocument(DocumentId, EntityID);
                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                //if fail
                if (changeStatusResult.CustomeRespons.ResponseID != "1" || changeStatusResult.CustomeRespons.ResponseID == "405")
                {
                    ViewBag.ErrorMsg = string.IsNullOrWhiteSpace(changeStatusResult.CustomeRespons.ResponseMsg) ? Resources.Resource.ErrCreatingInvoiceHead : changeStatusResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                //if success
                //check version
                if (changeStatusResult.CustomeRespons.Version != "0.9")
                {
                    ViewBag.Success = Resources.Resource.SentToSubmissionLog;
                    return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });

                }
                else
                {
                    ViewBag.Success = string.IsNullOrWhiteSpace(changeStatusResult.CustomeRespons.ResponseMsg) ? Resources.Resource.InvoiceSubmittedSuccess : changeStatusResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "success", changeStatusResult.CustomeRespons.UUID, view = RenderRazorViewToString("_Result") });
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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(17, ReturnViewType.Json)]
        public ActionResult FilterCreditNotesByInternalID(string internalID, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(internalID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InternalDCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var creditNotesByInternalID = _invoiceApiRepository.GetDocByInternalIDAndType(internalID, 2);

                int totalCount = creditNotesByInternalID.FirstOrDefault()?.TotalCount ?? 0;
                var pagedList = new StaticPagedList<DocumentViewModel>(creditNotesByInternalID, pageNo.Value, pageSize.Value, totalCount);

                var jsonResult = Json(new { msg = "success", exportType = RenderRazorViewToString("_ExcelExportLastWeek"), view = RenderRazorViewToString("CreditNote/_CreditNoteList", pagedList) });
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
        public ActionResult CreateCreditNoteHead(CreateInvoiceHead model)
        {
            try
            {
                var Vendors = _vendorsApiRepository.GetVendors(true);
                long UserID = Convert.ToInt64(Session["ID"]);
                int EntityID = Convert.ToInt16(Session["EntityID"]);
                var credit = Convert.ToInt32(InvoiceType.Credit);
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

                var createInvoiceHeadResult = _invoiceApiRepository.CreateInvoiceHead(model, credit.ToString());

                if (createInvoiceHeadResult == null || createInvoiceHeadResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                string redirecturl = string.Empty;
                if (PublicConfig.CreateDocumentVersion == "1")
                {
                    // small version document
                    redirecturl = Url.Action("CreditNoteDetails", "CreditNote", new { creditId = createInvoiceHeadResult.CustomeRespons.DocumentID });
                }
                else
                {
                    // full version
                    redirecturl = Url.Action("CreditNoteDetailsV2", "CreditNote", new { creditId = createInvoiceHeadResult.CustomeRespons.DocumentID });
                }

                return Json(new { redirecturl, msg = "success" });
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
        [PermissionFilter(17, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UpdateCreditNoteHead(long? invoiceID, UpdateInvoiceHead model)
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

                var updateInvoiceHeadResult = _invoiceApiRepository.UpdateInvoiceHead(model, invoiceID);

                if (updateInvoiceHeadResult == null || updateInvoiceHeadResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
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
        [HttpPost]
        public ActionResult UpdateLinkedInvoices(string creditNoteID, string docsStr, string total, string creditTotal)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(creditNoteID) || string.IsNullOrWhiteSpace(total) || string.IsNullOrWhiteSpace(creditTotal))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                double linkedInvoicesTotal = Convert.ToDouble(total);
                double creditNoteTotal = Convert.ToDouble(creditTotal);

                if (linkedInvoicesTotal > creditNoteTotal)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvoicesTotalErr;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                // update linked invoices
                var updateLinkedInvoicesResult = _invoiceApiRepository.UpdateInvoiceLinks(docsStr, creditNoteID);

                if (updateLinkedInvoicesResult == null || updateLinkedInvoicesResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (updateLinkedInvoicesResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = updateLinkedInvoicesResult.CustomeRespons.ResponseMsg ?? Resources.Resource.ErrorOccured;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
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
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
        }
        #endregion


        #region Credit Note Lines
        [HttpPost]
        [AuthFilter(ReturnViewType.Partial)]
        [PermissionFilter(17, ReturnViewType.Partial)]
        public ActionResult CreateCreditNoteLine(InvoiceLineCreateViewModel model)
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);
                int EntityID = Convert.ToInt16(Session["EntityID"]);

                model.ActionBy = UserID;

                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                if (model.percentageDiscount)
                    model.Discount = Convert.ToDouble(model.Discount) * (Convert.ToDouble(model.SoldTotal) / 100);
                //if (model.Currency == "1")
                //    model.ExchangeRate = 1;
                List<string> errors = new List<string>();
                if (model.Currency != "1" && model.ExchangeRate == 1)
                {
                    errors.Add(Resources.Resource.WrongExchangeRate);
                }
                if (model.Currency == "1" && model.ExchangeRate != 1)
                {
                    errors.Add(Resources.Resource.WrongExchangeRate);
                }
                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    if (model.ReturnPartial == 1)
                        return PartialView("_Result");

                    var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult1;
                }
                var createInvoiceLineResult = _invoiceApiRepository.CreateInvoiceLine(model);

                if (createInvoiceLineResult == null || createInvoiceLineResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (createInvoiceLineResult.CustomeRespons.ResponseID == "-1")
                {
                    ViewBag.ErrorMsg = string.IsNullOrWhiteSpace(createInvoiceLineResult.CustomeRespons.ResponseMsg) ? Resources.Resource.ErrCreatingInvoiceHead : createInvoiceLineResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                if (createInvoiceLineResult.CustomeRespons.ResponseID == "0")
                {
                    ViewBag.ErrorMsg = string.IsNullOrWhiteSpace(createInvoiceLineResult.CustomeRespons.ResponseMsg) ? Resources.Resource.ErrCreatingInvoiceHead : createInvoiceLineResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                // getting document totals
                var docTotals = _invoiceApiRepository.GetDocumentTotals(model.InvoiceInternalID).FirstOrDefault();

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("CreditNote/_CreditNoteLinesWithActions", _invoiceApiRepository.GetLines(Convert.ToInt64(model.InvoiceInternalID)).ToList()), totals = RenderRazorViewToString("~/Views/Shared/Invoice/_DocumentTotals.cshtml", docTotals) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "errors", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UpdateInvoiceLine(InvoiceLineCreateViewModel model)
        {
            try
            {
                long? actionBy = Convert.ToInt64(Session["ID"]);
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.Validation) });
                }

                if (model.percentageDiscount)
                    model.Discount = Convert.ToDouble(model.Discount) * (Convert.ToDouble(model.SoldTotal) / 100);
                //if (model.Currency == "1")
                //    model.ExchangeRate = 1;
                List<string> errors = new List<string>();
                if (model.Currency != "1" && model.ExchangeRate == 1)
                {
                    errors.Add(Resources.Resource.WrongExchangeRate);
                }
                if (model.Currency == "1" && model.ExchangeRate != 1)
                {
                    errors.Add(Resources.Resource.WrongExchangeRate);
                }
                if (errors.Count > 0)
                {
                    ViewBag.Validation = errors;
                    if (model.ReturnPartial == 1)
                        return PartialView("_Result");

                    var jsonResult1 = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult1;
                }

                var updateResult = _invoiceApiRepository.UpdateInvoiceLine(model, actionBy);

                if (updateResult == null || updateResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
                }

                if (updateResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = updateResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
                }

                var docTotals = _invoiceApiRepository.GetDocumentTotals(model.InvoiceInternalID).FirstOrDefault() ?? new ApiModels.DocumentTotal
                {
                    DocumentID = model.InvoiceInternalID,
                    ItemsDiscount = 0,
                    NetTotal = 0,
                    SalesTotal = 0,
                    TaxTotals = 0
                };

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Views/Shared/CreditNote/_CreditNoteLinesWithActions.cshtml", _invoiceApiRepository.GetLines(Convert.ToInt64(model.InvoiceInternalID)).ToList()), totals = RenderRazorViewToString("~/Views/Shared/Invoice/_DocumentTotals.cshtml", docTotals) });
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
        public ActionResult DeleteInvoiceLine(long? lineId, long? docId)
        {
            try
            {
                long? actionBy = Convert.ToInt64(Session["ID"]);

                if (lineId == null || docId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var deleteResult = _invoiceApiRepository.DeleteInvoiceLine(lineId, actionBy);

                if (deleteResult == null || deleteResult.CustomeRespons == null)
                {
                    ViewBag.ErorrMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (deleteResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = deleteResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var docTotals = _invoiceApiRepository.GetDocumentTotals(docId).FirstOrDefault() ?? new ApiModels.DocumentTotal
                {
                    DocumentID = docId,
                    ItemsDiscount = 0,
                    NetTotal = 0,
                    SalesTotal = 0,
                    TaxTotals = 0
                };

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Views/Shared/CreditNote/_CreditNoteLinesWithActions.cshtml", _invoiceApiRepository.GetLines(Convert.ToInt64(docId)).ToList()), totals = RenderRazorViewToString("~/Views/Shared/Invoice/_DocumentTotals.cshtml", docTotals) });
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
