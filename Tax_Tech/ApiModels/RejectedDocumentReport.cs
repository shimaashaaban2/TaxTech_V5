using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class RejectedDocumentReport
    {
        public string MOFRejectStatus { get; set; }
        public int? DocCount { get; set; }
    }
}
