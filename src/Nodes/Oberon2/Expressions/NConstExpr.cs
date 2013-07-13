using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("ConstExpr")]
    public class NConstExpr : ExpressionElement
    {
        public NExpr Inner { get { return (NExpr) Children.First(); } }

        public override OberonType FinalType
        {
            get { return Inner.FinalType; }
        }

        public override bool IsConstant
        {
            get { return true; }
        }

        public NConstExpr(ParseNode original)
            : base(original, false)
        {

        }

        public override IEnumerable<ParserException> FindTypeErrors()
        {
            foreach (var e in Inner.FindTypeErrors()) {
                yield return e;
            }

            if (!Inner.IsConstant) {
                yield return new ConstantExpectedException(Inner);
            }
        }

        public override string SerializeXML()
        {
            // TEMPORARY HACK
            var exceptions = FindTypeErrors();
            if (exceptions.Count() > 0) {
                throw exceptions.First();
            }

            return base.SerializeXML();
        }
    }
}
