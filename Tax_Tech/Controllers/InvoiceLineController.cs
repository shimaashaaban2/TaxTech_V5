using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class InvoiceLineController : BaseController
    {
        private readonly VendorsApiRepository _vendorsApiRepository;
        private readonly BranchesApiRepository _branchesApiRepository;
        private readonly AccountLookupApiRepository _accountLookupApiRepository;
        private readonly InvoiceApiRepository _invoiceApiRepository;

        public InvoiceLineController()
        {
            _vendorsApiRepository = new VendorsApiRepository();
            _branchesApiRepository = new BranchesApiRepository();
            _accountLookupApiRepository = new AccountLookupApiRepository();
            _invoiceApiRepository = new InvoiceApiRepository();
        }

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(17, ReturnViewType.Normal)]
        public ActionResult InvoiceDetails(string invoiceId, int? invoiceType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceId) || invoiceType == null)
                {
                    TempData["ErrorMsg"] = Resources.Resource.InvalidDataProvided;
                    return RedirectToAction("Error404", "Public");
                }

                if (invoiceType == 2)
                {
                    return RedirectToAction("CreditNoteDetails", new { controller = "CreditNote", area = "", creditId = invoiceId });
                }
                else if (invoiceType == 3)
                {
                    return RedirectToAction("DebitNoteDetails", new { controller = "DebitNote", area = "", debitId = invoiceId });
                }
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true,entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                ViewBag.Payments = _accountLookupApiRepository.GetAccounts(true);
                long.TryParse(invoiceId, out long invoiceID);
                var singleInvoiceResult = _invoiceApiRepository.GetSingleDocumentView(invoiceID);

                var invoiceToView = singleInvoiceResult.FirstOrDefault();

                if (invoiceToView == null)
                {
                    TempData["ErrorMsg"] = "Document Not Found";
                    return RedirectToAction("error", "Public");
                }

                return View(invoiceToView);
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
        public ActionResult InvoiceDetailsV2(string invoiceId, int? invoiceType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceId) || invoiceType == null)
                {
                    TempData["ErrorMsg"] = Resources.Resource.InvalidDataProvided;
                    return RedirectToAction("error", "Public");
                }

                if (invoiceType == 2)
                {
                    return RedirectToAction("CreditNoteDetailsV2", new { controller = "CreditNote", area = "", creditId = invoiceId });
                }
                else if (invoiceType == 3)
                {
                    return RedirectToAction("DebitNoteDetailsV2", new { controller = "DebitNote", area = "", debitId = invoiceId });
                }

                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true,entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                ViewBag.Payments = _accountLookupApiRepository.GetAccounts(true);
                long.TryParse(invoiceId, out long invoiceID);
                var singleInvoiceResult = _invoiceApiRepository.GetSingleDocumentView(invoiceID);

                var invoiceToView = singleInvoiceResult.FirstOrDefault();

                if (invoiceToView == null)
                {
                    return RedirectToAction("Error404", "Public");
                }

                return View(invoiceToView);
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

        #region Actions
        [HttpPost]
        [AuthFilter(ReturnViewType.Partial)]
        [PermissionFilter(17, ReturnViewType.Partial)]
        public ActionResult CreateInvoiceLine(InvoiceLineCreateViewModel model)
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);
                int EntityID = Convert.ToInt16(Session["EntityID"]);

                model.ActionBy = UserID;

                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
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

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Views/Shared/Invoice/_InvoiceLinesWithActionsList.cshtml", _invoiceApiRepository.GetLines(Convert.ToInt64(model.InvoiceInternalID)).ToList()), totals = RenderRazorViewToString("~/Views/Shared/Invoice/_DocumentTotals.cshtml", docTotals) });
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


        public ActionResult GetDocTotal(long? docId)
        {
            try
            {
                if (docId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDocID;
                    return PartialView("_Result");
                }

                var docTotals = _invoiceApiRepository.GetDocumentTotals(docId).FirstOrDefault();

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Views/Shared/Invoice/_DocumentTotals.cshtml", docTotals) });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
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

        public ActionResult GetItemListByID(int id, string partialCode)
        {
            try
            {
                if (partialCode == "item-name")
                    return PartialView("Invoice/_ItemsNameList", Convert.ToInt32(id));
                else if (partialCode == "item-code")
                    return PartialView("Invoice/_ItemsCodeList", Convert.ToInt32(id));
                return null;
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

        public ActionResult GetItemTax(long? itemId, double? itemNet)
        {
            try
            {
                if (itemId == null || itemNet == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    var jsonResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult;
                }

                var itemTaxResult = _invoiceApiRepository.CalculateItemTax(itemId, itemNet);

                if (itemTaxResult == null || itemTaxResult.CustomeRespons == null)
                {
                    var jsonResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult;
                }

                var jsResult = Json(new { msg = "success", data = itemTaxResult.CustomeRespons.ItemTotalTax });
                jsResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                var jsonResult = Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
        }

        public ActionResult GetItemsTaxList(int id)
        {
            try
            {
                return PartialView("Invoice/_ItemsTaxList", _invoiceApiRepository.GetItemsTax(id));
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

        public ActionResult GetInvoiceLineList(long id)
        {
            try
            {
                return PartialView("~/Views/Shared/Invoice/_InvoiceLinesWithActionsList.cshtml", _invoiceApiRepository.GetLines(Convert.ToInt64(id)).ToList());
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

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Views/Shared/Invoice/_InvoiceLinesWithActionsList.cshtml", _invoiceApiRepository.GetLines(Convert.ToInt64(docId)).ToList()), totals = RenderRazorViewToString("~/Views/Shared/Invoice/_DocumentTotals.cshtml", docTotals) });
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
        public ActionResult UpdateInvoiceLine(InvoiceLineCreateViewModel model)
        {
            try
            {
                long? actionBy = Convert.ToInt64(Session["ID"]);
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
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

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Views/Shared/Invoice/_InvoiceLinesWithActionsList.cshtml", _invoiceApiRepository.GetLines(Convert.ToInt64(model.InvoiceInternalID)).ToList()), totals = RenderRazorViewToString("~/Views/Shared/Invoice/_DocumentTotals.cshtml", docTotals) });
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

        [HttpPost]
        public ActionResult UpdateInvoiceTotals(long? documentId, double? extraDiscount)
        {
            try
            {
                if (documentId == null || extraDiscount == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var updateResult = _invoiceApiRepository.UpdateInvoiceTotalsWithDicount(documentId, extraDiscount);

                if (updateResult == null || updateResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (updateResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = updateResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var documentTotalsResult = _invoiceApiRepository.GetDocumentTotals(documentId).FirstOrDefault();

                if (documentTotalsResult == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Invoice/_DocumentTotals", documentTotalsResult) });
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