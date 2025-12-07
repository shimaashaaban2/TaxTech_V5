using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech.Areas.Configuration.Helpers
{
    public static class StaticLists
    {
        public static List<object> GetItemTypes()
        {
            return new List<object>
            {
                new { Id = "GS1", Name = "GS1" },
                new { Id = "EGS", Name = "EGS" }
            };
        }
    }
}
