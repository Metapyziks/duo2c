using System;
using System.Diagnostics;

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

            var ruleset = new Ruleset();

            // Rule token declarations
            var rModule = ruleset.CreateRuleToken("Module", true);
            var rImportList = ruleset.CreateRuleToken("ImportList");

            Debug.WriteLine("==== Defining ruleset ====");
            ruleset.Add(rModule, +kMODULE +ident +semicolon [+rImportList] +kEND +ident +fullstop);
            ruleset.Add(rImportList, +kIMPORT [+ident +assignop] +ident *(+comma [+ident +assignop] +ident) +semicolon);
            Debug.WriteLine("==========================");

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
