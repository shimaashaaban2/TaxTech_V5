using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class JobQueueModelV2
    {
        public long SessionId { get; set; }
        public byte JobStatus { get; set; }
        public int CreatedBy { get; set; }
    }
}