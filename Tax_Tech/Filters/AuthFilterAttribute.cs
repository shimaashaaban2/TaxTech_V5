using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Controllers;

namespace Tax_Tech.Filters
{
    public enum ReturnViewType
    {
        Normal = 0,
        Partial,
        Model,
        Json
    }

    public class AuthFilterAttribute : ActionFilterAttribute
    {
        private short formId;
        private byte actionId;
        private ReturnViewType returnViewType;

        public AuthFilterAttribute(ReturnViewType returnViewType)
        {
            this.returnViewType = returnViewType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isLoggedIn = true;
            //bool IsActive = true;
            isLoggedIn = IsLoggedIn(filterContext);

            //string User = Convert.ToString(filterContext.HttpContext.Session["ID"]);
            //if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(User))
            //    isLoggedIn = false;

            //if (isLoggedIn)
            //{
            //    long UserID = Convert.ToInt64(User);
            //    Tax_Tech.Areas.Configuration.ViewModels.UserAccountsViewModel UserRec = AccountsApiRepository.GetAccounts().GetAccountsList(UserID).Where(q => q.UserID == UserID).FirstOrDefault();

            //    //Tax_Tech.Areas.Configuration.ViewModels.UserAccountsViewModel UserRec = AccountsApiRepository.GetAccounts().GetAccountDataForEdit(Convert.ToInt64(User)).FirstOrDefault();
            //    if (UserRec != null)
            //    {
            //        filterContext.HttpContext.Session["RoleID"] = UserRec.RoleID;
            //        IsActive = UserRec.IsActive;
            //        if (!IsActive)
            //            isLoggedIn = false;
            //    }

            //}            

            if (!isLoggedIn)
            {
                if (returnViewType == ReturnViewType.Normal)
                    RedirectToRoute(filterContext, new { controller = "../Accounts", action = "Login", area = "" });
                else if (returnViewType == ReturnViewType.Json)
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { msg = "fail", view = RenderRazorViewToString("_Result", filterContext.Controller) }
                    };
                }
                else if (returnViewType == ReturnViewType.Model)
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "_ReLoginPopup"
                    };
                else
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "_ReLogin"
                    };
            }
            else
            {
              bool Continue=  ConcurrentSession(filterContext);
                if (Continue)
                    RoleCkeck(filterContext);

            }
        }

        private void RoleCkeck(ActionExecutingContext filterContext)
        {
            // if the user successfully logged in
            // checking the user role

            string roleID = Convert.ToString(filterContext.HttpContext.Session["RoleID"]);

            if (roleID != "1" && filterContext.HttpContext.Request.Url.AbsoluteUri.ToLower().Contains("configuration"))
            {
                if (returnViewType == ReturnViewType.Normal)
                    RedirectToRoute(filterContext, new { controller = "../Public", action = "Forbidden", area = "" });
                else if (returnViewType == ReturnViewType.Json)
                {
                    var jsonResult = new JsonResult
                    {
                        Data = new { msg = "fail", view = RenderRazorViewToString("_Result", filterContext.Controller) }
                    };
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    filterContext.Result = jsonResult;
                }
                else if (returnViewType == ReturnViewType.Model)
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "_ReLoginPopup"
                    };
                else
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "_ReLogin"
                    };
            }
        }
        private bool ConcurrentSession(ActionExecutingContext filterContext)
        {
            //string AuthToken = Convert.ToString(filterContext.HttpContext.Session["AuthToken"]);
            //long UserID = Convert.ToInt64(filterContext.HttpContext.Session["ID"]);
            //ViewModels.Users_UserLastSessionIDGetModelView UserRec =
            //    AccountsApiRepository.GetAccounts().GetAccountLastSessionID(UserID).FirstOrDefault();

            //if (UserRec != null)
            //    if (AuthToken != UserRec.LastSessionID)
            //    {
            //        filterContext.HttpContext.Session["ID"] = null;
            //        if (returnViewType == ReturnViewType.Normal)
            //            RedirectToRoute(filterContext, new { controller = "../Accounts", action = "Login", area = "" });
            //        else if (returnViewType == ReturnViewType.Json)
            //        {
            //            var jsonResult = new JsonResult
            //            {
            //                Data = new { msg = "fail", view = RenderRazorViewToString("_Result", filterContext.Controller) }
            //            };
            //            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //            filterContext.Result = jsonResult;
            //        }
            //        else if (returnViewType == ReturnViewType.Model)
            //            filterContext.Result = new PartialViewResult
            //            {
            //                ViewName = "_ConcurrentSessionPopup"
            //            };
            //        else
            //            filterContext.Result = new PartialViewResult
            //            {
            //                ViewName = "_ConcurrentSession"
            //            };

            //        return false;
            //    }

            return true;

        }

        private void RedirectToRoute(ActionExecutingContext context, object routeValues)
        {
            var rc = new RequestContext(context.HttpContext, context.RouteData);
            string url = System.Web.Routing.RouteTable.Routes.GetVirtualPath(rc,
                new System.Web.Routing.RouteValueDictionary(routeValues)).VirtualPath;
            context.HttpContext.Response.Redirect(url, true);
        }

        public string RenderRazorViewToString(string viewName, ControllerBase context)
        {
            using (var sw = new StringWriter())
            {
                context.ViewBag.ErrorMsg = Resources.Resource.LoginExpire;
                var viewResult = ViewEngines.Engines.FindPartialView(context.ControllerContext, viewName);
                var viewContext = new ViewContext(context.ControllerContext, viewResult.View, context.ViewData, context.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(context.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private bool IsLoggedIn(ActionExecutingContext filterContext)
        {
            bool IsLogged = false;
 
            if (filterContext.HttpContext.Session["ID"] != null && filterContext.HttpContext.Session["AuthToken"] != null
                       && filterContext.HttpContext.Request.Cookies["AuthToken"] != null)
            {
                if (filterContext.HttpContext.Session["AuthToken"].ToString().Equals(filterContext.HttpContext.Request.Cookies["AuthToken"].Value))
                    //IsLogged = true;
                    IsLogged = CheckRoleAndActivation(filterContext);
            }
            return IsLogged;
        }
        private bool CheckRoleAndActivation(ActionExecutingContext filterContext)
        {
            bool IsLogged = true;
            bool IsActive = true;
            long UserID = Convert.ToInt64(filterContext.HttpContext.Session["ID"]);
            Tax_Tech.Areas.Configuration.ViewModels.UserAccountsViewModel UserRec = AccountsApiRepository.GetAccounts().GetAccountsList(UserID)?.Where(q => q.UserID == UserID)?.FirstOrDefault();

            //Tax_Tech.Areas.Configuration.ViewModels.UserAccountsViewModel UserRec = AccountsApiRepository.GetAccounts().GetAccountDataForEdit(Convert.ToInt64(User)).FirstOrDefault();
            if (UserRec != null)
            {
                filterContext.HttpContext.Session["RoleID"] = UserRec.RoleID;
                IsActive = UserRec.IsActive;
                if (!IsActive)
                    IsLogged = false;
            }
            return IsLogged;
        }
    }
}