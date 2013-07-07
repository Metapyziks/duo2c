using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUO2C
{
    abstract class ParseNode
    {
        public String Token { get; set; }
        public abstract String String { get; }

        public ParseNode(String token = null)
        {
            Token = token;
        }

        public override String ToString()
        {
            return ToString(String.Empty);
        }

        public abstract String ToString(String indent);
    }

    class BranchNode : ParseNode, IEnumerable<ParseNode>
    {
        public IEnumerable<ParseNode> Children { get; private set; }
        public override String String
        {
            get {
                var builder = new StringBuilder();
                foreach (var node in Children) {
                    if (node != Children.First()) builder.Append(" ");
                    builder.Append(node.String);
                }
                return builder.ToString();
            }
        }

        public BranchNode(IEnumerable<ParseNode> children, String token = null)
            : base(token)
        {
            Children = children.SelectMany(x => x is BranchNode && x.Token == null ? ((BranchNode) x).Children : new ParseNode[] { x });
        }

        public override String ToString(string indent)
        {
            var nl = Environment.NewLine;
            var nextIndent = indent + "  ";
            return indent + Token + " : {" + nl + String.Join("," + nl, Children.Select(x => x.ToString(nextIndent))) + nl + indent + "}";
        }

        public IEnumerator<ParseNode> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Children.GetEnumerator();
        }
    }

    class LeafNode : ParseNode
    {
        private String _string;
        public override String String { get { return _string; } }

        public LeafNode(String str, String token = null)
            : base(token)
        {
            _string = str;
        }

        public override String ToString(string indent)
        {
            return indent + Token + " : \"" + _string + "\"";
        }
    }
}
