using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("DeclSeq")]
    public class NDeclSeq : SubstituteNode, ITypeErrorSource, IDeclarationSource
    {
        public IEnumerable<NConstDecl> Constants
        {
            get { return Children.Where(x => x is NConstDecl).Select(x => (NConstDecl) x); }
        }

        public IEnumerable<NTypeDecl> Types
        {
            get { return Children.Where(x => x is NTypeDecl).Select(x => (NTypeDecl) x); }
        }

        public IEnumerable<NVarDecl> Variables
        {
            get { return Children.Where(x => x is NVarDecl).Select(x => (NVarDecl) x); }
        }

        public IEnumerable<NForwardDecl> Procedures
        {
            get { return Children.Where(x => x is NForwardDecl).Select(x => (NForwardDecl) x); }
        }

        public NDeclSeq(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NConstDecl || x is NTypeDecl || x is NVarDecl
                || x is NForwardDecl);
        }

        public void FindDeclarations(Scope scope)
        {
            foreach (var child in Children.Where(x => x is IDeclarationSource)) {
                ((IDeclarationSource) child).FindDeclarations(scope);
            }
        }

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Children.SelectMany(x => x is ITypeErrorSource
                ? ((ITypeErrorSource) x).FindTypeErrors(scope)
                : new CompilerException[0]);
        }
    }
}
