using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUO2C
{
    /// <summary>
    /// Abstract class for a node parsed from a string.
    /// </summary>
    abstract class ParseNode
    {
        /// <summary>
        /// String identifying the type of this node.
        /// </summary>
        public String Token { get; set; }

        /// <summary>
        /// The parsed contents of this node.
        /// </summary>
        public abstract String String { get; }

        /// <summary>
        /// Abstract constructor for a new ParseNode.
        /// </summary>
        /// <param name="token">String identifying the type of this node</param>
        public ParseNode(String token = null)
        {
            Token = token;
        }

        public override String ToString()
        {
            return ToString(String.Empty);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String, allowing
        /// for the result to be indented using a specified prefix.
        /// </summary>
        /// <param name="indent">Prefix to indent the result with</param>
        /// <returns>System.String representing the value of this instance</returns>
        public abstract String ToString(String indent);
    }

    /// <summary>
    /// Represents nodes that contain other child nodes.
    /// </summary>
    class BranchNode : ParseNode, IEnumerable<ParseNode>
    {
        /// <summary>
        /// Collection of child nodes within this node.
        /// </summary>
        public IEnumerable<ParseNode> Children { get; private set; }

        public override String String
        {
            get {
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
            return indent + Token + " : {" + nl + String.Join("," + nl, Children.Select(x => (x != null ? x.ToString(nextIndent) : "null" ))) + nl + indent + "}";
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

    /// <summary>
    /// Represents nodes that cannot contain children.
    /// </summary>
    class LeafNode : ParseNode
    {
        private String _string;
        public override String String { get { return _string; } }

        /// <summary>
        /// Constructor for a new LeafNode.
        /// </summary>
        /// <param name="str">The parsed contents of this node</param>
        /// <param name="token">String identifying the type of this node</param>
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
