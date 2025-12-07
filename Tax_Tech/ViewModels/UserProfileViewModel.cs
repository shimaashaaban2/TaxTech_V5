using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.ViewModels
{
    public class UserProfileViewModel
    {
        public string Response { get; set; }
        public long UserID { get; set; }
        public int EntityID { get; set; }
        public string DisplayName { get; set; }
        public byte[] ProfileImage { get; set; }
        public int? RoleID { get; set; }
    }
}