﻿using System;
using System.Linq;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public abstract class Selector : SubstituteNode
    {
        public Selector(ParseNode original, bool leaf = false, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }
    }

    /// <summary>
    /// Substitution node for designators, which will either
    /// contain a single NQualifiedIdentifier or a NDesignator
    /// followed by an operation. 
    /// </summary>
    [SubstituteToken("Designator")]
    public class NDesignator : ExpressionElement
    {
        public bool IsRoot
        {
            get { return Element is NQualIdent; }
        }

        public ParseNode Element
        {
            get { return Children.First(); }
        }

        public Selector Operation
        {
            get { return (Selector) Children.Last(); }
        }

        public override string String
        {
            get { return String.Format("{0}{1}", Element.String, Operation.String); }
        }

        // TODO: Lookup actual type
        public override OberonType FinalType
        {
            get { return IntegerType.Default; }
        }

        public override bool IsConstant
        {
            get { return false; }
        }

        public NDesignator(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() >= 2 && Element is NQualIdent) {
                var prev = Children.Take(Children.Count() - 1);
                Children = new ParseNode[] { 
                    new NDesignator(new BranchNode(prev, Token)),
                    Children.Last()
                };
            }
        }
    }
}
