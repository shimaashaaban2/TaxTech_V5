using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Tax_Tech
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
         }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            //// secure the ASP.NET Session ID only if using SSL
            //// if you don't check for the issecureconnection, it will not work.
            ////if (Request.IsSecureConnection == true)
                //Response.Cookies["ASP.NET_SessionId"].Secure = true;
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //// this code will mark the forms authentication cookie and the
            //// session cookie as Secure.
            //if (Response.Cookies.Count > 0)
            //{
            //    foreach (string s in Response.Cookies.AllKeys)
            //    {
            //        if (s == FormsAuthentication.FormsCookieName || "ASP.NET_SessionId".Equals(s, StringComparison.InvariantCultureIgnoreCase))
            //        {
            //            Response.Cookies[s].Secure = true;
            //        }
            //    }
            //}
        }
    }
}
