using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Element")]
    public class NElement : SubstituteNode, ITypeErrorSource
    {
        public NExpr Min
        {
            get { return (NExpr) Children.First(); }
        }

        public NExpr Max
        {
            get { return (NExpr) Children.Last(); }
        }

        public bool SingleExpression
        {
            get { return Children.Count() == 1; }
        }

        public NElement(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NExpr);
        }

        public IEnumerable<ParserException> FindTypeErrors()
        {
            var errors = new List<ParserException>();
            errors.AddRange(Min.FindTypeErrors());
            if (!SingleExpression) {
                errors.AddRange(Max.FindTypeErrors());
            }

            if (!(Min.FinalType is IntegerType)) {
                errors.Add(new TypeMismatchException(IntegerType.Integer, Min));
            }

            if (!SingleExpression && !(Max.FinalType is IntegerType)) {
                errors.Add(new TypeMismatchException(IntegerType.Integer, Max));
            }

            return errors;
        }
    }

    [SubstituteToken("Set")]
    public class NSet : ExpressionElement
    {
        public IEnumerable<NElement> Elements
        {
            get { return Children.Select(x => (NElement) x); }
        }

        public override OberonType FinalType
        {
            get { return SetType.Default; }
        }

        public override bool IsConstant
        {
            get { return Elements.All(x => x.Min.IsConstant && (x.SingleExpression || x.Max.IsConstant)); }
        }

        public NSet(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NElement);
        }

        public override IEnumerable<ParserException> FindTypeErrors()
        {
            return Elements.SelectMany(x => x.FindTypeErrors());
        }
    }
}
