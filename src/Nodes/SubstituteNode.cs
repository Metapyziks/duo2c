using System;
using System.Reflection;
using System.Collections.Generic;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Attribute to specify which token a substitution
    /// node should substitute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
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
    /// Attribute to specify that a property in a substitute node
    /// should be serialized as an attribute with the specified
    /// attribute name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    class SerializeAttribute : Attribute
    {
        /// <summary>
        /// Attribute name to use when serializing the property
        /// marked with this attribute.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Constructor to create a new node property serialize
        /// attribute with a given name for the serialized attribute.
        /// </summary>
        /// <param name="name">Attribute name to use when serializing</param>
        public SerializeAttribute(String name)
        {
            Name = name;
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
            : base(original.StartIndex, original.Length,
                original.String, original.Token) { }

        public override string ToString(String indent)
        {
            var attribs = new List<String> {
                String.Format("index=\"{0}\"", StartIndex),
                String.Format("length=\"{0}\"", Length)
            };
            foreach (var prop in GetType().GetProperties()) {
                var attrib = prop.GetCustomAttribute<SerializeAttribute>();
                if (attrib != null) {
                    attribs.Add(String.Format("{0}=\"{1}\"", attrib.Name,
                        prop.GetValue(this).ToString()));
                }
            }

            return String.Format("{0}<{1} {2}>{3}</{1}>",
                indent, Token, String.Join(" ", attribs), String);
        }
    }
}
