using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tax_Tech.Controllers
{
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            HttpCookie darkModeCookie = requestContext.HttpContext.Request.Cookies["mode"];

            if(darkModeCookie == null)
            {
                darkModeCookie = new HttpCookie("mode", "white");
                requestContext.HttpContext.Response.Cookies.Add(darkModeCookie);
            }

            HttpCookie cookie = requestContext.HttpContext.Request.Cookies["lang"];

            if(cookie == null)
            {
                cookie = new HttpCookie("lang", "en-US");
                requestContext.HttpContext.Response.Cookies.Add(cookie);
            }
            
            requestContext.HttpContext.Request.Headers.Add("Accept-Language", cookie.Value);
            CultureInfo cultureInfo = new CultureInfo(cookie.Value);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            return base.BeginExecute(requestContext, callback, state);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }
}
