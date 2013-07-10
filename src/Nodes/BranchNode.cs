using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Represents nodes that contain other child nodes.
    /// </summary>
    public class BranchNode : ParseNode, IEnumerable<ParseNode>
    {
        /// <summary>
        /// Utility function to get the source index of a list of sibling nodes.
        /// </summary>
        /// <param name="children">List of siblings</param>
        /// <returns>Source index of the list</returns>
        static int GetBranchIndex(IEnumerable<ParseNode> children)
        {
            return children.First().StartIndex;
        }

        /// <summary>
        /// Utility function to get the source length of a list of sibling nodes.
        /// </summary>
        /// <param name="children">List of siblings</param>
        /// <returns>Source length of the list</returns>
        static int GetBranchLength(IEnumerable<ParseNode> children)
        {
            return children.Last().StartIndex - children.First().StartIndex + children.Last().Length;
        }

        /// <summary>
        /// Collection of child nodes within this node.
        /// </summary>
        public IEnumerable<ParseNode> Children { get; private set; }

        public override bool IsNull
        {
            get { return Token == null && Children.Count() == 0; }
        }

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
            : base(GetBranchIndex(children), GetBranchLength(children), token)
        {
            Children = children.SelectMany(x => x is BranchNode && x.Token == null ? ((BranchNode) x).Children : new ParseNode[] { x });
        }

        /// <summary>
        /// Constructor for an empty BranchNode.
        /// </summary>
        /// <param name="index">Start index of this node in the original source string</param>
        /// <param name="token">String identifying the type of this node</param>
        public BranchNode(int index, String token = null)
            : base(index, 0, token)
        {
            Children = new ParseNode[0];
        }

        public override String ToString(string indent)
        {
            var nl = Environment.NewLine;
            var nextIndent = indent + "  ";
            return String.Format("{0}<{1} index=\"{2}\" length=\"{3}\">", indent, Token, StartIndex, Length)
                + nl + String.Join(nl, Children.Select(x => (x != null ? x.ToString(nextIndent) : "null")))
                + nl + indent + String.Format("</{0}>", Token);
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
