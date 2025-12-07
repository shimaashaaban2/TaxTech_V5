using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.Classes;
using Tax_Tech.Helpers;
using Tax_Tech.Models;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Repository
{
    public class InvoiceApiRepository
    {

        //        SELECT dbo.Document_Basic.DocumentID, dbo.Document_Basic.ProcessStatusID, dbo.Lup_ProcessStatus.ProcessStatusTitle, dbo.Document_Basic.VendorID, dbo.Basic_Vendors.VendorName, dbo.Document_Basic.OwnerID,
        //                         dbo.Document_Basic.DocumentType, dbo.Document_Basic.dateTimeIssued, dbo.Document_Basic.taxpayerActivityCode, dbo.Lup_ActivityList.ActivityCode, dbo.Lup_ActivityList.ActivityEN, dbo.Document_Basic.internalID,
        //                         dbo.Document_Totals.taxTotals, dbo.Document_Totals.totalAmount, dbo.Document_Basic.UUID, dbo.Lup_DocumentTypeList.DocumentTypeTitle, dbo.Basic_Branches.BranchName,
        //                         dbo.Log_RejectResultAfterSubmission.ErrorDetails AS MOFRejectStatus, dbo.Document_Basic.EntityID
        //FROM            dbo.Document_Basic INNER JOIN
        //                         dbo.Lup_DocumentTypeList ON dbo.Document_Basic.DocumentType = dbo.Lup_DocumentTypeList.DocumetTypeID INNER JOIN
        //                         dbo.Basic_Branches ON dbo.Document_Basic.OwnerID = dbo.Basic_Branches.BranchID INNER JOIN
        //                         dbo.Lup_ProcessStatus ON dbo.Document_Basic.ProcessStatusID = dbo.Lup_ProcessStatus.ProcessStatusID INNER JOIN
        //                         dbo.Document_Totals ON dbo.Document_Basic.DocumentID = dbo.Document_Totals.DocumentID INNER JOIN
        //                         dbo.Basic_Vendors ON dbo.Document_Basic.VendorID = dbo.Basic_Vendors.VendorID INNER JOIN
        //                         dbo.Log_RejectResultAfterSubmission ON dbo.Document_Basic.DocumentID = dbo.Log_RejectResultAfterSubmission.DocID LEFT OUTER JOIN
        //                         dbo.Lup_ActivityList ON dbo.Document_Basic.taxpayerActivityCode = dbo.Lup_ActivityList.ActivityCode
        //WHERE        (dbo.Document_Basic.ProcessStatusID = 6)
        public IEnumerable<DocumentViewModel> GetInvoiceListLastWeek(long EntityID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListLastWeek?EntityID=" + EntityID, null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New GetInvoiceListLastWeek
        public IEnumerable<DocumentViewModel> NewGetInvoiceListLastWeek(long EntityID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListLastWeek?EntityID=" + EntityID, null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public IEnumerable<DocumentViewModel> GetInvoiceListLastWeek(long? entityId, int? pageNo, int? pageSize)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListLastWeekWithPagination?entityId={entityId}&pageNo={pageNo}&pageSize={pageSize}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(content);
        }

        #region New GetInvoiceListLastWeek
        public IEnumerable<DocumentViewModel> GetMasterReportPagination(MasterReportViewModel model)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetMasterReportPagination?ReportType={model.ReportType}&InternalID={model.InternalID}&UUID={model.UUID}&InputType={model.InputType}&DateFrom={model.DateFrom}&DateTo={model.DateTo}&ProccessStatusID={model.ProccessStatusID}&AccountID={model.AccountID}&UserId={model.UserId}&DocumentType={model.DocumentType}&EntityID={model.EntityID}&pageNo={model.pageNo}&pageSize={model.pageSize}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(content);
        }
        public IEnumerable<DocumentViewModel> GetMasterReportExcel(MasterReportViewModel model)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetMasterReportExcel?ReportType={model.ReportType}&InternalID={model.InternalID}&UUID={model.UUID}&InputType={model.InputType}&DateFrom={model.DateFrom}&DateTo={model.DateTo}&ProccessStatusID={model.ProccessStatusID}&AccountID={model.AccountID}&UserId={model.UserId}&DocumentType={model.DocumentType}&EntityID={model.EntityID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(content);
        }
        #endregion

        public IEnumerable<SearchDocument> GetDocumentByID(string InternalID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Document/Search/GetByInternalID?InternalID={InternalID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<SearchDocument>>(content);
        }

        public IEnumerable<SearchDocument> GetDocumentByUUID(string UUID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Document/Search/GetByUUID?UUID={UUID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<SearchDocument>>(content);
        }

        public IEnumerable<SearchDocument> GetDocumentByFilters(DocumentByFiltersModel model)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Document/Search/GetFilter?From={model.From}&To={model.To}&DocumentType={model.DocumentType}&MOStatus={model.MOStatus}&pageNo={model.pageNo}&pageSize={model.pageSize}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<SearchDocument>>(content);
        }
        #region GetMasterReport
        public IEnumerable<DocumentViewModel> GetMasterReport(long JobID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetMasterReportByJobID?JobID={JobID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(content);
        }
        #endregion

        public IEnumerable<DocumentViewModel> GetSingleDocumentView(long docId)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SingleDocumentView?DocumentID={docId}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(result);
        }


        #region New GetSingleDocument
        public IEnumerable<DocumentViewModel> NewGetSingleDocumentView(long docId)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SingleDocumentView?DocumentID={docId}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(result);
        }
        #endregion

        public IEnumerable<InvoiceLineViewModel> GetLines(long docId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SingleDocDetailsView?DocumentID={docId}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<InvoiceLineViewModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New GetLines
        public IEnumerable<InvoiceLineViewModel> NewGetLines(long docId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SingleDocDetailsView?DocumentID={docId}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<InvoiceLineViewModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Gets list of documents by status
        /// </summary>
        /// <param name="status">Document Status: 
        /// 7 for Rejected Locally
        /// 6 for MOF Rejected
        /// 5 for MOF Accepted
        /// </param>
        /// <returns></returns>
        public IEnumerable<DocumentViewModel> GetDocumentsOf(int? status, long EntityID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListByStatus?ProcessStatusID={status}&EntityID={EntityID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #region New GetDocumentsOf
        public IEnumerable<DocumentViewModel> NewGetDocumentsOf(int? status, long EntityID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListByStatus?ProcessStatusID={status}&EntityID={EntityID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
        public IEnumerable<DocumentViewModel> GetDocumentOf(int? status, long DocID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceByStatus?ProcessStatusID={status}&DocID={DocID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New GetDocumentOf
        public IEnumerable<DocumentViewModel> NewGetDocumentOf(int? status, long DocID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceByStatus?ProcessStatusID={status}&DocID={DocID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        public IEnumerable<DocumentActionLog> GetDocumentActionLog(long docID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceActionsLog?DocumentID={docID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentActionLog>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse CreateInvoiceHead(CreateInvoiceHead model, string docType)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/CreateInvoiceHead?VendorID={model.VendorID}&OwnerID={model.OwnerID}&dateTimeIssued={model.InvoiceIssueDate}&taxpayerActivityCode={model.ActivityCode}&internalID={model.InvoiceInternalID}&EntityID={model.EntityId}&ActionBy={model.ActionBy}&DocumentType={docType}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }

        #region New CreateInvoiceHead
        public ApiResponse NewCreateInvoiceHead(CreateInvoiceHead model, string docType)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/CreateInvoiceHead?VendorID={model.VendorID}&OwnerID={model.OwnerID}&dateTimeIssued={model.InvoiceIssueDate}&taxpayerActivityCode={model.ActivityCode}&internalID={model.InvoiceInternalID}&EntityID={model.EntityId}&ActionBy={model.ActionBy}&DocumentType={docType}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }
        #endregion
        #region CancellInvoice
        public ApiResponse CancellInvoice(string status, string docId, string processMsg, long actionBy)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/UpdateInvoiceStatus?ProcessStatusID={status}&DocumentID={docId}&ActionBy={actionBy}&ProcessMessage={processMsg}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }
        #endregion

        #region CancellManyInvoices
        public ApiResponse CancellManyInvoice(long InvoiceID, string Reason)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Invoice/Reject?InvoiceID={InvoiceID}&Reason={Reason}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }
        #endregion

        public ApiResponse CreateInvoiceHeadV2(CreateInvoiceHead model, string docType)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v3/WEB/Documents/CreateInvoiceHead?VendorID={model.VendorID}&OwnerID={model.OwnerID}&dateTimeIssued={model.InvoiceIssueDate}&taxpayerActivityCode={model.ActivityCode}&internalID={model.InvoiceInternalID}&EntityID={model.EntityId}&ActionBy={model.ActionBy}&DocumentType={docType}&PurchaseOrderReference={model.PurchaseOrderReference}&PurchaseOrderDescription={model.PurchaseOrderDescription}&SalesOrderReference={model.SalesOrderReference}&SalesOrderDescription={model.SalesOrderDescription}&ProformaInvoiceNumber={model.ProformaInvoiceNumber}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }

        #region New CreateInvoiceHeadV2
        public ApiResponse NewCreateInvoiceHeadV2(CreateInvoiceHead model, string docType)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v3/WEB/Documents/CreateInvoiceHead?VendorID={model.VendorID}&OwnerID={model.OwnerID}&dateTimeIssued={model.InvoiceIssueDate}&taxpayerActivityCode={model.ActivityCode}&internalID={model.InvoiceInternalID}&EntityID={model.EntityId}&ActionBy={model.ActionBy}&DocumentType={docType}&PurchaseOrderReference={model.PurchaseOrderReference}&PurchaseOrderDescription={model.PurchaseOrderDescription}&SalesOrderReference={model.SalesOrderReference}&SalesOrderDescription={model.SalesOrderDescription}&ProformaInvoiceNumber={model.ProformaInvoiceNumber}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }
        #endregion

        public ApiResponse CreateInvoiceLine(InvoiceLineCreateViewModel model)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/CreateInvoicLineManual?InvoiceInternalID={model.InvoiceInternalID}&ItemID={model.ItemID}&description={model.Description}&quantity={model.Quantity}&" +
                    $"currencySold={model.Currency}&amountSold={model.Amount}&currencyExchangeRate={model.ExchangeRate}&ItemsDiscount={model.Discount}&ActionBy={model.ActionBy}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ItemViewModel> GetItems(bool activeList)
        {
            try
            {
                HttpResponseMessage result = null;

                if (activeList)
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsActiveList", null);
                }
                else
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsList", null);
                }

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ItemViewModel> GetActiveItemsList()
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsActiveList", null);
                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ItemsTaxViewModel> GetItemsTax(int itemId)
        {
            try
            {
                HttpResponseMessage result = null;

                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsTaxActiveList?ItemID={itemId}", null);


                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<ItemsTaxViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse ChangeInvoiceStatus(string status, string docId, string processMsg, long actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/UpdateInvoiceStatus?ProcessStatusID={status}&DocumentID={docId}&ActionBy={actionBy}&ProcessMessage={processMsg}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New ChangeInvoiceStatus
        public ApiResponse NewChangeInvoiceStatus(string status, string docId, string processMsg, long actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/UpdateInvoiceStatus?ProcessStatusID={status}&DocumentID={docId}&ActionBy={actionBy}&ProcessMessage={processMsg}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        public ApiResponse InvoiceApproveAll(long actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/InvoiceApproveAll?ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New InvoiceApproveAll
        public ApiResponse NewInvoiceApproveAll(long actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/InvoiceApproveAll?ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public string GetActivityCodes(int entityId)
        {
            string ActivityCode = "";
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ReadStringKey?EntityID={entityId}&ConfigKey=3", null);

                result.EnsureSuccessStatusCode();

                string content = result.Content.ReadAsStringAsync().Result;
                CustomViewModel _ApiResponse = JsonConvert.DeserializeObject<CustomViewModel>(content);
                string Result = "";
                if (_ApiResponse != null)
                {
                    if (_ApiResponse.CustomeRespons.TryGetValue("Response ID", out Result))
                    {
                        if (Result == "0")
                        {
                            if (_ApiResponse.CustomeRespons.TryGetValue("Response MSG", out ActivityCode))
                            {

                            }
                        }
                    }
                }
                return ActivityCode;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<DocumentTotal> GetDocumentTotals(long? docId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceTotals?DocumentID={docId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentTotal>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New GetDocumentTotals
        public IEnumerable<DocumentTotal> NewGetDocumentTotals(long? docId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceTotals?DocumentID={docId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentTotal>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public IEnumerable<InvoiceLineViewModel> GetSingleInvoiceLine(long? lineId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceLine?LineID={lineId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<InvoiceLineViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse DeleteInvoiceLine(long? lineId, long? actionID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/DeleteInvoiceLine?LineID={lineId}&ActionBy={actionID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse UpdateInvoiceLine(InvoiceLineCreateViewModel model, long? actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/EditInvoicLineManual?LineID={model.LineID}&ItemID={model.ItemID}&description={model.Description ?? ""}&quantity={model.Quantity}&currencySold={model.Currency}&amountSold={model.Amount}&currencyExchangeRate={model.ExchangeRate}&ItemsDiscount={model.Discount}&ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse UpdateInvoiceTotals(long? docId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/UpdateInvoiceTotals?DocumentID={docId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse UpdateInvoiceTotalsWithDicount(long? docId, double? extraDiscount)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/UpdateInvoiceExtraDiscount?DocumentID={docId}&ExtraDiscountAmount={extraDiscount}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<DocumentViewModel> FilterDocuments(FilterViewModel model)
        {
            // date format 2021/4/1
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListFilter?StartDate={model.StartDate.Value.ToString("yyyy/MM/dd")}&EndDate={model.EndDate.Value.ToString("yyyy/MM/dd")}&DocumentType={model.DocumentType}&ProcessStatusID={model.ProcessStatusID}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(result);
        }

        public IEnumerable<DocumentViewModel> FilterDocumentsWithPagination(FilterViewModel model)
        {
            // date format 2021/4/1
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListFilterWithPagination?StartDate={model.StartDate.Value.ToString("yyyy/MM/dd")}&EndDate={model.EndDate.Value.ToString("yyyy/MM/dd")}&DocumentType={model.DocumentType}&ProcessStatusID={model.ProcessStatusID}&PageNo={model.PageNo}&PageSize={model.PageSize}&EntityID={model.EntityID}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(result);
        }
        //fatma 10-29-2023
        #region New FilterDocumentsWithPagination 
        public IEnumerable<DocumentViewModel> NewFilterDocumentsWithPagination(FilterViewModel model)
        {
            // date format 10-29-2023
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListFilterWithPagination?StartDate={model.StartDate.Value.ToString("yyyy/MM/dd")}&EndDate={model.EndDate.Value.ToString("yyyy/MM/dd")}&DocumentType={model.DocumentType}&ProcessStatusID={model.ProcessStatusID}&PageNo={model.PageNo}&PageSize={model.PageSize}&EntityID={model.EntityID}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(result);
        }
        #endregion

        public IEnumerable<TaxTotal> GetTaxTotals(string docId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceTaxtTotals?DocumentID={docId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<TaxTotal>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse CalculateItemTax(long? itemId, double? itemNet)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/CalculateItemTax?ItemID={itemId}&ItemNet={itemNet}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse SubmitDocument(long? DocumentID, long? EntityID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SubmitDocument?DocumentID={DocumentID}&EntityID={EntityID}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }
        #region New SubmitDocument
        public ApiResponse NewSubmitDocument(long? DocumentID, long? EntityID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SubmitDocument?DocumentID={DocumentID}&EntityID={EntityID}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }
        #endregion
        public ApiResponse SubmitDocumentV2(long? DocumentID, long? EntityID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v3/WEB/Documents/SubmitDocument?DocumentID={DocumentID}&EntityID={EntityID}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }
        #region New SubmitDocumentV2
        public ApiResponse NewSubmitDocumentV2(long? DocumentID, long? EntityID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v3/WEB/Documents/SubmitDocument?DocumentID={DocumentID}&EntityID={EntityID}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }
        #endregion

        public ApiResponse UpdateInvoiceHead(UpdateInvoiceHead model, long? docID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/UpdatenvoiceHead?VendorID={model.VendorID}&OwnerID={model.OwnerID}&dateTimeIssued={model.InvoiceIssueDate}&taxpayerActivityCode={model.ActivityCode}&internalID={model.InvoiceInternalIDHead}&DocumentID={docID}&ActionBy={model.ActionBy}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }

        #region New UpdateInvoiceHead
        public ApiResponse NewUpdateInvoiceHead(UpdateInvoiceHead model, long? docID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/UpdatenvoiceHead?VendorID={model.VendorID}&OwnerID={model.OwnerID}&dateTimeIssued={model.InvoiceIssueDate}&taxpayerActivityCode={model.ActivityCode}&internalID={model.InvoiceInternalIDHead}&DocumentID={docID}&ActionBy={model.ActionBy}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }
        #endregion

        public ApiResponse UpdateInvoiceHeadV2(UpdateInvoiceHead model, long? docID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v3/WEB/Documents/UpdatenvoiceHead?VendorID={model.VendorID}&OwnerID={model.OwnerID}&dateTimeIssued={model.InvoiceIssueDate}&taxpayerActivityCode={model.ActivityCode}&internalID={model.InvoiceInternalIDHead}&DocumentID={docID}&ActionBy={model.ActionBy}&PurchaseOrderReference={model.PurchaseOrderReference}&PurchaseOrderDescription={model.PurchaseOrderDescription}&SalesOrderReference={model.SalesOrderReference}&SalesOrderDescription={model.SalesOrderDescription}&ProformaInvoiceNumber={model.ProformaInvoiceNumber}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }

        #region New UpdateInvoiceHeadV2
        public ApiResponse NewUpdateInvoiceHeadV2(UpdateInvoiceHead model, long? docID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v3/WEB/Documents/UpdatenvoiceHead?VendorID={model.VendorID}&OwnerID={model.OwnerID}&dateTimeIssued={model.InvoiceIssueDate}&taxpayerActivityCode={model.ActivityCode}&internalID={model.InvoiceInternalIDHead}&DocumentID={docID}&ActionBy={model.ActionBy}&PurchaseOrderReference={model.PurchaseOrderReference}&PurchaseOrderDescription={model.PurchaseOrderDescription}&SalesOrderReference={model.SalesOrderReference}&SalesOrderDescription={model.SalesOrderDescription}&ProformaInvoiceNumber={model.ProformaInvoiceNumber}", null);

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }
        #endregion

        public IEnumerable<DocumentViewModel> GetLastWeekDocsByDocType(string docType, long EntityID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetDocByTypeLastWeek?DocumentType={docType}&EntityID={EntityID}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(response);
        }

        #region New GetLastWeekDocsByDocType
        public IEnumerable<DocumentViewModel> NewGetLastWeekDocsByDocType(string docType, long EntityID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetDocByTypeLastWeek?DocumentType={docType}&EntityID={EntityID}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(response);
        }
        #endregion

        public IEnumerable<DocumentViewModel> GetDocumentsOf(int? status, long entityId, int? pageNo, int? pageSize)
        {

            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListByStatusWithPagination?ProcessStatusID={status}&EntityID={entityId}&pageNo={pageNo}&pageSize={pageSize}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(result);
        }
        #region New GetDocumentsOf
        public IEnumerable<DocumentViewModel> NewGetDocumentsOf(int? status, long entityId, int? pageNo, int? pageSize)
        {

            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetInvoiceListByStatusWithPagination?ProcessStatusID={status}&EntityID={entityId}&pageNo={pageNo}&pageSize={pageSize}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(result);
        }
        #endregion

        public IEnumerable<DocumentViewModel> GetLastWeekDocsByDocType(string docType, long? entityId, int? pageNo, int? pageSize)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetDocByTypeLastWeekWithPagination?DocumentType={docType}&EntityID={entityId}&pageNo={pageNo}&pageSize={pageSize}", null);
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(content);
        }

        #region GetLastWeekDocsType
        public IEnumerable<DocumentViewModel> NewGetLastWeekDocsByDocType(string docType, long? entityId, int? pageNo, int? pageSize)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetDocByTypeLastWeekWithPagination?DocumentType={docType}&EntityID={entityId}&pageNo={pageNo}&pageSize={pageSize}", null);
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<IEnumerable<DocumentViewModel>>(content);
        }
        #endregion

        public ApiResponse InsertReceivedDocs(RecentDocument document)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/InsertRecivedDoc?publicUrl={document.PublicUrl}&uuid={document.UUID}&submissionUUID={document.SubmissionUUID}&longId={document.LongID}&internalId={document.InternalId}&typeName={document.TypeName}&documentTypeNamePrimaryLang={document.DocumentTypeNamePrimaryLang}&typeVersionName={document.TypeVersionName}&issuerId={document.IssuerId}&issuerName={document.IssuerName}&dateTimeIssued={document.DateTimeIssued}&dateTimeReceived={document.DateTimeReceived}&totalSales={document.TotalSales}&totalDiscount={document.TotalDiscount}&netAmount={document.NetAmount}&total={document.Total}&documentStatusReason={document.DocumentStatusReason}&status={document.Status}", null);

                httpResponseMessage.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<RecentDocument> GetReceivedDocuments()
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse("v2/WEB/Documents/GetRecivedInvoiceList", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<RecentDocument>>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<DocumentViewModel> GetInvoiceByInternalID(string internalID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SingleLocalDocumentView?internalID={internalID}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(response);
        }

        #region New GetInvoiceByInternalID
        public IEnumerable<DocumentViewModel> NewGetInvoiceByInternalID(string internalID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SingleLocalDocumentView?internalID={internalID}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(response);
        }
        #endregion

        public IEnumerable<DocumentViewModel> GetDocByInternalIDAndType(string internalID, int? docType)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/SingleLocalDocumentView?internalID={internalID}", null);

            return new ResponseResolver<DocumentViewModel>().ResolveListResponse(response).Where(d => d.DocumentType == docType).ToList();
        }

       
        public ApiResponse UpdateInvoiceLinks(string docsStr, string docID)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/UpdateLinkedInvoices?DocumentID={docID}&LinkedInvoice={docsStr}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse GetLinkedInvoices(string docId)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetLinkedInvoices?DocumentID={docId}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<DocumentViewModel> GetInvoicesList(string[] ids)
        {
            try
            {
                List<DocumentViewModel> docs = new List<DocumentViewModel>();

                foreach (string id in ids)
                {
                    var linkedInvoice = GetSingleDocumentView(Convert.ToInt64(id)).FirstOrDefault();
                    if (linkedInvoice != null)
                    {
                        docs.Add(linkedInvoice);
                    }
                }

                return docs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<RecentDocument> FilterReceivedDocs(RecievedDocsFilterViewModel model)
        {
            try
            {
                // date format 2021/4/1
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetRecivedInvoiceFilterd?DocType={model.DocumentTypetitle}&StartDate={model.StartDate.Value.ToString("yyyy/MM/dd")}&EndDate={model.EndDate.Value.ToString("yyyy/MM/dd")}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<RecentDocument>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region NewFilterReceivedDocs
        public IEnumerable<RecentDocument> NewFilterReceivedDocs(RecievedDocsFilterViewModel model)
        {
            try
            {
                // date format 2021/4/1
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetRecivedInvoiceFilterd?DocType={model.DocumentTypetitle}&StartDate={model.StartDate.Value.ToString("yyyy/MM/dd")}&EndDate={model.EndDate.Value.ToString("yyyy/MM/dd")}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<RecentDocument>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public ApiResponse GetDocumentEntity(long DocID)
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetDocumentEntity?DocID=" + DocID, null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New GetDocumentEntity
        public ApiResponse NewGetDocumentEntity(long DocID)
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Documents/GetDocumentEntity?DocID=" + DocID, null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public ApiResponseObject GetBranchPersonalID(long EntityID)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetBranchPersonalID?EntityID=" + EntityID, null);

            return new ResponseResolver<ApiResponseObject>().ResolveObjectResponse(result);
        }

        public IEnumerable<RecentDocument> GetReceivedDocuments(DateTime? from, DateTime? to)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Document/Received/Get?from={from}&to={to}", new { });

            return new ResponseResolver<RecentDocument>().ResolveListResponse(response);
        }

        #region New GetReceivedDocuments
        public IEnumerable<RecentDocument> NewGetReceivedDocuments(DateTime? from, DateTime? to)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Document/Received/Get?from={from}&to={to}", new { });

            return new ResponseResolver<RecentDocument>().ResolveListResponse(response);
        }
        #endregion

        public IEnumerable<RecentDocument> GetReceivedDocumentsByFilter(ReceivedDocumentSubmitFilterViewModel model)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Document/Received/Get?From={model.From}&To={model.To}&docTypeOption={model.docTypeOption}&mDocTypeID={model.mDocTypeID}&mOFStatusOption={model.mOFStatusOption}&mOStatus={model.mOStatus}&accountOption={model.accountOption}&accountName={model.accountName}", new { });

            return new ResponseResolver<RecentDocument>().ResolveListResponse(response);
        }

        public IEnumerable<SearchDocument> GetSearchDocuments(DateTime? from, DateTime? to, int? pageNo, int? pageSize)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Document/Search/Get?from={from}&to={to}&pageNo={pageNo}&pageSize={pageSize}", new { });

            return new ResponseResolver<SearchDocument>().ResolveListResponse(response);
        }

        #region New GetSearchDocuments
        public IEnumerable<SearchDocument> NewGetSearchDocuments(DateTime? from, DateTime? to, int? pageNo, int? pageSize)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Document/Search/Get?from={from}&to={to}&pageNo={pageNo}&pageSize={pageSize}", new { });

            return new ResponseResolver<SearchDocument>().ResolveListResponse(response);
        }
        #endregion
        public ApiResponse CancelDocument(long? invoiceId, string reason)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Invoice/Cancel?invoiceId={invoiceId}&Reason={reason}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }

        #region New CancelDocument
        public ApiResponse NewCancelDocument(long? invoiceId, string reason)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Invoice/Cancel?invoiceId={invoiceId}&Reason={reason}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }
        #endregion
        public ApiResponse RejectDocument(long? invoiceId, string reason)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Invoice/Reject?invoiceId={invoiceId}&reason={reason}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }

        //public DocumentPDFApiModel DownloadDocumentPdf(long? entityId, string uuid)
        //{
        //    ApiTokenRepository apiTokenRepository = ApiTokenRepository.GetAPI();
        //    apiTokenRepository.Client.BaseAddress = new Uri(PublicConfig.TaxPDFAPIUrl);

        //    HttpResponseMessage response = apiTokenRepository.PostResponse($"v1/WEB/Documents/PDF?UUID={uuid}&EntityID={entityId}", new { });

        //    return new ResponseResolver<DocumentPDFApiModel>().ResolveObjectResponse(response);
        //}
        public DocumentPDFApiModel DownloadDocumentPdf(string uuid)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Download/PDF?UUID={uuid}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<DocumentPDFApiModel>(content);
        }

        #region New DownloadDocumentPdf
        public DocumentPDFApiModel NewDownloadDocumentPdf(string uuid)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Download/PDF?UUID={uuid}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<DocumentPDFApiModel>(content);
        }
        #endregion

    }
}
