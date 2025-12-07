using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Filters;
using Tax_Tech.Helpers;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    public class LangController : BaseController
    {
        private readonly SettingsApiRepository _settingsApiRepository;

        public LangController()
        {
            _settingsApiRepository = new SettingsApiRepository();
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index(string lang)
        {
            try
            {
                if (lang != null)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                }

                HttpCookie httpCookie = new HttpCookie("lang", lang);
                Response.Cookies.Add(httpCookie);

                // changing the language user settings in API
                var userSettings = _settingsApiRepository.SetAppLang(Convert.ToString(Session["ID"]), lang);

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                string ErrorMsg = "";
                if (ex.InnerException != null)
                {
                    ErrorMsg = Convert.ToString(ex.InnerException.Message);
                }
                else
                {
                    ErrorMsg = Convert.ToString(ex.Message);
                }
                ViewBag.ErrorMsg = ErrorMsg;
                return RedirectToAction("error", "Public");
            }
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult DarkMode(string mode)
        {
            try
            {
                HttpCookie cookie = HttpContext.Request.Cookies["mode"];

                if (cookie == null || mode == null)
                {
                    cookie = new HttpCookie("mode", "white");
                    return Redirect(Request.UrlReferrer.ToString());
                }

                cookie.Value = mode;
                Response.Cookies.Add(cookie);

                // changing the design mode user settings in API
                var userSettings = _settingsApiRepository.SetAppDesignMode(Convert.ToString(Session["ID"]), mode == "white" ? false : true);

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                string ErrorMsg = "";
                if (ex.InnerException != null)
                {
                    ErrorMsg = Convert.ToString(ex.InnerException.Message);
                }
                else
                {
                    ErrorMsg = Convert.ToString(ex.Message);
                }
                ViewBag.ErrorMsg = ErrorMsg;
                return RedirectToAction("error", "Public");
            }
        }

        public ActionResult ChangeLangNoLogin(string lang)
        {
            try
            {
                if (lang != null)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                }

                HttpCookie httpCookie = new HttpCookie("lang", lang);
                Response.Cookies.Add(httpCookie);

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                string ErrorMsg = "";
                if (ex.InnerException != null)
                {
                    ErrorMsg = Convert.ToString(ex.InnerException.Message);
                }
                else
                {
                    ErrorMsg = Convert.ToString(ex.Message);
                }
                ViewBag.ErrorMsg = ErrorMsg;
                return RedirectToAction("error", "Public");
            }
        }

        public ActionResult DarkModeNoLogin(string mode)
        {
            try
            {
                HttpCookie cookie = HttpContext.Request.Cookies["mode"];

                if (cookie == null || mode == null)
                {
                    cookie = new HttpCookie("mode", "white");
                    return Redirect(Request.UrlReferrer.ToString());
                }

                cookie.Value = mode;
                Response.Cookies.Add(cookie);

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                string ErrorMsg = "";
                if (ex.InnerException != null)
                {
                    ErrorMsg = Convert.ToString(ex.InnerException.Message);
                }
                else
                {
                    ErrorMsg = Convert.ToString(ex.Message);
                }
                ViewBag.ErrorMsg = ErrorMsg;
                return RedirectToAction("error", "Public");
            }
        }
    }
}
