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

        public virtual IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                ? ((ITypeErrorSource) x).FindTypeErrors(scope)
                : new CompilerException[0]);
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

        public override string String
        {
            get {
                return String.Format("{0} := {1}", Assignee.String, Expression.String);
            }
        }

        public NAssignment(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NDesignator || x is NExpr);
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            var found = false;
            foreach (var e in base.FindTypeErrors(scope)) {
                found = true;
                yield return e;
            }

            if (!found) {
                var leftType = Assignee.GetFinalType(scope);
                var rightType = Expression.GetFinalType(scope);

                if (!leftType.CanTestEquality(rightType)) {
                    yield return new TypeMismatchException(leftType, rightType, Expression);
                }
            }
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
            if (Invocation.IsRoot || !(Invocation.Operation is NInvocation)) {
                Children = new ParseNode[] {
                    new NDesignator(new BranchNode(new ParseNode[] {
                        Invocation, new NInvocation(new BranchNode(Invocation.EndIndex, "Invocation"))
                    }, Invocation.Token))
                };
            }
        }
    }

    [SubstituteToken("IfThenElse")]
    public class NIfThenElse : Statement
    {
        public override string String
        {
            get {
                return String.Format("IF {0} THEN", Condition.String);
            }
        }

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
            get { return (NStatementSeq) Children.LastOrDefault(x => x is NStatementSeq && x != ThenBody); }
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

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool condErrorFound = false;
            foreach (var e in Condition.FindTypeErrors(scope)) {
                condErrorFound = true;
                yield return e;
            }

            if (!condErrorFound) {
                var condType = Condition.GetFinalType(scope);
                if (!condType.IsBool) {
                    yield return new TypeMismatchException(BooleanType.Default, condType, Condition);
                }
            }

            foreach (var e in ThenBody.FindTypeErrors(scope)) {
                yield return e;
            }

            if (ElseBody != null) {
                foreach (var e in ElseBody.FindTypeErrors(scope)) {
                    yield return e;
                }
            }
        }
    }

    [SubstituteToken("CaseLabels")]
    public class NCaseLabel : SubstituteNode, ITypeErrorSource
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

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            foreach (var e in Min.FindTypeErrors(scope)) {
                yield return e;
            }

            if (Max != Min) {
                foreach (var e in Max.FindTypeErrors(scope)) {
                    yield return e;
                }
            }
        }
    }

    [SubstituteToken("Case")]
    public class NCase : SubstituteNode, IDeclarationSource, ITypeErrorSource
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

        public void FindDeclarations(Scope scope)
        {
            Statements.FindDeclarations(scope);
        }

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Labels.SelectMany(x => x.FindTypeErrors(scope)).Union(Statements.FindTypeErrors(scope));
        }
    }

    [SubstituteToken("SwitchCase")]
    public class NSwitchCase : Statement, IDeclarationSource
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

        public void FindDeclarations(Scope scope)
        {
            foreach (var c in Cases) {
                c.FindDeclarations(scope);
            }
        }

        public NSwitchCase(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NExpr || x is NCase || x is NStatementSeq);
        }
    }

    [SubstituteToken("WhileLoop")]
    public class NWhileLoop : Statement, IDeclarationSource
    {
        public override string String
        {
            get {
                return String.Format("WHILE {0} DO", Condition.String);
            }
        }

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

        public void FindDeclarations(Scope scope)
        {
            Body.FindDeclarations(scope);
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool condErrorFound = false;
            foreach (var e in Condition.FindTypeErrors(scope)) {
                condErrorFound = true;
                yield return e;
            }

            if (!condErrorFound) {
                var condType = Condition.GetFinalType(scope);
                if (!condType.IsBool) {
                    yield return new TypeMismatchException(BooleanType.Default, condType, Condition);
                }
            }

            foreach (var e in Body.FindTypeErrors(scope)) {
                yield return e;
            }
        }
    }

    [SubstituteToken("RepeatUntil")]
    public class NRepeatUntil : Statement, IDeclarationSource
    {
        public override string String
        {
            get {
                return String.Format("REPEAT UNTIL {0}", Condition.String);
            }
        }

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

        public void FindDeclarations(Scope scope)
        {
            Body.FindDeclarations(scope);
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool condErrorFound = false;
            foreach (var e in Condition.FindTypeErrors(scope)) {
                condErrorFound = true;
                yield return e;
            }

            if (!condErrorFound) {
                var condType = Condition.GetFinalType(scope);
                if (!condType.IsBool) {
                    yield return new TypeMismatchException(BooleanType.Default, condType, Condition);
                }
            }

            foreach (var e in Body.FindTypeErrors(scope)) {
                yield return e;
            }
        }
    }

    [SubstituteToken("ForLoop")]
    public class NForLoop : Statement, IDeclarationSource
    {
        public override string String
        {
            get {
                return String.Format("FOR {0} := {1} TO {2} DO", IteratorName, Initial.String, Final.String);
            }
        }

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
        
        public void FindDeclarations(Scope scope)
        {
            Body.FindDeclarations(scope);
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            var iterDecl = scope.GetSymbolDecl(IteratorName);
            if (iterDecl == null) {
                yield return new UndeclaredIdentifierException(Children.First());
            } else if(!iterDecl.Type.IsNumeric) {
                yield return new TypeMismatchException(NumericType.Default, iterDecl.Type, Children.First());
                iterDecl = null;
            }

            bool found = false;
            foreach (var e in Initial.FindTypeErrors(scope)) {
                found = true;
                yield return e;
            }

            if (!found && iterDecl != null && !iterDecl.Type.CanTestEquality(Initial.GetFinalType(scope))) {
                yield return new TypeMismatchException(iterDecl.Type, Initial.GetFinalType(scope), Initial);
            }

            found = false;
            foreach (var e in Final.FindTypeErrors(scope)) {
                found = true;
                yield return e;
            }

            if (!found && iterDecl != null && !iterDecl.Type.CanTestEquality(Final.GetFinalType(scope))) {
                yield return new TypeMismatchException(iterDecl.Type, Final.GetFinalType(scope), Final);
            }

            if (Increment != null) {
                found = false;
                foreach (var e in Increment.FindTypeErrors(scope)) {
                    found = true;
                    yield return e;
                }

                if (!found && iterDecl.Type != null && !iterDecl.Type.CanTestEquality(Increment.GetFinalType(scope))) {
                    yield return new TypeMismatchException(iterDecl.Type, Increment.GetFinalType(scope), Increment);
                }
            }

            foreach (var e in Body.FindTypeErrors(scope)) {
                yield return e;
            }
        }
    }

    [SubstituteToken("UncondLoop")]
    public class NUncondLoop : Statement, IDeclarationSource
    {
        public override string String
        {
            get { return "LOOP"; }
        }

        public NStatementSeq Body
        {
            get { return (NStatementSeq) Children.First(); }
        }

        public NUncondLoop(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NStatementSeq);
        }

        public void FindDeclarations(Scope scope)
        {
            Body.FindDeclarations(scope);
        }
    }

    [SubstituteToken("WithCase")]
    public class NWithCase : SubstituteNode, IDeclarationSource
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

        public void FindDeclarations(Scope scope)
        {
            Body.FindDeclarations(scope);
        }
    }

    [SubstituteToken("WithDo")]
    public class NWithDo : Statement, IDeclarationSource
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

        public void FindDeclarations(Scope scope)
        {
            foreach (var c in Cases) {
                c.FindDeclarations(scope);
            }

            if (Else != null) {
                Else.FindDeclarations(scope);
            }
        }
    }

    [SubstituteToken("Exit")]
    public class NExit : Statement
    {
        public override string String
        {
            get { return "EXIT"; }
        }

        public NExit(ParseNode original)
            : base(original)
        {
            Children = new ParseNode[0];
        }
    }

    [SubstituteToken("Return")]
    public class NReturn : Statement
    {
        public override string String
        {
            get {
                if (Expression != null) {
                    return String.Format("RETURN {0}", Expression.String);
                } else {
                    return "RETURN";
                }
            }
        }

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
