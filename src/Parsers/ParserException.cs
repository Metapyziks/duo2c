using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Base class for exceptions thrown by the parsing process.
    /// </summary>
    class ParserException : Exception
    {
        /// <summary>
        /// Utility function that finds the line and column number of a 
        /// given index in a string.
        /// </summary>
        /// <param name="src">String to find a location within</param>
        /// <param name="index">Index of the symbol to find the location of</param>
        /// <param name="line">Outputted line number</param>
        /// <param name="column">Outputted column number</param>
        static void FindSymbolLocation(String str, int index, out int line, out int column)
        {
            if (index < 0 || index >= str.Length) {
                // Just in case...
                line = column = 0;
                return;
            }

            line = column = 1;
            for (int i = 0; i < index; ++i) {
                switch (str[i]) {
                    case '\n':
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
        /// Line number in the source string where this exception occurred.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Column number in the source string where this exception occurred.
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// Gets a message that describes the current ParseException.
        /// </summary>
        public String MessageNoLocation
        {
            get { return base.Message; }
        }

        /// <summary>
        /// Gets a message that describes the current ParseException, along with
        /// the location it occurred.
        /// </summary>
        public override String Message
        {
            get {
                return String.Format("{0}({1}:{2}) {3} : {4}", SourcePath ?? "",
                    Line, Column, (InnerException ?? this).GetType().Name, MessageNoLocation);
            }
        }

        /// <summary>
        /// Constructor to create a new parser exception, containing information
        /// about the location in the source string that the exception occurred.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="str">The source string being parsed</param>
        /// <param name="index">Start index in the source string of the exception</param>
        /// <param name="length">Length of the symbol that caused the exception</param>
        public ParserException(String message, String str, int index)
            : base(message)
        {
            SourcePath = null;
            SourceIndex = index;

            int line, column;
            FindSymbolLocation(str, index, out line, out column);

            Line = line;
            Column = column;
        }

        /// <summary>
        /// Constructor to create a clone of a parser exception, but with a given
        /// source file path to be presented with the exception message.
        /// </summary>
        /// <param name="clone">Parser exception to clone</param>
        /// <param name="srcPath">Path to the file that was being parsed</param>
        public ParserException(ParserException clone, String srcPath)
            : base(clone.MessageNoLocation, clone)
        {
            SourcePath = srcPath;
            SourceIndex = clone.SourceIndex;
            Line = clone.Line;
            Column = clone.Column;
        }
    }
}
