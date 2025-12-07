using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.Classes;


namespace Tax_Tech.Areas.Configuration.Repository
{
    public class del_ApiRepository1
    {
        public HttpClient Client
        {
            get;
            set;
        }
       
        private del_ApiRepository1()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(PublicConfig.API_URL.ToString());
 
        }
        private static del_ApiRepository1 _instance;
        public static del_ApiRepository1 GetAPI()
        {
            if (_instance == null)
            {
                _instance = new del_ApiRepository1();
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