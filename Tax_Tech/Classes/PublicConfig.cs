using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Tax_Tech.Classes
{
    public class PublicConfig
    {
        public readonly static string CreateDocumentMinDays = WebConfigurationManager.AppSettings["CreateDocumentMinDays"];
        public readonly static string TaxPDFAPIUrl = WebConfigurationManager.AppSettings["TaxPDFAPIUrl"];
        public readonly static string API_URL = WebConfigurationManager.AppSettings["APIURL"];
        public readonly static string ClientID = WebConfigurationManager.AppSettings["ClientID"];
        public readonly static string ClientSecret = WebConfigurationManager.AppSettings["ClientSecret"];

        public readonly static string PreProductionTokenAPIPath = WebConfigurationManager.AppSettings["PreprodTokenAPIPath"];
        public readonly static string PreProduction_API_URL = WebConfigurationManager.AppSettings["PreprodAPIPath"];

        public readonly static string ProductionTokenAPIPath = WebConfigurationManager.AppSettings["ProdTokenAPIPath"];
        public readonly static string Production_API_URL = WebConfigurationManager.AppSettings["ProdAPIPath"];

        public readonly static string GrantType = WebConfigurationManager.AppSettings["GrantType"];
        public readonly static string Scope = WebConfigurationManager.AppSettings["Scope"];
        public readonly static string IssuerID = WebConfigurationManager.AppSettings["IssuerID"];
        public readonly static string InvoiceTypeID = WebConfigurationManager.AppSettings["InvoiceTypeID"];
        public readonly static string CreditNoteTypeID = WebConfigurationManager.AppSettings["CreditNoteTypeID"];
        public readonly static string DebitNoteTypeID = WebConfigurationManager.AppSettings["DebitNoteTypeID"];
        public readonly static string AppDomain = WebConfigurationManager.AppSettings["AppDomain"];
        public readonly static string APIResponseNULL = "API Down";

        public readonly static string ShowInternalID = WebConfigurationManager.AppSettings["ShowInternalID"];

        public readonly static string CreateDocumentVersion = WebConfigurationManager.AppSettings["CreateDocumentVersion"];
        public readonly static string ChangePasswordLimit = WebConfigurationManager.AppSettings["ChangePasswordLimit"];


        public readonly static string RunEnhance = WebConfigurationManager.AppSettings["RunEnhance"];
        public readonly static string UserEnhance = WebConfigurationManager.AppSettings["UserEnhance"];
        public readonly static string PassEnhance = WebConfigurationManager.AppSettings["PassEnhance"];
        public readonly static string IsErecipt = WebConfigurationManager.AppSettings["IsErecipt"];
    }
}
