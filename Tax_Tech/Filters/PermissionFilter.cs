using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tax_Tech.Filters
{
    public class PermissionFilter : ActionFilterAttribute
    {
        private readonly long _actionId;
        private readonly ReturnViewType _returnViewType;

        public PermissionFilter(long actionId, ReturnViewType returnViewType)
        {
            _actionId = actionId;
            _returnViewType = returnViewType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                long? userId = Convert.ToInt64(filterContext.HttpContext.Session["ID"]);
                Tax_Tech.Areas.Configuration.Repository.PermissionRepository permissionRepository = new Areas.Configuration.Repository.PermissionRepository();
                var hasPermissionResult = permissionRepository.HasPermission(_actionId, userId);

                if(hasPermissionResult.CustomeRespons.HasPermission == "0")
                {
                    GetResult(filterContext);
                }
            }
            catch (Exception)
            {
                GetResult(filterContext);
            }
        }

        private void GetResult(ActionExecutingContext filterContext)
        {
            switch (_returnViewType)
            {
                case ReturnViewType.Json:
                    var jsonResult = new JsonResult
                    {
                        Data = new { msg = "fail", view = RenderRazorViewToString("_UnAuthorized", filterContext.Controller) },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    filterContext.Result = jsonResult;
                    break;
                case ReturnViewType.Model:
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "_UnAuthoizedPopup"
                    };
                    break;
                case ReturnViewType.Normal:
                    RedirectToRoute(filterContext, new { controller = "../Public", action = "Forbidden", area = "" });
                    break;
                case ReturnViewType.Partial:
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "_UnAuthorized"
                    };
                    break;
            }
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
    }
}
