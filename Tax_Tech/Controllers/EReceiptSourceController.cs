using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    public class EReceiptSourceController : BaseController
    {
        private readonly EReceiptSourceRepository _eReceiptSourceRepo;

        public static string Logtype { get; set; }
        public EReceiptSourceController()
        {
            _eReceiptSourceRepo = new EReceiptSourceRepository();
        }
        // GET: EReceiptSource
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetEReceiptSource(string FromDate, string ToDate, string source, int PageNumber=1 , int PageSize = 50)
        {
            try
            {
                if (source == "undefined") {

                    source = "EreceiptBilling";
                }
                ViewBag.CurrentPage = PageNumber;
                    EReceiptSourceListModel ReceiptSource = _eReceiptSourceRepo.GetEReceiptSource(FromDate, ToDate,  source, PageNumber, PageSize);
                return PartialView("EReceiptSource/_EReceiptSourceList", ReceiptSource);
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                    ViewBag.ErrorMsg = ex.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }

        }

        //[HttpGet]
        //public ActionResult GetbyFilterEReceiptSource()
        //{
        //    EReceiptSourceListModel ReceiptSource = _eReceiptSourceRepo.GetEReceiptSource();
        //    return PartialView("EReceipt/_EReceiptSourceList", ReceiptSource);
        //}

    }
}

//string From, string To, string source, int pageNo = 1, int pageSize = 50