using System;
using System.Collections.Generic;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for real numbers.
    /// </summary>
    [SubstituteToken("real")]
    public class NReal : LiteralElement
    {
        /// <summary>
        /// The parsed value of the real number.
        /// </summary>
        public double Value { get; private set; }
        
        public override string String
        {
            get {
                var intVal = BitConverter.DoubleToInt64Bits(_finalType.Range == RealRange.LongReal ? Value : (float) Value);
                return "0x" + intVal.ToString("x16");
            }
        }

        private RealType _finalType;
        public override OberonType GetFinalType(Scope scope)
        {
            return _finalType;
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
        }

        /// <summary>
        /// Constructor to create a new real number substitution node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NReal(ParseNode original)
            : base(original, true)
        {
            int expStart = Math.Max(base.String.IndexOf('E'), base.String.IndexOf('D'));
            if (expStart == -1) {
                Value = double.Parse(base.String);
                _finalType = RealType.Real;
            } else {
                double mantissa = double.Parse(base.String.Substring(0, expStart));
                double exponent = double.Parse(base.String.Substring(expStart + 1));
                Value = mantissa * Math.Pow(10.0, exponent);
                _finalType = base.String[expStart] == 'E' ? RealType.Real : RealType.LongReal;
            }
        }

        public NReal(ParseNode orig, double value, RealType type)
            : base(orig, true)
        {
            Value = value;
            _finalType = type;
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return EmptyExceptionArray;
        }

        public override LiteralElement EvaluateConst(NExpr orig, LiteralElement other, ExprOperator op, Scope scope)
        {
            if (other is NNumber) {
                other = ((NNumber) other).Inner;
            }

            if (other is NInteger) {
                var that = (NInteger) other;
                switch (op) {
                    case ExprOperator.Equals:
                        return new NBool(orig, this.Value == that.Value);
                    case ExprOperator.NotEquals:
                        return new NBool(orig, this.Value != that.Value);
                    case ExprOperator.GreaterThan:
                        return new NBool(orig, this.Value > that.Value);
                    case ExprOperator.GreaterThanOrEqual:
                        return new NBool(orig, this.Value >= that.Value);
                    case ExprOperator.LessThan:
                        return new NBool(orig, this.Value < that.Value);
                    case ExprOperator.LessThanOrEqual:
                        return new NBool(orig, this.Value <= that.Value);
                }
            } else if (other is NReal) {
                var that = (NReal) other;
                switch (op) {
                    case ExprOperator.Equals:
                        return new NBool(orig, this.Value == that.Value);
                    case ExprOperator.NotEquals:
                        return new NBool(orig, this.Value != that.Value);
                    case ExprOperator.GreaterThan:
                        return new NBool(orig, this.Value > that.Value);
                    case ExprOperator.GreaterThanOrEqual:
                        return new NBool(orig, this.Value >= that.Value);
                    case ExprOperator.LessThan:
                        return new NBool(orig, this.Value < that.Value);
                    case ExprOperator.LessThanOrEqual:
                        return new NBool(orig, this.Value <= that.Value);
                }
            }

            throw new NotImplementedException();
        }

        public override LiteralElement EvaluateConst(NSimpleExpr orig, LiteralElement other, SimpleExprOperator op, Scope scope)
        {
            if (other is NNumber) {
                other = ((NNumber) other).Inner;
            }

            if (other is NInteger) {
                var that = (NInteger) other;
                switch (op) {
                    case SimpleExprOperator.Add:
                        return new NReal(orig, this.Value + that.Value, (RealType) orig.GetFinalType(scope));
                    case SimpleExprOperator.Subtract:
                        return new NReal(orig, this.Value - that.Value, (RealType) orig.GetFinalType(scope));
                }
            } else if (other is NReal) {
                var that = (NReal) other;
                switch (op) {
                    case SimpleExprOperator.Add:
                        return new NReal(orig, this.Value + that.Value, (RealType) orig.GetFinalType(scope));
                    case SimpleExprOperator.Subtract:
                        return new NReal(orig, this.Value - that.Value, (RealType) orig.GetFinalType(scope));
                }
            }

            throw new NotImplementedException();
        }

        public override LiteralElement EvaluateConst(NTerm orig, LiteralElement other, TermOperator op, Scope scope)
        {
            if (other is NNumber) {
                other = ((NNumber) other).Inner;
            }

            if (other is NInteger) {
                var that = (NInteger) other;
                switch (op) {
                    case TermOperator.Multiply:
                        return new NReal(orig, this.Value * that.Value, (RealType) orig.GetFinalType(scope));
                    case TermOperator.Divide:
                        return new NReal(orig, this.Value / that.Value, (RealType) orig.GetFinalType(scope));
                }
            } else if (other is NReal) {
                var that = (NReal) other;
                switch (op) {
                    case TermOperator.Multiply:
                        return new NReal(orig, this.Value * that.Value, (RealType) orig.GetFinalType(scope));
                    case TermOperator.Divide:
                        return new NReal(orig, this.Value / that.Value, (RealType) orig.GetFinalType(scope));
                }
            }

            throw new NotImplementedException();
        }

        public override LiteralElement EvaluateConst(NUnary orig, UnaryOperator op, Scope scope)
        {
            switch (op) {
                case UnaryOperator.Identity:
                    return this;
                case UnaryOperator.Negation:
                    return new NReal(orig, -this.Value, (RealType) orig.GetFinalType(scope));
            }

            throw new NotImplementedException();
        }
    }
}
