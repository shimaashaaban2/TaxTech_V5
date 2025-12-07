using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;

namespace Tax_Tech.Filters
{
    
    public class ChangePasswordFilter : ActionFilterAttribute
    {
        private ReturnViewType returnViewType;

        public ChangePasswordFilter(ReturnViewType returnViewType)
        {
            this.returnViewType = returnViewType;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool CanChangePassword = true;

            CanChangePassword = IsCanChangePassword(filterContext);
            
            if (!CanChangePassword)
            {
                if (returnViewType == ReturnViewType.Json)
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { msg = "fail", view = RenderRazorViewToString("_ChangePasswordFilter", filterContext.Controller) }
                    };
                }
                else if (returnViewType == ReturnViewType.Model)
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "_ChangePasswordFilterPopup"
                    };
                else
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "_ReLogin"
                    };
            }
            
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
        private bool IsCanChangePassword(ActionExecutingContext filterContext)
        {
            bool CanChangePassword = false;

            if (filterContext.HttpContext.Session["ID"] != null && filterContext.HttpContext.Session["AuthToken"] != null
                       && filterContext.HttpContext.Request.Cookies["AuthToken"] != null)
            {
                if (filterContext.HttpContext.Session["AuthToken"].ToString().Equals(filterContext.HttpContext.Request.Cookies["AuthToken"].Value))
                    //IsLogged = true;
                    CanChangePassword = CheckChangePassword(filterContext);
            }
            return CanChangePassword;
        }
        private bool CheckChangePassword(ActionExecutingContext filterContext)
        {
             long UserID = Convert.ToInt64(filterContext.HttpContext.Session["ID"]);
            ViewModels.ChangePasswordDataViewModel Rec = 
                AccountsApiRepository.GetAccounts().GetChnagePasswordData(UserID).FirstOrDefault();

             if (Rec != null)
                if (Rec.CountOfChangePasswordTrying >= Convert.ToInt16(PublicConfig.ChangePasswordLimit))
                    return false;

            return true;
        }
    }




}