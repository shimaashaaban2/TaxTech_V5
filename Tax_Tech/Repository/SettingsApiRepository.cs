using Newtonsoft.Json;
using System;
using System.Net.Http;
using Tax_Tech.ApiModels;

namespace Tax_Tech.Repository
{
    public class SettingsApiRepository
    {
        

        public SettingsApiModel GetAppLangByUserID(string userId)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Settings/GetAppLanguage?UserID={userId}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<SettingsApiModel>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SettingsApiModel GetAppDesignMode(string userId)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Settings/GetAppDesignMode?UserID={userId}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<SettingsApiModel>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse SetAppLang(string userId, string lang)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Settings/SetAppLanguage?UserID={userId}&AppLanguage={lang}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse SetAppDesignMode(string userId, bool isDarkMode)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Settings/SetAppDesignMode?UserID={userId}&IsDarkMode={isDarkMode}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
