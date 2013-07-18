using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public class Statement : SubstituteNode, ITypeErrorSource
    {
        public Statement(ParseNode original)
            : base(original, false)
        {

        }

        public virtual IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                ? ((ITypeErrorSource) x).FindTypeErrors(scope)
                : new ParserException[0]);
        }
    }

    [SubstituteToken("Assignment")]
    public class NAssignment : Statement
    {
        public NDesignator Assignee
        {
            get { return (NDesignator) Children.First(); }
        }

        public NExpr Expression
        {
            get { return (NExpr) Children.Last(); }
        }

        public NAssignment(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NDesignator || x is NExpr);
        }
    }

    [SubstituteToken("InvocStmnt")]
    public class NInvocStmnt : Statement
    {
        public NDesignator Invocation
        {
            get { return (NDesignator) Children.First(); }
        }

        public NInvocStmnt(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NDesignator);
        }
    }

    [SubstituteToken("IfThenElse")]
    public class NIfThenElse : Statement
    {
        public NExpr Condition
        {
            get { return (NExpr) Children.First(); }
        }

        public NStatementSeq ThenBody
        {
            get { return (NStatementSeq) Children.First(x => x is NStatementSeq); }
        }

        public NStatementSeq ElseBody
        {
            get { return (NStatementSeq) Children.Last(x => x is NStatementSeq); }
        }

        public NIfThenElse(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NExpr || x is NStatementSeq);

            if (Children.Count(x => x is NExpr) > 1) {
                var inner = new NIfThenElse(new BranchNode(Children.Skip(2), Token));
                var stmnt = new NStatement(new BranchNode(new ParseNode[] { inner }, "Statement"));
                Children = new ParseNode[] {
                    Condition,
                    ThenBody,
                    new NStatementSeq(new BranchNode(new ParseNode[] { stmnt }, "StatementSeq"))
                };
            }
        }

    }

    [SubstituteToken("CaseLabels")]
    public class NCaseLabel : SubstituteNode
    {
        public NConstExpr Min
        {
            get { return (NConstExpr) Children.First(); }
        }

        public NConstExpr Max
        {
            get { return (NConstExpr) Children.Last(); }
        }

        public NCaseLabel(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NConstExpr);
        }
    }

    [SubstituteToken("Case")]
    public class NCase : SubstituteNode
    {
        public IEnumerable<NCaseLabel> Labels
        {
            get { return Children.Where(x => x is NCaseLabel).Select(x => (NCaseLabel) x); }
        }

        public NStatementSeq Statements
        {
            get { return (NStatementSeq) Children.Last(); }
        }

        public NCase(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NCaseLabel || x is NStatementSeq);
        }
    }

    [SubstituteToken("SwitchCase")]
    public class NSwitchCase : Statement
    {
        public NExpr Expression
        {
            get { return (NExpr) Children.First(); }
        }

        public IEnumerable<NCase> Cases
        {
            get { return Children.Where(x => x is NCase).Select(x => (NCase) x); }
        }

        public NStatementSeq Else
        {
            get { return Children.Last() as NStatementSeq; }
        }

        public NSwitchCase(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NExpr || x is NCase || x is NStatementSeq);
        }
    }

    [SubstituteToken("WhileLoop")]
    public class NWhileLoop : Statement
    {
        public NExpr Condition 
        {
            get { return (NExpr) Children.First(); }
        }

        public NStatementSeq Body
        {
            get { return (NStatementSeq) Children.Last(); }
        }

        public NWhileLoop(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NExpr || x is NStatementSeq);
        }
    }

    [SubstituteToken("RepeatUntil")]
    public class NRepeatUntil : Statement
    {
        public NStatementSeq Body
        {
            get { return (NStatementSeq) Children.First(); }
        }

        public NExpr Condition
        {
            get { return (NExpr) Children.Last(); }
        }

        public NRepeatUntil(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NExpr || x is NStatementSeq);
        }
    }

    [SubstituteToken("ForLoop")]
    public class NForLoop : Statement
    {
        public String IteratorName
        {
            get { return Children.First().String; }
        }

        public NExpr Initial
        {
            get { return (NExpr) Children.ElementAt(1); }
        }

        public NExpr Final
        {
            get { return (NExpr) Children.ElementAt(2); }
        }

        public NConstExpr Increment
        {
            get { return Children.ElementAt(3) as NConstExpr; }
        }

        public NStatementSeq Body
        {
            get { return (NStatementSeq) Children.Last(); }
        }

        public NForLoop(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NIdent || x is NExpr || x is NConstExpr || x is NStatementSeq);
        }
    }

    [SubstituteToken("UncondLoop")]
    public class NUncondLoop : Statement
    {
        public NStatementSeq Body
        {
            get { return (NStatementSeq) Children.First(); }
        }

        public NUncondLoop(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NStatementSeq);
        }
    }

    [SubstituteToken("WithCase")]
    public class NWithCase : SubstituteNode
    {
        public NQualIdent Identifier
        {
            get { return (NQualIdent) Children.First(); }
        }

        public NNamedType Type
        {
            get { return (NNamedType) Children.ElementAt(1); }
        }

        public NStatementSeq Body
        {
            get { return (NStatementSeq) Children.Last(); }
        }

        public NWithCase(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NQualIdent || x is NNamedType || x is NStatementSeq);
        }
    }

    [SubstituteToken("WithDo")]
    public class NWithDo : Statement
    {
        public IEnumerable<NWithCase> Cases
        {
            get { return Children.Where(x => x is NWithCase).Select(x => (NWithCase) x); }
        }

        public NStatementSeq Else
        {
            get { return Children.Last() as NStatementSeq; }
        }

        public NWithDo(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NWithCase || x is NStatementSeq);
        }
    }

    [SubstituteToken("Exit")]
    public class NExit : Statement
    {
        public NExit(ParseNode original)
            : base(original)
        {
            Children = new ParseNode[0];
        }
    }

    [SubstituteToken("Return")]
    public class NReturn : Statement
    {
        public NExpr Expression
        {
            get { return (NExpr) Children.FirstOrDefault(); }
        }

        public NReturn(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NExpr);
        }
    }
}
