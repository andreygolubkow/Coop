using System.Collections.Generic;
using System.IO;

namespace Coop.Web.PaysParser
{
    public interface IPaysParser
    {
        List<PayRecord> ParseFromStream(MemoryStream stream);
    }
}