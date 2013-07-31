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

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            var errors = new List<CompilerException>();
            errors.AddRange(Min.FindTypeErrors(scope));
            if (!SingleExpression) {
                errors.AddRange(Max.FindTypeErrors(scope));
            }

            if (!Min.GetFinalType(scope).IsInteger) {
                errors.Add(new TypeMismatchException(IntegerType.Integer, Min.GetFinalType(scope), Min));
            }

            if (!SingleExpression && !Max.GetFinalType(scope).IsInteger) {
                errors.Add(new TypeMismatchException(IntegerType.Integer, Max.GetFinalType(scope), Max));
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

        public override OberonType GetFinalType(Scope scope)
        {
            return SetType.Default;
        }

        public override bool IsConstant(Scope scope)
        {
            return Elements.All(x => x.Min.IsConstant(scope) && (x.SingleExpression || x.Max.IsConstant(scope)));
        }

        public NSet(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NElement);
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Elements.SelectMany(x => x.FindTypeErrors(scope));
        }
    }
}
