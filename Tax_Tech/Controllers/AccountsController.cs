using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Helpers;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;
using DEFC.Util.DataValidation;
using DocumentFormat.OpenXml.Drawing.Charts;


namespace Tax_Tech.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly SettingsLoader _settingsLoader;

        public AccountsController()
        {
            _settingsLoader = new SettingsLoader();
        }

        // GET: Auth
        #region Pages
        [HttpGet]
        public ActionResult Login()
        {
            Session["username"] = null;
            Session["EntityId"] = null;
            return View();
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            //Session["ID"] = null;
            //Session["EntityID"] = null;
            //Session["DisplayName"] = null;
            //Session["ProfileImage"] = null;
            //Session["RoleID"] = null;
            //Session["EntityName"] = null;


            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }


            return RedirectToAction("Login", "Accounts", new { area = "" });
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        //Fatma 11-1-2023
        [HttpGet]
        public ActionResult SetPassword()  //To change the password for the first login time
        {

            return View();
        }

        [HttpGet]
        public ActionResult AccountCreated()
        {
            ViewBag.Success = TempData["AccountCreated"];
            ViewBag.EmailRes = TempData["EmailRes"];
            ViewBag.email = TempData["email"];
            ViewBag.Entity = TempData["Entity"];
            return View();
        }
        #endregion

        #region Actions
        [HttpPost]
        [AuthFilterAttribute(ReturnViewType.Json)]
        [ChangePasswordFilter(ReturnViewType.Json)]
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
        public ActionResult SetMyPassword(FormCollection frm)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["username"])) ||
                  string.IsNullOrEmpty(Convert.ToString(Session["EntityId"])))
                {
                    return Json(new { msg = "success", redirecturl = Url.Action("Login", new { controller = "Accounts", area = "" }) });
                }

                string password = frm["password"];
                string ConfirmPassword = frm["Confirmpassword"];
                string Oldpassword = frm["Oldpassword"];

                List<string> ValidationList = ValidationsHelper.GetValidations().ChangePasswordValidation(frm);
                //if (Areas.Configuration.Helpers.methods.Getmethods().ValidComparePassword(frm["password"], frm["Confirmpassword"]))
                //    ValidationList.Add(Resources.Resource.SamePassword);

                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    //return PartialView("_Result");
                    return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });
                }

                string Username = Convert.ToString(Session["username"]);
                long EntityID = Convert.ToInt64(Session["EntityId"]);

                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().AccountLogin(Convert.ToInt64(EntityID), Username, Oldpassword);
                string Result = "";
                string CountOFTryoing = "";
                if (ResultModel != null)
                {
                    if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        ResultModel.CustomeRespons.TryGetValue("CountOfTrying", out CountOFTryoing);
                        if (CountOFTryoing =="0")
                        {
                            CustomViewModel ResultModel1 = AccountsApiRepository.GetAccounts().SetChangeMyPassword(EntityID,Username, password, Oldpassword);
                            if (ResultModel != null)
                            {
                                string Result1 = "";
                                if (ResultModel1.CustomeRespons.TryGetValue("Response ID", out Result1))
                                {
                                    if (Result1 == "1")
                                        return Json(new { msg = "success", redirecturl = Url.Action("Login", new { controller = "Accounts", area = "" }) });                                  
                                    else
                                        ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                                }
                            }
                        }
                        else
                            ViewBag.ErrorMsg = Resources.Resource.EnterValidCurrentPassword;
                    }
                    else
                        ViewBag.ErrorMsg = Resources.Resource.EnterValidCurrentPassword;
                }
                else
                    ViewBag.ErrorMsg = Resources.Resource.EnterValidCurrentPassword;
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = Resources.Resource.ErrorOccured;
            }

            return Json(new { msg = "fail", view = RenderRazorViewToString("_Result", null) });

        }


        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            try
            {
                Areas.Configuration.Repository.BranchesApiRepository _branchesApiRepository = new Areas.Configuration.Repository.BranchesApiRepository();
                string username = frm["username"];
                string password = frm["password"];
                string Entity = frm["EntityID"];
                List<string> ValidationList = ValidationsHelper.GetValidations().LoginValidation(frm);
                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    return PartialView("_Result");
                }

                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().AccountLogin(Convert.ToInt64(Entity), username, password);
                string Result = "";
                if (ResultModel != null)
                {
                    if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        if (Result == "0")
                        {
                            ViewBag.SingleValid = Resources.Resource.AccountNotRegistered;
                            return PartialView("_Result");
                        }
                        else if (Result == "3")
                        {
                            ViewBag.SingleValid = Resources.Resource.AccountSuspended;
                            return PartialView("_Result");
                        }
                        else if (Result == "2")
                        {

                            UserProfileViewModel profile = AccountsApiRepository.GetAccounts().GetAccountData(username, Convert.ToInt64(Entity)).FirstOrDefault();
                            if (profile != null)
                            {
                                if (ResultModel.CustomeRespons.TryGetValue("Token", out string Token)) { Session["Token"] = Token; }
                                Session["ID"] = profile.UserID;
                                Session["EntityID"] = profile.EntityID;
                                Session["DisplayName"] = profile.DisplayName;
                                Session["ProfileImage"] = profile.ProfileImage;
                                Session["RoleID"] = profile.RoleID;
                                // Session["EntityName"] = _branchesApiRepository.GetBranchById(Convert.ToInt16(Session["EntityID"])).FirstOrDefault().BranchName;
                                try
                                {
                                    long entityID = Convert.ToInt16(Session["EntityID"]);
                                    Session["EntityName"] = AccountsApiRepository.GetAccounts().GetEntitiesList().Where(q => q.IsActive == true && q.EntityId == entityID).FirstOrDefault().EntityTitle;

                                }
                                catch (Exception)
                                { }
                            }

                            // loading user settings
                            var userSettings = _settingsLoader.LoadUserSettings($"{profile.UserID}");

                            // setting the cookies
                            SetUserSettingsCookies("lang", userSettings.Language);
                            SetUserSettingsCookies("mode", userSettings.IsDarkMode == true ? "dark" : "white");

                            SessionFixation();
                            //route to Home
                            if (Tax_Tech.Classes.PublicConfig.IsErecipt == "1")
                            {
                                return Json(new
                                {
                                    redirecturl = Url.Action("Index", "LogsDashBoard", new { area = "" })
                                });

                            }
                            else
                            {
                                return Json(new
                                {
                                    redirecturl = Url.Action("Index", "Home", new { area = "" })
                                });
                            }

                        }
                        else if (Result == "1")
                        {

                            string Token = "";
                            if (ResultModel.CustomeRespons.TryGetValue("Token", out Token)) { Session["Token"] = Token; }
                            UserProfileViewModel profile = AccountsApiRepository.GetAccounts().GetAccountData(username, Convert.ToInt64(Entity)).FirstOrDefault();
                            if (profile != null)
                            {
                                Session["ID"] = profile.UserID;
                                Session["EntityID"] = profile.EntityID;
                                Session["DisplayName"] = profile.DisplayName;
                                Session["ProfileImage"] = profile.ProfileImage;
                                Session["RoleID"] = profile.RoleID;
                                //ession["EntityName"] = _branchesApiRepository.GetBranchById(Convert.ToInt16(Session["EntityID"])).FirstOrDefault().BranchName;
                                try
                                {
                                    long entityID = Convert.ToInt16(Session["EntityID"]);
                                    Session["EntityName"] = AccountsApiRepository.GetAccounts().GetEntitiesList().Where(q => q.IsActive == true && q.EntityId == entityID).FirstOrDefault().EntityTitle;

                                }
                                catch (Exception)
                                { }
                            }

                            // loading user settings
                            var userSettings = _settingsLoader.LoadUserSettings($"{profile.UserID}");

                            // setting the cookies
                            SetUserSettingsCookies("lang", userSettings.Language);
                            SetUserSettingsCookies("mode", userSettings.IsDarkMode == true ? "dark" : "white");
                            SessionFixation();

                            //route to Home
                            if (Tax_Tech.Classes.PublicConfig.IsErecipt == "1")
                            {
                                return Json(new
                                {
                                    redirecturl = Url.Action("Index", "LogsDashBoard", new { area = "" })
                                });
                                
                            }
                            else
                            {
                                return Json(new
                                {
                                    redirecturl = Url.Action("Index", "Home", new { area = "" })
                                });
                            }
                           
                        }
                        else if (Result == "4")
                        {
                            string CountOfTrying = "0";
                            if (ResultModel.CustomeRespons.TryGetValue("CountOfTrying", out CountOfTrying)) { }
                            ViewBag.SingleValid = Resources.Resource.WrongPasswordTrying;//+". " + Resources.Resource.CountOfTrying+" "+ CountOfTrying + " " + Resources.Resource.Times
                            return PartialView("_Result");
                        }
                        else if (Result == "5")
                        {
                            ViewBag.ErrorMsg = Resources.Resource.AccountSuspended;
                            return PartialView("_Result");
                        }
                        else if (Result == "200")
                        {
                            Session["username"] = username;
                            Session["EntityId"] = Entity;

                            //route to Home
                            return Json(new
                            {
                                redirecturl = Url.Action("SetPassword", "Accounts", new { area = "" })
                            });
                        }
                    }
                }
                else
                    ViewBag.ErrorMsg = Resources.Resource.LoginDataNotValid;

                return PartialView("_Result");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        [HttpPost]
        public ActionResult Register(FormCollection frm)
        {
            try
            {

                string username = frm["username"];
                string password = frm["password"];
                string Entity = frm["EntityID"];
                List<string> ValidationList = ValidationsHelper.GetValidations().RegisterValidation(frm);
                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    return PartialView("_Result");
                }
                else
                {
                    string ConfirmCode = Areas.Configuration.Helpers.methods.Getmethods().GetRandCode(10);
                    CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().AccountRegister(Convert.ToInt64(Entity), username, password, frm["email"], frm["DisplayName"], null, ConfirmCode);
                    if (ResultModel != null)
                    {
                        string Result = "";
                        if (ResultModel.CustomeRespons.TryGetValue("Status", out Result))
                        {
                            if (Result == "1")
                            {
                                TempData["AccountCreated"] = Resources.Resource.AccountCreated;
                                string Hours = "";
                                bool EmailRes = GenerateAndSendConfirmationEmail(frm["email"], ConfirmCode, out Hours, Convert.ToInt32(Entity));
                                TempData["EmailRes"] = EmailRes;
                                TempData["email"] = frm["email"];
                                TempData["Entity"] = Entity;
                                return Json(new
                                {
                                    redirecturl = Url.Action("AccountCreated", "Accounts", new { area = "" })
                                });
                            }
                            else if (Result == "11")
                                ViewBag.SingleValid = Resources.Resource.EmailIsAlreadyTaken;
                            else if (Result == "13")
                                ViewBag.SingleValid = Resources.Resource.UsernameIsAlreadyTaken;
                            else
                                ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                        }
                    }
                    else
                        ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;

                    return PartialView("_Result");
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        [HttpPost]
        public ActionResult ResendConfirmEmail(FormCollection frm)
        {
            try
            {
                bool countinue = true;
                List<string> ValidationList = ValidationsHelper.GetValidations().CheckEmail(frm);
                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    return PartialView("_Result");
                }


                if (countinue)
                {

                    string ConfirmCode = Areas.Configuration.Helpers.methods.Getmethods().GetRandCode(10);
                    CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().ResendConfirmationEmail(Convert.ToInt64(frm["ent"]), frm["email"], ConfirmCode);
                    if (ResultModel != null)
                    {
                        string Result = "";
                        if (ResultModel.CustomeRespons.TryGetValue("Status", out Result))
                        {
                            if (Result == "1")
                            {
                                string Hours = "";
                                bool EmailRes = GenerateAndSendConfirmationEmail(frm["email"], ConfirmCode, out Hours, Convert.ToInt32(frm["ent"]));
                                if (EmailRes)
                                    ViewBag.Success = @Tax_Tech.Resources.Resource.EmailSentSuccessfully;
                                else
                                    ViewBag.ErrorMsg = Resources.Resource.EmailSentFail;
                            }
                            else if (Result == "2")
                                ViewBag.SingleValid = Resources.Resource.EmailNotExists;
                            else
                                ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
                        }
                    }


                }

                return PartialView("_Result");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        [HttpPost]
        public ActionResult SendForgetPasswordEmail(FormCollection frm)
        {
            try
            {
                List<string> ValidationList = ValidationsHelper.GetValidations().CheckUserEmail(frm);
                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    return PartialView("_Result");
                }

                string ForgetPasswordCode = Areas.Configuration.Helpers.methods.Getmethods().GetRandCode(10);
                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().UsersForgetPasswordCode(Convert.ToInt64(frm["EntityId"]), frm["email"], ForgetPasswordCode);
                if (ResultModel != null)
                {
                    string Result = "";
                    if (ResultModel.CustomeRespons.ContainsKey("Response ID"))
                    {
                        Result = ResultModel.CustomeRespons["Response ID"];
                        if (Result == "0")
                        {
                            //send Email
                            string Hours = "";
                            bool EmailRes = GenerateAndSendForgetPasswordEmail(frm["email"], ForgetPasswordCode, out Hours, Convert.ToInt16(frm["EntityId"]));
                            if (EmailRes)
                                ViewBag.Success = Resources.Resource.SuccessSendEmail;
                            else
                                ViewBag.ErrorMsg = Resources.Resource.FailSendEmail;
                        }
                        else if (Result == "-1")
                            ViewBag.ErrorMsg = Resources.Resource.CanNotComplateTheOperation;
                    }
                }


                return PartialView("_Result");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }
        public ActionResult ConfirmEmail(string id)
        {
            //long? ConfirmStatus = _ent.Pre_UsersConfirmation(id).FirstOrDefault();
            //Confirmed 1
            //Already Confirmed 2
            //Expired 3
            //ViewBag.ConfirmStatus = ConfirmStatus;

            return View();
        }

        public ActionResult ForgetMyPassword(string Code, string ent)
        {
            ViewBag.ent = ent;
            CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().UsersForgetPassword(Code);
            if (ResultModel != null)
            {
                string Result = "";
                if (ResultModel.CustomeRespons.ContainsKey("Response ID"))
                {
                    Result = ResultModel.CustomeRespons["Response ID"];
                    if (Result == "0")
                    {
                        string Email = null;
                        if (ResultModel.CustomeRespons.ContainsKey("Email"))
                            Email = ResultModel.CustomeRespons["Email"];
                        ViewBag.email = Email;
                    }
                    else if (Result == "-1")
                        ViewBag.ErrorMsg = Resources.Resource.CanNotComplateTheOperation;
                }
            }

            return View();
        }
        [HttpPost]
        public ActionResult ChangePasswordByCode(FormCollection frm)
        {
            try
            {
                List<string> ValidationList = ValidationsHelper.GetValidations().ChangePasswordValidation(frm);
                if (ValidationList.Count() > 0)
                {
                    ViewBag.Validation = ValidationList;
                    return PartialView("_Result");
                }

                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().ChangePasswordByCode(Convert.ToInt64(frm["EntityID"]), frm["Email"], frm["password"]);
                if (ResultModel != null)
                {
                    string Response = "";
                    string result = "";
                    if (ResultModel.CustomeRespons.ContainsKey("Response ID"))
                    {
                        Response = ResultModel.CustomeRespons["Response ID"];
                        if (Response == "0")
                        {
                            if (ResultModel.CustomeRespons.ContainsKey("Response ID"))
                                result = ResultModel.CustomeRespons["Result"];
                            if (result == "1")
                                ViewBag.Success = Resources.Resource.PasswordSuccessToReset;
                            else
                                ViewBag.ErrorMsg = Resources.Resource.PasswordFailToReset;
                        }
                        else if (Response == "-1")
                            ViewBag.ErrorMsg = Resources.Resource.CanNotComplateTheOperation;
                    }
                }


                return PartialView("_Result");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }
        }

        #endregion

        #region Methods
        public void SetUserSettingsCookies(string key, string value)
        {
            //string SessionId = Request.Cookies["ASP.NET_SessionId"].Value;
            if (Request.Cookies[key] != null)
            {
                Request.Cookies[key].Value = value;
                Request.Cookies[key].Expires = DateTime.UtcNow.AddDays(10);
                HttpContext.Response.Cookies.Add(Request.Cookies[key]);
            }
            else
            {
                HttpCookie cookie = new HttpCookie(key, value);
                cookie.Expires = DateTime.UtcNow.AddDays(10);
                HttpContext.Response.Cookies.Add(cookie);
            }
        }
        public void DeleteUserSettingsCookies(string key)
        {
            if (Request.Cookies[key] != null)
            {

                HttpCookie currentUserCookie = Request.Cookies["currentUser"];
                HttpContext.Response.Cookies.Remove("currentUser");
                HttpContext.Request.Cookies.Remove("currentUser");

                Session["ID"] = null;
            }

        }
        private void SessionFixation()
        {
            string guid = Guid.NewGuid().ToString();
            Session["AuthToken"] = guid;
            // now create a new cookie with this guid value  
            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
            long UserID = Convert.ToInt64(Session["ID"]);
            CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().AccountLastSessionID(UserID, guid);
        }
        #endregion

        private bool GenerateAndSendConfirmationEmail(string Email, string ConfirmCode, out string Hours, long? EntityID)
        {

            var email = AccountsApiRepository.GetAccounts().GetEmailData(EntityID).FirstOrDefault();
            Hours = "";
            if (email != null)
            {
                string EmailIs = email.EMail;
                string emailPss = email.MailPassword;
                string emailPort = email.SMTPort;
                string host = email.SMTPServer;
                string RecepientEmail = Email;
                bool? EnableSsl = email.TLSFlag;

                string Subject = "";

                ApiResponse ResponseHours = AccountsApiRepository.GetAccounts().GetHours(EntityID);
                Hours = ResponseHours.CustomeRespons.Hours;


                string Body = PopulateConfirmBody(Email, ConfirmCode, Hours, 1);

                bool EmailRes = SendHtmlFormattedEmail(EmailIs, emailPss, emailPort, host, EnableSsl, RecepientEmail, Body, Subject);

                return EmailRes;
            }
            return false;
        }
        private bool GenerateAndSendForgetPasswordEmail(string Email, string ForgetPasswordCode, out string Hours, long? EntityID)
        {

            var email = AccountsApiRepository.GetAccounts().GetEmailData(EntityID).FirstOrDefault();
            Hours = "";
            if (email != null)
            {
                string EmailIs = email.EMail;
                string emailPss = email.MailPassword;
                string emailPort = email.SMTPort;
                string host = email.SMTPServer;
                string RecepientEmail = Email;
                bool? EnableSsl = email.TLSFlag;

                string Subject = Resources.Resource.ForgetPasswordEmailInEmail;

                ApiResponse ResponseHours = AccountsApiRepository.GetAccounts().GetForgetPasswordHours(EntityID);
                Hours = ResponseHours.CustomeRespons.Hours;


                string Body = PopulateForgetPasswordBody(Convert.ToString(EntityID), Email, ForgetPasswordCode, Hours, 1);

                bool EmailRes = SendHtmlFormattedEmail(EmailIs, emailPss, emailPort, host, EnableSsl, RecepientEmail, Body, Subject);

                return EmailRes;
            }
            return false;
        }
        private static bool SendHtmlFormattedEmail(string email, string emailPss, string emailPort, string host, bool? EnableSsl, string RecepientEmail, string body, string subject)
        {

            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    mailMessage.From = new MailAddress(email);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(RecepientEmail));

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = host;
                    smtp.Port = Convert.ToInt16(emailPort);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(email, emailPss);
                    smtp.EnableSsl = Convert.ToBoolean(EnableSsl);

                    smtp.Send(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    //EventLog.GetEventLog().AddToErrorTrack("Send Test Email", "SendHtmlFormattedEmail", ex.Message);
                    string ErrorMsg = "";
                    if (ex.InnerException != null)
                    {
                        ErrorMsg = Convert.ToString(ex.InnerException.Message);
                    }
                    return false;
                }

            }
        }
        private string PopulateConfirmBody(string Email, string ConfirmCode, string Hours, long EmailType)
        {
            //string AppDomain = _ent.Config_KeysListGetByStringKey(11).FirstOrDefault();


            string body = string.Empty;
            if (EmailType == 1)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath(@"\Email\RegisterEmail.html")))
                {
                    body = reader.ReadToEnd();
                }
            }

            string ConfirmURL = PublicConfig.AppDomain + "/Accounts/ConfirmEmail?id=" + ConfirmCode;
            //string WebsitePath = AppDomain + "Aiss/Index";
            body = body.Replace("{Email}", Email);
            body = body.Replace("{ConfirmURL}", ConfirmURL);
            //body = body.Replace("{WebsitePath}", WebsitePath);
            //body = body.Replace("{Address}", ContactData.Address);
            body = body.Replace("{WelcometoTaxTech}", Resources.Resource.WelcometoTaxTech);
            body = body.Replace("{Hours}", Resources.Resource.Hours);
            body = body.Replace("{ResponseHours}", Hours);
            body = body.Replace("{ConfirmEmail}", Resources.Resource.ConfirmEmail);
            body = body.Replace("{PleaseConfirmYourEmail}", Resources.Resource.PleaseConfirmYourEmail);
            body = body.Replace("{FromTheButtonBelow}", Resources.Resource.FromTheButtonBelow);
            body = body.Replace("{YourConfirmationCodeWillExpireIn}", Resources.Resource.YourConfirmationCodeWillExpireIn);

            //body = body.Replace("{AppDomain}", AppDomain);
            return body;

        }
        private string PopulateForgetPasswordBody(string EntityID, string Email, string ConfirmCode, string Hours, long EmailType)
        {
            string body = string.Empty;
            if (EmailType == 1)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath(@"\Email\ForgetPasswordEmail.html")))
                {
                    body = reader.ReadToEnd();
                }
            }

            string ConfirmURL = PublicConfig.AppDomain + "/Accounts/ForgetMyPassword?Code=" + ConfirmCode + "&ent=" + EntityID;
            body = body.Replace("{Welcometo}", Resources.Resource.WelcometoTaxTech);
            body = body.Replace("{PleaseConfirmYourEmail}", Resources.Resource.PleaseConfirmYourEmail);
            body = body.Replace("{FromTheButtonBelow}", Resources.Resource.FromTheButtonBelow);
            body = body.Replace("{Hours}", Resources.Resource.Hours);
            body = body.Replace("{ResponseHours}", Hours);
            body = body.Replace("{YourForgetPasswordCodeWillExpireIn}", Resources.Resource.YourConfirmationCodeWillExpireIn);
            body = body.Replace("{ForgetPasswordURL}", ConfirmURL);
            body = body.Replace("{ForgetPasswordEmail}", Resources.Resource.ForgetPasswordEmailInEmail);

            //body = body.Replace("{Email}", Email);
            //body = body.Replace("{ForgetPasswordURL}", ConfirmURL);      
            //body = body.Replace("{WelcometoTaxTech}", Resources.Resource.WelcometoTaxTech);
            //body = body.Replace("{Hours}", Resources.Resource.Hours);
            //body = body.Replace("{ResponseHours}", Hours);
            //body = body.Replace("{PleaseConfirmYourEmail}", Resources.Resource.PleaseConfirmYourEmail);
            //body = body.Replace("{FromTheButtonBelow}", Resources.Resource.FromTheButtonBelow);


            //body = body.Replace("{YourForgetPasswordCodeWillExpireIn}", Resources.Resource.YourForgetPasswordCodeWillExpireIn);


            return body;

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
       
    }
}