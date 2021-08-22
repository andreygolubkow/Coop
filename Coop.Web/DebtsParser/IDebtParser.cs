using System.Collections.Generic;
using System.IO;

namespace Coop.Web.DebtsParser
{
    public interface IDebtParser
    {
        List<DebtRecord> ParseFromStream(MemoryStream stream);
    }
}