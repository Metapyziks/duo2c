﻿using System;

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
        /// Start index of this node in the original source string.
        /// </summary>
        public int SourceIndex { get; private set; }

        /// <summary>
        /// Length of this node in the original source string.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Abstract constructor for a new ParseNode.
        /// </summary>
        /// <param name="index">Start index of this node in the original source string</param>
        /// <param name="length">Length of this node in the original source string</param>
        /// <param name="token">String identifying the type of this node</param>
        public ParseNode(int index, int length, String token = null)
        {
            SourceIndex = index;
            Length = length;

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
