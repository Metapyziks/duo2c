using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes
{
    public interface ITypeErrorSource
    {
        IEnumerable<CompilerException> FindTypeErrors(Scope scope);
    }

    public interface IAccessibilityErrorSource
    {
        IEnumerable<CompilerException> FindAccessibilityErrors(Scope scope);
    }

    public interface IDeclarationSource
    {
        void FindDeclarations(Scope scope);
    }
}
