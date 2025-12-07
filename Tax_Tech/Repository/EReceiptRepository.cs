using DocumentFormat.OpenXml.EMMA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI.WebControls;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.EReceiptApi;
using Tax_Tech.ApiModels.InvoicingApi;
using Tax_Tech.Helpers;
using Tax_Tech.Models;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Repository
{
    public class EReceiptRepository
    {
        public IEnumerable<EReceiptLogsModel> GetEReceiptLogByReceiptNumber(string EreceiptNumber)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/EReceiptNumber?EReceiptNumber={EreceiptNumber}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
        public IEnumerable<EReceiptLogsModel> GetEReceiptByUUID(string EReceiptUUID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/UUID?UUID={EReceiptUUID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
        public IEnumerable<EReceiptLogsModel> GetEReceiptByDate(DateTime? Datefrom, DateTime? Dateto, long? status)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/DateRange?From={Datefrom}&To={Dateto}&Status={status}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
        public IEnumerable<BillingLogsModel> GetBillingLogs(DateTime? datefrom, DateTime? dateto)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/billing/DateRange?From={datefrom}&To={dateto}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<BillingLogsModel>>(content);
        }
        public IEnumerable<WinCashLogsModel> GetWinCashLogs(DateTime? dateFrom, DateTime? dateTo)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Wincash/DateRange?From={dateFrom}&To={dateTo}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<WinCashLogsModel>>(content);
        }
        public IEnumerable<EReceiptLogsModel> GetEReceiptfullLogs(EReceiptFullLogsModel model)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Full?From={model.From}&To={model.To}&EReceiptSource={model.EReceiptSource}&EReceiptStatus={model.EReceiptStatus}&SubmitStatus={model.SubmitStatus}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
        public IEnumerable<ReceiptListModel> GetReceiptListByNumber(string ReceiptNumber)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/EReceiptNumber?EReceiptNumber={ReceiptNumber}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<ReceiptListModel>>(content);
        }
        public IEnumerable<ReceiptListModel> GetReceiptListByUUID(string ReceiptUUID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/UUID?UUID={ReceiptUUID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<ReceiptListModel>>(content);
        }
        public IEnumerable<ReceiptListModel> GetFullReceiptList(FullReceiptListModel model)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/DateRange?From={model.From}&To={model.To}&EReceiptSource={model.EReceiptSource}&EReceiptStatus={model.EReceiptStatus}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<ReceiptListModel>>(content);
        }
        //  public IEnumerable<ReceiptListModel> GetFullReceiptList2(DateTime From, DateTime To, byte EReceiptSource, string EReceiptStatus)
        //{
        //    HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/DateRange?From={From}&To={To}&EReceiptSource={EReceiptSource}&EReceiptStatus={EReceiptStatus}", new { });
        //    var content = response.Content.ReadAsStringAsync().Result;

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception(content);
        //    }
        //    return JsonConvert.DeserializeObject<IEnumerable<ReceiptListModel>>(content);
        //}

        public IEnumerable<EReceiptLogsModel> GetPayLoadLog(long LogID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/LogID?LogID={LogID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
        public List<DashBoardModel> GetDashBoard(DateTime From, DateTime To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Dashboard/Submitted?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<List<DashBoardModel>>(content);
        }
        public List<StatusDashBoardModel> GetstatusDashBoard(DateTime From, DateTime To)
        {

            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Dashboard/ByStatus?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<List<StatusDashBoardModel>>(content);
        }
        public List<CompareStatusModel> CompareStatusDashBoard(DateTime From, DateTime To)
        {

            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Dashboard/SystemIntegrationSummary?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<List<CompareStatusModel>>(content);
        }
        public IEnumerable<EReceiptLinesModel> GetEReceiptLines(long? EReceiptID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/Erecipt/Lines?EReciptID={EReceiptID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLinesModel>>(content);
        }
        public IEnumerable<EReceiptSingleViewModel> GetEReceiptSingleView(long? EReceiptID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/Erecipt/SingleView?EReciptID={EReceiptID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptSingleViewModel>>(content);
        }
        public IEnumerable<ReceiptLinesTotals> ReceiptLinesTotals(long? EReceiptID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/WEB/Erecipt/LinesTotals?EReciptID={EReceiptID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<ReceiptLinesTotals>>(content);
        }
        public IEnumerable<BillingValidationLogModel> billingValidation(string EReceiptNumber)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/ValidationLogs/Billing/EReceiptNumber?EReceiptNumber={EReceiptNumber}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<BillingValidationLogModel>>(content);
        }
        public IEnumerable<BillingValidationLogModel> billingValidationDateRange(string From, string To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/ValidationLogs/Billing/ByDate?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<BillingValidationLogModel>>(content);
        }
        public IEnumerable<ReceiptListModel> EReceiptQRCodePopup(long EReceiptID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/EReciptID?EReciptID={EReceiptID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<ReceiptListModel>>(content);
        }
        public IEnumerable<EReceiptTrackingModel> EReceiptTracking(string eReceiptnumber)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Tracking?EReceiptNumber={eReceiptnumber}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptTrackingModel>>(content);
        }
        public IEnumerable<BillingTrackingModel> BillingTracking(string eReceiptnumber)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Billing/Tracking?EReceiptNumber={eReceiptnumber}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<BillingTrackingModel>>(content);
        }
        public IEnumerable<TotalReceivedModel> TotalReceived(string From, string To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Dashboard/RecivedSummery?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<TotalReceivedModel>>(content);
        }
        public IEnumerable<ProcessingSummeryModel> ProcessingSummery(string From, string To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Dashboard/ProcessingSummery?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<ProcessingSummeryModel>>(content);
        }
        public IEnumerable<ResultSummeryModel> ResultSummery(string From, string To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Dashboard/ResultSummery?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<ResultSummeryModel>>(content);
        }

        public IEnumerable<EReceiptLogsModel> GetCancelReceiptList(DateTime From,DateTime To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Success/DateRange?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
        public IEnumerable<EReceiptLogsModel> GetCancelReceiptListWithPagination(DateTime From,DateTime To, int? pageNo = 1, int? pageSize = 100)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Success/DateRangePagination?From={From}&To={To}&pageNo={pageNo}&pageSize={pageSize}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }




        public IEnumerable<EReceiptLogsModel> CancelByReceiptNumber(string EReceiptNumber)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Success/EReceiptNumber?EReceiptNumber={EReceiptNumber}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
         public IEnumerable<EReceiptLogsModel> CancelByReceiptUUID(string EReceiptUUID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Success/UUID?UUID={EReceiptUUID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
      
        public ApiResponse RefundedReceipt(long LogID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/WEB/Cancel?LogID={LogID}", new { });

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
            //ResolveObjectResponse
            // return   JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
        }

        public ApiResponse ReSubmitReceipt(long LogID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/WEB/ReSubmit?LogID={LogID}", new { });

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
            //ResolveObjectResponse
            // return   JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
        }
        public ApiResponse ResetResult()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/ResetResult", new { });

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
            //ResolveObjectResponse
            // return   JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
        }

        public ApiResponse ParialRefundedReceipt(long LogID, List<PartialReturnViewModel> PartialReturnList)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/WEB/Partial/Cancel?LogID={LogID}", PartialReturnList);

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response); 
        }
        public IEnumerable<EReceiptLogsModel> RefundedReceiptByRange(DateTime From,DateTime To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Refunded/DateRange?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }

        public IEnumerable<EReceiptLogsModel> GetRefundedReceiptListWithPagination(DateTime From, DateTime To, int? pageNo = 1, int? pageSize = 100)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Refunded/DateRangePagination?From={From}&To={To}&pageNo={pageNo}&pageSize={pageSize}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }


        public IEnumerable<EReceiptLogsModel> RefundedReceiptByReceiptNumber(string EReceiptNumber)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Refunded/EReceiptNumber?EReceiptNumber={EReceiptNumber}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }
        public IEnumerable<EReceiptLogsModel> RefundedReceiptByReceiptUUID(string UUID)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/Submission/Refunded/UUID?UUID={UUID}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<EReceiptLogsModel>>(content);
        }


        //Total Report
        public IEnumerable<TotalReportsModel> TotalReports(DateTime From, DateTime To)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Totals?From={From}&To={To}", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<IEnumerable<TotalReportsModel>>(content);
        }

        public ExportItems ExportNotExsitItemsToExcel()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v1/Erecipt/Logs/ListWithItemsNotExist", new { });
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<ExportItems>(content);
        }
    }
}

