﻿using System;
using System.Reflection;
using System.Collections.Generic;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Represents nodes that cannot contain children.
    /// </summary>
    public class LeafNode : ParseNode
    {
        private String _string;
        public override String String { get { return _string; } }

        public override bool IsNull { get { return false; } }

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

        public override string SerializeXML()
        {
            return String.Format("<{0} index=\"{1}\" length=\"{2}\">{3}</{0}>",
                Token, StartIndex, Length, String);
        }
    }
}
