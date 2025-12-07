using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class SettingsApiModel
    {
        public string Language { get; set; }

        public bool? IsDarkMode { get; set; }
    }
}
