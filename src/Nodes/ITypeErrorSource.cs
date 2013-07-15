using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes
{
    public interface ITypeErrorSource
    {
        IEnumerable<ParserException> FindTypeErrors();
    }
}
