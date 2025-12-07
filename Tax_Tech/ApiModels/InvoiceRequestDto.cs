using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tax_Tech.ApiModels
{
    public class InvoiceRequestDto
    {
        public long DocumentId { get; set; }


        [StringLength(50)]
        public string PhoneNumber { get; set; } = "01234567890";
    }
}