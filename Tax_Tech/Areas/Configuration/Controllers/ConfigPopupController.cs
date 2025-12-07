using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Filters;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class ConfigPopupController : Tax_Tech.Controllers.BaseController
    {
        private readonly BankLookupApiRepository _bankLookupApiRepository;
        private readonly LookupApiRepository _lookupApiRepository;
        private readonly ItemsTaxApiRepository _itemsTaxApiRepository;
        private readonly VendorsApiRepository _vendorsApiRepository;
        private readonly BranchesApiRepository _branchesApiRepository;
        private readonly ItemsApiRepository _itemsApiRepository;
        private readonly AccountLookupApiRepository _accountLookupApiRepository;
        private readonly MailConfigRepository _mailConfigRepository;
        private readonly AccountsApiRepository _accountsApiRepository;

        public ConfigPopupController()
        {
            _bankLookupApiRepository = new BankLookupApiRepository();
            _lookupApiRepository = new LookupApiRepository();
            _itemsTaxApiRepository = new ItemsTaxApiRepository();
            _vendorsApiRepository = new VendorsApiRepository();
            _branchesApiRepository = new BranchesApiRepository();
            _itemsApiRepository = new ItemsApiRepository();
            _accountLookupApiRepository = new AccountLookupApiRepository();
            _mailConfigRepository = new MailConfigRepository();
            _accountsApiRepository = new AccountsApiRepository();
        }

        #region Banks
        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetInsertBankPopup()
        {
            try
            {
                return PartialView("~/Areas/Configuration/Views/Shared/Banks/_BankCreatePopup.cshtml");
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

        #region Payments
        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetCreatePaymentPopup()
        {
            try
            {
                ViewBag.BankList = _bankLookupApiRepository.GetBanks(true);
                return PartialView("~/Areas/Configuration/Views/Shared/Payments/_PaymentCreatePopup.cshtml");
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

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetUpdatePaymentPopup(int? paymentId)
        {
            try
            {
                if (paymentId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                ViewBag.BankList = _bankLookupApiRepository.GetBanks(true);
                return PartialView("~/Areas/Configuration/Views/Shared/Payments/_PaymentUpdatePopup.cshtml", _accountLookupApiRepository.GetPaymentById(paymentId.Value).FirstOrDefault());
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

        #region Branches
        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetCreateBranchPopup()
        {
            try
            {
                ViewBag.Countries = _lookupApiRepository.GetCountries(true);
                ViewBag.BranchTypes = _lookupApiRepository.GetVendorTypes();
                return PartialView("~/Areas/Configuration/Views/Shared/Branches/_BranchCreatePopup.cshtml");
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

        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetUpdateBranchPopup(int? branchId)
        {
            try
            {
                if(branchId == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                ViewBag.BranchTypes = _lookupApiRepository.GetVendorTypes();
                ViewBag.Countries = _lookupApiRepository.GetCountries(true).ToList();
                var branch = _branchesApiRepository.GetBranchById(branchId.Value);
                if (branch.Count()>0)
                {
                    return PartialView("~/Areas/Configuration/Views/Shared/Branches/_BranchUpdatePopup.cshtml", branch.FirstOrDefault());
                }
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
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

        #region Vendors
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(5, ReturnViewType.Model)]
        public ActionResult GetCreateVendorPopup()
        {
            try
            {
                ViewBag.EntitiesList = _accountsApiRepository.GetEntitiesList();
                ViewBag.VendorTypes = _lookupApiRepository.GetVendorTypes();
                ViewBag.Countries = _lookupApiRepository.GetCountries(true);
                return PartialView("~/Areas/Configuration/Views/Shared/Vendors/_VendorCreatePopup.cshtml");
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

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(6, ReturnViewType.Model)]
        public ActionResult GetUpdateVendorPopup(int? vendorID)
        {
            try
            {
                if(vendorID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                ViewBag.EntitiesList = _accountsApiRepository.GetEntitiesList();
                ViewBag.VendorTypes = _lookupApiRepository.GetVendorTypes().ToList();
                ViewBag.Countries = _lookupApiRepository.GetCountries(true).ToList();
                return PartialView("~/Areas/Configuration/Views/Shared/Vendors/_VendorUpdatePopup.cshtml", _vendorsApiRepository.GetVendorById(vendorID.Value).FirstOrDefault());
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

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(7, ReturnViewType.Model)]
        public ActionResult GetVendorsExemptedTaxPopup(int? vendorID)
        {
            try
            {
                if (vendorID == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                ViewBag.TaxTypes = _lookupApiRepository.GetTaxTypes().ToList();
                ViewBag.VendorID = vendorID;
                return PartialView("~/Areas/Configuration/Views/Shared/Vendors/_VendorExemptedTaxesPopup.cshtml");
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

        #region Items
        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(2, ReturnViewType.Model)]
        public ActionResult GetCreateItemPopup()
        {
            try
            {
                ViewBag.UnitTypes = _lookupApiRepository.GetUnitTypes();
                ViewBag.ItemTypes = StaticLists.GetItemTypes();
                ViewBag.EntitiesList = _accountsApiRepository.GetEntitiesList();
                return PartialView("~/Areas/Configuration/Views/Shared/Items/_ItemCreatePopup.cshtml");
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

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(4, ReturnViewType.Model)]
        public ActionResult GetItemTaxesPopup(int? itemId)
        {
            try
            {
                if(itemId == null)
                {
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                ViewBag.EntitiesList = _accountsApiRepository.GetEntitiesList();
                ViewBag.ItemId = itemId;
                ViewBag.Taxes = _lookupApiRepository.GetTaxTypes();
                ViewBag.ItemTaxes = _itemsTaxApiRepository.GetItemsTax(true, itemId.Value);
                return PartialView("~/Areas/Configuration/Views/Shared/Items/_ItemTaxes.cshtml");
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

        //[AuthFilter(ReturnViewType.Model)]
        //[PermissionFilter(4, ReturnViewType.Model)]
        //public ActionResult GetJopTotalPopup(int? itemId)
        //{
        //    try
        //    {
        //        if (itemId == null)
        //        {
        //            return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
        //        }

        //        ViewBag.EntitiesList = _accountsApiRepository.GetEntitiesList();
        //        ViewBag.ItemId = itemId;
        //        ViewBag.Taxes = _lookupApiRepository.GetTaxTypes();
        //        ViewBag.ItemTaxes = _itemsTaxApiRepository.GetItemsTax(true, itemId.Value);
        //        return PartialView("~/Areas/Configuration/Views/Shared/Items/_JobTotalLayout.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //            ViewBag.ErrorMsg = ex.InnerException.Message;
        //        else
        //            ViewBag.ErrorMsg = ex.Message;
        //        return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
        //    }
        //}

        [AuthFilter(ReturnViewType.Model)]
        [PermissionFilter(3, ReturnViewType.Model)]
        public ActionResult GetItemsUpdatePopup(int? itemId)
        {
            try
            {
                if(itemId == null)
                {
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                var item = _itemsApiRepository.GetItemById(itemId.Value).FirstOrDefault();

                ViewBag.EntitiesList = _accountsApiRepository.GetEntitiesList();
                ViewBag.UnitTypes = _lookupApiRepository.GetUnitTypes();
                ViewBag.ItemTypes = StaticLists.GetItemTypes();
                return PartialView("~/Areas/Configuration/Views/Shared/Items/_ItemUpdatePopup.cshtml", item);
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

        #region Delivery
        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetCreateDeliveryPopup()
        {
            try
            {
                ViewBag.UnitTypes = _lookupApiRepository.GetUnitTypes();
                ViewBag.ItemTypes = StaticLists.GetItemTypes();
                return PartialView("~/Areas/Configuration/Views/Shared/Delivery/_CreateDelivery.cshtml");
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

        #region Mail Config
        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetMailConfigEditPopup(int? id)
        {
            try
            {
                if(id == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                string entityId = Convert.ToString(Session["EntityID"]);

                var mailListResult = _mailConfigRepository.GetMailList(entityId);

                if(mailListResult == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.ApiDown;
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                var groupItem = mailListResult.FirstOrDefault(g => g.GroupID == id);

                if(groupItem == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.GroupNotfound;
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                return PartialView("~/Areas/Configuration/Views/Shared/MailConfig/MailConfigEditPopup.cshtml", groupItem);
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

        #region Global Config
        [AuthFilter(ReturnViewType.Model)]
        public ActionResult GetGlobalConfigEditPopup(int? id)
        {
            try
            {
                if(id == null)
                {
                    ViewBag.ErrorMsg = Resources.Resource.InvalidDataProvided;
                    return PartialView("~/Areas/Configuration/Views/Shared/_ErrorOccuredPopup.cshtml");
                }

                return PartialView("~/Areas/Configuration/Views/Shared/GlobalConfig/_GlobalConfigEditPopup.cshtml");
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
    }
}
