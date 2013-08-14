using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.CodeGen;
using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Module")]
    public class NModule : SubstituteNode
    {
        [Serialize("name")]
        public String Identifier { get; private set; }

        public ModuleType Type { get; private set; }

        public IEnumerable<String> Imports
        {
            get {
                var imports = Children.First() as NImportList;
                if (imports != null) {
                    return imports.Modules;
                } else {
                    return new String[0];
                }
            }
        }

        public NDeclSeq Declarations
        {
            get {
                return (NDeclSeq) Children.First(x => x is NDeclSeq);
            }
        }

        public NStatementSeq Body
        {
            get {
                return (NStatementSeq) Children.FirstOrDefault(x => x is NStatementSeq);
            }
        }

        public NModule(ParseNode original)
            : base(original, false)
        {
            Identifier = Children.First(x => x is NIdent).String;
            Children = Children.Where(x => x is NImportList
                || x is NDeclSeq || x is NStatementSeq);
        }

        public void FindDeclarations(RootScope scope)
        {
            Type = new ModuleType(this, scope);

            Declarations.FindDeclarations(Type.Scope);

            if (Body != null) {
                Body.FindDeclarations(Type.Scope);
            }
        }

        public IEnumerable<CompilerException> FindTypeErrors(RootScope scope)
        {
            return Children.SelectMany(x => x is ITypeErrorSource
                ? ((ITypeErrorSource) x).FindTypeErrors(Type.Scope)
                : new CompilerException[0]);
        }
    }
}
