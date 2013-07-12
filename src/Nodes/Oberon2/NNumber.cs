using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for numbers.
    /// </summary>
    [SubstituteToken("number")]
    public class NNumber : ExpressionElement
    {
        public ExpressionElement Inner { get; private set; }
        
        public override string String
        {
            get { return Inner.String; }
        }

        public override OberonType FinalType
        {
            get { return Inner.FinalType; }
        }

        public override bool IsConstant
        {
            get { return Inner.IsConstant; }
        }

        public NNumber(ParseNode original)
            : base(original, false)
        {
            Inner = (ExpressionElement) Children.First();
        }

        public override IEnumerable<ParserException> CheckTypes()
        {
            return EmptyExceptionArray;
        }
    }
}
