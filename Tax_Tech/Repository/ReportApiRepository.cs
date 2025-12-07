using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Repository
{
    public class ReportApiRepository
    {
        

        public IEnumerable<BranchTotals> GetBranchReport(string fromDate, string toDate, string docType)
        {
            try
            {
                // date format 2021-12-12
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Reports/GetBranchTotals?DocumentTypeID={docType}&StartRange={fromDate}&EndRange={toDate}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<BranchTotals>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<VendorTotals> GetVendorsReport(string fromDate, string toDate, string docType)
        {
            try
            {
                // date format 2021-12-12
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Reports/GetVendorTotals?DocumentTypeID={docType}&StartRange={fromDate}&EndRange={toDate}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<VendorTotals>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<ItemsTotals> GetItemsReport(string fromDate, string toDate, string docType)
        {
            try
            {
                // date format 2021-12-12
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Reports/GetItemsTotals?DocumentTypeID={docType}&StartRange={fromDate}&EndRange={toDate}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<ItemsTotals>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<DocumentTotalReport> GetDocumentsCountReport(string startDate, string endDate, string docTypeId)
        {
            try
            {
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Reports/GetDocumentsCount?DocumentTypeID={docTypeId}&StartRange={startDate}&EndRange={endDate}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentTotalReport>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<RejectedDocumentReport> GetRejectedDocumentReport(string startDate, string endDate,long EntityID)
        {
            try
            {
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Reports/GetRejectReason?StartRange={startDate}&EndRange={endDate}&EntityID={EntityID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<RejectedDocumentReport>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<DocumentTotalReport> GetDocumentValues(string startDate, string endDate, string docID, long EntityID)
        {
            try
            {
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Reports/GetDocumentsTotals?DocumentTypeID={docID}&StartRange={startDate}&EndRange={endDate}&EntityID={EntityID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<DocumentTotalReport>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<JobQueueDetails> GetFailedDocumentImports()
        {
            try
            {
                HttpResponseMessage response =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/InvoiceImportResult", null);

                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<IEnumerable<JobQueueDetails>>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CustomViewModel GetCloseJob(long itemId)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Jobs/Close?JobID={itemId}", null);

                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<CustomViewModel>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<TaxDetailsReportViewModel> GetTaxDetailsReport(DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Reports/TaxViewDetails?FromDate={FromDate}&ToDate={ToDate}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<TaxDetailsReportViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
