using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class DocumentPDFApiModel
    {
        public string Success { get; set; }
        public string MSG { get; set; }
        public string FileName { get; set; }
        public byte[] Stream { get; set; }

    }
}
