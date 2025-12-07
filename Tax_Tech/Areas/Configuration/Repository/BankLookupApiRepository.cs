using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.Repository;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class BankLookupApiRepository
    {
  

        public IEnumerable<Bank> GetBanks(bool activeList)
        {
            try
            {
                HttpResponseMessage result = null;

                if (!activeList)
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetBankList", null);
                }
                else
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetBankActiveList", null);
                }

                result.EnsureSuccessStatusCode();
                string txt = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<Bank>>(txt);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ApiResponse SetBankActive(bool status, long bankId, long actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/SetBankActive?BankID={bankId}&IsActive={status}&ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse UpdateBank(string bankName, long bankId, long actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/BankListUpdate?BankName={bankName}&ActionBy={actionBy}&BankID={bankId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse InsertBank(string bankName, long actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/BankListInsert?BankName={bankName}&ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
