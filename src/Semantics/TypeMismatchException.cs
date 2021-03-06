﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Parsers;
using DUO2C.Nodes;
using DUO2C.Nodes.Oberon2;

namespace DUO2C.Semantics
{
    public class TypeMismatchException : CompilerException
    {
        public OberonType Expected { get; private set; }
        public OberonType Actual { get; private set; }

        public TypeMismatchException(OberonType expected, OberonType actual, ParseNode node)
            : base(ParserError.Semantics, String.Format("Expected {0}, found {1}", expected, actual),
                node.StartIndex, node.Length)
        {
            Expected = expected;
            Actual = actual;
        }
    }
}
