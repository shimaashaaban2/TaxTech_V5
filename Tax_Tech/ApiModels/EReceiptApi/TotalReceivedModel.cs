using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels.EReceiptApi
{
    public class TotalReceivedModel
    {
        public int CountOfRecived { get; set; }
        public int CountOfAccepted { get; set; }
        public int CountOfAcceptedFromWincash { get; set; }
        public int CountOfAcceptedFromBiling { get; set; }
        public int CountOfRecovery { get; set; }
    }
}
