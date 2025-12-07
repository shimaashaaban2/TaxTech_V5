using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class MasterReportViewModel
    {
        public short? ReportType { get; set; }
        public string InternalID { get; set; }
        public string UUID { get; set; }
        public string InputType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public short? ProccessStatusID { get; set; }
        public long? AccountID { get; set; }
        public long? UserId { get; set; }
        public short? DocumentType { get; set; }
        public long? EntityID { get; set; }
        public int pageNo { get; set; } = 1;
        public int pageSize { get; set; } = 50;
        public bool DocumentCheckBox { get; set; }
        public bool ProccessStatusOption { get; set; }
        public bool ItemOption { get; set; }
        public bool accountOption { get; set; }
        public byte ReturnPartial { get; set; } = 0;
    }
}