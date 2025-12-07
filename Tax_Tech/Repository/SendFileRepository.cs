using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ApiModels;

namespace Tax_Tech.Repository
{
    public class SendFileRepository
    {
        public SendFileModel SendFileViaWhatsApp(InvoiceRequestDto requestDto)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"testApi/api/Invoice/generate", new {requestDto.DocumentId,requestDto.PhoneNumber});
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<SendFileModel>(content);
        }
    }
}