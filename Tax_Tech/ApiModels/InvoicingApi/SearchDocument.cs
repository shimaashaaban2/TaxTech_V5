using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.InvoicingApi
{
    public class SearchDocument
    {
        public long RecordID { get; set; }
        public long EntityID { get; set; }
        public string publicUrl { get; set; }
        public string uuid { get; set; }
        public string submissionUUID { get; set; }
        public string longId { get; set; }
        public string internalId { get; set; }
        public string typeName { get; set; }
        public string documentTypeNamePrimaryLang { get; set; }
        public string documentTypeNameSecondaryLang { get; set; }
        public string typeVersionName { get; set; }
        public string issuerId { get; set; }
        public string issuerName { get; set; }
        public string issuerType { get; set; }
        public string receiverId { get; set; }
        public string receiverName { get; set; }
        public string receiverType { get; set; }
        public Nullable<System.DateTime> dateTimeIssued { get; set; }
        public Nullable<System.DateTime> dateTimeReceived { get; set; }
        public Nullable<decimal> totalSales { get; set; }
        public Nullable<decimal> totalDiscount { get; set; }
        public Nullable<decimal> netAmount { get; set; }
        public Nullable<decimal> total { get; set; }
        public string cancelRequestDate { get; set; }
        public string rejectRequestDate { get; set; }
        public string cancelRequestDelayedDate { get; set; }
        public string rejectRequestDelayedDate { get; set; }
        public string declineCancelRequestDate { get; set; }
        public string declineRejectRequestDate { get; set; }
        public string status { get; set; }
        public string createdByUserId { get; set; }
        public string documentStatusReason { get; set; }
        public string lateSubmissionRequestNumber { get; set; }
    }
}