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

        public virtual IEnumerable<ParserException> FindTypeErrors()
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                ? ((ITypeErrorSource) x).FindTypeErrors()
                : (IEnumerable<ParserException>) new ParserException[0]);
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

    [SubstituteToken("Invocation")]
    public class NInvocation : Statement
    {
        public NDesignator Procedure
        {
            get { return (NDesignator) Children.First(); }
        }

        public IEnumerable<NExpr> Arguments
        {
            get { return ((NExprList) Children.Last()).Expressions; }
        }

        public NInvocation(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NDesignator || x is NExprList);
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

    [SubstituteToken("CaseLabel")]
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
        public NWhileLoop(ParseNode original)
            : base(original)
        {

        }

    }

    [SubstituteToken("RepeatUntil")]
    public class NRepeatUntil : Statement
    {
        public NRepeatUntil(ParseNode original)
            : base(original)
        {

        }

    }

    [SubstituteToken("ForLoop")]
    public class NForLoop : Statement
    {
        public NForLoop(ParseNode original)
            : base(original)
        {

        }

    }

    [SubstituteToken("UncondLoop")]
    public class NUncondLoop : Statement
    {
        public NUncondLoop(ParseNode original)
            : base(original)
        {

        }

    }

    [SubstituteToken("WithDo")]
    public class NWithDo : Statement
    {
        public NWithDo(ParseNode original)
            : base(original)
        {

        }

    }

    [SubstituteToken("Exit")]
    public class NExit : Statement
    {
        public NExit(ParseNode original)
            : base(original)
        {

        }

    }

    [SubstituteToken("Return")]
    public class NReturn : Statement
    {
        public NReturn(ParseNode original)
            : base(original)
        {

        }

    }
}
