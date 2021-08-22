using System.Collections.Generic;

namespace Coop.Web.Models
{
    public class UploadDebtViewModel
    {
        public List<string> Result { get; set; }
        
        public string ParserError { get; set; }
    }
}