using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.Classes;


namespace Tax_Tech.Repository
{
    public class ApiRepository
    {
        public HttpClient Client
        {
            get;
            set;
        }
       
        private ApiRepository()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(PublicConfig.API_URL.ToString());
 
        }
        private static ApiRepository _instance;
        public static ApiRepository GetAPI()
        {
            if (_instance == null)
            {
                _instance = new ApiRepository();
            }
            return _instance;
        }

        public HttpResponseMessage GetResponse(string url)
        {
            return Client.GetAsync(url).Result;


        }
        public HttpResponseMessage PutResponse(string url, object model)
        {
            return Client.PutAsJsonAsync(url, model).Result;
        }
        public HttpResponseMessage PostResponse(string url, object model)

        {
            return Client.PostAsJsonAsync(url, model).Result;
        }
 
        public HttpResponseMessage DeleteResponse(string url)
        {
            return Client.DeleteAsync(url).Result;
        }
    }
}