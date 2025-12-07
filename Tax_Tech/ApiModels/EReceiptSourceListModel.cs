using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class EReceiptSourceListModel
    {

        public List<EReceiptDataSource> Data { get; set; }
        public int totalCount { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string logtype { get; set; }
     

    }
    public class EReceiptDataSource
    {
        public long eReceiptID { get; set; }
        public DateTime dateTimeIssued { get; set; }
        public string receiptNumber { get; set; }
        public string uuid { get; set; }
        public string previousUUID { get; set; }
        public string referenceOldUUID { get; set; }
        public string currency { get; set; }
        public string exchangeRate { get; set; }
        public string receiptType { get; set; }
        public string typeVersion { get; set; }
        public string buyerType { get; set; }
        public string buyerID { get; set; }
        public string buyerName { get; set; }
        public string buyerPhone { get; set; }
        public string qrCode { get; set; }
        public string etaqrCode { get; set; }
        public string etauuid { get; set; }
        public byte eReceiptSource { get; set; }
        public string eReceiptSourceTitle { get; set; }
        public string eReceiptStatus { get; set; }
        public bool isDeleted { get; set; }
        public string recordStatus { get; set; }
        public string storeCode { get; set; }
        public string source { get; set; }
    }
}

/*
 *  "eReceiptID": 59204,
      "dateTimeIssued": "2025-11-16T13:55:40.293",
      "receiptNumber": "B0000122359102025",
      "uuid": null,
      "previousUUID": null,
      "referenceOldUUID": null,
      "currency": "80642",
      "exchangeRate": null,
      "receiptType": "3",
      "typeVersion": null,
      "buyerType": null,
      "buyerID": "1.195629404",
      "buyerName": "NotExist",
      "buyerPhone": null,
      "qrCode": null,
      "etaqrCode": null,
      "etauuid": null,
      "eReceiptSource": 1,
      "eReceiptSourceTitle": "System",
      "eReceiptStatus": "NA",
      "eReceiptStatusTitle": "Not Available",
      "isDeleted": false,
      "recordStatus": "Active",
      "storeCode": "1",
      "source": "EreceiptBilling"
 */