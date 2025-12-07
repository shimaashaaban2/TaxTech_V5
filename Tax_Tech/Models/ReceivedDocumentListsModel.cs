using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax_Tech.ApiModels;

namespace Tax_Tech.Models
{
    public class ReceivedDocumentListsModel
    {
        public IEnumerable<ApiModels.DocumentType> DocumentType { get; set; }
        public IEnumerable<MOFStatusListViewModel> MOFStatus { get; set; }
        public IEnumerable<ReceivedVendorListViewModel> ReceivedVendors { get; set; }
        public IEnumerable<ApiModels.ProcessStatus> Status { get; set; }

        public SendFileModel fileModel { get; set; }
    }
}