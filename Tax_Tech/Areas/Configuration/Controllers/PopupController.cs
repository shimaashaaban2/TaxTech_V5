using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Filters;
using Tax_Tech.Repository;

namespace Tax_Tech.Areas.Configuration.Controllers
{
    public class PopupController : Tax_Tech.Controllers.BaseController
    {
        private readonly Repository.AccountsApiRepository _accountsApiRepository;
        public PopupController()
        {
            _accountsApiRepository = new Repository.AccountsApiRepository();
        }
        [AuthFilterAttribute(ReturnViewType.Model)]
        public ActionResult GetCreateAccountPopup()
        {
            try
            {
                ViewBag.EntitiesList = _accountsApiRepository.GetEntitiesList();
                return PartialView("Accounts/_AccountCreatePopup");
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

        [AuthFilterAttribute(ReturnViewType.Model)]
        public ActionResult GetEditAccountPopup(long UserID)
        {
            try
            {
                return PartialView("Accounts/_AccountUpdatePopup", Repository.AccountsApiRepository.GetAccounts().GetAccountDataForEdit(UserID).FirstOrDefault());
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
        [AuthFilterAttribute(ReturnViewType.Model)]
        [ChangePasswordFilter(ReturnViewType.Model)]
        public ActionResult GetChangeMyPasswordPopup(long UserID)
        {
            try
            {
                return PartialView("Accounts/_ChangeMyPasswordPopup", Repository.AccountsApiRepository.GetAccounts().GetAccountDataForEdit(UserID).FirstOrDefault());
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
        [AuthFilterAttribute(ReturnViewType.Model)]
        public ActionResult GetResetPasswordPopup(long UserID)
        {
            try
            {
                return PartialView("Accounts/_ResetPasswordPopup", UserID);
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
    }
}