using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Tech
{
    public class DateNotBefore : ValidationAttribute
    {
        public int Days { get; set; }

        public override bool IsValid(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            if (date < DateTime.Now.AddDays(Days * -1))
                return false;
            return true;
        }
    }
}
