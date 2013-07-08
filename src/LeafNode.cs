using System;

namespace DUO2C
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
