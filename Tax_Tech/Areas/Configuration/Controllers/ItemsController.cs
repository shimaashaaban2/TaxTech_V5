using ClosedXML.Excel;
using DEFC.Util.DataValidation;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.ViewModels;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class ItemsController : Tax_Tech.Controllers.BaseController
    {
        private readonly ItemsApiRepository _itemsApiRepository;
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly ItemsTaxApiRepository _itemsTaxApiRepository;
        private readonly ValidationsHelper _validationsHelper;
        private readonly AccountsApiRepository _accountsApiRepository;

        public ItemsController()
        {
            _itemsApiRepository = new ItemsApiRepository();
            _lookupApiRepository = new LookupApiRepository();
            _itemsTaxApiRepository = new ItemsTaxApiRepository();
            _validationsHelper = ValidationsHelper.GetValidations();
            _accountsApiRepository = new AccountsApiRepository();
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
                return PartialView("~/Areas/Configuration/Views/Shared/Items/_ItemList.cshtml", _itemsApiRepository.GetItems(false).ToList());
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

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult CountOfAll()
        {
            try
            {
                var Result = _itemsApiRepository.GetItemsCount();

                if (Result == null || Result.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                return PartialView("~/Areas/Configuration/Views/Shared/Items/_ItemsCount.cshtml", Result.CustomeRespons.CountOfAll);
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

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult ListByFilter(string filter, int? pageNo = 1, int? pageSize = 100)
        {
            try
            {
                if (filter.Length < 3)
                {
                    ViewBag.ErrorMsg = Resources.Resource.PleaseEnterAtLeast3Chars;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var list = _itemsApiRepository.GetItemsByFilter(filter, pageNo, pageSize);
                int totalCount = list.FirstOrDefault()?.TotalCount ?? 0;
                var pagedList = new StaticPagedList<ItemViewModel>(list, pageNo.Value, pageSize.Value, totalCount);

                ViewBag.Filter = filter;
                return PartialView("~/Areas/Configuration/Views/Shared/Items/_ItemList.cshtml", pagedList);
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

        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetAllItems()
        {
            try
            {
                return PartialView("~/Areas/Configuration/Views/Shared/Items/_ItemList.cshtml", _itemsApiRepository.GetAllItems().ToList());
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

        public ActionResult GetTaxSubTypes(int? taxId)
        {
            try
            {
                if (taxId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var taxSubTypes = _itemsTaxApiRepository.GetTaxSubTypes(taxId);
                return PartialView("~/Areas/Configuration/Views/Shared/Items/_ItemTaxSubTypes.cshtml", taxSubTypes);
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

        [AuthFilter(ReturnViewType.Normal)]
        [PermissionFilter(9, ReturnViewType.Normal)]
        public ActionResult ExportToExecl()
        {
            try
            {
                string StringDate = DateTime.Now.Date.ToShortDateString().Replace("/", "-");
                string FileName = "Export" + "Items" + StringDate + ".xlsx";
                XLWorkbook wb = ExportHelper.ExportItems(_itemsApiRepository.GetItems(false).ToList(), $"ItemsList-{DateTime.Now}");

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
            catch (Tax_Tech.Exceptions.NotAuthorizedException ex)
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
        [PermissionFilter(2, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult CreateItem(ItemViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                model.ActionBy = actionBy;

                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                List<string> ValidateVendorList = _validationsHelper.ValidateItems(model);
                if (ValidateVendorList.Count() > 0)
                {
                    ViewBag.Validation = ValidateVendorList;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var checkItemErpCodeExistResult = _itemsApiRepository.ItemsInsertCheckERPExists(model.EntityID, model.ItemERPID);

                if (checkItemErpCodeExistResult == true)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ERPCodeIsAlreadyTakenforthisEntity;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var insertResult = _itemsApiRepository.InsertItem(model);

                if (insertResult == null || insertResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                if (insertResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = insertResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                ViewBag.Success = insertResult.CustomeRespons.ResponseMsg;
                return Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });

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
                return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(3, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult UpdateItem(ItemViewModel model)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);
                model.ActionBy = actionBy;

                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var checkItemErpCodeExistResult = _itemsApiRepository.ItemsUpdateCheckERPExists(model.EntityID, model.ItemERPID, model.ItemSerial);

                if (checkItemErpCodeExistResult == true)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ERPCodeIsAlreadyTakenforthisEntity;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var updateResult = _itemsApiRepository.UpdateItem(model);

                if (updateResult == null || updateResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                if (updateResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = updateResult.CustomeRespons.ResponseMsg;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                ViewBag.Success = updateResult.CustomeRespons.ResponseMsg;
                return Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });

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
                return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
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
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var changeStatusResult = _itemsApiRepository.ChangeStatus(status.Value, itemId.Value, actionBy);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = changeStatusResult.CustomeRespons.ResponseMsg;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
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
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }

        [AuthFilter(ReturnViewType.Json)]
        [PermissionFilter(4, ReturnViewType.Json)]
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
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var insertResult = _itemsTaxApiRepository.InsertItemTax(model);

                if (insertResult == null || insertResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                if (insertResult.CustomeRespons.ResponseID == "102")
                {
                    ViewBag.ErrorMsg = Resources.Resource.TaxExists;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }
                if (insertResult.CustomeRespons.ResponseID == "0")
                {
                    ViewBag.ErrorMsg = insertResult.CustomeRespons.ResponseMsg;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemTaxesList.cshtml", _itemsTaxApiRepository.GetItemsTax(true, model.ItemID.Value).ToList()) });
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
        public ActionResult ChangeStatusTaxItem(int? recordId, bool? status, int? itemId)
        {
            try
            {
                long actionBy = Convert.ToInt32(Session["ID"]);

                if (recordId == null || status == null || itemId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var changeStatusResult = _itemsTaxApiRepository.ChangeStatus(status.Value, recordId.Value, actionBy);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = changeStatusResult.CustomeRespons.ResponseMsg;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemTaxesList.cshtml", _itemsTaxApiRepository.GetItemsTax(true, itemId.Value).ToList()) });
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

        //Fatma 
        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult ItemPriceAfterTax(PriceAfterTaxModel model)
        {
            try
            {
                short actionBy = Convert.ToInt16(Session["ID"]);
                model.By = actionBy;
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }


                if (!DataType.IsDecimal(Convert.ToString(model.PriceAfterTax)))
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var changeStatusResult = _itemsTaxApiRepository.GetPriceAfterTax(model);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }
                ViewBag.Success = Resources.Resource.DataSavedSuccessfully;
                return Json(new { msg = "success", itemId = model.itemID, view = RenderRazorViewToString("_Result") });

                //var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemTaxesList.cshtml", _itemsTaxApiRepository.GetItemsTax(true,Convert.ToInt32(itemID)).ToList()) });
                //jsonResult.MaxJsonLength = int.MaxValue;
                //return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
        }


        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult ItemPriceAfterTaxMaster(PriceAfterTaxModelMaster model)
        {
            try
            {
                short actionBy = Convert.ToInt16(Session["ID"]);
                model.By = actionBy;
                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }


                if (!DataType.IsDecimal(Convert.ToString(model.PriceAfterTax)))
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var changeStatusResult = _itemsTaxApiRepository.GetPriceAfterTaxMaster(model);

                if (changeStatusResult == null || changeStatusResult.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                if (changeStatusResult.CustomeRespons.ResponseID != "1")
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }
                ViewBag.Success = Resources.Resource.DataSavedSuccessfully;
                return Json(new { msg = "success", itemId = model.itemID, view = RenderRazorViewToString("_Result") });

                //var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemTaxesList.cshtml", _itemsTaxApiRepository.GetItemsTax(true,Convert.ToInt32(itemID)).ToList()) });
                //jsonResult.MaxJsonLength = int.MaxValue;
                //return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
            }
        }
        #endregion
        //Fatma
        [AuthFilter(ReturnViewType.Partial)]
        public ActionResult GetPriceAfterTaxPopup(long? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    return PartialView("_ErrorOccuredPopup");
                }


                // IEnumerable<PriceDetailsViewModel> List = _itemsTaxApiRepository.GetPriceChanges(itemId);
                IEnumerable<PriceDetailsViewModel> List = _itemsTaxApiRepository.GetPriceChangesMaster(itemId);

                //ViewBag.List = List;

                return PartialView("Items/_PriceHistoryTableLayout", List);
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
       // [PermissionFilter(4, ReturnViewType.Json)]
        [HttpPost]
        public ActionResult AddItemGroup(AddItemGroups model) 
        {
            try
            {
                model.UserID = 2;

                if (!ModelState.IsValid)
                {
                    ViewBag.Validation = _validationsHelper.ModelValidation(ModelState.Values);
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var insertItem = _itemsTaxApiRepository.AddItemsGroup(model);

                if (insertItem == null || insertItem.CustomeRespons == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                if (insertItem.CustomeRespons.ResponseID == "102")
                {
                    ViewBag.ErrorMsg = Resources.Resource.TaxExists;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }
                if (insertItem.CustomeRespons.ResponseID == "0")
                {
                    ViewBag.ErrorMsg = insertItem.CustomeRespons.ResponseMsg;
                    return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
                }

                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemGroupList.cshtml", _itemsTaxApiRepository.GetItemsGroupList(model.ItemID)) });
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
        public ActionResult RemoveItemgroup(long ID,long ItemID)
        {
            try
            {
                if (ID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
                }

                var removeResult = _itemsTaxApiRepository.RemoveItemgroup(ID);


                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Items/_ItemGroupList.cshtml", _itemsTaxApiRepository.GetItemsGroupList(ItemID)) });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return Json(new { msg = "fail", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/_Result.cshtml") });
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
