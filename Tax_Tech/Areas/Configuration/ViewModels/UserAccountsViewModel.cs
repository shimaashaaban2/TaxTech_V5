using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax_Tech.Areas.Configuration.ViewModels
{
    public class UserAccountsViewModel
    {
        public long UserID { get; set; }
        public int EntityID { get; set; }
        public string DisplayName { get; set; }
        public byte[] ProfileImage { get; set; }
        public string Username { get; set; }
        public string UserMail { get; set; }
        public bool IsActive { get; set; } 
        public int RoleID { get; set; }
        public string RoleTitle { get; set; }
        public string EntityTitle { get; set; }
    }
}