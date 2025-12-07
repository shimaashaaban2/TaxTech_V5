using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.taxupdate;
using Tax_Tech.ApiModels.TaxUpdate;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    public class DocumentSourceController : BaseController
    {
        private readonly DocumentSourceRepo _documentSourceRepo;
        public DocumentSourceController()
        {
            _documentSourceRepo = new DocumentSourceRepo();
        }
        // GET: DocumentSource
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetDocFilterBySource(string From, string To, string source, int pageNo=1, int pageSize=50)
        {
            try
            {
                if(source == "")
                {
                    ViewBag.ErrorMSG = "Please select Source";
                    return   PartialView("_Result");
                }
                
                ViewBag.CurrentPage = pageNo;

                RootModel DocSource = _documentSourceRepo.GetDocFilterBySource(From, To, source, pageNo, pageSize);
                return PartialView("DocumentSource/_DocumentSourceList", DocSource);     
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
                return PartialView("_Result");
            }

        }
       

    }
}