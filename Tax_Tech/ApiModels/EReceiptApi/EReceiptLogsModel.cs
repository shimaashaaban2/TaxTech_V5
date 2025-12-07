using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class EReceiptLogsModel
    {
        public long? LogID { get; set; }
        public long? LogTypeID { get; set; }
        public long? EReceiptID { get; set; }
        public string EReceiptNumber { get; set; }
        public string UUID { get; set; }
        public string QRCode { get; set; }
        public string ReuestPayload { get; set; }
        public string ResponsePayload { get; set; }
        public string LogDatetime { get; set; }
        public string ETAResponsePayload { get; set; }
        public string ETARequestPayload { get; set; }
        public string ETAQRCode { get; set; }
        public string ETAUUID { get; set; }
        public string Error { get; set; }
        public string IsSendTosubmit { get; set; }
        public byte?  SubmitStatus { get; set; }
        public string EReceiptStatus { get; set; }
        public byte? EReceiptSource { get; set; }
        public string SubmitStatusTitle { get; set; }
        public string Source { get; set; }
        public string Msg { get; set; }
        public int CountOfRefund { get; set; }
        public int? TotalCount { get; set; }
    }
    //public class EReceiptRefundLogsModel
    //{
    //    public long LogID { get; set; }
    //    //public long EReceiptID { get; set; }
    //    public int LogTypeID { get; set; }
    //    public string EReceiptNumber { get; set; }
    //    public System.DateTime LogDatetime { get; set; }
    //    public bool IsSendTosubmit { get; set; }
    //    public byte SubmitStatus { get; set; }
    //    public string ReuestPayload { get; set; }
    //    public string ResponsePayload { get; set; }
    //    public string UUID { get; set; }
    //    public string QRCode { get; set; }
    //    public string ETARequestPayload { get; set; }
    //    public string ETAResponsePayload { get; set; }
    //    public string ETAQRCode { get; set; }
    //    public string ETAUUID { get; set; }
    //    public string Error { get; set; }
    //    public byte EReceiptSource { get; set; }
    //    public string EReceiptStatus { get; set; }
    //    //public bool IsWincashSubmit { get; set; }
    //    public string Msg { get; set; }
    //    //public Nullable<long> IsRefundTo { get; set; }
    //}
}