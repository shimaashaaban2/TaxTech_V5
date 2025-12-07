using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.EReceiptApi;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.ApiModels.ViewModels;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Helpers;
using Tax_Tech.Models;
using Tax_Tech.Repository;
//using Tax_Tech.Repository;
using Tax_Tech.Repository.InvoicingApi;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Controllers
{
    public class PopupController : BaseController
    {
        private readonly InvoiceApiRepository _invoiceApiRepository;
        private readonly BranchesApiRepository _branchesApiRepository;
        private readonly VendorsApiRepository _vendorsApiRepository;
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly ItemsTaxApiRepository _itemsTaxApiRepository;
        private readonly ItemsApiRepository _itemsApiRepository;
        private readonly Areas.Configuration.Repository.AccountsApiRepository _accountsApiRepository;
        private readonly JobQueueApiRepository _jobQueueApiRepository;
        private readonly EReceiptRepository _eReceiptRepo;
        private readonly QRCodeService _qrCodeService;

        public PopupController()
        {
            _invoiceApiRepository = new InvoiceApiRepository();
            _branchesApiRepository = new BranchesApiRepository();
            _vendorsApiRepository = new VendorsApiRepository();
            _lookupApiRepository = new LookupApiRepository();
            _itemsTaxApiRepository = new ItemsTaxApiRepository();
            _itemsApiRepository = new ItemsApiRepository();
            _jobQueueApiRepository = new JobQueueApiRepository();
            _accountsApiRepository = new Areas.Configuration.Repository.AccountsApiRepository();
            _eReceiptRepo = new EReceiptRepository();
            _qrCodeService = new QRCodeService();
        }

        #region Home
        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetJobsTrackerStatistics(long? jobId, byte? logType)
        {
            try
            {
                if (jobId == null || logType == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.JobID = jobId;
                ViewBag.LogType = logType;
                var list = _jobQueueApiRepository.GetJobsTrackerDetails(jobId, logType);
                return PartialView("Home/_JobQueueListByLogTypePopup", list.ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult DocByMOFStatus(long JobID, string status)
        {
            try
            {
                if (JobID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.JobID = JobID;
                var list = _jobQueueApiRepository.DocByMOFStatus(JobID, status);
                return PartialView("Home/_DocByMOFStatusPopup", list.ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }
        #endregion

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetRejectDocumentPopup()
        {
            return PartialView("Invoice/_RejectDocumentPopup");
        }

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(16, ReturnViewType.Model)]
        public ActionResult GetInvoiceFilterPopup()
        {
            try
            {
                ViewBag.ProcessStatusList = _lookupApiRepository.GetProcessStatusList();
                return PartialView("Invoice/_InvoiceFilterPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        //GetDocumentActionLog
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetInvoiceActionLog(long? docID)
        {
            try
            {
                if (docID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDocID;
                    return PartialView("_ErrorOccuredPopup");
                }

                var actionLogList = _invoiceApiRepository.GetDocumentActionLog(docID.Value);

                if (actionLogList == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_ErrorOccuredPopup");
                }

                return PartialView("Invoice/_InvoiceActionLogPopup", actionLogList.ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetInvoiceChangeStatusPopup(long? docID)
        {
            try
            {
                if (docID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDocID;
                    return PartialView("_ErrorOccuredPopup");
                }

                var processStatusList = _lookupApiRepository.GetProcessStatusList();

                if (processStatusList == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.DocID = docID;
                ViewBag.ProcessStatusList = processStatusList;
                return PartialView("Invoice/_InvoiceChangeStatusPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetFilterByInternalIDPopup(string docType)
        {
            try
            {
                ViewBag.DocType = docType;
                return PartialView("Invoice/_InvoiceFilterByInternalID");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        #region Invoice Head
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(10, ReturnViewType.Model)]
        public ActionResult GetCreateInvoiceHeadPopup()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true, entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                return PartialView("Invoice/_InvoiceHeadCreatePopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }
        #endregion

        #region Debit Note
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(10, ReturnViewType.Model)]
        public ActionResult GetCreateDebitHeadPopup()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true, entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                return PartialView("DebitNote/_DebitHeadCreatePopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetCreateDebitLinePopup(long? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true, entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                ViewBag.Curreny = _lookupApiRepository.GetCurrenyList();
                return PartialView("DebitNote/_DebitLineCreatePopup", Convert.ToInt64(id));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetEditDebitLinePopup(long? id, int? itemID)
        {
            try
            {
                if (id == null || itemID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }

                // Get Single Credit Line
                List<InvoiceLineViewModel> docs = _invoiceApiRepository.GetSingleInvoiceLine(id.Value).ToList();

                var docToEdit = docs.FirstOrDefault();
                if (docToEdit == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.DocumentNoteFound;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.ItemTaxes = _invoiceApiRepository.GetItemsTax(itemID.Value);
                ViewBag.TaxesTotal = _invoiceApiRepository.GetTaxTotals(Convert.ToString(id));
                ViewBag.Curreny = _lookupApiRepository.GetCurrenyList();
                return PartialView("DebitNote/_DebitLineEditPopup", docToEdit);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetLinkedInvoicesDebitPopup(string debitNoteID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(debitNoteID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.DebitNoteID = debitNoteID;
                return PartialView("DebitNote/_DebitLinkedInvoicesPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }
        #endregion

        #region Credit Note
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(10, ReturnViewType.Model)]
        public ActionResult GetCreateCreditHeadPopup()
        {
            try
            {
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true, entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                return PartialView("CreditNote/_CreditHeadCreatePopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetCreateCreditLinePopup(long? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true, entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                ViewBag.Curreny = _lookupApiRepository.GetCurrenyList();
                return PartialView("CreditNote/_CreditLineCreatePopup", Convert.ToInt64(id));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetEditCreditLinePopup(long? id, int? itemID)
        {
            try
            {
                if (id == null || itemID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }

                // Get Single Credit Line
                List<InvoiceLineViewModel> docs = _invoiceApiRepository.GetSingleInvoiceLine(id.Value).ToList();

                var docToEdit = docs.FirstOrDefault();
                if (docToEdit == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.DocumentNoteFound;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.ItemTaxes = _invoiceApiRepository.GetItemsTax(itemID.Value);
                ViewBag.TaxesTotal = _invoiceApiRepository.GetTaxTotals(Convert.ToString(id));
                ViewBag.Curreny = _lookupApiRepository.GetCurrenyList();
                return PartialView("CreditNote/_CreditLineEditPopup", docToEdit);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetLinkedInvoicesPopup(string creditNoteID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(creditNoteID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.CreditNoteID = creditNoteID;
                return PartialView("CreditNote/_CreditLinkedInvoicesPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }
        #endregion

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetCreateInvoiceLinePopup(long? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }
                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true, entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                ViewBag.Curreny = _lookupApiRepository.GetCurrenyList();
                return PartialView("Invoice/_InvoiceLineCreatePopup", Convert.ToInt64(id));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        [ChangePasswordFilter(ReturnViewType.Model)]
        public ActionResult GetChangePasswordPopup(long UserID)
        {
            try
            {
                return PartialView("Accounts/_ChangeMyPasswordPopup", Convert.ToInt64(UserID));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetRejectInvoicePopup(long? docID)
        {
            try
            {
                if (docID == null)
                {
                    return PartialView("_ErrorOccuredPopup");
                }
                ViewBag.DocID = docID;
                return PartialView("Invoice/_InvoiceChangeStatusMsgPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetRejectBulkInvoicePopup()
        {
            try
            {
                return PartialView("Invoice/_InvoiceChangeStatusBulkPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetEditInvoiceLinePopup(long? invoiceLineId, int? itemId)
        {
            try
            {
                if (invoiceLineId == null || itemId == null)
                {
                    return PartialView("_ErrorOccuredPopup");
                }

                var invoiceLine = _invoiceApiRepository.GetSingleInvoiceLine(invoiceLineId).FirstOrDefault();

                if (invoiceLine == null)
                {
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.TaxesTotal = _invoiceApiRepository.GetTaxTotals(Convert.ToString(invoiceLineId));
                ViewBag.ItemTaxes = _invoiceApiRepository.GetItemsTax(itemId.Value);
                ViewBag.Curreny = _lookupApiRepository.GetCurrenyList();
                return PartialView("Invoice/_InvoiceLineEditPopup", invoiceLine);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetAddExtraDiscountPopup(long? docID)
        {
            try
            {
                if (docID == null)
                {
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.DocID = docID;
                return PartialView("Invoice/_InvoiceAddExtraDiscountPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }


        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetRecievedDocFilterPopup()
        {
            try
            {
                return PartialView("Invoice/_ReceivedDocsFilterPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetInvoiceTrackSubmissionPopup(long DocID)
        {
            try
            {
                return PartialView("Invoice/_InvoiceTrackSubmissionPopup", SubmissionRepository.Get().GetSubmissionLogByDocID(DocID).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetInvoiceMOFRejectPopup(long? DocID)
        {
            try
            {
                return PartialView("Invoice/_InvoiceMOFRejectPopup", DocID);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(17, ReturnViewType.Model)]
        public ActionResult GetInvoiceInternalRejectPopup(long? DocID)
        {
            try
            {
                return PartialView("Invoice/_InvoiceInternalRejectPopup", DocID);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }
        #region Invoicing API
        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetDocumentStatus(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                {
                    ViewBag.ErrorMsg = $"{Resources.Resource.InvalidUUID}, {Resources.Resource.YouhavetoSubmitDocumentFirst}";
                    return PartialView("_ErrorOccuredPopup");
                }

                // Login as a taxpayer
                string entityId = Convert.ToString(Session["EntityID"]);
                var _authApiRepository = new AuthApiRepository(entityId);
                var _documentsApiRepository = new DocumentsApiRepository(entityId);

                var loginResult = _authApiRepository.LoginAsTaxPayer();

                // get document details
                var documentDetails = _documentsApiRepository.GetDocumentDetails(uuid, loginResult.AccessToken);

                documentDetails.UUID = uuid;
                return PartialView("Invoice/_InvoiceStatusPopup", documentDetails);
            }
            catch (UnauthorizedAccessException ex)
            {
                ViewBag.ErrorMsg = JsonConvert.DeserializeObject<ErrorModel>(ex.Message)?.Error;
                return PartialView("_ErrorOccuredPopup");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMsg = JsonConvert.DeserializeObject<ResultModel>(ex.Message).Error.Message;
                return PartialView("_ErrorOccuredPopup");
            }
            catch (Exception ex)
            {
                var result = JsonConvert.DeserializeObject<ResultModel>(ex.Message);
                ViewBag.ErrorMsg = result?.Error;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetCancelDocReasonPopup(long? invoiceId)
        {
            try
            {
                if (invoiceId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDocID;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.InvoiceID = invoiceId;
                return PartialView("Invoice/_InvoiceCancelReasonPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ViewBag.ErrorMsg = $"{ex.InnerException.Message}";
                }
                else
                {
                    ViewBag.ErrorMsg = $"{ex.Message}";
                }
                return PartialView("_ErrorOccuredPopup");
            }
        }
        #endregion

        #region Vendors
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(25, ReturnViewType.Model)]
        public ActionResult GetVendorsExemptedTaxPopup(int? vendorID)
        {
            try
            {
                if (vendorID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.TaxTypes = _lookupApiRepository.GetTaxTypes().ToList();
                ViewBag.VendorID = vendorID;
                return PartialView("Vendors/_VendorExemptedTaxesPopup");
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

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(23, ReturnViewType.Model)]
        public ActionResult GetCreateVendorPopup()
        {
            try
            {
                ViewBag.VendorTypes = _lookupApiRepository.GetVendorTypes();
                ViewBag.Countries = _lookupApiRepository.GetCountries(true);
                return PartialView("Vendors/_VendorCreatePopup");
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

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(24, ReturnViewType.Model)]
        public ActionResult GetUpdateVendorPopup(int? vendorID)
        {
            try
            {
                if (vendorID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.VendorTypes = _lookupApiRepository.GetVendorTypes().ToList();
                ViewBag.Countries = _lookupApiRepository.GetCountries(true).ToList();
                return PartialView("Vendors/_VendorUpdatePopup", _vendorsApiRepository.GetVendorById(vendorID.Value).FirstOrDefault());
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

        #region Items
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(20, ReturnViewType.Model)]
        public ActionResult GetCreateItemPopup()
        {
            try
            {
                ViewBag.UnitTypes = _lookupApiRepository.GetUnitTypes();
                ViewBag.ItemTypes = StaticLists.GetItemTypes();
                return PartialView("Items/_ItemCreatePopup");
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

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(22, ReturnViewType.Model)]
        public ActionResult GetItemTaxesPopup(int? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.ItemId = itemId;
                ViewBag.Taxes = _lookupApiRepository.GetTaxTypes();
                ViewBag.ItemTaxes = _itemsTaxApiRepository.GetItemsTax(true, itemId.Value);
                return PartialView("Items/_ItemTaxes");
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

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(21, ReturnViewType.Model)]
        public ActionResult GetItemsUpdatePopup(int? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    return PartialView("_ErrorOccuredPopup");
                }

                var item = _itemsApiRepository.GetItemById(itemId.Value).FirstOrDefault();

                ViewBag.UnitTypes = _lookupApiRepository.GetUnitTypes();
                ViewBag.ItemTypes = StaticLists.GetItemTypes();
                return PartialView("Items/_ItemUpdatePopup", item);
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

        //Price after tax Calling Code here
        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetPriceAfterTaxPopup(int? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    return PartialView("_ErrorOccuredPopup");
                }

                ViewBag.itemId = itemId;
                //IEnumerable<PriceDetailsViewModel> List = _itemsTaxApiRepository.GetPriceChangesMaster(itemId);
                IEnumerable<PriceDetailsViewModel> List = _itemsTaxApiRepository.GetPriceChangesMaster(itemId);

                //ViewBag.List = List;

                return PartialView("Items/_ItemPriceAfterTaxPopup", List);
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
        public ActionResult GetGroupItemsPopup(long id)
        {

            try
            {
                ViewBag.ItemId = id;
                IEnumerable<ItemGroupsListModel> itemGroups = _itemsTaxApiRepository.GetItemsGroup();
                IEnumerable<ItemGroupsListByItemID> itemGroupList = _itemsTaxApiRepository.GetItemsGroupList(id);
                ViewBag.itemGroup= itemGroups;
                ViewBag.ItemGroupList = itemGroupList;
                return PartialView("Items/_GroupItemsPopup");
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }


        #endregion

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetCancelInvoicePopup(long JobID, string DocID)
        {
            try
            {
                ViewBag.JobID = JobID;
                ViewBag.DocID = DocID;
                return PartialView("Invoice/_CancellInvoicePopup");
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
        public ActionResult GetCreateEReceiptPopup()
        {
            try
            {

                long entityId = Convert.ToInt64(Session["EntityID"]);
                ViewBag.Branches = _branchesApiRepository.GetBranches(true, entityId);
                ViewBag.Vendors = _vendorsApiRepository.GetVendors(true);
                return PartialView("EReceipt/_CreateEReceiptPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }


        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetReboundPopup()
        {
            try
            {
                return PartialView("Invoice/_GetReboundPopup");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }


        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetReceiptFilterPopup()
        {
            try
            {

                return PartialView("EReceipt/_GetEReceiptFilter");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetEReceiptDetails(long ID)
        {
            try
            {
                var payLoad = _eReceiptRepo.GetPayLoadLog(ID);

                return PartialView("EReceipt/_ReceiptDetails", payLoad);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

      
        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetEReceiptQR(long EReceiptID)
        {
            try
              {
                var model = _eReceiptRepo.EReceiptQRCodePopup(EReceiptID);
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(model.FirstOrDefault().QRCode, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(60);
                var stream = new MemoryStream();
                QrBitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                string Qr = string.Format("data:image/png;base64,{0}", System.Convert.ToBase64String(stream.ToArray()));
                var UUID = model.FirstOrDefault().UUID;
                ViewBag.QrCodeUri = Qr;
                ViewBag.UUID = UUID;

                return PartialView("EReceipt/_QRCodeImage");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }


        [AuthFilter(ReturnViewType.Partial)]
      
        public ActionResult GetReceiptLinesToCancel(long LogID, long? EReceiptID)
        {
            try
            {
                if (EReceiptID == 0)
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    return PartialView("_Result");
                }

                IEnumerable<EReceiptLinesModel> EReceipt = _eReceiptRepo.GetEReceiptLines(EReceiptID.Value);
                ViewBag.LogID = LogID;

                return PartialView("EReceipt/_EReceiptLinesListToCancel", EReceipt.ToList());

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
        public ActionResult EReceiptNumberDetails()
        {
            try
            {
                return PartialView("EReceipt/_EReceiptNumberDetails");
            }
           
             catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_ErrorOccuredPopup");
            }
        }

        public ActionResult JobStop()
        {
            return PartialView("JobStatus/_JobStop");
        }
        public ActionResult JobResume()
        {
            return PartialView("JobStatus/_JobResume");
        }
        public ActionResult JobPriority()
        {
            return PartialView("JobStatus/_JobPriority");
        }
        public ActionResult JobResubmit()
        {
            return PartialView("JobStatus/_JobResubmit");
        }
    }
}

