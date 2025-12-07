using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class EReceiptSingleViewModel
    {
        public string Rin { get; set; }
        public string CompanyTradeName { get; set; }
        public string BranchCode { get; set; }
        public string Country { get; set; }
        public string Governate { get; set; }
        public string regionCity { get; set; }
        public string street { get; set; }
        public string buildingNumber { get; set; }
        public string dateTimeIssued { get; set; }
        public string receiptNumber { get; set; }
        public string UUID { get; set; }
        public string previousUUID { get; set; }
        public string currency { get; set; }
        public int? exchangeRate { get; set; }
        public string receiptType { get; set; }
        public string typeVersion { get; set; }
        public long? EReceiptID { get; set; }
        public string buyerType { get; set; }
        public string buyerID { get; set; }
        public string buyerName { get; set; }
        public string buyerPhone { get; set; }
        public string activityCode { get; set; }
        public string deviceSerialNumber { get; set; }
    }
}