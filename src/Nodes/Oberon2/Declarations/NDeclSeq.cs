using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("DeclSeq")]
    public class NDeclSeq : SubstituteNode, ITypeErrorSource
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

        public IEnumerable<ParserException> FindTypeErrors()
        {
            return Children.SelectMany(x => x is ITypeErrorSource
                ? ((ITypeErrorSource) x).FindTypeErrors()
                : new ParserException[0]);
        }
    }
}
