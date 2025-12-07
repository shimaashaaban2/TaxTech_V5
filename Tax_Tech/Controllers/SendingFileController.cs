using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.Filters;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    public class SendingFileController : BaseController
    {
        private readonly SendFileRepository _sendFileRepository;
        public SendingFileController()
        {
            _sendFileRepository = new SendFileRepository();
        }
        #region SendFile Via WhatsApp
        //[AuthFilter(ReturnViewType.Json)]
        //[HttpPost]
        //public ActionResult Sendfile(InvoiceRequestDto requestDto)
        //{
        //    SendFileModel model = _sendFileRepository.SendFileViaWhatsApp(requestDto);
        //    return Json(model);
        //}
        //[HttpPost]
        //public ActionResult Sendfile(string DocumentId ,string PhoneNumber)
        //{
        //    SendFileModel model = _sendFileRepository.SendFileViaWhatsApp(DocumentId, PhoneNumber);

        //    return Json(new
        //    {
        //        //success = model != null && model.Status == "Sent", // adjust condition as needed
        //        message = model?.Message ?? "No response",
        //        data = model
        //    }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        [AuthFilter(ReturnViewType.Json)]
        public ActionResult GenerateInvoice(InvoiceRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                // Returns 400 BadRequest with validation errors
                Response.StatusCode = 400;
                return Json(ModelState, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var result = _sendFileRepository.SendFileViaWhatsApp(requestDto);

                return Json(new
                {
                    success = true,
                    data = result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                Response.StatusCode = 500;
                return Json(new
                {
                    success = false,
                    message = "An internal server error occurred: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}