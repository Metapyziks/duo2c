using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for designators, which will either
    /// contain a single NQualifiedIdentifier or a NDesignator
    /// followed by an operation. 
    /// </summary>
    [SubstituteToken("Designator")]
    class NDesignator : SubstituteNode
    {
        public bool IsRoot
        {
            get { return Element is NQualIdent; }
        }

        public ParseNode Element
        {
            get { return Children.First(); }
        }

        public DesignatorOperation Operation
        {
            get { return (DesignatorOperation) Children.Last(); }
        }

        public override string String
        {
            get { return String.Format("{0}{1}", Element.String, Operation.String); }
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
