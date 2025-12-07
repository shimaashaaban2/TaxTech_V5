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
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class BanksController : Tax_Tech.Controllers.BaseController
    {
        private readonly BankLookupApiRepository _bankLookupApiRepository;
        private readonly ValidationsHelper _validationsHelper;

        public BanksController()
        {
            _bankLookupApiRepository = new BankLookupApiRepository();
            _validationsHelper = ValidationsHelper.GetValidations();
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
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
                return RedirectToAction("error", "ConfigPublic");
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult BankList()
        {
            try
            {
                return PartialView("~/Areas/Configuration/Views/Shared/Banks/_BankList.cshtml", _bankLookupApiRepository.GetBanks(false).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }

        #region Actions
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult InsertBank(BankViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                if(!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var insertResult = _bankLookupApiRepository.InsertBank(model.BankName, actionBy);

                if(insertResult == null || insertResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                if(insertResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = insertResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var banksList = _bankLookupApiRepository.GetBanks(false).ToList();

                JsonResult jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Banks/_BankList.cshtml", banksList) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UpdateBank(BankViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                if(!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                if(model.BankID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var updateResult = _bankLookupApiRepository.UpdateBank(model.BankName, model.BankID.Value, actionBy);

                if(updateResult == null || updateResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                if(updateResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = updateResult.CustomeRespons.ResponseMsg;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var banksList = _bankLookupApiRepository.GetBanks(false).ToList();

                JsonResult jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Banks/_BankList.cshtml", banksList) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }

        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult ChangeBankStatus(bool? status, long? bankId)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                if (status == null || bankId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var changeStatusResult = _bankLookupApiRepository.SetBankActive(status.Value, bankId.Value, actionBy);

                if(changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                if(changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = changeStatusResult.CustomeRespons.ResponseMsg;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var banksList = _bankLookupApiRepository.GetBanks(false).ToList();

                JsonResult jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Banks/_BankList.cshtml", banksList) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
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
