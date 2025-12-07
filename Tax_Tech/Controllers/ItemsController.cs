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
    public class ItemsController : BaseController
    {
        private readonly ItemsApiRepository _itemsApiRepository;
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly ItemsTaxApiRepository _itemsTaxApiRepository;
        private readonly ValidationsHelper _validationsHelper;

        public ItemsController()
        {
            _itemsApiRepository = new ItemsApiRepository();
            _lookupApiRepository = new LookupApiRepository();
            _itemsTaxApiRepository = new ItemsTaxApiRepository();
            _validationsHelper = ValidationsHelper.GetValidations();
        }

        #region Pages
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
                return PartialView("Items/_ItemList", _itemsApiRepository.GetItems(false).ToList());
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
                var Result = _itemsApiRepository.GetItemsCountByEntityId(entityId);

                if (Result == null || Result.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                return PartialView("Items/_ItemsCount", Result.CustomeRespons.CountOfAll);
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

                var list = _itemsApiRepository.GetItemsByFilterAndEntityID(entityId, filter, pageNo, pageSize);
                int totalCount = list.FirstOrDefault()?.TotalCount ?? 0;
                var pagedList = new StaticPagedList<ItemViewModel>(list, pageNo.Value, pageSize.Value, totalCount);

                ViewBag.Filter = filter;
                return PartialView("Items/_ItemList", pagedList);
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

        public ActionResult GetTaxSubTypes(int? taxId)
        {
            try
            {
                if (taxId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                var taxSubTypes = _itemsTaxApiRepository.GetTaxSubTypes(taxId);
                return PartialView("Items/_ItemTaxSubTypes", taxSubTypes);
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

        //[AuthFilter(ReturnViewType.Normal)]
        //[PermissionFilter(27, ReturnViewType.Normal)]
        //public ActionResult ExportToExecl_Old()
        //{
        //    try
        //    {
        //        string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
        //        string FileName = "Export" + "Items" + StringDate + ".xlsx";
        //        XLWorkbook wb = ExportHelper.ExportItems(_itemsApiRepository.GetItems(false).ToList(), $"ItemsList-{DateTime.Now}");

        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
        //        using (MemoryStream MyMemoryStream = new MemoryStream())
        //        {
        //            wb.SaveAs(MyMemoryStream);
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }
        //        wb.Dispose();
        //        return Content("", "application/ms-excel");
        //    }
        //    catch (NotAuthorizedException ex)
        //    {
        //        if (ex.InnerException != null)
        //            TempData["ErrorMsg"] = ex.InnerException.Message;
        //        else
        //            TempData["ErrorMsg"] = ex.Message;
        //        return RedirectToAction("Forbidden", "Public");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //            TempData["ErrorMsg"] = ex.InnerException.Message;
        //        else
        //            TempData["ErrorMsg"] = ex.Message;
        //        return RedirectToAction("error", "Public");
        //    }
        //}

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(27, ReturnViewType.Normal)]
        public ActionResult ExportToExecl()
        {
            try
            {
                //string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                //string FileName = "Export" + "Items" + StringDate + ".xlsx";
                //XLWorkbook wb = ExportHelper.ExportItems(_itemsApiRepository.GetItems(false).ToList(), $"ItemsList-{DateTime.Now}");

                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Vendors" + StringDate + ".xlsx";
                XLWorkbook wb = ExportHelper.ExportItems(_itemsApiRepository.GetItems(false).ToList(), $"ItemsList-{DateTime.Now}");
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
        #endregion

        #region Actions
        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(20, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult CreateItem(ItemViewModel model)
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

                List<string> ValidateVendorList = _validationsHelper.ValidateItems(model);
                if (ValidateVendorList.Count() > 0)
                {
                    ViewBag.Validation = ValidateVendorList;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });

                }

                var insertResult = _itemsApiRepository.InsertItem(model);

                if (insertResult == null || insertResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }

                if (insertResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = insertResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
                }
                return Json(new { msg = "success", view = RenderRazorViewToString("_Result") });

                //var jsonResult = Json( new {msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemList.cshtml", _itemsApiRepository.GetItems(false).ToList()) });
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
        [PermissionFilter(21, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UpdateItem(ItemViewModel model)
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

                var updateResult = _itemsApiRepository.UpdateItem(model);

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

                //var jsonResult = Json( new {msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemList.cshtml", _itemsApiRepository.GetItems(false).ToList()) });
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
        public ActionResult ChangeStatus(int? itemId, bool? status)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);

                if (itemId == null || status == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                var changeStatusResult = _itemsApiRepository.ChangeStatus(status.Value, itemId.Value, actionBy);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = changeStatusResult.CustomeRespons.ResponseMsg;
                    return PartialView("_Result");
                }
                return PartialView("_Result");
                //var jsonResult = Json( new {msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemList.cshtml", _itemsApiRepository.GetItems(false).ToList()) });
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

        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(22, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult AddTaxToItem(ItemsTaxViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                model.ActionBy = actionBy;

                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return PartialView("_Result");
                }

                var insertResult = _itemsTaxApiRepository.InsertItemTax(model);

                if (insertResult == null || insertResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }
                if (insertResult.CustomeRespons.ResponseID == "102")
                {
                    ViewBag.ErrorMsg = Resources.Resource.TaxExists;
                    return PartialView("_Result");
                }
                if (insertResult.CustomeRespons.ResponseID == "0")
                {
                    ViewBag.ErrorMsg = insertResult.CustomeRespons.ResponseMsg;
                    return PartialView("_Result");
                }


                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Items/_ItemTaxesList", _itemsTaxApiRepository.GetItemsTax(true, model.ItemID.Value).ToList()) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
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
        public ActionResult ChangeStatusTaxItem(int? recordId, bool? status, int? itemId)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);

                if (recordId == null || status == null || itemId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("_Result");
                }

                var changeStatusResult = _itemsTaxApiRepository.ChangeStatus(status.Value, recordId.Value, actionBy);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("_Result");
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = changeStatusResult.CustomeRespons.ResponseMsg;
                    return PartialView("_Result");
                }

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("Items/_ItemTaxesList", _itemsTaxApiRepository.GetItemsTax(true, itemId.Value).ToList()) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
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
