using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUO2C
{
    /// <summary>
    /// Represents nodes that contain other child nodes.
    /// </summary>
    public class BranchNode : ParseNode, IEnumerable<ParseNode>
    {
        /// <summary>
        /// Collection of child nodes within this node.
        /// </summary>
        public IEnumerable<ParseNode> Children { get; private set; }

        public override String String
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var node in Children) {
                    builder.Append(node.String);
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Constructor for a new BranchNode.
        /// </summary>
        /// <param name="children">Collection of child nodes within this node</param>
        /// <param name="token">String identifying the type of this node</param>
        public BranchNode(IEnumerable<ParseNode> children, String token = null)
            : base(token)
        {
            Children = children.SelectMany(x => x is BranchNode && x.Token == null ? ((BranchNode) x).Children : new ParseNode[] { x });
        }

        public override String ToString(string indent)
        {
            var nl = Environment.NewLine;
            var nextIndent = indent + "  ";
            return indent + Token + " : {" + nl + String.Join("," + nl, Children.Select(x => (x != null ? x.ToString(nextIndent) : "null"))) + nl + indent + "}";
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
}
