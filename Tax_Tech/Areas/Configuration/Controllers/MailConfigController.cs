using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Areas.Configuration.ViewModels;
using Tax_Tech.Classes;
using Tax_Tech.Filters;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class MailConfigController : Tax_Tech.Controllers.BaseController
    {
        private readonly MailConfigRepository _mailConfigRepository;

        public MailConfigController()
        {
            _mailConfigRepository = new MailConfigRepository();
        }

        #region Pages
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            try
            {
                string entityId = Convert.ToString(Session["EntityID"]);

                var mailListReslt = _mailConfigRepository.GetMailList(entityId);

                if(mailListReslt == null)
                {
                    return RedirectToAction("error", "ConfigPublic");
                }

                return View(mailListReslt);
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
        #endregion

        #region Actions
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult Edit(MailListViewModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    ViewBag.Validation = ValidationsHelper.GetValidations().ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                if(string.IsNullOrWhiteSpace(model.TLSorSSL))
                {
                    model.TLSFlag = false;
                }
                else
                {
                    model.TLSFlag = methods.Getmethods().IsChecked(model.TLSorSSL);
                }

                model.EntityID = Convert.ToInt64(Session["EntityID"]);
                var updateResult = _mailConfigRepository.UpdateMailList(model);

                if(updateResult == null || updateResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                return Json(new { msg = "success", redirecturl = Url.Action("Index", new { controller = "MailConfig", area = "Configuration" }) });
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
