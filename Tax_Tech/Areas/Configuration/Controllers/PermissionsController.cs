using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.Filters;
using Tax_Tech.Areas.Configuration.Repository;
using System.IO;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class PermissionsController : Tax_Tech.Controllers.BaseController
    {
        private readonly PermissionRepository _permissionRepository;
        private readonly LookupApiRepository _lookupApiRepository;

        public PermissionsController()
        {
            _permissionRepository = new PermissionRepository();
            _lookupApiRepository = new LookupApiRepository();
        }

        #region Pages
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            try
            {
                ViewBag.Users = _lookupApiRepository.GetUsers();
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Error", "ConfigPublic", new { area = "Configuration" });
            }
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetPermissionsOfUser(long? userId)
        {
            try
            {
                if(userId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.SelectUser;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var list = _permissionRepository.GetUserPermissions(userId);
                return PartialView("~/Areas/Configuration/Views/Shared/Permissions/_PermissionsList.cshtml", list.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }
        #endregion

        #region Actions
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult AddActionToUser(long? actionId, long? userId)
        {
            try
            {
                if(actionId == null || userId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.SelectUser;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var addActionToUserResult = _permissionRepository.AddActionToUser(actionId, userId);

                if(addActionToUserResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = addActionToUserResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                ViewBag.Success = Resources.Resource.PermissionhasbeenAddedSuccessfully;
                return Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult RemoveActionFromUser(long? actionId, long? userId)
        {
            try
            {
                if (actionId == null || userId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.SelectUser;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var removeActionFromUserResult = _permissionRepository.RemoveActionFromUser(actionId, userId);

                if(removeActionFromUserResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = removeActionFromUserResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                ViewBag.Success = Resources.Resource.PermissionhasbeenRemovedSuccessfully;
                return Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
            catch (Exception ex)
            {
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
