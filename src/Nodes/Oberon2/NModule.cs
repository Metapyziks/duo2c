using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes.Oberon2;

namespace DUO2C.Nodes
{
    [SubstituteToken("Module")]
    public class NModule : SubstituteNode, ITypeErrorSource
    {
        [Serialize("name")]
        public String Identifier { get; private set; }

        public IEnumerable<String> Imports
        {
            get {
                var imports = Children.ElementAt(1) as NImportList;
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

        public IEnumerable<ParserException> FindTypeErrors()
        {
            return Children.SelectMany(x => x is ITypeErrorSource
                ? ((ITypeErrorSource) x).FindTypeErrors()
                : new ParserException[0]);
        }
    }
}
