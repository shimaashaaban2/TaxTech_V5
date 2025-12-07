using ClosedXML.Excel;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Exceptions;
using Tax_Tech.Filters;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Controllers
{
    public class VendorsController : BaseController
    {
        private readonly VendorsApiRepository _vendorsApiRepository;
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly ValidationsHelper _validationsHelper;

        public VendorsController()
        {
            _lookupApiRepository = new LookupApiRepository();
            _vendorsApiRepository = new VendorsApiRepository();
            _validationsHelper = ValidationsHelper.GetValidations();
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index(string filter = "", int? pageNo = 1, int? pageSize = 100)
        {
            ViewBag.Filter = filter;
            ViewBag.PageNo = pageNo;
            ViewBag.PageSize = pageSize;
            return View();
        }

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult List()
        {
            try
            {
                return PartialView("Vendors/_VendorsList", _vendorsApiRepository.GetVendors(false).ToList());
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
        public ActionResult CountOfAll()
        {
            try
            {
                int? entityId = Convert.ToInt32(Session["EntityID"]);
                var Result = _vendorsApiRepository.GetVendorsCountByEntityId(entityId);

                if (Result == null || Result.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("Vendors/_VendorsCount", Result.CustomeRespons.CountOfAll);
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
        public ActionResult ListByFilter(string filter, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                if(filter.Length < 3)
                {
                    ViewBag.ErrorMsg = Resources.Resource.PleaseEnterAtLeast3Chars;
                    return PartialView("_Result");
                }

                int? entityId = Convert.ToInt32(Session["EntityID"]);

                var list = _vendorsApiRepository.GetVendorsByFilterAndEntityId(entityId, filter, pageNo, pageSize);
                int totalCount = list.FirstOrDefault()?.TotalCount ?? 0;
                var pagedList = new StaticPagedList<VendorViewModel>(list, pageNo.Value, pageSize.Value, totalCount);

                ViewBag.Filter = filter;
                return PartialView("Vendors/_VendorsList", pagedList);
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

        #region Tax Exemption
        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetAllIExemptedTaxesByVendorId(long? vendorId)
        {
            try
            {
                if (vendorId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                var list = _vendorsApiRepository.GetTaxExemptionList(vendorId);
                return PartialView("Vendors/_VendorsExemptedTaxesList", list.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        #endregion

        #region Actions
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(24, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UpdateVendor(VendorViewModel model)
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

                var updateResult = _vendorsApiRepository.UpdateVendor(model);

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
                return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });

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
        [PermissionFilter(23, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult CreateVendor(VendorViewModel model)
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

                var createResult = _vendorsApiRepository.InsertVendor(model);

                if (createResult == null || createResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (createResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = createResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });

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
        [PermissionFilter(24, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult ChangeStatus(int? vendorID, bool? status)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);

                if (vendorID == null || status == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var changeStatusResult = _vendorsApiRepository.ChangeStatus(status.Value, vendorID.Value, actionBy);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = changeStatusResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });

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
                return PartialView("_Result");
            }
        }
        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(26, ReturnViewType.Normal)]
        public ActionResult ExportToExecl_Old()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Vendors" + StringDate + ".xlsx";
                XLWorkbook wb = ExportHelper.ExportVendors(_vendorsApiRepository.GetVendors(false).ToList(), $"VendorsList-{DateTime.Now}");

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
            catch(NotAuthorizedException ex)
            {
                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Forbidden", "Public");
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
        public ActionResult ExportToExeclV2()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Vendors" + StringDate + ".xlsx";
                XLWorkbook wb = ExportHelper.ExportVendors(_vendorsApiRepository.GetVendors(false).ToList(), $"VendorsList-{DateTime.Now}");

                // Save the workbook to a memory stream
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.Position = 0;
                    // Return the file as a download
                    return File(MyMemoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
                }
            }
            catch (NotAuthorizedException ex)
            {
                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;

                // You can return an error page or JSON with the error message
                return Json(new { success = false, error = TempData["ErrorMsg"] });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;

                // Return an error message
                return Json(new { success = false, error = TempData["ErrorMsg"] });
            }
        }



        public ActionResult ExportToExecl()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Vendors" + StringDate + ".xlsx";
                XLWorkbook wb = ExportHelper.ExportVendors(_vendorsApiRepository.GetVendors(false).ToList(), $"VendorsList-{DateTime.Now}");
                // Save the workbook to a memory stream
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.Position = 0;
                    // Return the file as a download
                    return File(MyMemoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
                }
            }
            catch (NotAuthorizedException ex)
            {
                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;
                // You can return an error page or JSON with the error message
                return Json(new { success = false, error = TempData["ErrorMsg"] });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;
                // Return an error message
                return Json(new { success = false, error = TempData["ErrorMsg"] });
            }
        }

        public ActionResult ExportToExeclByEntity()
        {
            try
            {
                int? entityId = Convert.ToInt32(Session["EntityID"]);
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Vendors" + StringDate + ".xlsx";
                XLWorkbook wb = ExportHelper.ExportVendors(_vendorsApiRepository.GetVendorsByEntity(entityId).ToList(), $"VendorsList-{DateTime.Now}");
                // Save the workbook to a memory stream
                //using (MemoryStream MyMemoryStream = new MemoryStream())
                //{
                //    wb.SaveAs(MyMemoryStream);
                //    MyMemoryStream.Position = 0;
                //    // Return the file as a download
                //    return File(MyMemoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
                //}
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
            catch (NotAuthorizedException ex)
            {
                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;
                // You can return an error page or JSON with the error message
                return Json(new { success = false, error = TempData["ErrorMsg"] });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    TempData["ErrorMsg"] = ex.InnerException.Message;
                else
                    TempData["ErrorMsg"] = ex.Message;
                // Return an error message
                return Json(new { success = false, error = TempData["ErrorMsg"] });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(25, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult AddToExemptedTaxes(long? vendorId, byte? taxTypeId)
        {
            try
            {
                if (vendorId == null || taxTypeId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var createResult = _vendorsApiRepository.CreateTaxExemption(vendorId, taxTypeId, true);
                if (createResult.CustomeRespons.ResponseID == "102")
                {
                    ViewBag.ErrorMsg = Resources.Resource.TaxExists;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                var list = _vendorsApiRepository.GetTaxExemptionList(vendorId);
                return Json(new { msg = "success", view = RenderRazorViewToString("Vendors/_VendorsExemptedTaxesList", list.ToList()) });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(25, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult RemoveTaxExemptation(long? id, long? vendorId)
        {
            try
            {
                if (id == null || vendorId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                var removeResult = _vendorsApiRepository.RemoveTaxExemption(id);

                var list = _vendorsApiRepository.GetTaxExemptionList(vendorId);
                return Json(new { msg = "success", view = RenderRazorViewToString("Vendors/_VendorsExemptedTaxesList", list.ToList()) });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
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
