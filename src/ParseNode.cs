using System;

namespace DUO2C
{
    /// <summary>
    /// Abstract class for a node parsed from a string.
    /// </summary>
    public abstract class ParseNode
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
}
