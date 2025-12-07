using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Media3D;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.EReceiptApi;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Helpers;
using Tax_Tech.Models;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Controllers
{
   
    public class EReceiptController : BaseController
    {
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly VendorsApiRepository _vendorsApiRepository;
        private readonly EReceiptRepository _eReceiptRepo;
        private readonly BranchesApiRepository _branchesApiRepository;
        private readonly AccountLookupApiRepository _accountLookupApiRepository;
        private readonly Areas.Configuration.Helpers.ValidationsHelper _validationsHelper;
        private readonly Logger _logger;
        private readonly QRCodeService _qrCodeService;
        public EReceiptController()
        {
            _lookupApiRepository = new LookupApiRepository();
            _logger = new Logger();
            _accountLookupApiRepository = new AccountLookupApiRepository();
            _branchesApiRepository = new BranchesApiRepository();
            _validationsHelper = Areas.Configuration.Helpers.ValidationsHelper.GetValidations();
            _vendorsApiRepository = new VendorsApiRepository();
            _eReceiptRepo = new EReceiptRepository();
            _qrCodeService = new QRCodeService();
        }

        #region Pages

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult Index() 
        {
            return View();
        }

        #region Receipt List

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult AllReceipts() 
        {
            try
            {
                IEnumerable<EReceiptSourceModel> eReceiptSources = _lookupApiRepository.GetEReceiptSource();
                IEnumerable<EReceiptStatusModel> eReceipStatus = _lookupApiRepository.GetEReceiptStatus();
                EReceiptStatusListsModel eReceiptStatusLists = new EReceiptStatusListsModel();
                eReceiptStatusLists.eReceiptSources = eReceiptSources;
                eReceiptStatusLists.eReceiptStatuses = eReceipStatus;
                return View(eReceiptStatusLists);
            }
            catch (Exception ex)
            {
                string ErrorMsg = "";
                if (ex.InnerException != null)
                {
                    ErrorMsg = Convert.ToString(ex.InnerException.Message);
                }
                else
                {
                    ErrorMsg = Convert.ToString(ex.Message);
                }
                TempData["ErrorMsg"] = "ERROR: " + ErrorMsg;
                return RedirectToAction("error", "Public");
            }
        }
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetReceiptByNumber(string ReceiptNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ReceiptNumber))
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                IEnumerable<ReceiptListModel> ReceiptList = _eReceiptRepo.GetReceiptListByNumber(ReceiptNumber);
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_ReceiptList", ReceiptList) });
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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult ReceiptListByUUID(string ReceiptUUID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ReceiptUUID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptUUIDCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                IEnumerable<ReceiptListModel> ReceiptList = _eReceiptRepo.GetReceiptListByUUID(ReceiptUUID);
                var receiptType = ReceiptList.FirstOrDefault().receiptType;
               
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_ReceiptList", ReceiptList) });
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

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult FullReceiptsList(FullReceiptListModel model)
        {
            try
            {
                if (model.EReceiptStatus == null)
                {
                    model.EReceiptStatus = "NA";
                }
                model.To = model.From;
                IEnumerable<ReceiptListModel> ReceiptLogList = _eReceiptRepo.GetFullReceiptList(model);
              
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_ReceiptList", ReceiptLogList) });
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


        //[AuthFilter(ReturnViewType.Json)]

        //public ActionResult BillingTrackingData(string eReceiptnumber)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(eReceiptnumber))
        //        {
        //            ViewBag.ErrorMsg = Resources.Resource.EReceiptUUIDCannotBeEmpty;
        //            return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
        //        }
        //        IEnumerable<BillingTrackingModel> BillingTracking = _eReceiptRepo.BillingTracking(eReceiptnumber);
        //        var jsonResult = Json(new { msg="success",view= RenderRazorViewToString("EReceipt/_BillingTracking", BillingTracking) });
        //        jsonResult.MaxJsonLength = int.MaxValue;
        //        return jsonResult;
        //    }
        //    catch (Exception ex) 
        //    {
        //        if (ex.InnerException != null)
        //            ViewBag.ErrorMsg = ex.InnerException.Message;
        //        else
        //            ViewBag.ErrorMsg = ex.Message;

        //        return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
        //    }

        //}

        #endregion

        #region Submission Log
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult SubmissionLog()
        {
            try
            {

               IEnumerable<EReceiptSourceModel> eReceiptSources = _lookupApiRepository.GetEReceiptSource();
               IEnumerable<EReceiptStatusModel> eReceipStatus = _lookupApiRepository.GetEReceiptStatus();
               IEnumerable<SubmitStatusModel> submitStatuses = _lookupApiRepository.GetSubmitStatus();
               
                if (eReceiptSources==null|| eReceipStatus ==null|| submitStatuses==null)
                {
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                EReceiptStatusListsModel eReceiptStatusLists = new EReceiptStatusListsModel();
                eReceiptStatusLists.eReceiptSources = eReceiptSources;
                eReceiptStatusLists.eReceiptStatuses = eReceipStatus;
                eReceiptStatusLists.submitStatuses = submitStatuses;
                  return View(eReceiptStatusLists);

            }
            catch(Exception ex)
            {

                string ErrorMsg = "";
                if (ex.InnerException != null)
                {
                    ErrorMsg = Convert.ToString(ex.InnerException.Message);
                   
                }
                else
                {
                    ErrorMsg = Convert.ToString(ex.Message);
                    
                }
                TempData["ErrorMsg"] = "ERROR: " + ErrorMsg;
                return RedirectToAction("error", "Public");
            }
           
        }


        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult EReceiptFullLogs(EReceiptFullLogsModel model)
        {
            try
            {
                if (model.EReceiptStatus == null)
                {
                    model.EReceiptStatus = "NA";
                }
                //var LinesTotals = _eReceiptRepo.ReceiptLinesTotals(EReceiptID.Value);
                IEnumerable<EReceiptLogsModel> EReceiptLogList = _eReceiptRepo.GetEReceiptfullLogs(model);
               
               
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_EReceiptLogList", EReceiptLogList) });
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
        //public ActionResult EReceiptLogByDate(DateTime? Datefrom ,DateTime? Dateto , long? status=1)
        //{
        //    try
        //    {
        //        IEnumerable<EReceiptLogsModel> EReceiptLogList =_eReceiptRepo.GetEReceiptByDate(Datefrom, Dateto, status);

        //        /*var*//* jsonResult =*/ 
        //        //jsonResult.MaxJsonLength = int.MaxValue;
        //        return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_EReceiptLogList", EReceiptLogList) });




        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //            ViewBag.ErrorMsg = ex.InnerException.Message;
        //        else
        //            ViewBag.ErrorMsg = ex.Message;

        //        return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
        //    }

        //}
       
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult EReceiptLogByReceiptNumber(string EReceiptNumber)
        {
            try
            {
                

                if (string.IsNullOrWhiteSpace(EReceiptNumber))
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                IEnumerable<EReceiptLogsModel> EReceiptLogList = _eReceiptRepo.GetEReceiptLogByReceiptNumber(EReceiptNumber);
               
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_EReceiptLogList", EReceiptLogList) });
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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult EReceiptLogByUUID(string EReceiptUUID)
        {
            try
            {
                ViewBag.EReceiptNumber = "";


                if (string.IsNullOrWhiteSpace(EReceiptUUID))
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptUUIDCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                IEnumerable<EReceiptLogsModel> EReceiptLogList = _eReceiptRepo.GetEReceiptByUUID(EReceiptUUID);
               
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_EReceiptLogList", EReceiptLogList) });
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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetPayLoadLog(long LogID)
        {
            return View();
        }
        #endregion

        #region EReceiptTracking

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        [HttpGet]
        public ActionResult EReceiptTracking(string eReceiptnumber)
        {
            ViewBag.eReceiptnumber = eReceiptnumber;
            return View();
        }
        //[AuthFilter(ReturnViewType.Json)]
        //[HttpPost]
        //public ActionResult EReceiptTracking(string eReceiptnumber)
        //{
        //    if (string.IsNullOrEmpty(eReceiptnumber))
        //    {
        //        ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
        //        return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
        //    }

        //    IEnumerable<EReceiptTrackingModel> eReceiptTracking = _eReceiptRepo.EReceiptTracking(eReceiptnumber);
        //    IEnumerable<BillingTrackingModel> BillingTracking = _eReceiptRepo.BillingTracking(eReceiptnumber);
        //    EReceiptTrackingViewModel eReceiptTrackingViewModel = new EReceiptTrackingViewModel();
        //    eReceiptTrackingViewModel.eReceiptTrackings = eReceiptTracking;
        //    eReceiptTrackingViewModel.billingTrackings = BillingTracking;
        //    ViewBag.BillingTrack = BillingTracking;
        //    var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_EReceiptTrackingData", eReceiptTrackingViewModel) });
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}


        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult EReceiptTrackingData(string eReceiptnumber)
        {
            try
            {
                if (string.IsNullOrEmpty(eReceiptnumber))
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                IEnumerable<EReceiptTrackingModel> eReceiptTracking = _eReceiptRepo.EReceiptTracking(eReceiptnumber);
                IEnumerable<BillingTrackingModel> BillingTracking = _eReceiptRepo.BillingTracking(eReceiptnumber);
                EReceiptTrackingViewModel eReceiptTrackingViewModel = new EReceiptTrackingViewModel();
                eReceiptTrackingViewModel.eReceiptTrackings = eReceiptTracking;
                eReceiptTrackingViewModel.billingTrackings = BillingTracking;
                ViewBag.BillingTrack = BillingTracking;
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_EReceiptTrackingData", eReceiptTrackingViewModel) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
                // return View(eReceiptTrackingViewModel);

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
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult EReceiptDetailsTrack()
        {
            return View();
        }
        #endregion

        #region Billing
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult BillingLogs()
        {
            return View();
        }
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult BillingLogsByDate(DateTime?datefrom ,DateTime?dateto)
        {
            try
            {
                IEnumerable<BillingLogsModel> billingLogs = _eReceiptRepo.GetBillingLogs(datefrom, dateto);
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_BillingLogList", billingLogs) });
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
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult BillingValidationLog()
        {
            return View();
        }

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult BillingValidationList(string EReceiptNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EReceiptNumber))
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                IEnumerable<BillingValidationLogModel> billingValidation = _eReceiptRepo.billingValidation(EReceiptNumber);
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_BillingValidationList", billingValidation) });
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

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult BillingValidationByDate(string From, string To)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(From) || string.IsNullOrWhiteSpace(To))
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                IEnumerable<BillingValidationLogModel> billingDateLogs = _eReceiptRepo.billingValidationDateRange(From, To);
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_BillingValidationList", billingDateLogs) });
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

        #region Wincash log
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult WinCashLog()
        {
            return View();
        }
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult WinCashLogByDate(DateTime? dateFrom, DateTime? dateTo)
        {
            try
            {
                IEnumerable<WinCashLogsModel> WincashLogs = _eReceiptRepo.GetWinCashLogs(dateFrom, dateTo);
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_WinCashLogList", WincashLogs) });
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

        #region Cancel Receipt
        [HttpGet]
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult CancelReceipt()
        {
            return View();
        }

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetCancelbyDate(DateTime From, DateTime To)
        {
            try
            {
               
               
                IEnumerable<EReceiptLogsModel> CancelList = _eReceiptRepo.GetCancelReceiptList(From,To);
                if (CancelList == null)
                {
                    ViewBag.ErrorMsg = Tax_Tech.Resources.Resource.ApiDown;

                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
               // return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_CancelReceiptList", CancelList) });
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_CancelReceiptList", CancelList) });
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

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetCancelbyDateWithPagination(DateTime From, int pageNo = 1, int pageSize = 100)
        {
            try
            {
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;

                IEnumerable<EReceiptLogsModel> CancelList = _eReceiptRepo.GetCancelReceiptListWithPagination(From, From, pageNo, pageSize);
                if (CancelList == null)
                {
                    ViewBag.ErrorMsg = Tax_Tech.Resources.Resource.ApiDown;

                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                // return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_CancelReceiptList", CancelList) });
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_CancelReceiptList", CancelList) });
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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult CancelbyReceiptNumber(string EReceiptNumber)
        {
            try
            {
                
               
                IEnumerable<EReceiptLogsModel> CancelList = _eReceiptRepo.CancelByReceiptNumber(EReceiptNumber);

                return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_CancelReceiptList", CancelList) });
                
               
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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult CancelbyReceiptUUID(string EReceiptUUID)
        {
            try
            {
                
               
                IEnumerable<EReceiptLogsModel> CancelList = _eReceiptRepo.CancelByReceiptUUID(EReceiptUUID);

                return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_CancelReceiptList", CancelList) });
                
               
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


        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult RefundedReceipt(long LogID) 
        {
            try
            {
                var RefundedReceipt = _eReceiptRepo.RefundedReceipt(LogID);
                if (RefundedReceipt.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = Tax_Tech.Resources.Resource.Fail;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                else if(RefundedReceipt.CustomeRespons.ResponseID == "1")
                {
                    ViewBag.Success = Tax_Tech.Resources.Resource.Refunded;
                    return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });
                }
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
            
            catch(Exception ex) 
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;

                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
           
        }
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult ReSubmitReceipt(long LogID) 
        {
            try
            {
                var RefundedReceipt = _eReceiptRepo.ReSubmitReceipt(LogID);
                if (RefundedReceipt.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = Tax_Tech.Resources.Resource.Fail;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                else if(RefundedReceipt.CustomeRespons.ResponseID == "1")
                {
                    ViewBag.Success = Tax_Tech.Resources.Resource.Resubmitted;
                    return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });
                }
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
            
            catch(Exception ex) 
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;

                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
           
        }
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult ResetResult()
        {
            try
            {
                var RefundedReceipt = _eReceiptRepo.ResetResult();
                if (RefundedReceipt.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = Tax_Tech.Resources.Resource.ResetWatingResultFail;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                else if (RefundedReceipt.CustomeRespons.ResponseID == "1")
                {
                    ViewBag.Success = Tax_Tech.Resources.Resource.ResetWatingResultSuccess;
                    return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });
                }
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
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
        [HttpGet]
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult RefundReceiptFilter()
        {
            return View();
        }

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetRefundedbyDate(DateTime From, DateTime To)
        {
            try
            {


                IEnumerable<EReceiptLogsModel> RefundedList = _eReceiptRepo.RefundedReceiptByRange(From, To);
                if (RefundedList == null)
                {
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_RefundedReceiptList", RefundedList) });


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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GetRefundedbyDateWithPagination(DateTime From,  int pageNo = 1, int pageSize = 100)
        {
            try
            {
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;

                IEnumerable<EReceiptLogsModel> RefundedList = _eReceiptRepo.GetRefundedReceiptListWithPagination(From, From, pageNo, pageSize);
                if (RefundedList == null)
                {
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_RefundedReceiptList", RefundedList) });


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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult RefundedbyReceiptNumber(string EReceiptNumber)
        {
            try
            {


                IEnumerable<EReceiptLogsModel> RefundedList = _eReceiptRepo.RefundedReceiptByReceiptNumber(EReceiptNumber);

                return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_RefundedReceiptList", RefundedList) });


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
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult RefundedbyReceiptUUID(string EReceiptUUID)
        {
            try
            {


                IEnumerable<EReceiptLogsModel> RefundedList = _eReceiptRepo.RefundedReceiptByReceiptUUID(EReceiptUUID);

                return Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_RefundedReceiptList", RefundedList) });


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
        #region Report
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public ActionResult TotalReport()
        {
            return View();
        }
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult FilteredTotalReports(DateTime From, DateTime To)
        {
            try
            {
                IEnumerable<TotalReportsModel> TotalReportsList = _eReceiptRepo.TotalReports(From, To);
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("EReceipt/_TotalReportsList", TotalReportsList) });
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
        [HttpGet]
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(200, ReturnViewType.Normal)]
        public async Task<ActionResult> ExportTotalReports(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "ReportsTotals" + StringDate + ".xlsx";
                XLWorkbook wb = null;
                wb = ExportHelper.ExportTotalReports(_eReceiptRepo.TotalReports(fromDate.Value, toDate.Value), $"TotalReports-{DateTime.Now}");
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
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(17, ReturnViewType.Normal)]
        [HttpGet]
        public ActionResult ReceiptPreview(long? EReceiptID)
        {
            try
            {
                if (EReceiptID == 0)
                {
                   // ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    string ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    TempData["ErrorMsg"] = "ERROR: " + ErrorMsg;
                    return RedirectToAction("error", "Public");
                }
               
                //var EReceipt = _eReceiptRepo.GetEReceiptLines(EReceiptID.Value);
                var SingleEReceipt = _eReceiptRepo.GetEReceiptSingleView(EReceiptID.Value).FirstOrDefault();
               var LinesTotals = _eReceiptRepo.ReceiptLinesTotals(EReceiptID.Value);

                if (SingleEReceipt != null)
                {
                    ViewBag.linesTotals = LinesTotals;
                    return View(SingleEReceipt);
                }
                TempData["ErrorMsg"] = "Document Not Found";
                return RedirectToAction("error", "Public");
            }
            // return View("ReceiptPreview",EReceipt);
            catch(Exception ex) 
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
        [HttpGet]
        public ActionResult GetReceiptLines(long? EReceiptID) 
        {
            try
            {
                if (EReceiptID == 0)
                {
                    ViewBag.ErrorMsg = Resources.Resource.EReceiptNumberCannotBeEmpty;
                    return PartialView("_Result");
                }

                IEnumerable<EReceiptLinesModel> EReceipt = _eReceiptRepo.GetEReceiptLines(EReceiptID.Value);
                

               return PartialView("EReceipt/_EReceiptLinesList", EReceipt.ToList());
             
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
        [HttpPost]
        public ActionResult PartialReturn(long LogID,FormCollection frm) 
        {
            try
            {
                // Initialize lists to store quantities
              List<PartialReturnViewModel>   PartialReturnList =new List<PartialReturnViewModel>();

                // Loop through the form collection
                foreach (var key in frm.AllKeys)
                {
                    
                    if (key.StartsWith("txt-quentity-"))
                    {
                        if (Convert.ToInt16(frm[key])>0)
                        {
                             PartialReturnList.Add(new PartialReturnViewModel (){ LineID = Convert.ToInt64(key.Replace("txt-quentity-", "")), Quentity = Convert.ToInt16(frm[key]) });
                         }
                    }
                }

                if (PartialReturnList.Count()>0)
                {
                    var RefundedReceipt = _eReceiptRepo.ParialRefundedReceipt(LogID, PartialReturnList);

                    if (RefundedReceipt.CustomeRespons.ResponseID != "1")
                    {
                        ViewBag.ErrorMsg = Tax_Tech.Resources.Resource.Fail;
                        return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                    }
                    else if (RefundedReceipt.CustomeRespons.ResponseID == "1")
                    {
                        ViewBag.Success = Tax_Tech.Resources.Resource.Refunded;
                        return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });
                    }
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                ViewBag.ErrorMsg = "No Lines To Refund";
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });


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

        #region ExportToExcel
        public ActionResult ExportAllReceiptsToExecl(DateTime From,bool EReceiptSourceId=false,bool EReceiptStatusId=false,byte EReceiptSource=0,string EReceiptStatus="")
        {
            
            try
            {
                
                if (EReceiptStatus == "")
                {
                    EReceiptStatus = "NA";
                }
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "EReceipts" + StringDate + ".xlsx";

                XLWorkbook wb = null;
                IEnumerable<ReceiptListModel> AllReceipts = _eReceiptRepo.GetFullReceiptList(
                    new FullReceiptListModel
                    {
                        From = From,
                         To =From,
                        EReceiptSource = EReceiptSource,
                        EReceiptStatus = EReceiptStatus
                    });
                

                wb = ExportHelper.ExportAllReceipts(AllReceipts, $"AllReceipts-{DateTime.Now}");

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

        public ActionResult ExportReceiptsNumberToExecl(string ReceiptNumber)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "EReceipts" + StringDate + ".xlsx";

                XLWorkbook wb = null;
                IEnumerable<ReceiptListModel> ReceiptList = _eReceiptRepo.GetReceiptListByNumber(ReceiptNumber);
                wb = ExportHelper.ExportAllReceipts(ReceiptList, $"ReceiptNumber-{DateTime.Now}");

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
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("error", "Public");
            }
        }
        public ActionResult ExportReceiptsUUIDToExecl(string ReceiptUUID)
        {

            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "EReceipts" + StringDate + ".xlsx";

                XLWorkbook wb = null;
                IEnumerable<ReceiptListModel> ReceiptList = _eReceiptRepo.GetReceiptListByUUID(ReceiptUUID);
                wb = ExportHelper.ExportAllReceipts(ReceiptList, $"ReceiptUUID-{DateTime.Now}");

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
        public ActionResult ExportLogReceiptsNumberToExecl(string EReceiptNumber)
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "EReceipts" + StringDate + ".xlsx";

                XLWorkbook wb = null;
                IEnumerable<EReceiptLogsModel> ReceiptList = _eReceiptRepo.GetEReceiptLogByReceiptNumber(EReceiptNumber);
                wb = ExportHelper.ExportFullReceipts(ReceiptList, $"ReceiptNumber-{DateTime.Now}");

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

        public ActionResult ExportLogReceiptsUUIDToExecl(string EReceiptUUID)
        {

            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "EReceipts" + StringDate + ".xlsx";

                XLWorkbook wb = null;
                IEnumerable<EReceiptLogsModel> ReceiptList = _eReceiptRepo.GetEReceiptByUUID(EReceiptUUID);
                wb = ExportHelper.ExportFullReceipts(ReceiptList, $"ReceiptUUID-{DateTime.Now}");

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

        public ActionResult ExportLogReceiptsToExecl(DateTime From, DateTime To, bool EReceiptSourceId = false, bool EReceiptStatusId = false, byte EReceiptSource = 0, string EReceiptStatus = "", bool SubmitStatusId = false, byte SubmitStatus = 0)
        {

            try
            {

                if (EReceiptStatus == "")
                {
                    EReceiptStatus = "NA";
                }
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "EReceipts" + StringDate + ".xlsx";

                XLWorkbook wb = null;
                IEnumerable<EReceiptLogsModel> fullReceipts = _eReceiptRepo.GetEReceiptfullLogs(
                    new EReceiptFullLogsModel
                    {
                        From = From,
                        To = To,
                        EReceiptSource = EReceiptSource,
                        EReceiptStatus = EReceiptStatus,
                        SubmitStatus = SubmitStatus,
                    });


                wb = ExportHelper.ExportFullReceipts(fullReceipts, $"fullReceipts-{DateTime.Now}");

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
        public ActionResult ExportNotExsitItemsToExcel()
        {

            try
            {


                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "NotExsitItems" + StringDate + ".xlsx";

                XLWorkbook wb = null;


                ExportItems exportItems = _eReceiptRepo.ExportNotExsitItemsToExcel();
                IEnumerable<AllItemsNotExist> items = exportItems.AllItemsNotExist;

                wb = ExportHelper.ExportItems(items, $"ReceiptItems-{DateTime.Now}");

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


        #region Vendors List
        //[AuthFilter(ReturnViewType.Json)]
        //[HttpPost]
        //public async Task<ActionResult> GetVendorsList(string VendorName)
        //{
        //    var Vendors = _vendorsApiRepository.GetVendors(true);
        //    var VendorsName = Vendors.Select(a => a.VendorName);//get only list of names
        //    //TempData["VendorsId"] = Convert.ToString(Vendors.Where(a => a.VendorName == VendorName).Select(i => i.VendorID).FirstOrDefault());
        //    //ViewBag.VendorsId = Vendors.Where(a => a.VendorName == VendorName).Select(i => i.VendorID).FirstOrDefault();
        //    var Namesres = VendorName.ToLower();
        //    var NameSelect = VendorsName.Where(suggestion => suggestion.ToLower().Contains(Namesres)).ToList();
        //    var vendorsJson = JsonConvert.SerializeObject(NameSelect);
        //    return Json(vendorsJson);//send list here
        //}
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
