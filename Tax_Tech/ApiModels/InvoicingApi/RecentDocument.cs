using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels.InvoicingApi
{
    public class RecentDocument
    {
        public long? RecordID { get; set; }

        [JsonProperty("publicUrl")]
        public string PublicUrl { get; set; }

        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [JsonProperty("submissionUUID")]
        public string SubmissionUUID { get; set; }

        [JsonProperty("longId")]
        public string LongID { get; set; }

        [JsonProperty("internalId")]
        public string InternalId { get; set; }

        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("documentTypeNamePrimaryLang")]
        public string DocumentTypeNamePrimaryLang { get; set; }

        [JsonProperty("documentTypeNameSecondaryLang")]
        public string DocumentTypeNameSecondaryLang { get; set; }

        [JsonProperty("typeVersionName")]
        public string TypeVersionName { get; set; }

        [JsonProperty("issuerId")]
        public string IssuerId { get; set; }

        [JsonProperty("issuerName")]
        public string IssuerName { get; set; }

        [JsonProperty("receiverId")]
        public string ReceiverId { get; set; }

        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

        [JsonProperty("dateTimeIssued")]
        public string DateTimeIssued { get; set; }

        [JsonProperty("dateTimeReceived")]
        public string DateTimeReceived { get; set; }

        [JsonProperty("totalSales")]
        public double? TotalSales { get; set; }

        [JsonProperty("totalDiscount")]
        public double? TotalDiscount { get; set; }

        [JsonProperty("netAmount")]
        public double? NetAmount { get; set; }

        [JsonProperty("total")]
        public double? Total { get; set; }

        [JsonProperty("maxPercision")]
        public int? MaxPercision { get; set; }

        [JsonProperty("invoiceLineItemCodes")]
        public string InvoiceLineItemCodes { get; set; }

        [JsonProperty("cancelRequestDate")]
        public string CancelRequestDate { get; set; }

        [JsonProperty("rejectRequestDate")]
        public string RejectRequestDate { get; set; }

        [JsonProperty("cancelRequestDelayedDate")]
        public string CancelRequestDelayedDate { get; set; }

        [JsonProperty("rejectRequestDelayedDate")]
        public string RejectRequestDelayedDate { get; set; }

        [JsonProperty("declineCancelRequestDate")]
        public string DeclineCancelRequestDate { get; set; }

        [JsonProperty("declineRejectRequestDate")]
        public string DeclineRejectRequestDate { get; set; }

        [JsonProperty("documentStatusReason")]
        public string DocumentStatusReason { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdByUserId")]
        public string CreatedByUserId { get; set; }
    }
}
