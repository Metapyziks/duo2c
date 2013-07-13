using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Receiver")]
    public class NReceiver : SubstituteNode
    {
        public String Identifier
        {
            get { return Children.First().String; }
        }

        [Serialize("type")]
        public String TypeName { get; private set; }

        public NReceiver(ParseNode original)
            : base(original, true)
        {
            Children = Children.Where(x => x.Token != "keyword");
            TypeName = Children.Last().String;
            Children = new ParseNode[] { Children.First() };
        }
    }

    [SubstituteToken("FPSection")]
    public class NFPSection : SubstituteNode
    {
        [Serialize("byref")]
        public bool ByReference { get; private set; }

        public NFPSection(ParseNode original)
            : base(original, false)
        {
            ByReference = Children.First().Token == "keyword";

            Children = Children.Where(x => x is NIdent || x is NType);
        }
    }

    [SubstituteToken("FormalPars")]
    public class NFormalPars : SubstituteNode
    {
        public IEnumerable<NFPSection> FPSections
        {
            get { return Children.Where(x => x is NFPSection).Select(x => (NFPSection) x); }
        }

        public NType ReturnType
        {
            get { return (NType) Children.FirstOrDefault(x => x is NType); }
        }

        public NFormalPars(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NFPSection || x is NType);
        }
    }

    [SubstituteToken("ForwardDecl")]
    public class NForwardDecl : Declaration
    {
        public NReceiver Receiver
        {
            get { return Children.FirstOrDefault() as NReceiver; }
        }

        private NIdentDef _identDef;
        public override NIdentDef IdentDef
        {
            get { return _identDef; }
        }

        public NForwardDecl(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x.Token != "keyword");
            _identDef = (NIdentDef) Children.First(x => x is NIdentDef);
        }
    }

    [SubstituteToken("ProcDecl")]
    public class NProcDecl : NForwardDecl
    {
        public NProcDecl(ParseNode original)
            : base(original)
        {

        }
    }
}
