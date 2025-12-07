using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.Classes;
using Tax_Tech.Exceptions;

namespace Tax_Tech.Repository
{
    public class ApiTokenRepository
    {
        private static ApiTokenRepository _instance;

        public HttpClient Client
        {
            get;
            set;
        }

        private ApiTokenRepository(string Token,string key)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(PublicConfig.API_URL.ToString());

            Client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            if (key!=null)
                Client.DefaultRequestHeaders.Add("key", key);  
        }
        
        public static ApiTokenRepository GetAPI()
        {
           string Token=Convert.ToString(HttpContext.Current.Session["Token"]);
            _instance = new ApiTokenRepository(Token,null);
            return _instance;
        }

        public static ApiTokenRepository GetAPIWithHeader(string key)
        {
            string Token = Convert.ToString(HttpContext.Current.Session["Token"]);
            _instance = new ApiTokenRepository(Token, key);
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
            var response = Client.PostAsJsonAsync(url, model).Result;
            if(response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
            {
                throw new NotAuthorizedException(Resources.Resource.NotAuthorized);
            }

            return response;
        }

        public HttpResponseMessage DeleteResponse(string url)
        {
            return Client.DeleteAsync(url).Result;
        }
    }
}