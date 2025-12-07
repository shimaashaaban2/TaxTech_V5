 using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ApiModels;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Repository
{
    public class AccountsApiRepository
    {
        
        private static AccountsApiRepository _instance;
       // private readonly ApiTokenRepository _apiRepository; 
        public static AccountsApiRepository GetAccounts()
        {
            if (_instance == null)
            {
                _instance = new AccountsApiRepository();
            }
            return _instance;
        }
        public AccountsApiRepository()
        {
          //  _apiRepository = ApiTokenRepository.GetAPI();
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

        public CustomViewModel UserChangeMyPassword(string EntityID, long UserID, string Username, string Password, string Oldpassword)
        {
            try
            {
                //JsonConvert.SerializeObject(jsonFrm)
                string jsonStr = Tax_Tech.Helpers.JsonConverter.GetLookups().ConvertToJson(EntityID);

                HttpResponseMessage response = BasicAuthApiRepository.GetAPI(EntityID, Username, Oldpassword).PostResponse($"v2/WEB/Password/ChangeMyPassword?UserID={UserID}&Password={Password}", jsonStr);

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel SetChangeMyPassword(Int64 EntityID, string Username, string NewPassword, string Oldpassword)
        {
            try
            {
                HttpResponseMessage result = BasicAuthApiRepository.GetAPI(Convert.ToString(EntityID), Username, Oldpassword).PostResponse($"v2/WEB/Auth/ChangePasswordForResetAccount?" +
                    $"EntityID={EntityID}&UserName={Username}&OldPassword={Oldpassword}&NewPassword={NewPassword}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel AccountLogin(long EntityID,string UserName,string Password)
        {
            try
            {
                HttpResponseMessage result = BasicAuthApiRepository.GetAPI(Convert.ToString(EntityID),UserName, Password).PostResponse($"v2/WEB/Auth/AccountLogin?EntityID={EntityID}&UserName={UserName}&Password={Password}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel ChangePasswordForResetAccount(long EntityID, string UserName, string OldPassword, string NewPassword)
        {
            try
            {
                HttpResponseMessage result = BasicAuthApiRepository.GetAPI(Convert.ToString(EntityID), UserName, OldPassword).PostResponse($"v2/WEB/Auth/ChangePasswordForResetAccount?EntityID={EntityID}&UserName={UserName}&OldPassword={OldPassword}&NewPassword={NewPassword}", null);
                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel AccountRegister(long EntityID, string UserName, string Password,string Email,string DisplayName,byte[] ProfileImage , string ConfirmCode)
        {
            try
            {
                string jsonStr = Tax_Tech.Helpers.JsonConverter.GetLookups().ConvertToImageJson(ProfileImage);

                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/Accountregister?EntityID={EntityID}&UserMail={Email}&ConfirmCode={ConfirmCode}&DisplayName={DisplayName}&UserName={UserName}&Password={Password}", jsonStr);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel CreatAccount(long EntityID, string UserName, string Password, string Email, string DisplayName, byte[] ProfileImage,long RCBy)
        {
            try
            {
                string jsonStr = Tax_Tech.Helpers.JsonConverter.GetLookups().ConvertToImageJson(ProfileImage);

                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/CreatAccount?EntityID={EntityID}&UserMail={Email}&DisplayName={DisplayName}&UserName={UserName}&Password={Password}&RCBy={RCBy}", jsonStr);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<UserProfileViewModel>  GetAccountData(string UserName, long EntityID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/GetProfile?UserName={UserName}&EntityID={EntityID}", null);
               long StatusCode= Convert.ToInt16(result.StatusCode);
                if (StatusCode == 200)
                {
                    result.EnsureSuccessStatusCode();

                    var data = result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<IEnumerable<UserProfileViewModel>>(data);
                }
                else
                {
                    List<UserProfileViewModel> list=new List<UserProfileViewModel>() ;
                    UserProfileViewModel GetResponse = new UserProfileViewModel();
                    if (StatusCode == 401)
                        GetResponse.Response = Resources.Resource.NotAuthenticated;

                    list.Add(GetResponse);
                    return list;
                }
           
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

        public IEnumerable<Email> GetEmailData(long?EntityID)
        {
            try
            {
               
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v1/WEB/Config/GetRegistrationEmail?EntityID={EntityID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<Email>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse GetHours(long?EntityID)
        {
            try
            {
               
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v2/WEB/Config/GetEntityRegistrationHooursConfig?EntityID={EntityID}", null);

                result.EnsureSuccessStatusCode();
                var val = result.Content.ReadAsStringAsync().Result;
               //IEnumerable<test> value = JsonConvert.DeserializeObject<IEnumerable<test>>(result.Content.ReadAsStringAsync().Result);
                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ApiResponse GetForgetPasswordHours(long?EntityID)
        {
            try
            {
               
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v1/WEB/Config/GetEntityForgetPasswordHoursConfig?EntityID={EntityID}", null);

                result.EnsureSuccessStatusCode();
                var val = result.Content.ReadAsStringAsync().Result;
               //IEnumerable<test> value = JsonConvert.DeserializeObject<IEnumerable<test>>(result.Content.ReadAsStringAsync().Result);
                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel ResendConfirmationEmail(long EntityID, string Email, string ConfirmCode)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Accounts/ResetRegistConfirmation?EntityID={EntityID}&UserMail={Email}&ConfirmCode={ConfirmCode}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel UsersForgetPasswordCode(long EntityID, string Email, string ForgetPasswordCode)
        {
            try
            {
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v2/WEB/Accounts/UsersForgetPasswordCode?EntityID={EntityID}&UserMail={Email}&ForgetPasswordCode={ForgetPasswordCode}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel UsersForgetPassword(string ForgetPasswordCode)
        {
            try
            {
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v2/WEB/Accounts/UsersForgetPassword?ForgetPasswordCode={ForgetPasswordCode}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel ChangePasswordByCode(long EntityID, string Email, string Password)
        {
            try
            {
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v2/WEB/Accounts/ChangePasswordByCode?EntityID={EntityID}&UserMail={Email}&Password={Password}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CustomViewModel CheckUserEmailExists(long EntityID, string Email)
        {
            try
            {
                HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v2/WEB/Accounts/CheckUserEmailExists?EntityID={EntityID}&Email={Email}", null);

                result.EnsureSuccessStatusCode();

                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<CustomViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    
}