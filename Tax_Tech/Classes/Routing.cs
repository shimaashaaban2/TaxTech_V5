using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tax_Tech.Classes
{
    public class Routing
    {
   
    private static Routing _instance;
    public static Routing Get()
    {
        if (_instance == null)
        {
            _instance = new Routing();
        }
        return _instance;
    }
        //public string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        //{

        //    if (model != null)
        //    {
        //        controller.ViewData.Model = model;
        //    }
        //    using (var sw = new StringWriter())
        //    {
        //        var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext,
        //                                                                 viewName);
        //        var viewContext = new ViewContext(controller.ControllerContext, viewResult.View,
        //                                     controller.ViewData, controller.TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}
    }
}