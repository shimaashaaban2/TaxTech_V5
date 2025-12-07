using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ViewModels;

namespace Tax_Tech.ApiModels
{
    public class RuningJobQueueApiModel
    {
        public int? SessionID { get; set; }
        public string JobTypeTitle { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatorName { get; set; }
        public string FileName { get; set; }
        public string JobStatusTitle { get; set; }
        public int? JobStatus { get; set; }
        public int? JobTypeID { get; set; }
        public string OrginalFN { get; set; }
        public long? TemplateID { get; set; }
        public long? CreatedBy { get; set; }
    }
}
