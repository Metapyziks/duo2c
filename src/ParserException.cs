using System;

namespace DUO2C
{
    /// <summary>
    /// Attribute used to compare the usefulness of syntax
    /// parsing errors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExceptionUtilityAttribute : Attribute
    {
        /// <summary>
        /// The usefulness of this parser exception.
        /// </summary>
        public int Utility { get; private set; }

        /// <summary>
        /// Creates a new ExceptionUtility attribute.
        /// </summary>
        /// <param name="utility">The usefulness of this parser exception</param>
        public ExceptionUtilityAttribute(int utility)
        {
            Utility = utility;
        }
    }

    public enum ParserError
    {
        Syntax,
        Semantics
    }

    /// <summary>
    /// Base class for exceptions thrown by the parsing process.
    /// </summary>
    [ExceptionUtility(0)]
    public class ParserException : Exception
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
        /// Gets a message that describes the current ParseException.
        /// </summary>
        public String MessageNoLocation
        {
            get { return base.Message; }
        }

        public ParserError ErrorType { get; private set; }

        /// <summary>
        /// Gets a message that describes the current ParseException, along with
        /// the location it occurred.
        /// </summary>
        public override String Message
        {
            get {
                return String.Format("{0} ({1},{2}) : {3}{4}", SourcePath ?? "",
                    Line, Column, MessageNoLocation,
                    SourceString != null ? String.Format(" ({0})", SourceString) : "");
            }
        }

        /// <summary>
        /// The usefulness of this parser exception.
        /// </summary>
        public int Utility
        {
            get {
                var attrib = GetType().GetCustomAttributes(typeof(ExceptionUtilityAttribute), true)[0];
                return ((ExceptionUtilityAttribute) attrib).Utility;
            }
        }

        /// <summary>
        /// Constructor to create a new parser exception, containing information
        /// about the location in the source string that the exception occurred.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="index">Start index in the source string of the exception</param>
        public ParserException(ParserError type, String message, int index, int length = 0)
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
            FindSymbolLocation(srcString, SourceIndex, out line, out column);

            Line = line;
            Column = column;

            if (SourceLength > 0) {
                SourceString = srcString.Substring(SourceIndex, SourceLength);
            }
        }

        internal void SetSourcePath(String srcPath)
        {
            SourcePath = srcPath;
        }
    }
}
