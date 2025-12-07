using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Repository
{
    public class UploadAPIRepository
    {
        private static UploadAPIRepository _instance; 
        public static UploadAPIRepository Get()
        {
            if (_instance == null)
            {
                _instance = new UploadAPIRepository();
            }
            return _instance;
        }

        public CustomViewModel UploadExcel(byte[] File, string JobType, string fileName)
        {
            try
            {
                string jsonStr = Tax_Tech.Helpers.JsonConverter.GetLookups().ConvertToFileJson(File);
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Upload/Excel?JobType={JobType}&fileName={fileName}", jsonStr);

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