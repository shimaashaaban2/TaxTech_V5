using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Areas.Configuration.ViewModels;
using Tax_Tech.Classes;
using Tax_Tech.Filters;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class MOFConfigController : Tax_Tech.Controllers.BaseController
    {
        private readonly MOFConfigRepository _MOFConfigRepository;

        public MOFConfigController()
        {
            if (_MOFConfigRepository==null)
                _MOFConfigRepository = new MOFConfigRepository();

        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            try
            {
                string entityId = Convert.ToString(Session["EntityID"]);

                var MOFConfigReslt = _MOFConfigRepository.GetMOFConfig(entityId);

                return View(MOFConfigReslt);
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

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult GetMOFConfig(string EntityId)
        {
            try
            {
                if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(EntityId))
                {
                    ViewBag.SingleValid = Resources.Resource.PlsSelectEntity;
                    return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });

                }

                MOFConfigViewModel Rec = _MOFConfigRepository.GetMOFConfig(EntityId);
                return Json(new { msg = "result", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/MOFConfig/_MOFConfig.cshtml", Rec )});

             }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult PostMOFData(byte card,long Entityid,string ClientID, string ClientSecret, string DefaultActivityCode, string DocumentVersion, string EnviromentBool)
        {
            try
            {
                //validation
                ViewBag.ScrollTop = "1";
                List <String> errorsListOfStrings =ValidationsHelper.GetValidations().MOFValidation( card,  Entityid,  ClientID,  ClientSecret,  DefaultActivityCode,DocumentVersion,  EnviromentBool);
                if (errorsListOfStrings .Count>0)
                {
                    ViewBag.Validation = errorsListOfStrings;
                    return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }
                if (card==1)
                {
                    string BeforeEnc = string.Format("{0}:{1}", ClientID, ClientSecret);
                    string key = Encryption.GetEncryption().Encrypt(BeforeEnc);

                    MOFConfigUpdateViewModel model = new MOFConfigUpdateViewModel();
                    model.EntityID = Entityid;
                    model.IsEncrypted = true;
                    model.key = key;
 
                    var updateResult = _MOFConfigRepository.UpdateMOFConfigKeys(model);
                    if (updateResult == null || updateResult.CustomeRespons == null)
                    {
                        ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                        return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                    }
                     
                }
                else if (card==2)
                {
                    MOFConfigUpdateViewModel model1 = new MOFConfigUpdateViewModel();
                    model1.configKeyTitle = "DefaultActivityCode";
                    model1.configKeyStringValue = DefaultActivityCode;
                    model1.configKeyBoolValue = null;
                    model1.EntityID = Entityid;
                    model1.IsEncrypted = false;

                    var updateResult1 = _MOFConfigRepository.UpdateMOFConfig(model1);
                    if (updateResult1 == null || updateResult1.CustomeRespons == null)
                    {
                        ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                        return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                    }

                    MOFConfigUpdateViewModel model2 = new MOFConfigUpdateViewModel();
                    model2.configKeyTitle = "DocumentVersion";
                    model2.configKeyStringValue = DocumentVersion;
                    model2.configKeyBoolValue = null;
                    model2.EntityID = Entityid;
                    model2.IsEncrypted = false;

                    var updateResult2 = _MOFConfigRepository.UpdateMOFConfig(model2);

                    if (updateResult2 == null || updateResult2.CustomeRespons == null)
                    {
                        ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                        return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                    }
                }
                else if (card==3)
                {
                    MOFConfigUpdateViewModel model1 = new MOFConfigUpdateViewModel();
                    model1.configKeyTitle = "Enviroment";
                    if (EnviromentBool == "1")
                    {
                        model1.configKeyStringValue = "Production";
                        model1.configKeyBoolValue = true;

                    }
                    else
                    {
                        model1.configKeyStringValue = "Pre-Production";
                        model1.configKeyBoolValue = false;
                    }
                         
                    model1.EntityID = Entityid;
                    model1.IsEncrypted = false;


                    var updateResult1 = _MOFConfigRepository.UpdateMOFConfig(model1);
                    if (updateResult1 == null || updateResult1.CustomeRespons == null)
                    {
                        ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                        return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                    }
                }
                ViewBag.Success = Resources.Resource.DataSavedSuccessfully;
                return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "msg", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
        }

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
    }
}