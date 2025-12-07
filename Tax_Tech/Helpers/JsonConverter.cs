using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.Helpers
{
    public class JsonConverter
    {
        private static JsonConverter _instance;
        public static JsonConverter GetLookups()
        {
            if (_instance == null)
            {
                _instance = new JsonConverter();
            }
            return _instance;
        }

        public string ConvertToJson(string EntityID)
        {
            Dictionary<string, string> jsonFrm = new Dictionary<string, string>();
            jsonFrm.Add("param3", EntityID);
           

            return JsonConvert.SerializeObject(jsonFrm);
        }
        public string ConvertToImageJson(byte[] ProfileImage)
        {
           
            Dictionary<string, string> jsonFrm = new Dictionary<string, string>();
            if (ProfileImage == null)
                jsonFrm.Add("img", null);

            else
                jsonFrm.Add("img", Convert.ToBase64String(ProfileImage));


            return JsonConvert.SerializeObject(jsonFrm);
        }
        public string ConvertToFileJson(byte[] File)
        {

            Dictionary<string, string> jsonFrm = new Dictionary<string, string>();
            if (File == null)
                jsonFrm.Add("file", null);

            else
                jsonFrm.Add("file", Convert.ToBase64String(File));


            return JsonConvert.SerializeObject(jsonFrm);
        }
    }
}