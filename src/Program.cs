using System;
using System.Diagnostics;
using System.IO;

namespace DUO2C
{
    class Program
    {
        static void Main(string[] args)
        {
            // Identifier
            var ident = new PIdent();

            // Symbols & Operators
            var fullstop = new PKeyword(".");
            var comma = new PKeyword(",");
            var semicolon = new PKeyword(";");
            var assignop = new PKeyword(":=");

            // Keywords
            var kMODULE = new PKeyword("MODULE");
            var kIMPORT = new PKeyword("IMPORT");
            var kEND = new PKeyword("END");
            var kCONST = new PKeyword("CONST");

            var ruleset = Ruleset.Parse(File.ReadAllText("oberon2.bnf"));

            var src = @"
                MODULE Testing;
                    IMPORT Blah, Thing, Test := Cheese, Another;
                END Testing.
            ";

            var tree = Parser.Parse(src, ruleset);

            Console.WriteLine(tree.ToString());
            Console.ReadKey();
        }
    }
}
