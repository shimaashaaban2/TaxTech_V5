using DocumentFormat.OpenXml.VariantTypes;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels.EReceiptApi;
using Tax_Tech.Filters;
using Tax_Tech.Models;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    [AuthFilter(ReturnViewType.Normal)]
    [PermissionFilter(200, ReturnViewType.Normal)]
    public class LogsDashBoardController : BaseController
    {
        private readonly EReceiptRepository _eReceiptRepo;
        public LogsDashBoardController()
        {
            _eReceiptRepo = new EReceiptRepository();
        }


        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            return View();
        }

        #region Actions

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult GetDashBoard(DateTime From, DateTime To)
        {
            if (From > To)
            {
                TempData["ErrorMsg"] = Resources.Resource.FromDateShouldBeLessThanToDate;
                return Json("error", "Public");
            }
            List<object> list = new List<object>();
            List<DashBoardModel> model = _eReceiptRepo.GetDashBoard(From,To);
            List<string> labels = model.Select(m => m.Source).ToList();
            List<int> Data = model.Select(m => m.CountOfEReceipt).ToList();
            list.Add(labels);
            list.Add(Data);
            return Json(list);
           
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult GetStatusChart(DateTime From, DateTime To)
        {

            if (From > To)
            {
                TempData["ErrorMsg"] = Resources.Resource.FromDateShouldBeLessThanToDate;
                return Json("error", "Public");
            }
            List<object> list = new List<object>();
            List<StatusDashBoardModel> model = _eReceiptRepo.GetstatusDashBoard(From,To);
            List<string> labels = model.Select(m => m.EReceiptStatus).ToList();
            List<int> Data = model.Select(m => m.CountOfEReceipt).ToList();
            list.Add(labels);
            list.Add(Data);
            return Json(list);
            
        }

        [AuthFilter(ReturnViewType.Json)]
        [HttpPost]
        public ActionResult CompareStatus(DateTime From, DateTime To)
        {

            if (From > To)
            {
                TempData["ErrorMsg"] = Resources.Resource.FromDateShouldBeLessThanToDate;
                return Json("error", "Public");
            }
            List<object> list = new List<object>();
            List<CompareStatusModel> model = _eReceiptRepo.CompareStatusDashBoard(From,To);
            List<string> labels = model.Select(m => m.Status).ToList();
           // List<string> labels = model.Where(m => m.Status).ToList();
            List<int> Data = model.Select(m => m.Wincash).ToList();
            List<int> Data2 = model.Select(m => m.Billing).ToList();
            list.Add(labels);
            list.Add(Data);
            list.Add(Data2);
            return Json(model);

        }

        [HttpPost]
        public ActionResult TotalReceived(string From,string To)
        {
            try
            {
                IEnumerable<TotalReceivedModel> totalReceivedViews = _eReceiptRepo.TotalReceived(From, To);
                IEnumerable<ProcessingSummeryModel> processingModel = _eReceiptRepo.ProcessingSummery(From, To);
                IEnumerable<ResultSummeryModel> resultSummeries = _eReceiptRepo.ResultSummery(From, To);
                TotalsReceivedViewModel totalsReceived = new TotalsReceivedViewModel();
                totalsReceived.totalReceiveds = totalReceivedViews;
                totalsReceived.processingSummery = processingModel;
                totalsReceived.resultSummeries = resultSummeries;
                var jsonResult = Json(new { msg = "success", view = RenderRazorViewToString("LogsDashBoard/_TotalRecived", totalsReceived) });
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;

                return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
            }
           
        }
        
        #endregion

        public ContentResult CompareStatus2(DateTime From, DateTime To)
        {
            var Result = _eReceiptRepo.CompareStatusDashBoard(From, To);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(Result), "application/json");
        }
        public ContentResult GetDashBoard2(DateTime From, DateTime To)
        {
            var Result = _eReceiptRepo.GetDashBoard(From, To);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(Result), "application/json");
        }
        public ContentResult GetStatusChart2(DateTime From, DateTime To)
        {
            var Result = _eReceiptRepo.GetstatusDashBoard(From, To);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(Result), "application/json");
        }

        #region Methods
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
        #endregion

    }
}

