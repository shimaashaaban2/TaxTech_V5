using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ViewModels
{
    public class DocumentViewModel
    {
        public int? DocumentID { get; set; }
        public int? ProcessStatusID { get; set; }
        public string ProcessStatusTitle { get; set; }
        public int? VendorID { get; set; }
        public string VendorName { get; set; }
        public int? OwnerID { get; set; }
        public int? DocumentType { get; set; }
        public string UUID { get; set; }
        public string MOFRejectStatus { get; set; }

        [JsonProperty("dateTimeIssued")]
        public string DateTimeIssued { get; set; }

        [JsonProperty("taxpayerActivityCode")]
        public string TaxpayerActivityCode { get; set; }
        public int? ActivityCode { get; set; }
        public string ActivityEN { get; set; }
        
        [JsonProperty("internalID")]
        public string InternalID { get; set; }
        
        [JsonProperty("taxTotals")]
        public double? TaxTotals { get; set; }

        [JsonProperty("totalAmount")]
        public double? TotalAmount { get; set; }

        [JsonProperty("totalSalesAmount")]
        public double? TotalSalesAmount { get; set; }

        [JsonProperty("totalDiscountAmount")]
        public double? TotalDiscountAmount { get; set; }

        [JsonProperty("netAmount")]
        public double? NetAmount { get; set; }

        [JsonProperty("extraDiscountAmount")]
        public double? ExtraDiscountAmount { get; set; }

        [JsonProperty("totalItemsDiscountAmount")]
        public double? TotalItemsDiscountAmount { get; set; }

        public string VendorTypeList { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string ISO2 { get; set; }

        [JsonProperty("governate")]
        public string Governate { get; set; }

        [JsonProperty("regionCity")]
        public string RegionCity { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("buildingNumber")]
        public string BuildingNumber { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("floor")]
        public string Floor { get; set; }

        [JsonProperty("flat")]
        public string Flat { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }

        [JsonProperty("additionalInformation")]
        public string AdditionalInformation { get; set; }

        public string VendorRegInfo { get; set; }

        [JsonProperty("purchaseOrderReference")]
        public string PurchaseOrderReference { get; set; }

        [JsonProperty("purchaseOrderDescription")]
        public string PurchaseOrderDescription { get; set; }

        [JsonProperty("salesOrderReference")]
        public string SalesOrderReference { get; set; }

        [JsonProperty("salesOrderDescription")]
        public string SalesOrderDescription { get; set; }

        [JsonProperty("proformaInvoiceNumber")]
        public string ProformaInvoiceNumber { get; set; }

        public int? PaymentAccountID { get; set; }
        public int? PaymentAccountERPID { get; set; }

        public string BankAccountNo { get; set; }
        public string BankAccountIBAN { get; set; }
        public string SwiftCode { get; set; }
        public string BranchAddress { get; set; }
        public int? BankID { get; set; }
        public string BankName { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryApproach { get; set; }

        public string DeliveryPackaging { get; set; }
        public string DeliveryDateValidty { get; set; }
        public string DeliveryExportPort { get; set; }
        public string DeliveryCOO { get; set; }
        public double? PackageGrowth { get; set; }
        public double? PackageNet { get; set; }
        public string DeliveryTerms { get; set; }
        public string LinkedInvoice { get; set; }
        public int? TotalCount { get; set; }
    }
}
