using System;
using System.Linq;
using System.Collections.Generic;

namespace DUO2C
{
    public enum ParserError
    {
        ExpectedSymbol,
        Semantics
    }

    /// <summary>
    /// Base class for exceptions thrown by the compiling process.
    /// </summary>
    public class CompilerException : Exception
    {
        /// <summary>
        /// Utility function that finds the line and column number of a 
        /// given index in a string.
        /// </summary>
        /// <param name="src">String to find a location within</param>
        /// <param name="index">Index of the symbol to find the location of</param>
        /// <param name="line">Outputted line number</param>
        /// <param name="column">Outputted column number</param>
        static void FindSymbolLocation(String str, int index, out int line, out int column, out String snippet)
        {
            if (index < 0 || index >= str.Length) {
                // Just in case...
                line = column = 0;
                snippet = "";
                return;
            }

            int linestart = 0;
            line = column = 1;
            for (int i = 0; i < index; ++i) {
                switch (str[i]) {
                    case '\n':
                        linestart = i + 1;
                        ++line; column = 1;
                        break;
                    case '\r':
                        column = 1;
                        break;
                    case '\t':
                        column += 4;
                        break;
                    case '\0':
                        break;
                    default:
                        ++column;
                        break;
                }
            }

            if (linestart < str.Length) {
                int end = str.IndexOf('\n', linestart);
                if (end == -1) end = str.Length;
                snippet = str.Substring(linestart, end - linestart);
            } else {
                snippet = "";
            }
        }

        /// <summary>
        /// Path to the file that contains the code being parsed.
        /// </summary>
        public String SourcePath { get; private set; }

        /// <summary>
        /// Start index of the location of this exception in the original
        /// source string.
        /// </summary>
        public int SourceIndex { get; private set; }

        /// <summary>
        /// Length of the element that caused this exception in the original
        /// source string.
        /// </summary>
        public int SourceLength { get; private set; }

        /// <summary>
        /// Substring of the source element that caused this exception if one is
        /// available, otherwise null.
        /// </summary>
        public String SourceString { get; private set; }

        /// <summary>
        /// Line number in the source string where this exception occurred.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Column number in the source string where this exception occurred.
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// The source text of the line in which the exception was found.
        /// </summary>
        public String LineSnippet { get; private set; }

        /// <summary>
        /// Gets a message that describes the current ParseException.
        /// </summary>
        public virtual String MessageNoLocation
        {
            get { return base.Message; }
        }

        public ParserError ErrorType { get; private set; }

        public String Location
        {
            get { return String.Format("{0} ({1},{2})", SourcePath ?? "", Line, Column); }
        }

        /// <summary>
        /// Gets a message that describes the current ParseException, along with
        /// the location it occurred.
        /// </summary>
        public override String Message
        {
            get {
                return String.Format("{0} : {1}", Location, MessageNoLocation);
            }
        }

        /// <summary>
        /// The usefulness of this parser exception.
        /// </summary>
        public int Utility { get; protected set; }

        /// <summary>
        /// Constructor to create a new parser exception, containing information
        /// about the location in the source string that the exception occurred.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="index">Start index in the source string of the exception</param>
        public CompilerException(ParserError type, String message, int index, int length = 0)
            : base(message)
        {
            ErrorType = type;

            SourcePath = null;
            SourceIndex = index;
            SourceLength = length;

            Line = -1;
            Column = -1;
        }

        internal void FindLocationInfo(String srcString)
        {
            int line, column;
            String snippet;
            FindSymbolLocation(srcString, SourceIndex, out line, out column, out snippet);

            Line = line;
            Column = column;
            LineSnippet = snippet;

            if (SourceLength > 0) {
                SourceString = srcString.Substring(SourceIndex, SourceLength);
            }
        }

        internal void SetSourcePath(String srcPath)
        {
            SourcePath = srcPath;
        }
    }

    public class SymbolExpectedException : CompilerException
    {
        public String Symbol { get; private set; }

        public SymbolExpectedException(String symbol, int index, int utility = 0)
            : base(ParserError.ExpectedSymbol, String.Format("{0} expected", symbol), index)
        {
            Symbol = symbol;
            Utility = utility;
        }
    }

    public class CombinedException : CompilerException
    {
        private IEnumerable<CompilerException> _exceptions;

        public override string MessageNoLocation
        {
            get {
                if (_exceptions.All(x => x is SymbolExpectedException)) {
                    var es = _exceptions.Select(x => ((SymbolExpectedException) x).Symbol).Distinct().OrderBy(x => x);
                    return String.Join(", ", es.Where(x => x != es.Last()))
                        + (es.Count() > 0 ? " or " + es.Last() : "") + " expected";
                }
                return String.Join(" or ",  _exceptions.Select(x => x is SymbolExpectedException
                    ? ((SymbolExpectedException) x).Symbol : x.MessageNoLocation).Distinct());
            }
        }

        public CombinedException(params CompilerException[] exceptions)
            : base(exceptions.First().ErrorType, null, exceptions.First().SourceIndex, 0)
        {
            _exceptions = exceptions.SelectMany(x => x is CombinedException
                ? ((CombinedException) x)._exceptions : new CompilerException[] { x });

            Utility = _exceptions.Max(x => x.Utility);
        }
    }
}
