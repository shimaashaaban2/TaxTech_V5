using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Tax_Tech.Classes;

namespace Tax_Tech.Repository
{
    public class BasicAuthApiRepository
    {
        public HttpClient Client
        {
            get;
            set;
        }

        private BasicAuthApiRepository(string Entity,string UserName,string Password)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(PublicConfig.API_URL.ToString());
            Client.DefaultRequestHeaders.Add("Entity", Entity);
            string X = Convert.ToBase64String(
                      System.Text.Encoding.ASCII.GetBytes(
                          string.Format("{0}:{1}", PublicConfig.UserEnhance, PublicConfig.PassEnhance)));
            if (PublicConfig.RunEnhance=="1")
            {
                Client.DefaultRequestHeaders.Authorization =
              new AuthenticationHeaderValue(
                  "Basic",
                  Convert.ToBase64String(
                      System.Text.Encoding.ASCII.GetBytes(
                          string.Format("{0}:{1}", PublicConfig.UserEnhance, PublicConfig.PassEnhance))));
            }
            else
            {
                Client.DefaultRequestHeaders.Authorization =
              new AuthenticationHeaderValue(
                  "Basic",
                  Convert.ToBase64String(
                      System.Text.Encoding.ASCII.GetBytes(
                          string.Format("{0}:{1}", UserName, Password))));
            }
        }
        private static BasicAuthApiRepository _instance;
        public static BasicAuthApiRepository GetAPI(string Entity,string UserName, string Password)
        {
            _instance = new BasicAuthApiRepository(Entity, UserName, Password);
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