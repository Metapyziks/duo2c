using System;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Substitution node for strings.
    /// </summary>
    [SubstituteToken("string")]
    public class NString : SubstituteNode
    {
        private String _string;

        public override string String
        {
            get {
                return _string;
            }
        }

        /// <summary>
        /// Constructor to create a new string substitution
        /// node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NString(ParseNode original)
            : base(original, true)
        {
            var str = base.String.Trim();
            str = str.Substring(1, str.Length - 2);
            _string = String.Empty;

            int i = 0; var escaped = false;
            while (i < str.Length) {
                var c = str[i++];
                if (!escaped) {
                    if (c == '\\') {
                        escaped = true;
                    } else _string += c;
                } else {
                    switch (c) {
                        case 'e':
                            _string += '\\'; break;
                        case 'r':
                            _string += '\r'; break;
                        case 'n':
                            _string += '\n'; break;
                        case 't':
                            _string += '\t'; break;
                        case '0':
                            _string += '\0'; break;
                        default:
                            _string += c; break;
                    }
                    escaped = false;
                }
            }
        }
    }
}
