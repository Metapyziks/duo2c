using System;
using System.Collections.Generic;
using System.Globalization;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for integers.
    /// </summary>
    [SubstituteToken("integer")]
    public class NInteger : LiteralElement
    {
        /// <summary>
        /// The parsed value of the integer.
        /// </summary>
        public long Value { get; private set; }

        public override string String
        {
            get {
                return Value.ToString();
            }
        }

        /// <summary>
        /// Finds the type of integer this literal represents
        /// based on the number of bytes required to store it.
        /// </summary>
        public override OberonType GetFinalType(Scope scope)
        {
            if (sbyte.MinValue <= Value && Value <= sbyte.MaxValue) {
                return IntegerType.Byte;
            } else if (short.MinValue <= Value && Value <= short.MaxValue) {
                return IntegerType.ShortInt;
            } else if (int.MinValue <= Value && Value <= int.MaxValue) {
                return IntegerType.Integer;
            } else {
                return IntegerType.LongInt;
            }
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
        }

        /// <summary>
        /// Constructor to create a new integer substitution
        /// node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NInteger(ParseNode original)
            : base(original, true)
        {
            if (base.String.EndsWith("H")) {
                // Hex integer literals end with a "H"
                Value = int.Parse(base.String.Substring(0, base.String.Length - 1), NumberStyles.HexNumber);
            } else {
                Value = int.Parse(base.String);
            }
        }

        public NInteger(ParseNode orig, long value)
            : base(orig, true)
        {
            Value = value;
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
                        return new NInteger(orig, this.Value + that.Value);
                    case SimpleExprOperator.Subtract:
                        return new NInteger(orig, this.Value - that.Value);
                    case SimpleExprOperator.Or:
                        return new NInteger(orig, this.Value | that.Value);
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
                        return new NInteger(orig, this.Value * that.Value);
                    case TermOperator.Divide:
                        return new NReal(orig, (double) this.Value / that.Value, (RealType) orig.GetFinalType(scope));
                    case TermOperator.IntDivide:
                        return new NInteger(orig, this.Value / that.Value);
                    case TermOperator.Modulo:
                        return new NInteger(orig, this.Value % that.Value);
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
                    return new NInteger(orig, -this.Value);
                case UnaryOperator.Not:
                    return new NInteger(orig, ~this.Value);
            }

            throw new NotImplementedException();
        }
    }
}
