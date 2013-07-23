using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.CodeGen;
using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C.Nodes
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

        public void FindDeclarations(RootScope scope)
        {
            Type = new ModuleType(Identifier, scope);

            Type.Scope.Declare("NEW", new ProcedureType(null, new[] {
                new Parameter(true, "_", new PointerType(RecordType.Base))
            }));

            Declarations.FindDeclarations(Type.Scope);

            if (Body != null) {
                Body.FindDeclarations(Type.Scope);
            }
        }

        public IEnumerable<ParserException> FindTypeErrors(RootScope scope)
        {
            return Children.SelectMany(x => x is ITypeErrorSource
                ? ((ITypeErrorSource) x).FindTypeErrors(Type.Scope)
                : new ParserException[0]);
        }

        public override void GenerateCode(GenerationContext ctx)
        {
            ctx = ctx + "; Module " + Identifier;
            ctx.NewLine().Enter();

            base.GenerateCode(ctx);

            ctx.NewLine().NewLine().Leave();
            ctx = ctx + "; End " + Identifier;
        }
    }
}
