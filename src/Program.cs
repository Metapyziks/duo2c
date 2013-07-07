using System;
using System.Diagnostics;

namespace DUO2C
{
    class Program
    {
        static void Main(string[] args)
        {
            var ident = new PIdent();
            var fullstop = new PKeyword(".");
            var comma = new PKeyword(",");
            var semicolon = new PKeyword(";");
            var assignop = new PKeyword(":=");

            Func<String, PKeyword> keyword = (x) => new PKeyword(x);

            var kMODULE = keyword("MODULE");
            var kIMPORT = keyword("IMPORT");
            var kEND = keyword("END");

            var ruleset = new Ruleset();

            var rModule = ruleset.CreateRuleToken("Module", true);
            var rImport = ruleset.CreateRuleToken("Import");
            var rImportList = ruleset.CreateRuleToken("ImportList");

            Debug.WriteLine("==== Defining ruleset ====");
            ruleset.Add(rModule,        +kMODULE +ident +semicolon [+rImportList] +kEND +ident +fullstop    );
            ruleset.Add(rImport,        +ident [+assignop +ident] [+comma +rImport]                         );
            ruleset.Add(rImportList,    +kIMPORT +rImport +semicolon                                        );
            Debug.WriteLine("==========================");

            var src = @"
                MODULE Testing;
                    IMPORT Blah, Thing, Test := Cheese, Another;
                END Testing.
            ";

            var tree = Parser.Parse(src, ruleset);

            Console.WriteLine(tree.ToString());
            Console.WriteLine("========");
            Console.WriteLine(tree.String);
            Console.ReadKey();
        }
    }
}
