using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.ApiModels
{
    public class UserPermissionApiModel
    {
        public long? ActionID { get; set; }
        public string ActionFriendlyName { get; set; }
        public int? UserActionID { get; set; }
    }
}
