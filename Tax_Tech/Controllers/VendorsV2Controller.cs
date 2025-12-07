using ClosedXML.Excel;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Exceptions;
using Tax_Tech.Filters;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Controllers
{
    public class VendorsV2Controller : BaseController
    {
       
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly ValidationsHelper _validationsHelper;
        private readonly VendorsApiRepositoryV2 _vendorsApiRepositoryV2;
        private readonly VendorsApiRepository _vendorsApiRepository;
       
       

        public VendorsV2Controller()
        {
            _lookupApiRepository = new LookupApiRepository();
            _vendorsApiRepositoryV2 = new VendorsApiRepositoryV2();
            _vendorsApiRepository = new VendorsApiRepository();
            _validationsHelper = ValidationsHelper.GetValidations();
        }




        //[AuthFilter(ReturnViewType.Json)]
        //[PermissionFilter(23, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UpdateVendorV2(VendorViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                int entityID = Convert.ToInt32(Session["EntityID"]);
                model.ActionBy = actionBy;
                model.EntityID = entityID;
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (model.VendorID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var errorsList = ValidationsHelper.GetValidations().ValidateVendorForUpdate(model);
                if (errorsList.Count() > 0)
                {
                    ViewBag.Validation = errorsList;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var checkVendorERPCodeExistResult = _vendorsApiRepository.CheckVendorErpCodeExistForUpdate(model.EntityID, model.ERPInternalID, model.VendorID);
                if (checkVendorERPCodeExistResult == true)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ERPCodeIsAlreadyTakenforthisEntity;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var updateResult = _vendorsApiRepositoryV2.UpdateVendorV2(model);

                if (updateResult == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                //if (updateResult.CustomeRespons.ResponseID != "1")
                //{
                //    ViewBag.ErrorMsg = updateResult.CustomeRespons.ResponseMsg;
                //    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                //}

                //if (updateResult != "1")
                //{
                //    ViewBag.Success = updateResult;
                //    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                //}
                ViewBag.Success = updateResult;
                return Json(new
                {
                    success = true,
                    data = updateResult,
                    view = RenderRazorViewToString("_Result", updateResult)
                });
                // return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });

                //var jsonResult = Json( new {msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Vendors/_VendorsList.cshtml", _vendorsApiRepository.GetVendors(false).ToList()) });
                //jsonResult.MaxJsonLength = int.MaxValue;
                //return jsonResult;
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
        public ActionResult CreateVendorV2(VendorViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                int entityID = Convert.ToInt32(Session["EntityID"]);
                model.ActionBy = actionBy;
                model.EntityID = entityID;
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                List<string> ValidateVendorList = _validationsHelper.ValidateVendor(model);
                if (ValidateVendorList.Count() > 0)
                {
                    ViewBag.Validation = ValidateVendorList;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var createResult = _vendorsApiRepositoryV2.InsertVendorV2(model);

                if (createResult == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                //if (createResult != "1")
                //{
                //    ViewBag.Success = createResult;
                //    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                //}
                ViewBag.Success = createResult;
                return Json(new
                {
                    msg="success",
                    data = createResult, view = RenderRazorViewToString("_Result",createResult) });

                //var jsonResult = Json( new {msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Vendors/_VendorsList.cshtml", _vendorsApiRepository.GetVendors(false).ToList()) });
                //jsonResult.MaxJsonLength = int.MaxValue;
                //return jsonResult;
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