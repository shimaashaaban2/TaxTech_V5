using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.EReceiptApi;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.ApiModels.TaxUpdate;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Helpers
{
    public static class ExportHelper
    {
        public static XLWorkbook ExportVendors(IEnumerable<VendorViewModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Vendor ID", typeof(string));
            dt.Columns.Add("Vendor Name", typeof(string));
            dt.Columns.Add("Erp ID", typeof(string));
            dt.Columns.Add("Entity", typeof(string));
            dt.Columns.Add("Country", typeof(string));
            dt.Columns.Add("Governate", typeof(string));
            dt.Columns.Add("Region City", typeof(string));
            dt.Columns.Add("Street", typeof(string));
            dt.Columns.Add("Building No.", typeof(string));
            dt.Columns.Add("Postal Code", typeof(string));
            dt.Columns.Add("Floor", typeof(string));
            dt.Columns.Add("Flat", typeof(string));
            dt.Columns.Add("Landmark", typeof(string));
            dt.Columns.Add("Addtional Info", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.VendorID;
                row[1] = item.VendorName;
                row[2] = item.ERPInternalID;
                row[3] = item.EntityTitle;
                row[4] = item.CountryName;
                row[5] = item.Governate;
                row[6] = item.RegionCity;
                row[7] = item.Street;
                row[8] = item.BuildingNo;
                row[9] = item.PostalCode;
                row[10] = item.Floor;
                row[11] = item.Flat;
                row[12] = item.Landmark;
                row[13] = item.AdditionalInformation;
                row[14] = item.IsActive == true ? "Active" : "InActive";
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportBranches(IEnumerable<BranchViewModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Branch Code", typeof(string));
            dt.Columns.Add("Branch Name", typeof(string));
            dt.Columns.Add("Erp ID", typeof(string));
            dt.Columns.Add("Country", typeof(string));
            dt.Columns.Add("Governate", typeof(string));
            dt.Columns.Add("Region City", typeof(string));
            dt.Columns.Add("Street", typeof(string));
            dt.Columns.Add("Building No.", typeof(string));
            dt.Columns.Add("Postal Code", typeof(string));
            dt.Columns.Add("Floor", typeof(string));
            dt.Columns.Add("Flat", typeof(string));
            dt.Columns.Add("Landmark", typeof(string));
            dt.Columns.Add("Addtional Info", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.BranchCode;
                row[1] = item.BranchName;
                row[2] = item.ERPInternalID;
                row[3] = item.CountryName;
                row[4] = item.Governate;
                row[5] = item.RegionCity;
                row[6] = item.Street;
                row[7] = item.BuildingNo;
                row[8] = item.PostalCode;
                row[9] = item.Floor;
                row[10] = item.Flat;
                row[11] = item.Landmark;
                row[12] = item.AdditionalInformation;
                row[13] = item.IsActive == true ? "Active" : "InActive";
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportItems(IEnumerable<ItemViewModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("ERP ID", typeof(string));
            dt.Columns.Add("Item Code", typeof(string));
            dt.Columns.Add("Item Name", typeof(string));
            dt.Columns.Add("Unit Title EN", typeof(string));
            dt.Columns.Add("Item Type", typeof(string));
            dt.Columns.Add("Entity", typeof(string));
            dt.Columns.Add("Item Serial", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.ItemERPID;
                row[1] = item.ItemCode;
                row[2] = item.UnitTitleEN;
                row[3] = item.ItemName;
                row[4] = item.ItemType;
                row[5] = item.EntityTitle;
                row[6] = item.ItemSerial;
                row[7] = item.IsActive == true ? "Active" : "InActive";
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportAllDocs(IEnumerable<DocumentViewModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Document ID", typeof(string));
            dt.Columns.Add("Document Status", typeof(string));
            dt.Columns.Add("Document Type", typeof(string));
            //dt.Columns.Add("Details", typeof(string));
            dt.Columns.Add("Internal ID", typeof(string));
            dt.Columns.Add("Date Time Issued", typeof(string));
            dt.Columns.Add("Vendor Name", typeof(string));
            dt.Columns.Add("Activity Code", typeof(int));
            dt.Columns.Add("Activity Title", typeof(string));
            dt.Columns.Add("Bank Name", typeof(string));
            dt.Columns.Add("Net Total", typeof(decimal));
            dt.Columns.Add("Tax Total", typeof(decimal));
            dt.Columns.Add("Total Amount", typeof(decimal));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.DocumentID;
                row[1] = item.ProcessStatusTitle;
                row[2] = item.DocumentType;
                row[3] = item.InternalID;
                row[4] = item.DateTimeIssued;
                row[5] = item.VendorName;
                row[6] = item.ActivityCode;
                row[7] = item.ActivityEN;
                row[8] = item.BankName;
                row[9] = item.TotalAmount - item.TaxTotals;
                row[10] = item.TaxTotals;
                row[11] = item.TotalAmount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportInvoices(IEnumerable<DocumentViewModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Document ID", typeof(string));
            dt.Columns.Add("Document Status", typeof(string));
            dt.Columns.Add("Document Type", typeof(string));
            dt.Columns.Add("Details", typeof(string));
            dt.Columns.Add("Vendor Name", typeof(string));
            dt.Columns.Add("int", typeof(string));
            dt.Columns.Add("Activity Title", typeof(string));
            dt.Columns.Add("Bank Name", typeof(string));
            dt.Columns.Add("Net Total", typeof(decimal));
            dt.Columns.Add("Tax Total", typeof(decimal));
            dt.Columns.Add("Total Amount", typeof(decimal));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.DocumentID;
                row[1] = item.ProcessStatusTitle;
                row[2] = item.DocumentType;
                row[3] = $"{item.InternalID} - {item.DateTimeIssued}";
                row[4] = item.VendorName;
                row[5] = item.ActivityCode;
                row[6] = item.ActivityEN;
                row[7] = item.BankName;
                row[8] = item.TotalAmount - item.TaxTotals;
                row[9] = item.TaxTotals;
                row[10] = item.TotalAmount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportRecievedDocs(IEnumerable<RecentDocument> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("UUID", typeof(string));
            dt.Columns.Add("Submission UUID", typeof(string));
            dt.Columns.Add("Details", typeof(string));
            dt.Columns.Add("Document Type", typeof(string));
            dt.Columns.Add("Issuer Id", typeof(string));
            dt.Columns.Add("Issuer Name", typeof(string));
            dt.Columns.Add("DateTime Received", typeof(string));
            dt.Columns.Add("Total Sales", typeof(decimal));
            dt.Columns.Add("Total Discount", typeof(decimal));
            dt.Columns.Add("Net Amount", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Status", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.UUID;
                row[1] = item.SubmissionUUID;
                row[2] = $"{item.InternalId} - {item.DateTimeIssued}";
                row[3] = item.DocumentTypeNamePrimaryLang;
                row[4] = item.IssuerId;
                row[5] = item.IssuerName;
                row[6] = item.DateTimeReceived;
                row[7] = item.TotalSales;
                row[8] = item.TotalDiscount;
                row[9] = item.NetAmount;
                row[10] = item.Total;
                row[11] = item.Status;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }
        public static XLWorkbook ExportSearchDocs(IEnumerable<SearchDocument> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("UUID", typeof(string));
            dt.Columns.Add("Submission UUID", typeof(string));
            dt.Columns.Add("Details", typeof(string));
            dt.Columns.Add("Document Type", typeof(string));
            dt.Columns.Add("Receiver Id", typeof(string));
            dt.Columns.Add("Receiver Name", typeof(string));
            dt.Columns.Add("DateTime Issued", typeof(string));
            dt.Columns.Add("Total Sales", typeof(decimal));
            dt.Columns.Add("Total Discount", typeof(decimal));
            dt.Columns.Add("Net Amount", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Status", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.uuid;
                row[1] = item.submissionUUID;
                row[2] = $"{item.internalId} - {item.dateTimeIssued}";
                row[3] = item.documentTypeNamePrimaryLang;
                row[4] = item.receiverId;
                row[5] = item.receiverName;
                row[6] = item.dateTimeIssued;
                row[7] = item.totalSales;
                row[8] = item.totalDiscount;
                row[9] = item.netAmount;
                row[10] = item.total;
                row[11] = item.status;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportFailedImportedDocuments(IEnumerable<JobQueueDetails> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Invoice Internal ID", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Import Result", typeof(string));
            dt.Columns.Add("Row Index", typeof(string));
            dt.Columns.Add("Sheet Name", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.DocID;
                row[1] = item.DocDate;
                row[2] = item.ImportResult;
                row[3] = item.ExcelRowIndex;
                row[4] = item.ExcelSheetName;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportGlobalLogDocuments(IEnumerable<JobQueueEnhancedDetailsApiModel> list, string fileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Internal ID", typeof(string));
            dt.Columns.Add("Error Message", typeof(string));
            dt.Columns.Add("Error Description", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.InternalID;
                row[1] = item.Error;
                row[2] = item.ErrorDetails;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }
        public static XLWorkbook ExportAllReceipts(IEnumerable<ReceiptListModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Date Time Issued", typeof(string));
            dt.Columns.Add("EReceipt Number", typeof(string));
            dt.Columns.Add("QRCode", typeof(string));
            dt.Columns.Add("UUID", typeof(string));
            dt.Columns.Add("receipt Type", typeof(string));
            dt.Columns.Add("ETAUUID", typeof(string));
            dt.Columns.Add("EReceiptStatus", typeof(string));
            dt.Columns.Add("EReceiptSource", typeof(string));
            dt.Columns.Add("SubmitStatusTitle", typeof(string));
            dt.Columns.Add("buyerName", typeof(string));
            dt.Columns.Add("buyerID", typeof(string));
            dt.Columns.Add("buyerPhone", typeof(string));
            dt.Columns.Add("PersonalID", typeof(string));
            dt.Columns.Add("totalAmount", typeof(decimal));
            dt.Columns.Add("netAmount", typeof(decimal));
            dt.Columns.Add("taxTotals", typeof(decimal));
            //dt.Columns.Add("totalAmount", typeof(decimal));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
              
                row[0] = item.dateTimeIssued; 
                row[1] = item.receiptNumber;
                row[2] = item.QRCode;
                row[3] = item.UUID;
                row[4] = item.receiptType;
                row[5] = item.ETAUUID;
                row[6] = item.EReceiptStatus;
                row[7] = item.EReceiptSource;
                row[8] = item.SubmitStatusTitle;
                row[9] = item.buyerName;
                row[10] = item.buyerID;
                row[11] = item.buyerPhone;
                row[12] = item.PersonalID;
                row[13] = item.totalAmount;
                row[14] = item.netAmount;
                row[15] = item.taxTotals;
              
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }
        #region Reports
        public static XLWorkbook ExportVendorsTotals(IEnumerable<VendorTotals> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Vendor ID", typeof(string));
            dt.Columns.Add("Vendor Name", typeof(string));
            dt.Columns.Add("Documents Count", typeof(int));
            dt.Columns.Add("Tax Total", typeof(decimal));
            dt.Columns.Add("Total Amount", typeof(decimal));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.VendorID;
                row[1] = item.VendorName;
                row[2] = item.DocCount;
                row[3] = item.TaxSum;
                row[4] = item.TotalAmount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportBranchesTotals(IEnumerable<BranchTotals> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Owner ID", typeof(string));
            dt.Columns.Add("Branch Name", typeof(string));
            dt.Columns.Add("Documents Count", typeof(int));
            dt.Columns.Add("Tax Total", typeof(decimal));
            dt.Columns.Add("Total Amount", typeof(decimal));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.OwnerID;
                row[1] = item.BranchName;
                row[2] = item.DocCount;
                row[3] = item.TaxSum;
                row[4] = item.TotalAmount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportItemsTotals(IEnumerable<ItemsTotals> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Item Name", typeof(string));
            dt.Columns.Add("Document Number", typeof(decimal));
            dt.Columns.Add("Tax Total", typeof(decimal));
            dt.Columns.Add("Total Amount", typeof(decimal));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.ItemName;
                row[1] = item.DocNo;
                row[2] = item.TaxTotal;
                row[3] = item.TotalAmount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportDocCount(IEnumerable<DocumentTotalReport> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Process Status Title", typeof(string));
            dt.Columns.Add("Documents Count", typeof(int));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.ProcessStatusTitle;
                row[1] = item.DocCount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportDocValues(IEnumerable<DocumentTotalReport> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Process Status Title", typeof(string));
            dt.Columns.Add("Tax Totals", typeof(decimal));
            dt.Columns.Add("Total Amount", typeof(decimal));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.ProcessStatusTitle;
                row[1] = item.TaxTotals;
                row[2] = item.TotalAmount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportDocsRejectedValues(IEnumerable<RejectedDocumentReport> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("MOF Reject Status", typeof(string));
            dt.Columns.Add("Documents Count", typeof(int));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.MOFRejectStatus;
                row[1] = item.DocCount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportFailedImportedDocsValues(IEnumerable<JobQueueDetails> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Job ID", typeof(string));
            dt.Columns.Add("Document ID", typeof(string));
            dt.Columns.Add("Document Date", typeof(string));
            dt.Columns.Add("Excel Row Index", typeof(string));
            dt.Columns.Add("Excel Sheet Name", typeof(string));
            dt.Columns.Add("Import Result", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.JobID;
                row[1] = item.DocID;
                row[2] = item.DocDate;
                row[3] = item.ExcelRowIndex;
                row[4] = item.ExcelSheetName;
                row[5] = item.ImportResult;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportFullReceipts(IEnumerable<EReceiptLogsModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Log Date time", typeof(string));
            dt.Columns.Add("EReceipt Number", typeof(string));
            dt.Columns.Add("QRCode", typeof(string));
            dt.Columns.Add("UUID", typeof(string));
            // dt.Columns.Add("receipt Type", typeof(string));
            dt.Columns.Add("ETAUUID", typeof(string));
            dt.Columns.Add("ETAQRCode", typeof(string));
            dt.Columns.Add("EReceiptStatus", typeof(string));
            dt.Columns.Add("EReceiptSource", typeof(string));
            dt.Columns.Add("Submit Status Title", typeof(string));
            dt.Columns.Add("Message", typeof(string));
            //dt.Columns.Add("buyerName", typeof(string));
            //dt.Columns.Add("buyerID", typeof(string));
            //dt.Columns.Add("buyerPhone", typeof(string));
            //dt.Columns.Add("PersonalID", typeof(string));
            //dt.Columns.Add("totalAmount", typeof(decimal));
            //dt.Columns.Add("netAmount", typeof(decimal));
            //dt.Columns.Add("taxTotals", typeof(decimal));
            //dt.Columns.Add("totalAmount", typeof(decimal));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();

                row[0] = String.Format("{0:yyyy-MM-dd}", item.LogDatetime); ;
                row[1] = item.EReceiptNumber;
                row[2] = item.QRCode;
                row[3] = item.UUID;
                // row[4] = item.receiptType;
                row[4] = item.ETAUUID;
                row[5] = item.ETAQRCode;
                row[6] = item.EReceiptStatus;
                row[7] = item.EReceiptSource;
                row[8] = item.SubmitStatusTitle;
                row[9] = item.Msg;


                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }
        public static XLWorkbook ExportTotalReports(IEnumerable<TotalReportsModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add(@Tax_Tech.Resources.Resource.Date, typeof(string));
            dt.Columns.Add(@Tax_Tech.Resources.Resource.Sources, typeof(string));
            dt.Columns.Add(@Tax_Tech.Resources.Resource.ReceiptType, typeof(string));
            dt.Columns.Add(@Tax_Tech.Resources.Resource.SalesTotal, typeof(decimal));
            dt.Columns.Add(@Tax_Tech.Resources.Resource.ItemsDiscount, typeof(decimal));
            dt.Columns.Add(@Tax_Tech.Resources.Resource.NetTotal, typeof(decimal));
            dt.Columns.Add(@Tax_Tech.Resources.Resource.TaxTotals, typeof(decimal));
            dt.Columns.Add(@Tax_Tech.Resources.Resource.extraDiscountAmount, typeof(decimal));


            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = String.Format("{0:yyyy-MM-dd}", item.dateTimeIssued);
                row[1] = item.Source;
                row[2] = item.receiptType;
                row[3] = item.SalesTotal;
                row[4] = item.ItemsDiscount;
                row[5] = item.NetTotal;
                row[6] = item.TaxTotals;
                row[7] = item.extraDiscountAmount;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportItems(IEnumerable<AllItemsNotExist> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add(@Tax_Tech.Resources.Resource.InternalID, typeof(string));
            dt.Columns.Add(@Tax_Tech.Resources.Resource.Description, typeof(string));


            foreach (var item in list)
            {
                DataRow row = dt.NewRow();

                row[0] = item.internalCode;
                row[1] = item.description;



                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }

        public static XLWorkbook ExportJobListToExcel(IEnumerable<JobTrackingModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("job ID", typeof(string));
            dt.Columns.Add("total Count", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Head", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Lines", typeof(string));
           

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.jobID;
                row[1] = item.totalCount;
                row[2] = item.status;
                row[3] = item.insertedDocuments;
                row[3] = item.notInsertedDocuments;
                row[4] = item.createdAt;
                row[4] = item.updatedAt;
                row[5] = item.insertedLines;
                row[5] = item.notInsertedLines;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }
        public static XLWorkbook ExportJobIdToExcel(IEnumerable<JobTrackingModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("job ID", typeof(string));
            dt.Columns.Add("total Count", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Head", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Lines", typeof(string));


            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.jobID;
                row[1] = item.totalCount;
                row[2] = item.status;
                row[3] = item.insertedDocuments;
                row[3] = item.notInsertedDocuments;
                row[4] = item.createdAt;
                row[4] = item.updatedAt;
                row[5] = item.insertedLines;
                row[5] = item.notInsertedLines;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }
        public static XLWorkbook ExportEReceiptJobIdToExcel(IEnumerable<EreceiptJobTrackingModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("job ID", typeof(string));
            dt.Columns.Add("total Count", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Head", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Lines", typeof(string));


            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                dt.Columns.Add("job ID", typeof(string));
                dt.Columns.Add("total Count", typeof(string));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Head", typeof(string));
                dt.Columns.Add("Date", typeof(string));
                dt.Columns.Add("Lines", typeof(string));
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }



        public static XLWorkbook ExportEReceiptJobListToExcel(IEnumerable<EreceiptJobTrackingModel> list, string FileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("job ID", typeof(string));
            dt.Columns.Add("total Count", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Head", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Lines", typeof(string));


            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row[0] = item.jobID;
                row[1] = item.totalCount;
                row[2] = item.status;
                row[3] = item.insertedHeader;
                row[4] = item.createdAt;
                row[5] = item.insertedLines;
              
                
              
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            if (ds.Tables.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return wb;
            }
            return null;
        }
        #endregion

        #region ErrorLog
        //public XLWorkbook ExportErroLogByDateAndName(IEnumerable<Filters_Log_ErrorTrackGetByDateAndName_Result> list, string FileName)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Error Date", typeof(string));
        //    dt.Columns.Add("App Name", typeof(string));
        //    dt.Columns.Add("Method Name", typeof(string));
        //    dt.Columns.Add("Page Name", typeof(string));
        //    dt.Columns.Add("Error Data", typeof(string));
        //    dt.Columns.Add("Description", typeof(string));

        //    foreach (var item in list)
        //    {
        //        DataRow row = dt.NewRow();
        //        row[0] = item.ErrorDate;
        //        row[1] = item.AppName;
        //        row[2] = item.MethodName;
        //        row[3] = item.PageName;
        //        row[4] = item.ErrorData;
        //        row[5] = item.Description;
        //        dt.Rows.Add(row);
        //    }
        //    ds.Tables.Add(dt);
        //    if (ds.Tables.Count > 0)
        //    {
        //        XLWorkbook wb = new XLWorkbook();
        //        wb.Worksheets.Add(ds);
        //        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        wb.Style.Font.Bold = true;
        //        return wb;
        //    }
        //    return null;
        //}
        #endregion
    }
}