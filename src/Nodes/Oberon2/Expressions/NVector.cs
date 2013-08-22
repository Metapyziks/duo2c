using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Vector")]
    public class NVector : LiteralElement
    {
        public IEnumerable<NExpr> Expressions
        {
            get { return ((NExprList) Children.First()).Expressions; }
        }

        public override string String
        {
            get { return String.Format("<{0}>", String.Join(", ", Expressions.Select(x => x.String))); }
        }

        public override bool IsConstant(Scope scope)
        {
            return Expressions.All(x => x.IsConstant(scope));
        }

        public override OberonType GetFinalType(Scope scope)
        {
            var first = Expressions.First().GetFinalType(scope);
            if (first.IsNumeric) {
                return new VectorType(Expressions.Skip(1).Select(x => x.GetFinalType(scope)).Aggregate(first,
                    (s, x) => NumericType.Largest(s.As<NumericType>(), x.As<NumericType>())), Expressions.Count());
            } else {
                return new VectorType(first, Expressions.Count());
            }
        }

        public NVector(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NExprList);
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool found = false;
            foreach (var e in Expressions.SelectMany(x => x.FindTypeErrors(scope))) {
                found = true;
                yield return e;
            }

            if (!found) {
                var first = Expressions.First().GetFinalType(scope);
                foreach (var expr in Expressions.Skip(1)) {
                    var type = expr.GetFinalType(scope);
                    if (!first.CanTestEquality(type) && !type.CanTestEquality(first)) {
                        yield return new TypeMismatchException(first, type, expr);
                    }
                }
            }
        }

        public override LiteralElement EvaluateConst(NExpr orig, LiteralElement other, ExprOperator op, Scope scope)
        {
            var otherElems = (other is NVector
                ? ((NVector) other).Expressions.Select(x => x.EvaluateConst(scope))
                : Enumerable.Range(0, Expressions.Count()).Select(x => other));

            foreach (var pair in Expressions.Zip(otherElems, (x, y) => Tuple.Create(x, y))) {
                pair.Item1.SimpleExpr.Term.Factor.OverwriteConst(pair.Item1.EvaluateConst(scope)
                    .EvaluateConst(orig, pair.Item2, op, scope));
            }

            return this;
        }

        public override LiteralElement EvaluateConst(NSimpleExpr orig, LiteralElement other, SimpleExprOperator op, Scope scope)
        {
            var otherElems = (other is NVector
                ? ((NVector) other).Expressions.Select(x => x.EvaluateConst(scope))
                : Enumerable.Range(0, Expressions.Count()).Select(x => other));

            foreach (var pair in Expressions.Zip(otherElems, (x, y) => Tuple.Create(x, y))) {
                pair.Item1.SimpleExpr.Term.Factor.OverwriteConst(pair.Item1.EvaluateConst(scope)
                    .EvaluateConst(orig, pair.Item2, op, scope));
            }

            return this;
        }

        public override LiteralElement EvaluateConst(NTerm orig, LiteralElement other, TermOperator op, Scope scope)
        {
            var otherElems = (other is NVector
                ? ((NVector) other).Expressions.Select(x => x.EvaluateConst(scope))
                : Enumerable.Range(0, Expressions.Count()).Select(x => other));

            foreach (var pair in Expressions.Zip(otherElems, (x, y) => Tuple.Create(x, y))) {
                pair.Item1.SimpleExpr.Term.Factor.OverwriteConst(pair.Item1.EvaluateConst(scope)
                    .EvaluateConst(orig, pair.Item2, op, scope));
            }

            return this;
        }

        public override LiteralElement EvaluateConst(NUnary orig, UnaryOperator op, Scope scope)
        {
            foreach (var expr in Expressions) {
                expr.SimpleExpr.Term.Factor.OverwriteConst(expr.EvaluateConst(scope)
                    .EvaluateConst(orig, op, scope));
            }

            return this;
        }
    }
}
