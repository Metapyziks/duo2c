using System;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Represents nodes that cannot contain children.
    /// </summary>
    public class LeafNode : ParseNode
    {
        private String _string;
        public override String String { get { return _string; } }

        /// <summary>
        /// Constructor for a new LeafNode.
        /// </summary>
        /// <param name="index">Start index of this node in the original source string</param>
        /// <param name="length">Length of this node in the original source string</param>
        /// <param name="str">The parsed contents of this node</param>
        /// <param name="token">String identifying the type of this node</param>
        public LeafNode(int index, int length, String str, String token = null)
            : base(index, length, token)
        {
            _string = str;
        }

        public override String ToString(String indent)
        {
            return String.Format("{0}<{1} index=\"{2}\" length=\"{3}\">{4}</{1}>",
                indent, Token, StartIndex, Length, _string);
        }
    }
}
