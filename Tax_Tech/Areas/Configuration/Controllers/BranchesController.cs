using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class BranchesController : Tax_Tech.Controllers.BaseController
    {
        private readonly BranchesApiRepository _branchesApiRepository;
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly ValidationsHelper _validationsHelper;

        public BranchesController()
        {
            _branchesApiRepository = new BranchesApiRepository();
            _lookupApiRepository = new LookupApiRepository();
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
        public ActionResult List()
        {
            try
            {
                return PartialView("~/Areas/Configuration/Views/Shared/Branches/_BranchesList.cshtml", _branchesApiRepository.GetBranches(false,0).ToList());
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
        public ActionResult CreateBranch(BranchViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                long entityID = Convert.ToInt32(Session["EntityID"]);
                model.ActionBy = actionBy;
               // model.EntityID = entityID;
                if(!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }
                List<string> ValidateVendorList = _validationsHelper.ValidateBranch(model);
                if (ValidateVendorList.Count() > 0)
                {
                    ViewBag.Validation = ValidateVendorList;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });

                }

                var insertResult = _branchesApiRepository.InsertBranch(model);

                if(insertResult == null || insertResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                if (insertResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = insertResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var jsonResult = Json( new {msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Branches/_BranchesList.cshtml", _branchesApiRepository.GetBranches(false,0).ToList()) });
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
        public ActionResult ChangeStatus(long? branchId, bool? status)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                
                if (branchId == null || status == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var changeStatusResult = _branchesApiRepository.ChangeBranchStatus(status.Value, branchId.Value, actionBy);

                if(changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = changeStatusResult.CustomeRespons.ResponseMsg;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var jsonResult = Json( new {msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Branches/_BranchesList.cshtml", _branchesApiRepository.GetBranches(false,0).ToList()) });
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

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UpdateBranch(BranchViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                long entityID = Convert.ToInt32(Session["EntityID"]);
                model.ActionBy = actionBy;
                //model.EntityID = entityID;
                if(!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new {msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var updateResult = _branchesApiRepository.UpdateBranch(model);

                if(updateResult == null || updateResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new {msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                if (updateResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = updateResult.CustomeRespons.ResponseMsg;
                    return Json(new {msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var jsonResult = Json( new {msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Branches/_BranchesList.cshtml", _branchesApiRepository.GetBranches(false,0).ToList()) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return Json(new {msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult ExportToExecl()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Branches" + StringDate + ".xlsx";
                XLWorkbook wb = ExportHelper.ExportBranches(_branchesApiRepository.GetBranches(false,0).ToList(), $"BranchesList-{DateTime.Now}");

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