using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;

namespace DUO2C.CodeGen
{
    public class GenerationContext
    {
        public static GenerationContext operator +(GenerationContext ctx, SubstituteNode node)
        {
            return ctx.Write(node);
        }

        public static GenerationContext operator +(GenerationContext ctx, String str)
        {
            return ctx.Write(str);
        }

        private int _indentCols;

        private String _indent;
        private bool _newLine;
        private StringBuilder _builder;

        public String GeneratedCode
        {
            get { return _builder.ToString(); }
        }

        public GenerationContext(int indentCols = 4)
        {
            _indentCols = indentCols;

            _indent = String.Empty;
            _newLine = true;
            _builder = new StringBuilder();

        }

        public GenerationContext NewLine()
        {
            _builder.AppendLine();
            _newLine = true;

            return this;
        }

        public GenerationContext Write(SubstituteNode node)
        {
            node.GenerateCode(this);
            return this;
        }

        public GenerationContext Write(String format, params Object[] args)
        {
            if (_newLine) {
                _builder.Append(_indent);
                _newLine = false;
            }

            if (args.Length == 0) {
                _builder.Append(format);
            } else {
                _builder.AppendFormat(format, args);
            }

            return this;
        }

        public GenerationContext Enter()
        {
            for (int i = 0; i < _indentCols; ++i) {
                _indent += " ";
            }

            return this;
        }

        public GenerationContext Leave()
        {
            _indent = _indent.Substring(0, _indent.Length - _indentCols);
            
            return this;
        }
    }
}
