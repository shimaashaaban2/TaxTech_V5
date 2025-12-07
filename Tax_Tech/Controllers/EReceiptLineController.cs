using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.Classes;
using Tax_Tech.Filters;
using Tax_Tech.Repository;

namespace Tax_Tech.Controllers
{
    [AuthFilter(ReturnViewType.Normal)]
    [PermissionFilter(200, ReturnViewType.Normal)]
    public class EReceiptLineController : BaseController
    {
        private readonly VendorsApiRepository _vendorsApiRepository;
        private readonly EReceiptRepository _eReceiptRepo;
        private readonly Logger _logger;
        public EReceiptLineController()
        {
            _logger = new Logger();
            _vendorsApiRepository = new VendorsApiRepository();
            _eReceiptRepo = new EReceiptRepository();
        }
        // GET: EReceiptLine
        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult Index()
        {
            return View();
        }


        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult EReceiptLine()
        {
            return View();
        }

        [AuthFilter(ReturnViewType.Normal)]
        public ActionResult EReceiptDetails()
        {
            return View();
        }
    }
}