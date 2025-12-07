using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ApiModels;

namespace Tax_Tech.Repository
{
    public class EReceiptSourceRepository
    {
        public EReceiptSourceListModel GetEReceiptSource(string From, string To, string source, int pageNo, int pageSize)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().GetResponse($"JobAPI/api/JobTracker/GetEreceiptSource?FromDate={From}&ToDate={To}&source={source}&PageNumber={pageNo}&PageSize={pageSize}");
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<EReceiptSourceListModel>(content);
        }
    }
}

//string From, string To, string source, int pageNo, int pageSize