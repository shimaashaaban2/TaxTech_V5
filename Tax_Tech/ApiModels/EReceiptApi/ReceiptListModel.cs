using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class ReceiptListModel
    {
        public long? EReceiptID { get; set; }
        public DateTime? dateTimeIssued { get; set; }
        public string receiptNumber { get; set; }
        public string UUID { get; set; }
        public string previousUUID { get; set; }
        public string referenceOldUUID { get; set; }
        public string currency { get; set; }
        public int exchangeRate { get; set; }
        public string receiptType { get; set; }
        public string typeVersion { get; set; }
        public long? sellerID { get; set; }
        public string buyerType { get; set; }
        public string buyerID { get; set; }
        public string buyerName { get; set; }
        public string buyerPhone { get; set; }
        public string QRCode { get; set; }
        public string ETAQRCode { get; set; }
        public string ETAUUID { get; set; }
        public byte EReceiptSource { get; set; }
        public string BranchName { get; set; }
        public string PersonalID { get; set; }
        public string Source { get; set; }
        public string EReceiptStatus { get; set; }
        public string SubmitStatusTitle { get; set; }
        public decimal totalAmount { get; set; }
        public decimal netAmount { get; set; }
        public decimal taxTotals { get; set; }
    }
}