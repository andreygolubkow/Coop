using System.Collections.Generic;

namespace Coop.Web.Models
{
    public class UploadResultViewModel
    {
        public List<string> Result { get; set; } = new List<string>();
        
        public string ParserError { get; set; }
    }
}