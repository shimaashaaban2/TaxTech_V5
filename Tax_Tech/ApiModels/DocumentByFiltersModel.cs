using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class DocumentByFiltersModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string DocumentType { get; set; }
        public string MOStatus { get; set; }
        public int pageNo { get; set; } = 1;
        public int pageSize { get; set; } = 100;
        public bool AllDoc { get; set; }
        public bool AllStatus { get; set; }
        public byte ReturnPartial { get; set; } = 0;
    }
}
