using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.Areas.Configuration.ViewModels; 
using Tax_Tech.Repository;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class AccountsApiRepository
    {       
        private static AccountsApiRepository _instance;
        public static AccountsApiRepository GetAccounts()
        {
             if (_instance == null)
            {
                _instance = new AccountsApiRepository();
            }
            return _instance;
        }
        public CustomViewModel UserResetChnagePassword(long UserID)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/UserResetChnagePassword?UserID={UserID}", null);

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<Tax_Tech.ViewModels.ChangePasswordDataViewModel> GetChnagePasswordData(long UserID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/ChnagePasswordData?UserID={UserID}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<Tax_Tech.ViewModels.ChangePasswordDataViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel AccountLastSessionID(long UserID, string LastSessionID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/AccountLastSessionID?UserID={UserID}&LastSessionID={LastSessionID}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<Tax_Tech.ViewModels.Users_UserLastSessionIDGetModelView> GetAccountLastSessionID(long UserID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/GetAccountLastSessionID?UserID={UserID}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<Tax_Tech.ViewModels.Users_UserLastSessionIDGetModelView>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel CreatAccount(long EntityID, string UserName, string Password, string Email, string DisplayName, byte[] ProfileImage,long RCBy,long RoleID)
        {
            try
            {
                //string jsonStr = Tax_Tech.Helpers.JsonConverter.GetLookups().ConvertToImageJson(ProfileImage);
                string jsonStr = null;
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/CreatAccount?EntityID={EntityID}&UserMail={Email}&DisplayName={DisplayName}&UserName={UserName}&Password={Password}&RCBy={RCBy}&RoleID={RoleID}", jsonStr);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<EntityViewModel> GetEntitiesList()
        {
            try
            {
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v1/WEB/Lookups/GetEntitiesList", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<EntityViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }       
        public IEnumerable<EntityViewModel> GetEntitiesListByUser(long UserID)
        {
            try
            {
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetEntitiesListByUser?UserID={UserID}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<EntityViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<UserAccountsViewModel> GetAccountsList(long UserID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/GetAccountList?UserID={UserID}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<UserAccountsViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<UserAccountsViewModel> GetUserFilter(long UserID,long OptionID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/GetAccountListByFilter?UserID={UserID}&OptionID={OptionID}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<UserAccountsViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<UserAccountsViewModel> GetAccountDataForEdit(long UserID /*, long EntityID*/)
        {
            try
            {
                //&EntityID ={ EntityID}
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/GetAccountInfo?UserID={UserID}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<UserAccountsViewModel>>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel UserActivation(long UserID,bool IsActive)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/SetAccountActiveStatus?UserID={UserID}&IsActive={IsActive}", null);

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel EditAccount(long UserID, string Email, string DisplayName, byte[] ProfileImage, long LABy , long RoleID)
        {
            try
            {
                string jsonStr = Tax_Tech.Helpers.JsonConverter.GetLookups().ConvertToImageJson(ProfileImage);
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/EditAccount?UserID={UserID}&UserMail={Email}&DisplayName={DisplayName}&LABy={LABy}&RoleID={RoleID}", jsonStr);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }       
        public CustomViewModel UserChangeMyPassword(string EntityID, long UserID, string Username, string Password,string Oldpassword)
        {
            try
            {
                //JsonConvert.SerializeObject(jsonFrm)
                string jsonStr=Tax_Tech.Helpers.JsonConverter.GetLookups().ConvertToJson(EntityID);
               
                HttpResponseMessage response = BasicAuthApiRepository.GetAPI(EntityID,Username, Oldpassword).PostResponse($"v2/WEB/Password/ChangeMyPassword?UserID={UserID}&Password={Password}", jsonStr);

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel GetUserNameByIDandPassword(long UserID, string Password)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/UserNameByIDandPassword?UserID={UserID}&Password={Password}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel UserResetPassword(long UserID, long By, string Password)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Password/ResetPassword?UserID={UserID}&By={By}&Password={Password}", null);
                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}