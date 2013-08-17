using System;
using System.Collections.Generic;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for strings.
    /// </summary>
    [SubstituteToken("string")]
    public class NString : ExpressionElement
    {
        private String _string;

        public override string String
        {
            get {
                return _string;
            }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            return new ArrayType(CharType.Default, String.Length);
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
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
                            _string += '\x1B'; break;
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

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return EmptyExceptionArray;
        }
    }
}
