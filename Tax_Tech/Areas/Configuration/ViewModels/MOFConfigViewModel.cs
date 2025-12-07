using System;
using Tax_Tech.ApiModels;

namespace Tax_Tech.Areas.Configuration.ViewModels
{
    public class MOFConfigViewModel
    {
        public CustomResponse CustomeRespons { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string DefaultActivityCode { get; set; }
        public string DocumentVersion { get; set; }
        public string Enviroment { get; set; }
        public Nullable<bool> EnviromentBool { get; set; }
    }
}