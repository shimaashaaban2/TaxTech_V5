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
    public class AccountsController : Tax_Tech.Controllers.BaseController
    {

        [HttpGet]
        [AuthFilterAttribute(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            return View();
        }
       
        [HttpPost]
        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult Create(FormCollection frm)
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);
                string username = frm["username"];
                string password = frm["password"];
                string Entity = frm["EntityID"];
                string RoleID = frm["RoleID"];
                List<string> ValidationList = ValidationsHelper.GetValidations().CreateValidation(frm);
                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    //return PartialView("_Result");
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });

                }

                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().CreatAccount(Convert.ToInt64(Entity), username, password, frm["email"], frm["DisplayName"], null, UserID, Convert.ToInt32(RoleID));
                if (ResultModel != null)
                {
                    string Result = "";
                    if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        if (Result == "1")
                            //ViewBag.Success = "Account Created Successfully";
                            return Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Accounts/_AccountsList.cshtml", AccountsApiRepository.GetAccounts().GetAccountsList(UserID).ToList()) });

                        else if (Result == "2")
                            ViewBag.SingleValid = Resources.Resource.UsernameIsAlreadyTaken;
                        else
                            ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                    }
                }
                else
                    ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
            }
            return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });

        }
        [HttpPost]
        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult ChangeMyPassword(FormCollection frm)
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);
                string password = frm["password"];
                string EntityID = Convert.ToString(Session["EntityID"]);
                string Oldpassword = frm["Oldpassword"];
                string Username = "0";
                List<string> ValidationList = ValidationsHelper.GetValidations().ChangePasswordValidation(frm);
                if (Areas.Configuration.Helpers.methods.Getmethods().ValidComparePassword(frm["password"], frm["Oldpassword"]))
                    ValidationList.Add(Resources.Resource.SamePassword);

                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    //return PartialView("_Result");
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });
                }

                Username = GetUsername(UserID, Oldpassword, Username);
                if (Username == null || Username == "0")
                {
                    ViewBag.SingleValid = Resources.Resource.EnterValidCurrentPassword;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });

                }

                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().UserChangeMyPassword(EntityID, Convert.ToInt64(UserID), Username, password, Oldpassword);
                    if (ResultModel != null)
                    {
                        string Result = "";
                        if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                        {
                            if (Result == "1")
                                return Json(new { msg = "success", redirecturl = Url.Action("Login", new { controller = "Accounts", area = "" }) });

                            else if (Result == "2")
                                ViewBag.SingleValid = Resources.Resource.UsernameIsAlreadyTaken;
                            else
                                ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                        }
                    }
                    else
                        ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                
            }
            catch (Exception ex) 
            {

                ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
            }

            return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });

        }
        [HttpPost]
        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult ResetPassword(FormCollection frm)
        {
            try
            {
                long By = Convert.ToInt64(Session["ID"]);
                long UserID = Convert.ToInt64(frm["ser"]);
                string password = frm["password"];

                List<string> ValidationList = ValidationsHelper.GetValidations().ChangePasswordValidation(frm);

                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    //return PartialView("_Result");
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });
                }


                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().UserResetPassword(Convert.ToInt64(UserID), By, password);
                if (ResultModel != null)
                {
                    string Result = "";
                    if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        if (Result == "1")
                            ViewBag.Success = Resources.Resource.PasswordSuccessToReset;
                        else
                            ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                    }
                }
                else
                    ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;

            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
            }

            return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });

        }
        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult ResetChangePassword(long id)
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);


                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().UserResetChnagePassword(id);
                string Result = "";
                if (ResultModel != null)
                {
                    if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        if (Result == "0")
                        {
                            ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                        }
                        else if (Result == "1")
                        {
                            ViewBag.Success = $"{Resources.Resource.AccountSuccessfullyReseted}";
                        }
                    }
                }
                return PartialView("~/Areas/Configuration/Views/Shared/Accounts/_AccountsList.cshtml", AccountsApiRepository.GetAccounts().GetAccountsList(UserID).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
                else
                    ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }
        [HttpPost]
        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult Edit(FormCollection frm)
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);
                string RoleID = frm["RoleID"];

                List<string> ValidationList = ValidationsHelper.GetValidations().EditValidation(frm);
                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.Validation) });
                }

                HttpPostedFileBase ProfileImg = Request.Files["ProfileImage"] as HttpPostedFileBase;
                byte[] ProfileImgPath = null;
                if (ProfileImg != null)
                    ProfileImgPath = SaveImg(ProfileImg);


                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().EditAccount(Convert.ToInt64(frm["UserID"]), frm["UserMail"], frm["DisplayName"], ProfileImgPath, UserID, Convert.ToInt32(frm["RoleID"]));
                if (ResultModel != null)
                {
                    string Result = "";
                    if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        if (Result == "1")
                            return Json(new { msg = "success", view = RenderRazorViewToString("~/Areas/Configuration/Views/Shared/Accounts/_AccountsList.cshtml", AccountsApiRepository.GetAccounts().GetAccountsList(UserID).ToList()) });
                        else
                            ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                    }
                }
                else
                    ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
            }
            return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", ViewBag.ErrorMsg) });
        }
        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult AccountsList()
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);
                return PartialView("~/Areas/Configuration/Views/Shared/Accounts/_AccountsList.cshtml", AccountsApiRepository.GetAccounts().GetAccountsList(UserID).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
                else
                    ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }

        //Fatma 26-10-23
        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult UsersFilter(long OptionID)
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);
                return PartialView("~/Areas/Configuration/Views/Shared/Accounts/_AccountsList.cshtml", AccountsApiRepository.GetAccounts().GetUserFilter(UserID,OptionID).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
                else
                    ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }

        [AuthFilterAttribute(ReturnViewType.Partial)]
        public ActionResult SetActivationStatus(long id,bool status)
        {
            try
            {
                long UserID = Convert.ToInt64(Session["ID"]);
                string StringStatus = Resources.Resource.Suspended2;
                if (status)
                    StringStatus = Resources.Resource.Activated;

                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().UserActivation(id, status);
                string Result = "";
                if (ResultModel != null)
                {
                    if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        if (Result == "0")
                        {
                            ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                         }
                        else if (Result == "1")
                        {
                            ViewBag.Success = $"{Resources.Resource.AccountSuccessfully} {StringStatus}";
                         }
                    }
                }
                return PartialView("~/Areas/Configuration/Views/Shared/Accounts/_AccountsList.cshtml", AccountsApiRepository.GetAccounts().GetAccountsList(UserID).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
                else
                    ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
                return PartialView("~/Areas/Configuration/Views/Shared/_Result.cshtml");
            }
        }

        #region Methods
        private static string GetUsername(long UserID, string Oldpassword, string Username)
        {
            CustomViewModel UsernameResultModel = AccountsApiRepository.GetAccounts().GetUserNameByIDandPassword(UserID, Oldpassword);
            if (UsernameResultModel != null)
            {
                string Result = "";
                if (UsernameResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                {
                    if (Result == "1")
                        if (UsernameResultModel.CustomeRespons.TryGetValue("Username", out Username)) ;
                }
            }

            return Username;
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
        private byte[] SaveImg(HttpPostedFileBase file)
        {
            try
            {

                System.IO.MemoryStream target = new System.IO.MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] img = target.ToArray();

                return img;

            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion
    }
}