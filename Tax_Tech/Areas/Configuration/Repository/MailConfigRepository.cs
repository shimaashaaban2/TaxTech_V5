using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.Areas.Configuration.ViewModels;
using Tax_Tech.Repository;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class MailConfigRepository
    {
        

        public IEnumerable<MailListViewModel> GetMailList(string entityId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/MailListGet?EntityID={entityId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<MailListViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse UpdateMailList(MailListViewModel model)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/MailListUpdate?GroupID={model.GroupID}&EntityID={model.EntityID}&EMail={model.Email}&MailPassword={model.MailPassword}&SMTPServer={model.SMTPServer}&SMTPort={model.Port}&TLSFlag={model.TLSFlag}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
