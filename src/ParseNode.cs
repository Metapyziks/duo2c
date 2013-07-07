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
    }

    class BranchNode : ParseNode
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

        public override string ToString()
        {
            return Token + ":{" + String.Join(", ", Children) + "}";
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

        public override string ToString()
        {
            return Token + ":\"" + _string + "\"";
        }
    }
}
