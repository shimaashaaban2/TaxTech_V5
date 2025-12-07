using System.Collections.Generic;
using System.Data;

namespace Tax_Tech.ViewModels
{
    public class ExeclValidtorResult
    {
        public List<string> Errors { get; set; }
        public DataTable Data { get; set; }
    }
}
