using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class AccountLookupApiRepository
    {

        public IEnumerable<BankAccount> GetAccounts(bool activeList)
        {
            try
            {
                HttpResponseMessage result = null;

                if (!activeList)
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetBankAccountsList", null);
                }
                else
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetBankAccountsActiveList", null);
                }

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<BankAccount>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse InsertAccount(PaymentViewModel model)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/BankAccountsListInsert?PaymentAccountERPID={model.AccountErp}&BankAccountNo={model.BankAccountNo}&BankAccountIBAN={model.BankAccountIban}&SwiftCode={model.SwiftCode}&BranchAddress={model.BranchAddress}&BankID={model.BankID}&EntityID={model.EntityID}&ActionBy={model.ActionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse UpdateAccount(PaymentViewModel model)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/BankAccountsListUpdate?PaymentAccountERPID={model.AccountErp}&BankAccountNo={model.BankAccountNo}&BankAccountIBAN={model.BankAccountIban}&SwiftCode={model.SwiftCode}&BranchAddress={model.BranchAddress}&BankID={model.BankID}&EntityID={model.EntityID}&ActionBy={model.ActionBy}&PaymentAccountID={model.PaymentAccountID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse ChangeAccountStatus(bool status, long accountId, long actionBy)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/SetBankAccountActive?PaymentAccountID={accountId}&IsActive={status}&ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<BankAccount> GetPaymentById(long accountId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetPaymentTypeByID?PaymentAccountID={accountId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<BankAccount>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New GetPaymentById
        public IEnumerable<BankAccount> NewGetPaymentById(long accountId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetPaymentTypeByID?PaymentAccountID={accountId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<BankAccount>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
