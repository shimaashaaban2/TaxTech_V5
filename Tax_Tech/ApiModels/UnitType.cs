using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class UnitType
    {
        public int? UnitID { get; set; }
        public string TaxPortalUnitID { get; set; }
        public string UnitTitleEN { get; set; }
        public string UnitTitleAR { get; set; }
    }
}
