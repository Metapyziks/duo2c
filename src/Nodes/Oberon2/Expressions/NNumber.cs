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
    public class NNumber : LiteralElement
    {
        public LiteralElement Inner { get; private set; }
        
        public override string String
        {
            get { return Inner.String; }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            return Inner.GetFinalType(scope);
        }

        public override bool IsConstant(Scope scope)
        {
            return Inner.IsConstant(scope);
        }

        public NNumber(ParseNode original)
            : base(original, false)
        {
            Inner = (LiteralElement) Children.First();
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return EmptyExceptionArray;
        }
    }
}
