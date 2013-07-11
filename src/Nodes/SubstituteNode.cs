using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Attribute to specify which token a substitution
    /// node should substitute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SubstituteTokenAttribute : Attribute
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
    public class SerializeAttribute : Attribute
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
    public abstract class SubstituteNode : BranchNode
    {
        public bool IsLeaf
        {
            get; private set;
        }

        public bool HasPayload
        {
            get; private set;
        }

        /// <summary>
        /// Abstract constructor, the parameter list of which must be used
        /// by any extending classes that wish to be used as valid substitution
        /// nodes.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public SubstituteNode(ParseNode original, bool leaf = true, bool hasPayload = true)
            : base(original is BranchNode
                ? ((BranchNode) original).Children
                : new ParseNode[] { original }, original.Token)
        {
            IsLeaf = leaf;
            HasPayload = hasPayload;
        }

        public override String SerializeXML()
        {
            var attribs = new List<String> {
                String.Format("index=\"{0}\"", StartIndex),
                String.Format("length=\"{0}\"", Length)
            };

            foreach (var prop in GetType().GetProperties()) {
                var attrib = prop.GetCustomAttribute<SerializeAttribute>();
                if (attrib != null) {
                    var val = prop.GetValue(this);
                    if (val != null) {
                        attribs.Add(String.Format("{0}=\"{1}\"", attrib.Name,
                            prop.GetValue(this).ToString()));
                    }
                }
            }

            if (!HasPayload) {
                return String.Format("<{0} {1} />", Token, String.Join(" ", attribs));
            } if (IsLeaf) {
                return String.Format("<{0} {1}>{2}</{0}>", Token, String.Join(" ", attribs), String);
            } else {
                var nl = Environment.NewLine;
                return String.Format("<{0} {1}>", Token, String.Join(" ", attribs))
                    + (Children.Count() > 0 ? nl + String.Join(nl, Children.Select(x => x.SerializeXML())) : "")
                        .Replace("\n", "\n  ") + nl + String.Format("</{0}>", Token);
            }
        }
    }
}
