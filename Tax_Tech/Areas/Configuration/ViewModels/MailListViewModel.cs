using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.Areas.Configuration.ViewModels
{
    public class MailListViewModel
    {
        public int? GroupID { get; set; }
        public string GroupName { get; set; }
        public long? EntityID { get; set; }
        public string EntityTitle { get; set; }

        [JsonProperty("EMail")]
        public string Email { get; set; }

        public string MailPassword { get; set; }
        public string SMTPServer { get; set; }
        [JsonProperty("SMTPort")]
        public string Port { get; set; }
        public bool? TLSFlag { get; set; }

        public string TLSorSSL { get; set; }
    }
}
