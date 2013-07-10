using System;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Attribute to specify which token a substitution
    /// node should substitute.
    /// </summary>
    class SubstituteTokenAttribute : Attribute
    {
        /// <summary>
        /// The token that is matched and substituted by the class
        /// with this attribute.
        /// </summary>
        public String Token { get; private set; }

        /// <summary>
        /// Constructor to create a new substitute token attribute which
        /// will substitute the given parsed token with the class with
        /// this attribute.
        /// </summary>
        /// <param name="token">The token that is matched and substituted
        /// by the class with this attribute</param>
        public SubstituteTokenAttribute(String token)
        {
            Token = token;
        }
    }

    /// <summary>
    /// Abstract base class for nodes that substitute generic branch or
    /// leaf nodes and provide some semantical parsing.
    /// </summary>
    abstract class SubstituteNode : LeafNode
    {
        /// <summary>
        /// Abstract constructor, the parameter list of which must be used
        /// by any extending classes that wish to be used as valid substitution
        /// nodes.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public SubstituteNode(ParseNode original)
            : base(original.SourceIndex, original.Length,
                original.String, original.Token) { }
    }
}
