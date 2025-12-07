using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ViewModels
{
    public class ChangePasswordDataViewModel
    {
        public long UserID { get; set; }
        public byte CountOfChangePasswordTrying { get; set; }
    }
}