using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.Filters;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    public class JobQueueV2Controller : BaseController
    {
        private readonly JobQueueRepositoryV2 _jobQueueRepositoryV2;
        private readonly AccountsApiRepository _accountRepository;
        public JobQueueV2Controller()
        {
            _jobQueueRepositoryV2 = new JobQueueRepositoryV2();
        }
        //[HttpPut]
        //public ActionResult JobClosedV2(JobQueueModelV2 jobQueue)
        //{

        //    try
        //    {
        //        var ResultModel = _jobQueueRepositoryV2.GetCloseJob(jobQueue);
        //        if (ResultModel != null)
        //        {
        //            string Result = "";
        //            if (ResultModel.CustomeRespons.TryGetValue("Response ID", out Result))
        //            {
        //                if (Result == "1")
        //                {
        //                    var jsonResult = Json(new
        //                    {
        //                        redirecturl = Url.Action("Index", "Home", new { area = "" })
        //                    });
        //                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //                    return jsonResult;
        //                }
        //                else
        //                    ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;
        //            }
        //        }
        //        else
        //            ViewBag.ErrorMsg = Resources.Resource.NoDataReturned;

        //        return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });



        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMsg = ex.Message;
        //        return Json(new { msg = "fail", view = RenderRazorViewToString("_Result") });
        //    }
        //}
        [HttpPost]
        public ActionResult ChangeJobStatus(JobQueueModelV2 jobQueue)
        {
            try
            {
                if (jobQueue == null)
                    return new HttpStatusCodeResult(400, "Invalid request.");

                var result = _jobQueueRepositoryV2.GetCloseJobV2Async(jobQueue); // synchronous call

                return Json(new
                {
                    success = true,
                    data = result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ChangeJobStatus2(JobQueueModelV2 jobQueue)
        {
            try
            {
                if (jobQueue == null)
                    return new HttpStatusCodeResult(400, "Invalid request.");

                // ✅ Get current user and entity info
                string currentUser = User.Identity.Name;
                long entityId = Convert.ToInt64(Session["EntityID"]);
                long CreatedBy = Convert.ToInt64(Session["UserID"]);
                var accountData = _accountRepository.GetAccountData(currentUser, entityId);
                var userProfile = accountData?.FirstOrDefault();
                CreatedBy = userProfile?.UserID ?? 0;
                ViewBag.CreatedBy = jobQueue.CreatedBy;
                // ✅ Update job status
                var result = _jobQueueRepositoryV2.GetCloseJobV2Async(jobQueue);

                // ✅ Return job result + user ID
                return Json(new
                {
                    success = true,
                    CreatedBy = CreatedBy,
                    data = result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
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