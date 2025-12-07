using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class Email
    {
        public long? GroupID { get; set; }
        public long? EntityID { get; set; }
        public string GroupName { get; set; }
        public string EMail { get; set; }
        public string MailPassword { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPort { get; set; }
        public bool? TLSFlag { get; set; }

    }
}
